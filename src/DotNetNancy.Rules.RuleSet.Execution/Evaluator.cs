using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetNancy.Rules.RuleSet.MSRuleSetTranslation;
using System.Workflow.Activities.Rules;
using System.Workflow.ComponentModel.Compiler;
using System.CodeDom;

namespace DotNetNancy.Rules.RuleSet.MSRuleSetExecution
{
    public class Evaluator<T>
    {

        private log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //may or may not be needed but because it was already instantiated and the reflection has already been done i just pass
        //it around so that the reflection does not need to be done again if we need to inspect the object
       CodeDomObject _codeDomObject = null;
       MSRuleSetTranslationResult _msRuleSetTranslationResult = null;
       T _instanceOfObject;

        public Evaluator()
        {
            
        }       

        public MSRuleSetEvaluationResult Evaluate(MSRuleSetTranslationResult ruleSetTranslationResult, T instanceOfObject)
        {
            _msRuleSetTranslationResult = ruleSetTranslationResult;
            _instanceOfObject = instanceOfObject;
            _codeDomObject = _msRuleSetTranslationResult.ReferenceToCodeDomObject;

            return EvaluateRuleSet(ruleSetTranslationResult.DotNetRuleSet, true);
        }

        public MSRuleSetEvaluationResult EvaluateRuleSet(System.Workflow.Activities.Rules.RuleSet ruleSet, bool translatedRule)
        {
            MSRuleSetEvaluationResult ruleSetEvaluationResult = null;

            if (translatedRule)
            {
                ruleSetEvaluationResult = new MSRuleSetEvaluationResult(_msRuleSetTranslationResult);
            }
            else
            {
                ruleSetEvaluationResult = new MSRuleSetEvaluationResult();
            }

            RuleValidation rv = new RuleValidation(typeof(T), null);

            ruleSet.Validate(rv);

            ValidationErrorCollection errors = rv.Errors;

            if (errors.Count > 0)
            {
                //string validationErrorMessages = Helper.GetErrorMessageFromValidationErrorsCollection(errors);

                // if the rule set has errors at top level should we stop here or try each individual rule?  i think for now
                //we can continue to try each individual rule and then after evaluations report the errors in the return execution result
                //object

                ruleSetEvaluationResult.AddRuleSetValidationErrors(errors);
            }

            foreach (System.Workflow.Activities.Rules.Rule rule in ruleSet.Rules)
            {
                if (rule.Active)
                {
                    try
                    {
                        EvaluateRule(rule, ref ruleSetEvaluationResult, ref rv);
                    }
                    catch (RuleEvaluationException rex)
                    {
                        //_log.Error("Rule Name:  " + rule.Name + "threw a Rule Evaluation Exception during its evaluation.  ", rex);
                        //loop again cause if this one failed we still want to try to evaluate the other rules in this rule set
                        if (rex.InnerException != null)
                        {
                            ruleSetEvaluationResult.AddEvaluationError(rule.Name, rule.Description, (Exception)rex.InnerException);
                        }
                        else
                        {
                            ruleSetEvaluationResult.AddEvaluationError(rule.Name, rule.Description, (Exception)rex);

                        }
                        continue;
                    }
                    catch (Exception ex)
                    {
                        //_log.Error("Unhandled exception during evaluation of Rule Name:  " + rule.Name, ex);
                        ruleSetEvaluationResult.AddEvaluationError(rule.Name,rule.Description ,ex);
                        continue;
                    }
                }
                else
                {
                    ruleSetEvaluationResult.AddRuleToInactive(rule.Name,rule.Description);
                }
            }

            return ruleSetEvaluationResult;
        }

        private void EvaluateRule(Rule rule, ref MSRuleSetEvaluationResult ruleSetEvaluationResult, ref RuleValidation rv)
        {
            rv.Errors.Clear();

            IRuleExpression ruleExpression = (IRuleExpression)((RuleExpressionCondition)rule.Condition).Expression;
         
            RuleExecution re = new RuleExecution(rv, _instanceOfObject);
            RuleExpressionInfo info = RuleExpressionWalker.Validate(rv, (CodeExpression)ruleExpression, true);

            if (rv.Errors.Count > 0)
            {
                //string validationErrorMessages = Helper.GetErrorMessageFromValidationErrorsCollection(errors);

                ruleSetEvaluationResult.AddValidationError(rule.Name,rule.Description, rv.Errors);
            }
            else
            {

                RuleExpressionResult result = RuleExpressionWalker.Evaluate(re, (CodeExpression)ruleExpression);
                ruleSetEvaluationResult.AddEvaluationResult(rule.Name, rule.Description, result);
            }
        }

        internal MSRuleSetEvaluationResult Evaluate(System.Workflow.Activities.Rules.RuleSet ruleSet, T instanceOfObject)
        {
            _instanceOfObject = instanceOfObject;
            return EvaluateRuleSet(ruleSet,false);
        }
    }
}
