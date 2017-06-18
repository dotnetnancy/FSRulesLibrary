using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.CodeDom;
using System.Workflow.Activities.Rules;
using System.Workflow.ComponentModel.Compiler;
using DotNetNancy.Rules.RuleSet.RuleSetDefinition;
using DotNetNancy.Rules.RuleSet.Translation;
using System.Text.RegularExpressions;
using DotNetNancy.Rules.RuleSet.Common;

namespace DotNetNancy.Rules.RuleSet.MSRuleSetTranslation
{
    public class WildCardSearchExpression : CodeExpression, IRuleExpression
    {
        static RuleLiteralResult resultTrue = new RuleLiteralResult(true);
        static RuleLiteralResult resultFalse = new RuleLiteralResult(false);
        public const string PARSER_DISPLAY_OPERATION = "WildCardSearch";
        public const string REGEX_IS_MATCH_METHOD_NAME = "IsMatch";
        private const string TO_STRING_METHOD_NAME = "ToString";
        private const string TO_LOWER_METHOD_NAME = "ToLower";        

        CodeExpression _leftExpression = null;
        string _originalValueToCompare = null;
        ComparisonType _comparisonType = ComparisonType.NotSupported;
        StringComparisonType _stringComparisonType = StringComparisonType.NotSupported;
        bool _caseSensitive = false;
        string _patternToMatch = null;


        //derives from Regex
        WildCard _wildCard = null;        

        CodeExpression _regexMatchMethodInvocationExpression = null;
        CodeExpression _toStringMethodInvocationExpression = null;
        CodeExpression _toLowerMethodInvocationExpression = null;


        public WildCardSearchExpression()
        {
            // constructor required for deserialization
        }

        public WildCardSearchExpression(CodeExpression leftExpression, string valueToCompare, ComparisonType comparisonType, bool caseSensitive)
        {
            // constructor required by parser
            _leftExpression = leftExpression;
            _comparisonType = comparisonType;
            _originalValueToCompare = valueToCompare;
            _caseSensitive = caseSensitive;

            if (_leftExpression == null || String.IsNullOrEmpty(_originalValueToCompare) || _comparisonType == ComparisonType.NotSupported)
            {
                throw new ApplicationException("left expression is null or value to compare is null or empty or comparison type is not supported");
            }

            LoadRegexMatchExpression(_comparisonType);
        }       

        public WildCardSearchExpression(CodeExpression leftExpression, string valueToCompare, StringComparisonType stringComparisonType, bool caseSensitive)
        {
            // constructor required by parser
            _leftExpression = leftExpression;
            _stringComparisonType = stringComparisonType;
            _originalValueToCompare = valueToCompare;
            _caseSensitive = caseSensitive;

            if (_leftExpression == null || String.IsNullOrEmpty(_originalValueToCompare) || _stringComparisonType == StringComparisonType.NotSupported)
            {
                throw new ApplicationException("left expression is null or value to compare is null or empty or string comparison type is not supported");
            }
            LoadRegexMatchExpression(_stringComparisonType);
        }

        private void LoadRegexMatchExpression(StringComparisonType stringComparisonType)
        {
            //for a string the comparison type should always be contains, startswith, endswith etc are not supported in this class
            if (stringComparisonType == StringComparisonType.Contains ||
                stringComparisonType == StringComparisonType.NotContains)
            {
                //stringcomparisonType denotes items that are compatible with the contains function in c# for this class it means
                //that we are comparing the property on the left with a regex match call on the right

                if (_caseSensitive)
                {
                    //default behaviour of .net is to do it case sensitive match
                    _wildCard = new WildCard(_originalValueToCompare);
                    _patternToMatch = _wildCard.WildcardToRegex(_originalValueToCompare);
                }
                else
                {
                    //only if specified will it do a case insensitive match
                    _wildCard = new WildCard(_originalValueToCompare, RegexOptions.IgnoreCase);
                    _patternToMatch = _wildCard.WildcardToRegex(_originalValueToCompare.ToLower());
                }

                BuildRegexMatchMethodInvokeExpression();

            }
            else
            {
                throw new ApplicationException("Wildcard search is not supported for any string compare types other than Contains or No Contains");
            }
        }

         private void LoadRegexMatchExpression(ComparisonType comparisonType)
        {
            //for a string the comparison type should always be equal, (greater than or less than) are not supported in this class
            if (comparisonType == ComparisonType.Equal)
            {
                //comparisonType denotes items that are compatible with the CompareTo function in c# for this class it means
                //that we are comparing the property on the left with a regex match call on the right

                
                if(_caseSensitive)
                {
                    //default behaviour of .net is to do it case sensitive match
                    _wildCard = new WildCard(_originalValueToCompare);
                    _patternToMatch = _wildCard.WildcardToRegex(_originalValueToCompare);
                }
                else
                {
                    //only if specified will it do a case insensitive match
                    _wildCard = new WildCard(_originalValueToCompare, RegexOptions.IgnoreCase);
                    _patternToMatch = _wildCard.WildcardToRegex(_originalValueToCompare.ToLower());
                }

                BuildRegexMatchMethodInvokeExpression();

            }
            else
            {
                throw new ApplicationException("Wildcard search is not supported for any compare types other than Equal");
            }
        }

         private void BuildRegexMatchMethodInvokeExpression()
         {             

             if (_caseSensitive)
             {
                 //do nothing it is case sensitive
             }
             else
             {
                 _originalValueToCompare = _originalValueToCompare.ToLower();
             }

             _toStringMethodInvocationExpression =
                 new CodeMethodInvokeExpression(_leftExpression, TO_STRING_METHOD_NAME, new CodeExpression[] { });

             _toLowerMethodInvocationExpression = new CodeMethodInvokeExpression(_toStringMethodInvocationExpression, TO_LOWER_METHOD_NAME,
                 new CodeExpression[] { });

             if (_caseSensitive)
             {
                 _regexMatchMethodInvocationExpression =
                    new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(Regex)), REGEX_IS_MATCH_METHOD_NAME, new CodeExpression[] { _toStringMethodInvocationExpression,
                                                   new CodePrimitiveExpression(_patternToMatch)});

             }
             else
             {
                 _regexMatchMethodInvocationExpression =
                     new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(Regex)), REGEX_IS_MATCH_METHOD_NAME, new CodeExpression[] { _toLowerMethodInvocationExpression,
                                                    new CodePrimitiveExpression(_patternToMatch) });
             }
         }

        public void AnalyzeUsage(RuleAnalysis analysis, bool isRead, bool isWritten, RulePathQualifier qualifier)
        {
            // check what the 2 expressions use
            RuleExpressionWalker.AnalyzeUsage(analysis, _regexMatchMethodInvocationExpression, isRead, isWritten, qualifier);
        }

        public CodeExpression Clone()
        {
            WildCardSearchExpression result = new WildCardSearchExpression();

            result._toStringMethodInvocationExpression = RuleExpressionWalker.Clone(_toStringMethodInvocationExpression);
            result._toLowerMethodInvocationExpression = RuleExpressionWalker.Clone(_toLowerMethodInvocationExpression);
            result._regexMatchMethodInvocationExpression = RuleExpressionWalker.Clone(_regexMatchMethodInvocationExpression);

            return result;
        }

        public void Decompile(StringBuilder stringBuilder, CodeExpression parentExpression)
        {
            RuleExpressionWalker.Decompile(stringBuilder, _regexMatchMethodInvocationExpression, this);
        }


        public RuleExpressionResult Evaluate(RuleExecution execution)
        {
            RuleValidation rv = new RuleValidation(execution.ThisObject.GetType(), null);

            ValidateExpression(rv, _regexMatchMethodInvocationExpression, false);

            RuleExpressionResult singleResult = Evaluate(execution, _regexMatchMethodInvocationExpression);

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
            WildCardSearchExpression other = expression as WildCardSearchExpression;
            return (other != null) &&
                RuleExpressionWalker.Match(_toLowerMethodInvocationExpression, other._toLowerMethodInvocationExpression) &&
                RuleExpressionWalker.Match(_toStringMethodInvocationExpression, other._toStringMethodInvocationExpression) &&
                RuleExpressionWalker.Match(_regexMatchMethodInvocationExpression, other._regexMatchMethodInvocationExpression);
        }

        public RuleExpressionInfo Validate(RuleValidation validation, bool isWritten)
        {
            ValidateExpression(validation, _regexMatchMethodInvocationExpression, "RegexMatchMethodInvocationExpression");
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


