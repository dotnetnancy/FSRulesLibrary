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
    public class StringCompare : CodeExpression, IRuleExpression
    {

        static RuleLiteralResult resultTrue = new RuleLiteralResult(true);
        static RuleLiteralResult resultFalse = new RuleLiteralResult(false);
        private const string COMPARE_METHOD_NAME = "Compare";
        public const string PARSER_DISPLAY_OPERATION = "Compare";

        private CodeExpression _compareMethodInvocationExpression = null;

        public CodeExpression CompareMethodInvocationExpression
        {
            get { return _compareMethodInvocationExpression; }
            set { _compareMethodInvocationExpression = value; }
        }

        CodeExpression _firstStringExpression = null;

        public CodeExpression FirstStringExpression
        {
            get { return _firstStringExpression; }
            set { _firstStringExpression = value; }
        }
        CodeExpression _secondStringExpression = null;

        public CodeExpression SecondStringExpression
        {
            get { return _secondStringExpression; }
            set { _secondStringExpression = value; }
        }

        CodeExpression _caseSensitiveExpression = null;

        public CodeExpression CaseSensitiveExpression
        {
            get { return _caseSensitiveExpression; }
            set { _caseSensitiveExpression = value; }
        }



        CodeExpression _binaryExpressionBasedOnComparisonType = null;

        public CodeExpression BinaryExpressionBasedOnComparisonType
        {
            get { return _binaryExpressionBasedOnComparisonType; }
            set { _binaryExpressionBasedOnComparisonType = value; }
        }

        private ComparisonType _comparisonType = ComparisonType.Equal;

        public StringCompare()
        {
            // constructor required for deserialization
        }

        public StringCompare(CodeExpression firstStringExpression, CodeExpression secondStringExpression, bool caseSensitive, ComparisonType comparisonType)
        {
            _comparisonType = comparisonType;
            _firstStringExpression = firstStringExpression;
            _secondStringExpression = secondStringExpression;
            //the parameter coming in says "case sensitive = true or false" wheras the String.Compare has a parameter of ignore case?  which is the opposite
            //of what the consumer passed in so negate the caseSensitive value to be passed to String.Compare
            _caseSensitiveExpression = new CodePrimitiveExpression(!caseSensitive);

            if (_secondStringExpression == null || _firstStringExpression == null || _comparisonType == ComparisonType.NotSupported)
            {
                throw new ApplicationException("first string expression or second string expression is null or comparison type is not supported");
            }

            
    CodeMethodInvokeExpression toStringOnFirstString = new CodeMethodInvokeExpression(_firstStringExpression, "ToString", new CodeExpression[] { });
    CodeMethodInvokeExpression toStringOnSecondString = new CodeMethodInvokeExpression(_secondStringExpression, "ToString", new CodeExpression[] { });

    _firstStringExpression = toStringOnFirstString;
    _secondStringExpression = toStringOnSecondString;



            _compareMethodInvocationExpression =
               new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("System.String"), COMPARE_METHOD_NAME,
                   new CodeExpression[] { _firstStringExpression,_secondStringExpression,_caseSensitiveExpression });

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
            RuleExpressionWalker.AnalyzeUsage(analysis, _firstStringExpression, isRead, isWritten, qualifier);
            RuleExpressionWalker.AnalyzeUsage(analysis, _secondStringExpression, isRead, isWritten, qualifier);
            RuleExpressionWalker.AnalyzeUsage(analysis, _binaryExpressionBasedOnComparisonType, isRead, isWritten, qualifier);
        }

        public CodeExpression Clone()
        {
            StringCompare result = new StringCompare();

            result._compareMethodInvocationExpression = RuleExpressionWalker.Clone(_compareMethodInvocationExpression);
            result._firstStringExpression = RuleExpressionWalker.Clone(_firstStringExpression);
            result._secondStringExpression = RuleExpressionWalker.Clone(_secondStringExpression);
            result._binaryExpressionBasedOnComparisonType = RuleExpressionWalker.Clone(_binaryExpressionBasedOnComparisonType);
            result._caseSensitiveExpression = RuleExpressionWalker.Clone(_caseSensitiveExpression);

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
            StringCompare other = expression as StringCompare;
            return (other != null) &&
                RuleExpressionWalker.Match(_binaryExpressionBasedOnComparisonType, other._binaryExpressionBasedOnComparisonType) &&
                RuleExpressionWalker.Match(_secondStringExpression, other._secondStringExpression) &&
                RuleExpressionWalker.Match(_firstStringExpression, other._firstStringExpression) &&
                RuleExpressionWalker.Match(_compareMethodInvocationExpression, other._compareMethodInvocationExpression) &&
                RuleExpressionWalker.Match(_caseSensitiveExpression, other._caseSensitiveExpression);

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


