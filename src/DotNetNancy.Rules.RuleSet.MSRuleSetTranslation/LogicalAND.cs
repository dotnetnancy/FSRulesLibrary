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
    public class LogicalAND : CodeExpression, IRuleExpression
    {
        private CodeExpression _leftExpression;
        CodeExpression _rightExpression;
        static RuleLiteralResult resultTrue = new RuleLiteralResult(true);
        static RuleLiteralResult resultFalse = new RuleLiteralResult(false);
        public const string PARSER_DISPLAY_OPERATION = "&&";


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

        CodeBinaryOperatorType _logicalANDOperator = CodeBinaryOperatorType.BooleanAnd;

        public CodeBinaryOperatorType LogicalAndOperator
        {
            get { return _logicalANDOperator; }
            set { _logicalANDOperator = value; }
        }

        CodeBinaryOperatorExpression _logicalAndExpression = null;

        public CodeBinaryOperatorExpression LogicalAndExpression
        {
            get { return _logicalAndExpression; }
            set { _logicalAndExpression = value; }
        }




        public LogicalAND()
        {
            // constructor required for deserialization
        }

        public LogicalAND(CodeExpression left, CodeExpression right)
        {
            // constructor required by parser
            _leftExpression = left;
            _rightExpression = right;

            if (_rightExpression == null || _leftExpression == null)
            {
                throw new ApplicationException("left expression or right expression is null");
            }
            _logicalAndExpression =
                new CodeBinaryOperatorExpression(_leftExpression, _logicalANDOperator, _rightExpression);
        }

        public void AnalyzeUsage(RuleAnalysis analysis, bool isRead, bool isWritten, RulePathQualifier qualifier)
        {
            // check what the 2 expressions use
            RuleExpressionWalker.AnalyzeUsage(analysis, _leftExpression, isRead, isWritten, qualifier);
            RuleExpressionWalker.AnalyzeUsage(analysis, _rightExpression, isRead, isWritten, qualifier);
        }

        public CodeExpression Clone()
        {
            LogicalAND result = new LogicalAND();
            result._leftExpression = RuleExpressionWalker.Clone(_leftExpression);
            result._rightExpression = RuleExpressionWalker.Clone(_rightExpression);
            return result;
        }

        public void Decompile(StringBuilder stringBuilder, CodeExpression parentExpression)
        {
            //stringBuilder.Append(PARSER_DISPLAY_OPERATION);
            //stringBuilder.Append("(");
            //RuleExpressionWalker.Decompile(stringBuilder, _leftExpression, this);
            ////stringBuilder.Append(", ");
            //RuleExpressionWalker.Decompile(stringBuilder, _rightExpression, this);
            //stringBuilder.Append(")");

            RuleExpressionWalker.Decompile(stringBuilder, _logicalAndExpression, this);

        }


        public RuleExpressionResult Evaluate(RuleExecution execution)
        {            
            RuleValidation rv = new RuleValidation(execution.ThisObject.GetType(), null);

            ValidateExpression(rv, _logicalAndExpression, false);

            // start by doing the first 2 expressions
            RuleExpressionResult singleResult = Evaluate(execution, _logicalAndExpression);

            if (rv.Errors.Count > 0)
            {
                ValidationErrorCollection errors = rv.Errors;

                string validationErrorMessages = Helper.GetErrorMessageFromValidationErrorsCollection(errors);

                throw new Exception(validationErrorMessages);
            }

            // start by doing the first 2 expressions

            bool boolSingleResult = (bool)singleResult.Value;

            if (boolSingleResult)
            {
                // left and right true
                return resultTrue;
            }

            else
                //either left or right or both are false
                return resultFalse;
        }

        private RuleExpressionResult Evaluate(RuleExecution execution, CodeExpression expression)
        {
            return RuleExpressionWalker.Evaluate(execution, expression);
        }


        public bool Match(CodeExpression expression)
        {
            LogicalAND other = expression as LogicalAND;
            return (other != null) &&
                RuleExpressionWalker.Match(_leftExpression, other._leftExpression) &&
                RuleExpressionWalker.Match(_rightExpression, other._rightExpression);
        }

        public RuleExpressionInfo Validate(RuleValidation validation, bool isWritten)
        {
            ValidateExpression(validation, _leftExpression, "LeftExpression");
            ValidateExpression(validation, _rightExpression, "RightExpression");
            ValidateExpression(validation, _logicalAndExpression, "LogicalAndExpression");
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


