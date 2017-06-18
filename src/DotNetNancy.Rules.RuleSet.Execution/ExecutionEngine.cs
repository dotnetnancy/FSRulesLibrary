using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetNancy.Rules.RuleSet.MSRuleSetTranslation;

namespace DotNetNancy.Rules.RuleSet.MSRuleSetExecution
{
    public  static class ExecutionEngine<T>
    {
        public static MSRuleSetEvaluationResult
            Evaluate(MSRuleSetTranslationResult ruleSetTranslationResult, T instanceOfObject)
        {
            Evaluator<T> evaluator = new Evaluator<T>();
            return evaluator.Evaluate(ruleSetTranslationResult, instanceOfObject);
        }

        public static MSRuleSetEvaluationResult Evaluate(System.Workflow.Activities.Rules.RuleSet ruleSet, T ruleSetPnr)
        {
            Evaluator<T> evaluator = new Evaluator<T>();
            return evaluator.Evaluate(ruleSet, ruleSetPnr);
        }

        public static MSRuleSetExecutionResult
           Execute(MSRuleSetTranslationResult ruleSetTranslationResult, T instanceOfObject, bool removeExceptionCausingRules)
        {
            Executor<T> executor = new Executor<T>();
            if (removeExceptionCausingRules)
            {
                RemoveExceptionCausingRules(ruleSetTranslationResult, instanceOfObject);
            }
            return executor.Execute(ruleSetTranslationResult, instanceOfObject);
        }

        private static void RemoveExceptionCausingRules(MSRuleSetTranslationResult ruleSetTranslationResult, T instanceOfObject)
        {
            Evaluator < T > evaluator = new Evaluator<T>();

            MSRuleSetEvaluationResult evaluationResult = 
             evaluator.Evaluate(ruleSetTranslationResult, instanceOfObject);

            ruleSetTranslationResult.RemoveExceptionCausingRules(evaluationResult.EvaluationErrors.Keys.ToList());
        }

        public static MSRuleSetExecutionResult Execute(System.Workflow.Activities.Rules.RuleSet ruleSet, T instanceOfObject)
        {
            Executor<T> executor = new Executor<T>();
            return executor.Execute(ruleSet, instanceOfObject);
        }
    }
}
