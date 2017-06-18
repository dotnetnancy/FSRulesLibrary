using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.CodeDom;
using System.Workflow.Activities.Rules;
using System.Workflow.ComponentModel.Compiler;

namespace DotNetNancy.Rules.RuleSet.MSRuleSetTranslation
{
    public class EndsWithString : CodeExpression, IRuleExpression
    {

        static RuleLiteralResult resultTrue = new RuleLiteralResult(true);
        static RuleLiteralResult resultFalse = new RuleLiteralResult(false);
        private const string ENDS_WITH_METHOD_NAME = "EndsWith";
        private const string TO_STRING_METHOD_NAME = "ToString";
        private const string TO_LOWER_METHOD_NAME = "ToLower";
        public const string PARSER_DISPLAY_OPERATION = "EndsWith";

        private CodeExpression _toStringMethodInvocationExpression = null;

        public CodeExpression ToStringMethodInvocationExpression
        {
            get { return _toStringMethodInvocationExpression; }
            set { _toStringMethodInvocationExpression = value; }
        }
        private CodeExpression _endsWithMethodInvocationExpression = null;

        public CodeExpression EndsWithMethodInvocationExpression
        {
            get { return _endsWithMethodInvocationExpression; }
            set { _endsWithMethodInvocationExpression = value; }
        }

        private CodeExpression _toLowerMethodInvocationExpression = null;

        public CodeExpression ToLowerMethodInvocationExpression
        {
            get { return _toLowerMethodInvocationExpression; }
            set { _toLowerMethodInvocationExpression = value; }
        }

        private bool _caseSensitive = true;


        private string _valueToLookFor = string.Empty;

        public EndsWithString()
        {
            // constructor required for deserialization
        }

        public EndsWithString(CodeExpression itemToOperateOn, string valueToLookFor, bool caseSensitive)
        {
            //StringComparer.CurrentCultureIgnoreCase;
            //by default the endsWith method and any of those String methods are case sensitive unless otherwise specified using the 
            //StringComparer.CurrentCultureIgnoreCase had trouble using this as as an  overload to the EndsWith method so this is sort of hokey
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
                _endsWithMethodInvocationExpression =
                   new CodeMethodInvokeExpression(_toStringMethodInvocationExpression, ENDS_WITH_METHOD_NAME, new CodeExpression[] { new CodePrimitiveExpression(_valueToLookFor) });
            }
            else
            {
                _endsWithMethodInvocationExpression =
                    new CodeMethodInvokeExpression(_toLowerMethodInvocationExpression, ENDS_WITH_METHOD_NAME, new CodeExpression[] { new CodePrimitiveExpression(_valueToLookFor) });
            }


        }

        public void AnalyzeUsage(RuleAnalysis analysis, bool isRead, bool isWritten, RulePathQualifier qualifier)
        {
            RuleExpressionWalker.AnalyzeUsage(analysis, _endsWithMethodInvocationExpression, isRead, isWritten, qualifier);
        }

        public CodeExpression Clone()
        {
            EndsWithString result = new EndsWithString();

            result._toStringMethodInvocationExpression = RuleExpressionWalker.Clone(_toStringMethodInvocationExpression);
            result._toLowerMethodInvocationExpression = RuleExpressionWalker.Clone(_toLowerMethodInvocationExpression);
            result._endsWithMethodInvocationExpression = RuleExpressionWalker.Clone(_endsWithMethodInvocationExpression);

            return result;
        }

        public void Decompile(StringBuilder stringBuilder, CodeExpression parentExpression)
        {
            //stringBuilder.Append(PARSER_DISPLAY_OPERATION);
            //stringBuilder.Append("(");
            //RuleExpressionWalker.Decompile(stringBuilder, _endsWithMethodInvocationExpression, this);
            //stringBuilder.Append(")");

            RuleExpressionWalker.Decompile(stringBuilder, _endsWithMethodInvocationExpression, this);
        }


        public RuleExpressionResult Evaluate(RuleExecution execution)
        {
            //StringComparer.CurrentCultureIgnoreCase;

            RuleValidation rv = new RuleValidation(execution.ThisObject.GetType(), null);

            ValidateExpression(rv, _endsWithMethodInvocationExpression, false);

            RuleExpressionResult singleResult = Evaluate(execution, _endsWithMethodInvocationExpression);

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
            EndsWithString other = expression as EndsWithString;
            return (other != null) &&
                RuleExpressionWalker.Match(_toLowerMethodInvocationExpression, other._toLowerMethodInvocationExpression) &&
                RuleExpressionWalker.Match(_toStringMethodInvocationExpression, other._toStringMethodInvocationExpression) &&
                RuleExpressionWalker.Match(_endsWithMethodInvocationExpression, other._endsWithMethodInvocationExpression);

        }

        public RuleExpressionInfo Validate(RuleValidation validation, bool isWritten)
        {
            ValidateExpression(validation, _endsWithMethodInvocationExpression, "EndsWithMethodInvocationExpression");
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


