using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.Workflow.Activities.Rules;
using DotNetNancy.Rules.RuleSet.Translation;
using System.Workflow.ComponentModel.Compiler;

namespace DotNetNancy.Rules.RuleSet.MSRuleSetTranslation
{
    public class In : CodeExpression, IRuleExpression
    {
        string[] _splitStringvalues = null;
        const char _delimiter = ',';

        CodeExpression _finalLogicalOrExpression = null;
        List<CodeExpression> _individualExpressions = null;

        static RuleLiteralResult resultTrue = new RuleLiteralResult(true);
        static RuleLiteralResult resultFalse = new RuleLiteralResult(false);

        public In()
        {
        }

        public In(CodeMethodReferenceExpression methodReferenceExpression, string commaSeparatedValues)
        {

            if (String.IsNullOrEmpty(commaSeparatedValues))
            {
                throw new ApplicationException("In expects a string with comma separated values, this string is null or empty, cannot generate In operator");
            }

            _splitStringvalues = commaSeparatedValues.Split(_delimiter);
            _individualExpressions = new List<CodeExpression>();

            foreach (string commaSeparatedValue in _splitStringvalues)
            {
                _individualExpressions.Add(GetIndividualCodeExpression(methodReferenceExpression, commaSeparatedValue));
            }
            GenerateFinalLogicalOrExpression();

        }


        
        public In(CodePropertyReferenceExpression propertyReferenceExpression, string commaSeparatedValues)
        {
            if (String.IsNullOrEmpty(commaSeparatedValues))
            {
                throw new ApplicationException("In expects a string with comma separated values, this string is null or empty, cannot generate In operator");
            }
            //if the delimeter is not found this is what microsoft says is returned
            //"Return Value: An array consisting of a single element containing this instance, if this instance contains none of the characters in separator."
            _splitStringvalues = commaSeparatedValues.Split(_delimiter);

            _individualExpressions = new List<CodeExpression>();

            foreach (string commaSeparatedValue in _splitStringvalues)
            {
                _individualExpressions.Add(GetIndividualCodeExpression(propertyReferenceExpression, commaSeparatedValue));
            }

            GenerateFinalLogicalOrExpression();
        }

        private void GenerateFinalLogicalOrExpression()
        {    
            CodeExpression mainOr = null;

            int countExpressions = _individualExpressions.Count;

            if (countExpressions > 1)
            {

                bool even = (countExpressions % 2 == 0) ? true : false;

                if (even)
                {
                    List<LogicalOR> list = new List<LogicalOR>();

                    for (int i = 2; i <= countExpressions; i += 2)
                    {
                        LogicalOR or = new LogicalOR(_individualExpressions[i - 2], _individualExpressions[i - 1]);
                        list.Add(or);
                    }

                    if (list.Count % 2 == 0)
                    {
                        GetLogicalOrByEvenListOfOrs(ref mainOr, list);
                    }
                    else
                    {
                        LogicalOR oddItem = list[0];
                        list.RemoveAt(0);

                        GetLogicalOrByEvenListOfOrs(ref mainOr, list);
                        if (mainOr == null)
                        {
                            mainOr = oddItem;
                        }
                        else
                        {
                            mainOr = new LogicalOR(oddItem, mainOr);
                        }
                    }

                    this._finalLogicalOrExpression = mainOr;

                }
                else
                {
                    List<LogicalOR> list = new List<LogicalOR>();

                    for (int i = 2; i <= countExpressions; i += 2)
                    {
                        LogicalOR or = new LogicalOR(_individualExpressions[i - 2], _individualExpressions[i - 1]);
                        list.Add(or);
                        //this is the last one since it is not even
                        if (i == countExpressions - 1)
                        {
                            mainOr = _individualExpressions[i];

                            if (list.Count % 2 == 0)
                            {
                                GetLogicalOrByEvenListOfOrs(ref mainOr, list);
                            }
                            else
                            {
                                LogicalOR oddItem = list[0];
                                list.RemoveAt(0);

                                GetLogicalOrByEvenListOfOrs(ref mainOr, list);
                                if (mainOr == null)
                                {
                                    mainOr = oddItem;
                                }
                                else
                                {
                                    mainOr = new LogicalOR(oddItem, mainOr);
                                }
                            }

                            //then there is one left
                        }

                    }


                }
            }
            else
            {
                //if there is not at least one i hope the ui will not allow the rule to be saved
                //but if so then the final expression will be just the expression like property=value
                mainOr = _individualExpressions[0];
            }

            this._finalLogicalOrExpression = mainOr;

        }

        private void GetLogicalOrByEvenListOfOrs(ref CodeExpression mainExpression, List<LogicalOR> individualLogicalOrs)
        {
           

            //make sure it is even
            if (individualLogicalOrs.Count % 2 != 0)
            {
                throw new ApplicationException("GetLogicalOrByEvenListOfOrs, was given a list that does not have an even number of items");
            }

            if (individualLogicalOrs.Count > 0)
            {
                for (int i = 2; i <= individualLogicalOrs.Count; i += 2)
                {
                    LogicalOR or = new LogicalOR(individualLogicalOrs[i-2], individualLogicalOrs[i -1]);
                    if (mainExpression == null)
                    {
                        mainExpression = or;
                    }
                    else
                    {
                        mainExpression = new LogicalOR(mainExpression, or);
                    }
                    individualLogicalOrs.Remove(individualLogicalOrs[0]);
                    individualLogicalOrs.Remove(individualLogicalOrs[0]);

                    if (individualLogicalOrs.Count > 0)
                    {

                        GetLogicalOrByEvenListOfOrs(ref mainExpression, individualLogicalOrs);
                    }
                    else
                    {
                        //terminating condition if the list of items we are processing is empty
                        return;
                    }
                }
            }
            //terminating condition for the recursion if count of list items == 0
            else
            {
                return;
            }

        }


        private CodeExpression GetIndividualCodeExpression(CodeMethodReferenceExpression methodReferenceExpression, string commaSeparatedValue)
        {
            CodeMethodInvokeExpression invoke = new CodeMethodInvokeExpression(methodReferenceExpression, new CodeExpression[] { new CodePrimitiveExpression(commaSeparatedValue) });
            SingleBooleanResultExpression expression = new SingleBooleanResultExpression(invoke);
            return expression;
        }


        private CodeExpression GetIndividualCodeExpression(CodePropertyReferenceExpression propertyReferenceExpression, string commaSeparatedValue)
        {
            SingleBooleanResultExpression expression = new SingleBooleanResultExpression(new CompareTo(propertyReferenceExpression,
                new CodePrimitiveExpression(commaSeparatedValue),
                ComparisonType.Equal));
            return expression;
        }


        #region IRuleExpression Members

        public void AnalyzeUsage(RuleAnalysis analysis, bool isRead, bool isWritten, RulePathQualifier qualifier)
        {
            RuleExpressionWalker.AnalyzeUsage(analysis, _finalLogicalOrExpression, isRead, isWritten, qualifier);
        }

        public CodeExpression Clone()
        {
            In result = new In();
            result._finalLogicalOrExpression = RuleExpressionWalker.Clone(_finalLogicalOrExpression);
            return result;
        }

        public void Decompile(StringBuilder stringBuilder, CodeExpression parentExpression)
        {
            RuleExpressionWalker.Decompile(stringBuilder, _finalLogicalOrExpression, this);  
        }

        public RuleExpressionResult Evaluate(RuleExecution execution)
        {
            RuleValidation rv = new RuleValidation(execution.ThisObject.GetType(), null);

            ValidateExpression(rv, _finalLogicalOrExpression, false);

            if (rv.Errors.Count > 0)
            {
                ValidationErrorCollection errors = rv.Errors;

                string validationErrorMessages = Helper.GetErrorMessageFromValidationErrorsCollection(errors);

                throw new Exception(validationErrorMessages);
            }

            // start by doing the first 2 expressions
            RuleExpressionResult singleResult = Evaluate(execution, _finalLogicalOrExpression);

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
            In other = expression as In;
            return (other != null) &&
                RuleExpressionWalker.Match(_finalLogicalOrExpression, other._finalLogicalOrExpression);
        }

        public RuleExpressionInfo Validate(RuleValidation validation, bool isWritten)
        {
            ValidateExpression(validation, _finalLogicalOrExpression, "FinalLogicalOrExpression");
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


        #endregion
    }
}
