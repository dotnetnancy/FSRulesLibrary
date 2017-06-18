using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.CodeDom;
using System.Workflow.Activities.Rules;
using System.Workflow.ComponentModel.Compiler;

namespace DotNetNancy.Rules.RuleSet.MSRuleSetTranslation
{
    public class StartsWithString : CodeExpression, IRuleExpression
    {

        static RuleLiteralResult resultTrue = new RuleLiteralResult(true);
        static RuleLiteralResult resultFalse = new RuleLiteralResult(false);
        private const string STARTS_WITH_METHOD_NAME = "StartsWith";
        private const string TO_STRING_METHOD_NAME = "ToString";
        private const string TO_LOWER_METHOD_NAME = "ToLower";
        public const string PARSER_DISPLAY_OPERATION = "StartsWith";

        private CodeExpression _toStringMethodInvocationExpression = null;

        public CodeExpression ToStringMethodInvocationExpression
        {
            get { return _toStringMethodInvocationExpression; }
            set { _toStringMethodInvocationExpression = value; }
        }
        private CodeExpression _startsWithMethodInvocationExpression = null;

        public CodeExpression StartsWithMethodInvocationExpression
        {
            get { return _startsWithMethodInvocationExpression; }
            set { _startsWithMethodInvocationExpression = value; }
        }

        private CodeExpression _toLowerMethodInvocationExpression = null;

        public CodeExpression ToLowerMethodInvocationExpression
        {
            get { return _toLowerMethodInvocationExpression; }
            set { _toLowerMethodInvocationExpression = value; }
        }

        private bool _caseSensitive = true;


        private string _valueToLookFor = string.Empty;

        public StartsWithString()
        {
            // constructor required for deserialization
        }

        public StartsWithString(CodeExpression itemToOperateOn, string valueToLookFor, bool caseSensitive)
        {
            //StringComparer.CurrentCultureIgnoreCase;
            //by default the startsWith method and any of those String methods are case sensitive unless otherwise specified using the 
            //StringComparer.CurrentCultureIgnoreCase had trouble using this as as an  overload to the StartsWith method so this is sort of hokey
            //but it does the job

            _caseSensitive = caseSensitive;

            if (_caseSensitive)
            {
                _valueToLookFor = valueToLookFor;
            }
            else
            {
                _valueToLookFor = valueToLookFor.ToLower();
            }

            _toStringMethodInvocationExpression =
                new CodeMethodInvokeExpression(itemToOperateOn, TO_STRING_METHOD_NAME, new CodeExpression[] { });

            _toLowerMethodInvocationExpression = new CodeMethodInvokeExpression(_toStringMethodInvocationExpression, TO_LOWER_METHOD_NAME,
                new CodeExpression[] { });

            if (caseSensitive)
            {
                _startsWithMethodInvocationExpression =
                   new CodeMethodInvokeExpression(_toStringMethodInvocationExpression, STARTS_WITH_METHOD_NAME, new CodeExpression[] { new CodePrimitiveExpression(_valueToLookFor) });
            }
            else
            {
                _startsWithMethodInvocationExpression =
                    new CodeMethodInvokeExpression(_toLowerMethodInvocationExpression, STARTS_WITH_METHOD_NAME, new CodeExpression[] { new CodePrimitiveExpression(_valueToLookFor) });
            }


        }

        public void AnalyzeUsage(RuleAnalysis analysis, bool isRead, bool isWritten, RulePathQualifier qualifier)
        {
            RuleExpressionWalker.AnalyzeUsage(analysis, _startsWithMethodInvocationExpression, isRead, isWritten, qualifier);
        }

        public CodeExpression Clone()
        {
            StartsWithString result = new StartsWithString();

            result._toStringMethodInvocationExpression = RuleExpressionWalker.Clone(_toStringMethodInvocationExpression);
            result._toLowerMethodInvocationExpression = RuleExpressionWalker.Clone(_toLowerMethodInvocationExpression);
            result._startsWithMethodInvocationExpression = RuleExpressionWalker.Clone(_startsWithMethodInvocationExpression);

            return result;
        }

        public void Decompile(StringBuilder stringBuilder, CodeExpression parentExpression)
        {
            //stringBuilder.Append(PARSER_DISPLAY_OPERATION);
            //stringBuilder.Append("(");
            //RuleExpressionWalker.Decompile(stringBuilder, _startsWithMethodInvocationExpression, this);
            //stringBuilder.Append(")");

            RuleExpressionWalker.Decompile(stringBuilder, _startsWithMethodInvocationExpression, this);
        }


        public RuleExpressionResult Evaluate(RuleExecution execution)
        {
            //StringComparer.CurrentCultureIgnoreCase;

            RuleValidation rv = new RuleValidation(execution.ThisObject.GetType(), null);

            ValidateExpression(rv, _startsWithMethodInvocationExpression, false);

            RuleExpressionResult singleResult = Evaluate(execution, _startsWithMethodInvocationExpression);

            if (rv.Errors.Count > 0)
            {
                ValidationErrorCollection errors = rv.Errors;

                string validationErrorMessages = Helper.GetErrorMessageFromValidationErrorsCollection(errors);

                throw new Exception(validationErrorMessages);
            }

            bool boolSingleResult = (bool)singleResult.Value;

            if (boolSingleResult)
            {

                return resultTrue;
            }

            else

                return resultFalse;
        }

        private RuleExpressionResult Evaluate(RuleExecution execution, CodeExpression expression)
        {
            return RuleExpressionWalker.Evaluate(execution, expression);
        }


        public bool Match(CodeExpression expression)
        {
            StartsWithString other = expression as StartsWithString;
            return (other != null) &&
                RuleExpressionWalker.Match(_toLowerMethodInvocationExpression, other._toLowerMethodInvocationExpression) &&
                RuleExpressionWalker.Match(_toStringMethodInvocationExpression, other._toStringMethodInvocationExpression) &&
                RuleExpressionWalker.Match(_startsWithMethodInvocationExpression, other._startsWithMethodInvocationExpression);

        }

        public RuleExpressionInfo Validate(RuleValidation validation, bool isWritten)
        {
            ValidateExpression(validation, _startsWithMethodInvocationExpression, "StartsWithMethodInvocationExpression");
            return new RuleExpressionInfo(typeof(bool));
        }

        private void ValidateExpression(RuleValidation validation, CodeExpression expression, string propertyName)
        {
            ValidationError error;
            if (expression == null)
            {
                error = new ValidationError(propertyName + " cannot be null", 123);
                validation.Errors.Add(error);
            }
            else
            {

                RuleExpressionInfo result = ValidateExpression(validation, expression, false);
                if ((result == null) || (result.ExpressionType != typeof(bool)))
                {
                    error = new ValidationError(propertyName + " must return boolean result", 123);
                    validation.Errors.Add(error);
                }
            }
        }

        public RuleExpressionInfo ValidateExpression(RuleValidation validation, CodeExpression expression, bool isWritten)
        {
            RuleExpressionInfo result = RuleExpressionWalker.Validate(validation, expression, false);

            return result;
        }
    }
}


