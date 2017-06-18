using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.CodeDom;
using System.Workflow.Activities.Rules;
using System.Workflow.ComponentModel.Compiler;
using DotNetNancy.Rules.RuleSet.RuleSetDefinition;

namespace DotNetNancy.Rules.RuleSet.MSRuleSetTranslation
{
    public class SingleBooleanResultExpression : CodeExpression, IRuleExpression
    {
        CodeExpression _singleExpression;
        static RuleLiteralResult resultTrue = new RuleLiteralResult(true);
        static RuleLiteralResult resultFalse = new RuleLiteralResult(false);
        public const string PARSER_DISPLAY_OPERATION = "SingleBooleanResultExpression";


        public CodeExpression SingleExpression
        {
            get { return _singleExpression; }
            set { _singleExpression = value; }
        }      


        public SingleBooleanResultExpression()
        {
            // constructor required for deserialization
        }

        public SingleBooleanResultExpression(CodeExpression singleExpression)
        {
            // constructor required by parser
            _singleExpression = singleExpression;

            if (_singleExpression == null)
            {
                throw new ApplicationException("single expression is null");
            }
        }

        public void AnalyzeUsage(RuleAnalysis analysis, bool isRead, bool isWritten, RulePathQualifier qualifier)
        {
            // check what the 2 expressions use
            RuleExpressionWalker.AnalyzeUsage(analysis, _singleExpression, isRead, isWritten, qualifier);
        }

        public CodeExpression Clone()
        {
            SingleBooleanResultExpression result = new SingleBooleanResultExpression();
            result._singleExpression = RuleExpressionWalker.Clone(_singleExpression);
            return result;
        }

        public void Decompile(StringBuilder stringBuilder, CodeExpression parentExpression)
        {
            RuleExpressionWalker.Decompile(stringBuilder, _singleExpression, this);           

        }


        public RuleExpressionResult Evaluate(RuleExecution execution)
        {
            RuleValidation rv = new RuleValidation(execution.ThisObject.GetType(), null);

            ValidateExpression(rv, _singleExpression, false);

            if (rv.Errors.Count > 0)
            {
                ValidationErrorCollection errors = rv.Errors;

                string validationErrorMessages = Helper.GetErrorMessageFromValidationErrorsCollection(errors);

                throw new Exception(validationErrorMessages);
            }

            // start by doing the first 2 expressions
            RuleExpressionResult singleResult = Evaluate(execution, _singleExpression);

            // start by doing the first 2 expressions

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
            SingleBooleanResultExpression other = expression as SingleBooleanResultExpression;
            return (other != null) &&
                RuleExpressionWalker.Match(_singleExpression, other._singleExpression);
        }

        public RuleExpressionInfo Validate(RuleValidation validation, bool isWritten)
        {
            ValidateExpression(validation, _singleExpression, "SingleExpression");
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


