using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.Workflow.Activities.Rules;
using System.Workflow.ComponentModel.Compiler;
using DotNetNancy.Rules.RuleSet.MSRuleSetTranslation;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{
    public class NotEqual : CodeExpression, IRuleExpression
    {
        static RuleLiteralResult resultTrue = new RuleLiteralResult(true);
        static RuleLiteralResult resultFalse = new RuleLiteralResult(false);

        CodeExpression _leftExpression = null;

        public CodeExpression LeftExpression
        {
            get { return _leftExpression; }
            set { _leftExpression = value; }
        }
        CodeExpression _rightExpression = null;

        public CodeExpression RightExpression
        {
            get { return _rightExpression; }
            set { _rightExpression = value; }
        }
        CodeBinaryOperatorType _equalsOperator = CodeBinaryOperatorType.ValueEquality;

        public CodeBinaryOperatorType EqualsOperator
        {
            get { return _equalsOperator; }
            set { _equalsOperator = value; }
        }

        CodeBinaryOperatorExpression _equalsExpression = null;

        public CodeBinaryOperatorExpression EqualsExpression
        {
            get { return _equalsExpression; }
            set { _equalsExpression = value; }
        }

        CodeBinaryOperatorExpression _notEqualsExpression = null;

        public CodeBinaryOperatorExpression NotEqualsExpression
        {
            get { return _notEqualsExpression; }
            set { _notEqualsExpression = value; }
        }

         public NotEqual()
         {
                // constructor required for deserialization
         }
            

        public NotEqual(CodeExpression left, CodeExpression right)
        {
            // constructor required by parser
            _leftExpression = left;
            _rightExpression = right;
            _equalsExpression =
               new CodeBinaryOperatorExpression(_leftExpression, _equalsOperator, _rightExpression);

            _notEqualsExpression = new CodeBinaryOperatorExpression(_equalsExpression,
                CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(false));

        }

        #region IRuleExpression Members

        public void AnalyzeUsage(RuleAnalysis analysis, bool isRead, bool isWritten, RulePathQualifier qualifier)
        {
            RuleExpressionWalker.AnalyzeUsage(analysis, _leftExpression, isRead, isWritten, qualifier);
            RuleExpressionWalker.AnalyzeUsage(analysis, _rightExpression, isRead, isWritten, qualifier);

        }

        public CodeExpression Clone()
        {
            NotEqual result = new NotEqual();
            result._leftExpression = RuleExpressionWalker.Clone(_leftExpression);
            result._rightExpression = RuleExpressionWalker.Clone(_rightExpression);
            return result;
        }

        public void Decompile(StringBuilder stringBuilder, CodeExpression parentExpression)
        {
            // what should be displayed by the parser
            //stringBuilder.Append("\"");
            RuleExpressionWalker.Decompile(stringBuilder,_notEqualsExpression,this);
        }

        public RuleExpressionResult Evaluate(RuleExecution execution)
        {           

            RuleValidation rv = new RuleValidation(execution.ThisObject.GetType(), null);

            ValidateExpression(rv, _equalsExpression, false);

            if (rv.Errors.Count > 0)
            {
                ValidationErrorCollection errors = rv.Errors;

                string validationErrorMessages = Helper.GetErrorMessageFromValidationErrorsCollection(errors);

                throw new Exception(validationErrorMessages);
            }

            ValidateExpression(rv, _notEqualsExpression, false);

            if (rv.Errors.Count > 0)
            {
                ValidationErrorCollection errors = rv.Errors;

                string validationErrorMessages = Helper.GetErrorMessageFromValidationErrorsCollection(errors);

                throw new Exception(validationErrorMessages);
            }


            // start by doing the first 2 expressions
            RuleExpressionResult singleResult = Evaluate(execution, _notEqualsExpression);

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
                // left and right are equal so return true
                return resultTrue;
            }

            else
                //left and right are not equal so return false
                return resultFalse;
        }

        private RuleExpressionResult Evaluate(RuleExecution execution, CodeExpression expression)
        {
            return RuleExpressionWalker.Evaluate(execution, expression);
        }

        public bool Match(CodeExpression expression)
        {
            NotEqual other = expression as NotEqual;
            return (other != null) &&
                RuleExpressionWalker.Match(_leftExpression, other._leftExpression) &&
                RuleExpressionWalker.Match(_rightExpression, other._rightExpression);
        }

        public RuleExpressionInfo Validate(RuleValidation validation, bool isWritten)
        {
            ValidateExpression(validation, _leftExpression, "LeftExpression");
            ValidateExpression(validation, _rightExpression, "RightExpression");
            ValidateExpression(validation, _equalsExpression, "EqualsExpression");
            ValidateExpression(validation, _notEqualsExpression, "NotEqualsExpression");
            return new RuleExpressionInfo(typeof(bool));
        }

        /// <summary>
        /// we could also do here alot of custom things say if we wanted to try to resolve the problem by providing a default
        /// or whatever we have many options here...
        /// </summary>
        /// <param name="validation"></param>
        /// <param name="expression"></param>
        /// <param name="propertyName"></param>
        private void ValidateExpression(RuleValidation validation, CodeExpression expression, string propertyName)
        {
            ValidationError error;
            if (expression == null)
            {
                error = new ValidationError(propertyName + " cannot be null", 123);
                validation.Errors.Add(error);
            }
            // here would could make sure that the value is not null if we want
            //or perhaps we could verify that the result is a particular type, we have
            //lots of options here to do whatever granular custom validation that we want.
            else
            {
                //do not remove this line very important, will not execute w/o it
                RuleExpressionInfo result = ValidateExpression(validation, expression, false);

                //if (result == null)
                //{
                //    error = new ValidationError(propertyName + " cannot be null value ", 123);
                //    validation.Errors.Add(error);
                //}
            }
        }

        public RuleExpressionInfo ValidateExpression(RuleValidation validation, CodeExpression expression, bool isWritten)
        {
            RuleExpressionInfo result = RuleExpressionWalker.Validate(validation, expression, false);
            return result;
        }

        #endregion
    }
}
