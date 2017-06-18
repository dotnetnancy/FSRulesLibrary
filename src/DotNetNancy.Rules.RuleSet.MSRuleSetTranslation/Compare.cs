using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.CodeDom;
using System.Workflow.Activities.Rules;
using System.Workflow.ComponentModel.Compiler;
using DotNetNancy.Rules.RuleSet.Translation;
using DotNetNancy.Rules.RuleSet.RuleSetDefinition;


namespace DotNetNancy.Rules.RuleSet.MSRuleSetTranslation
{
    /// <summary>
    /// it is very important that when you pass the left and right expression into this class that they are casted to the appropriate
    /// type for example if property you are comparing to is double then make sure you cast the left expression to double as well (or vice versa)
    /// whichever your intention is
    /// same thing goes for any other type like DateTime etc
    /// </summary>
    public class CompareTo : CodeExpression, IRuleExpression
    {

        static RuleLiteralResult resultTrue = new RuleLiteralResult(true);
        static RuleLiteralResult resultFalse = new RuleLiteralResult(false);
        private const string COMPARE_METHOD_NAME = "CompareTo";
        public const string PARSER_DISPLAY_OPERATION = "CompareTo";       

        private CodeExpression _compareMethodInvocationExpression = null;

        public CodeExpression CompareMethodInvocationExpression
        {
            get { return _compareMethodInvocationExpression; }
            set { _compareMethodInvocationExpression = value; }
        }

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

        CodeExpression _binaryExpressionBasedOnComparisonType = null;

        public CodeExpression BinaryExpressionBasedOnComparisonType
        {
            get { return _binaryExpressionBasedOnComparisonType; }
            set { _binaryExpressionBasedOnComparisonType = value; }
        }

        private ComparisonType _comparisonType = ComparisonType.Equal;

        public CompareTo()
        {
            // constructor required for deserialization
        }

        public CompareTo(CodeExpression left , CodeExpression right, ComparisonType comparisonType)
        {
            _comparisonType = comparisonType;
            _leftExpression = left;
            _rightExpression = right;            

            if (_rightExpression == null || _leftExpression == null || _comparisonType == ComparisonType.NotSupported)
            {
                throw new ApplicationException("left expression or right expression is null or comparison type is not supported");
            }

            _compareMethodInvocationExpression =
               new CodeMethodInvokeExpression(_leftExpression, COMPARE_METHOD_NAME, new CodeExpression[] 
               {_rightExpression });

            _binaryExpressionBasedOnComparisonType = GetBinaryExpressionByComparisonType();

        }

        private CodeExpression GetBinaryExpressionByComparisonType()
        {
            CodeExpression codeBinaryOperatorExpression = null;
            
            switch (_comparisonType)
            {
                case ComparisonType.Equal:
                    {
                        // equals 0 if they are equal
                        codeBinaryOperatorExpression = new CodeBinaryOperatorExpression(_compareMethodInvocationExpression,
                            CodeBinaryOperatorType.ValueEquality,
                            new CodePrimitiveExpression(0));
                        break;
                    }
                case ComparisonType.NotEqual:
                    {
                        CodeExpression compareToEquals0 = new CodeBinaryOperatorExpression(_compareMethodInvocationExpression,
                            CodeBinaryOperatorType.ValueEquality,
                            new CodePrimitiveExpression(0));

                        //negation
                        codeBinaryOperatorExpression = new CodeBinaryOperatorExpression(compareToEquals0,
                            CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(false));

                        break;
                    }
                case ComparisonType.GreaterThan:
                    {
                        //return is greater than 0 if greater than
                        codeBinaryOperatorExpression =
                            new CodeBinaryOperatorExpression(_compareMethodInvocationExpression,
                                CodeBinaryOperatorType.GreaterThan,
                                new CodePrimitiveExpression(0));
                        break;
                    }
                case ComparisonType.LessThan:
                    {
                        //return is less than 0 if less than
                        codeBinaryOperatorExpression =
                            new CodeBinaryOperatorExpression(_compareMethodInvocationExpression,
                                CodeBinaryOperatorType.LessThan,
                                new CodePrimitiveExpression(0));
                        break;
                    }

                case ComparisonType.GreaterThanOrEqual:
                    {
                        //return of compare to method will be greater than or equal to 0 if greater than or equal to
                        codeBinaryOperatorExpression =
                            new CodeBinaryOperatorExpression(_compareMethodInvocationExpression,
                                CodeBinaryOperatorType.GreaterThanOrEqual,
                                new CodePrimitiveExpression(0));
                        break;
                    }

                case ComparisonType.LessThanOrEqual:
                    {
                        codeBinaryOperatorExpression =
                            new CodeBinaryOperatorExpression(_compareMethodInvocationExpression,
                                CodeBinaryOperatorType.LessThanOrEqual,
                                new CodePrimitiveExpression(0));
                        break;
                    }
                

            }

            return codeBinaryOperatorExpression;
        }

        public void AnalyzeUsage(RuleAnalysis analysis, bool isRead, bool isWritten, RulePathQualifier qualifier)
        {
            RuleExpressionWalker.AnalyzeUsage(analysis, _compareMethodInvocationExpression, isRead, isWritten, qualifier);
            RuleExpressionWalker.AnalyzeUsage(analysis, _leftExpression, isRead, isWritten, qualifier);
            RuleExpressionWalker.AnalyzeUsage(analysis, _rightExpression, isRead, isWritten, qualifier);
            RuleExpressionWalker.AnalyzeUsage(analysis, _binaryExpressionBasedOnComparisonType, isRead, isWritten, qualifier);
        }

        public CodeExpression Clone()
        {
            CompareTo result = new CompareTo();

            result._compareMethodInvocationExpression = RuleExpressionWalker.Clone(_compareMethodInvocationExpression);
            result._leftExpression = RuleExpressionWalker.Clone(_leftExpression);
            result._rightExpression = RuleExpressionWalker.Clone(_rightExpression);
            result._binaryExpressionBasedOnComparisonType = RuleExpressionWalker.Clone(_binaryExpressionBasedOnComparisonType);

            return result;
        }

        public void Decompile(StringBuilder stringBuilder, CodeExpression parentExpression)
        {
            //stringBuilder.Append(PARSER_DISPLAY_OPERATION);
            //stringBuilder.Append("(");
            //RuleExpressionWalker.Decompile(stringBuilder, _binaryExpressionBasedOnComparisonType, this);
            //stringBuilder.Append(")");

            RuleExpressionWalker.Decompile(stringBuilder, _binaryExpressionBasedOnComparisonType, this);
        }


        public RuleExpressionResult Evaluate(RuleExecution execution)
        {

            //StringComparer.CurrentCultureIgnoreCase;

            RuleValidation rv = new RuleValidation(execution.ThisObject.GetType(), null);

            ValidateExpression(rv, _binaryExpressionBasedOnComparisonType, false);



            // start by doing the first 2 expressions
            RuleExpressionResult singleResult = Evaluate(execution, _binaryExpressionBasedOnComparisonType);

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
            CompareTo other = expression as CompareTo;
            return (other != null) &&
                RuleExpressionWalker.Match(_binaryExpressionBasedOnComparisonType, other._binaryExpressionBasedOnComparisonType) &&
                RuleExpressionWalker.Match(_rightExpression, other._rightExpression) &&
                RuleExpressionWalker.Match(_leftExpression, other._leftExpression) &&
                RuleExpressionWalker.Match(_compareMethodInvocationExpression, other._compareMethodInvocationExpression);

        }

        public RuleExpressionInfo Validate(RuleValidation validation, bool isWritten)
        {
            ValidateExpression(validation, _binaryExpressionBasedOnComparisonType, "BinaryExpressionBasedOnComparisonType");
            return new RuleExpressionInfo(typeof(bool));
        }

        private void ValidateExpression(RuleValidation validation, CodeExpression expression, string propertyName)
        {
            ValidationError error;
            if (expression == null)
            {
                error = new ValidationError(propertyName + " cannot be null", 123456);
                validation.Errors.Add(error);
            }
            else
            {

                RuleExpressionInfo result = ValidateExpression(validation, expression, false);
                if ((result == null) || (result.ExpressionType != typeof(bool)))
                {
                    error = new ValidationError(propertyName + " must return boolean result", 123456);
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


