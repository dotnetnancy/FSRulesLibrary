using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.CodeDom;
using System.Workflow.Activities.Rules;
using System.Workflow.ComponentModel.Compiler;

namespace DotNetNancy.Rules.RuleSet.MSRuleSetTranslation
{
    public class LogicalOR : CodeExpression, IRuleExpression
    {
        CodeExpression _leftExpression;
        CodeExpression _rightExpression;
        static RuleLiteralResult resultTrue = new RuleLiteralResult(true);
        static RuleLiteralResult resultFalse = new RuleLiteralResult(false);
        public const string PARSER_DISPLAY_OPERATION = "||";


        public CodeExpression LeftExpression
        {
            get { return _leftExpression; }
            set { _leftExpression = value; }
        }

        public CodeExpression RightExpression
        {
            get { return _rightExpression; }
            set { _rightExpression = value; }
        }

        CodeBinaryOperatorType _logicalOROperator = CodeBinaryOperatorType.BooleanOr;

        public CodeBinaryOperatorType LogicalOROperator
        {
            get { return _logicalOROperator; }
            set { _logicalOROperator = value; }
        }

        CodeBinaryOperatorExpression _logicalOrExpression = null;

        public CodeBinaryOperatorExpression LogicalOrExpression
        {
            get { return _logicalOrExpression; }
            set { _logicalOrExpression = value; }
        }

        public LogicalOR()
        {
            // constructor required for deserialization
        }

        public LogicalOR(CodeExpression left, CodeExpression right)
        {
            // constructor required by parser
            _leftExpression = left;
            _rightExpression = right;

            if (_rightExpression == null || _leftExpression == null)
            {
                throw new ApplicationException("left expression or right expression is null");
            }
            _logicalOrExpression =
               new CodeBinaryOperatorExpression(_leftExpression, _logicalOROperator, _rightExpression);
        }

        public void AnalyzeUsage(RuleAnalysis analysis, bool isRead, bool isWritten, RulePathQualifier qualifier)
        {
            // check what the 2 expressions use
            RuleExpressionWalker.AnalyzeUsage(analysis, _leftExpression, isRead, isWritten, qualifier);
            RuleExpressionWalker.AnalyzeUsage(analysis, _rightExpression, isRead, isWritten, qualifier);
        }

        public CodeExpression Clone()
        {
            LogicalOR result = new LogicalOR();
            result._leftExpression = RuleExpressionWalker.Clone(_leftExpression);
            result._rightExpression = RuleExpressionWalker.Clone(_rightExpression);
            return result;
        }

        public void Decompile(StringBuilder stringBuilder, CodeExpression parentExpression)
        {
            
            RuleExpressionWalker.Decompile(stringBuilder, _logicalOrExpression, this);
        }


        public RuleExpressionResult Evaluate(RuleExecution execution)
        {           
            RuleValidation rv = new RuleValidation(execution.ThisObject.GetType(), null);

            ValidateExpression(rv, _logicalOrExpression, false);

            // start by doing the first 2 expressions
            RuleExpressionResult singleResult = Evaluate(execution, _logicalOrExpression);

            if (rv.Errors.Count > 0)
            {
                ValidationErrorCollection errors = rv.Errors;

                string validationErrorMessages = Helper.GetErrorMessageFromValidationErrorsCollection(errors);

                throw new Exception(validationErrorMessages);
            }
            bool boolSingleResult = (bool)singleResult.Value;

            if (boolSingleResult)
            {
                // either left or right is true
                return resultTrue;
            }

            else
                //neither left or right is true
                return resultFalse;
        }

        private RuleExpressionResult Evaluate(RuleExecution execution, CodeExpression expression)
        {
            return RuleExpressionWalker.Evaluate(execution, expression);
        }

        public bool Match(CodeExpression expression)
        {
            LogicalOR other = expression as LogicalOR;
            return (other != null) &&
                RuleExpressionWalker.Match(_leftExpression, other._leftExpression) &&
                RuleExpressionWalker.Match(_rightExpression, other._rightExpression);
        }

        public RuleExpressionInfo Validate(RuleValidation validation, bool isWritten)
        {
            ValidateExpression(validation, _leftExpression, "LeftExpression");
            ValidateExpression(validation, _rightExpression, "RightExpression");
            ValidateExpression(validation, _logicalOrExpression, "LogicalOrExpression");
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


