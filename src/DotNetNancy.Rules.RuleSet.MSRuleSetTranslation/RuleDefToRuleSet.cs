using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Activities.Rules;
using DotNetNancy.Rules.RuleSet.RuleSetDefinition;

namespace DotNetNancy.Rules.RuleSet.MSRuleSetTranslation
{
    public static class RuleDefToRuleSet<T>
    {
        public static MSRuleSetTranslationResult
            Translate(RuleSetDefinition.RuleSetDefinition ruleSetDefinition)
        {
            Translator<T> translator = new Translator<T>();
            return translator.TranslateAndAssignCondition(ruleSetDefinition);
        }

        public static MSRuleSetTranslationResult Translate(RuleSetDefinitions ruleSetDefinitions)
        {
            Translator<T> translator = new Translator<T>();
            return translator.TranslateAndAssignCondition(ruleSetDefinitions);
        }
    }
}
