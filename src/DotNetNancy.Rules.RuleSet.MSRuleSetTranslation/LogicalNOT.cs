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
    public class LogicalNOT : CodeExpression, IRuleExpression
    {
        CodeExpression _singleExpression;
        static RuleLiteralResult resultTrue = new RuleLiteralResult(true);
        static RuleLiteralResult resultFalse = new RuleLiteralResult(false);
        public const string PARSER_DISPLAY_OPERATION = "!";


        public CodeExpression SingleExpression
        {
            get { return _singleExpression; }
            set { _singleExpression = value; }
        }


        CodeBinaryOperatorType _logicalEqualsOperator = CodeBinaryOperatorType.ValueEquality;

        public CodeBinaryOperatorType LogicalEqualsOperator
        {
            get { return _logicalEqualsOperator; }
            set { _logicalEqualsOperator = value; }
        }


        public LogicalNOT()
        {
            // constructor required for deserialization
        }

        public LogicalNOT(CodeExpression singleExpression)
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
            RuleExpressionWalker.AnalyzeUsage(analysis, _singleExpression,isRead,isWritten,qualifier);
        }

        public CodeExpression Clone()
        {
            LogicalNOT result = new LogicalNOT();
            result._singleExpression = RuleExpressionWalker.Clone(_singleExpression);
            return result;
        }

        public void Decompile(StringBuilder stringBuilder, CodeExpression parentExpression)
        {
            stringBuilder.Append("  ");
            stringBuilder.Append(PARSER_DISPLAY_OPERATION);
            stringBuilder.Append("(");
            RuleExpressionWalker.Decompile(stringBuilder, _singleExpression, this);
            stringBuilder.Append(")");
            stringBuilder.Append("  ");

        }


        public RuleExpressionResult Evaluate(RuleExecution execution)
        {
            RuleValidation rv = new RuleValidation(execution.ThisObject.GetType(), null);

            ValidateExpression(rv, _singleExpression, false);

            // start by doing the first 2 expressions
            RuleExpressionResult singleResult = Evaluate(execution, _singleExpression);

            if (rv.Errors.Count > 0)
            {
                ValidationErrorCollection errors = rv.Errors;

                string validationErrorMessages = Helper.GetErrorMessageFromValidationErrorsCollection(errors);

                throw new Exception(validationErrorMessages);
            }
            // start by doing the first 2 expressions

            bool boolSingleResult = (bool)singleResult.Value;

            bool negatedSingleResult = !boolSingleResult;

            if (negatedSingleResult)
            {
                // return negated value
                return resultTrue;
            }

            else
                //return negated value
                return resultFalse;
        }

        private RuleExpressionResult Evaluate(RuleExecution execution, CodeExpression expression)
        {
            return RuleExpressionWalker.Evaluate(execution, expression);
        }


        public bool Match(CodeExpression expression)
        {
            LogicalNOT other = expression as LogicalNOT;
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


