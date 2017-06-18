using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetNancy.Rules.RuleSet.MSRuleSetTranslation;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.Activities.Rules;
using System.Runtime.Serialization;

namespace DotNetNancy.Rules.RuleSet.MSRuleSetExecution
{
    [DataContract]
    public class MSRuleSetExecutionResult
    {
        private log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        MSRuleSetTranslationResult _ruleSetTranslationResult = null;
        Dictionary<Guid, ValidationErrorCollection> _validationErrors = new Dictionary<Guid, ValidationErrorCollection>();
        Dictionary<string, RuleExecution> _evaluatedRules = new Dictionary<string, RuleExecution>();
        ValidationErrorCollection _ruleSetLevelValidationErrors = new ValidationErrorCollection();

        Dictionary<Guid, Exception> _evaluationErrors = new Dictionary<Guid, Exception>();

        public Dictionary<Guid, Exception> ExecutionErrors
        {
            get { return _evaluationErrors; }
            set { _evaluationErrors = value; }
        }

        Dictionary<string, Exception> _nonTranslatedRuleExecutionErrors = new Dictionary<string, Exception>();

        public Dictionary<string, Exception> NonTranslatedRuleExecutionErrors
        {
            get { return _nonTranslatedRuleExecutionErrors; }
            set { _nonTranslatedRuleExecutionErrors = value; }
        }

        bool _translatedRuleSet = false;
        Dictionary<string, ValidationErrorCollection> _nonTranslatedRulesValidationErrors = new Dictionary<string, ValidationErrorCollection>();
        Dictionary<string, RuleExecution> _nonTranslatedRulesExecutedRules = new Dictionary<string, RuleExecution>();
        Dictionary<Guid, string> _inactiveRules = new Dictionary<Guid, string>();
        List<string> _nonTranslatedRulesInactiveRulesList = new List<string>();

        public Dictionary<Guid, string> InactiveRules
        {
            get { return _inactiveRules; }
        }

        public List<string> NonTranslatedRulesInactiveRulesList
        {
            get { return _nonTranslatedRulesInactiveRulesList; }
        }

        public Dictionary<string, ValidationErrorCollection> NonTranslatedRulesValidationErrors
        {
            get { return _nonTranslatedRulesValidationErrors; }
        }

        public Dictionary<string, RuleExecution> NonTranslatedRulesExecutedRules
        {
            get { return _nonTranslatedRulesExecutedRules; }
        }

        public Dictionary<string, RuleExecution> ExecutedRules
        {
            get { return _evaluatedRules; }
        }

        public Dictionary<Guid, ValidationErrorCollection> ValidationErrors
        {
            get { return _validationErrors; }            
        }

        public MSRuleSetExecutionResult()
        {
        }

        public MSRuleSetExecutionResult(MSRuleSetTranslationResult ruleSetTranslationResult)
        {
            _translatedRuleSet = true;
            _ruleSetTranslationResult = ruleSetTranslationResult;
            RemoveInactiveRulesFromRuleSet();
        }

        private void RemoveInactiveRulesFromRuleSet()
        {
            Dictionary<Guid,RuleSetDefinition.RuleSetDefinition> inactiveRules = _ruleSetTranslationResult.RemoveInactiveRules();
            AddRulesToInactive(inactiveRules);
        }

        private void AddRulesToInactive(Dictionary<Guid, DotNetNancy.Rules.RuleSet.RuleSetDefinition.RuleSetDefinition> inactiveRules)
        {
            foreach (KeyValuePair<Guid, RuleSetDefinition.RuleSetDefinition> kvp in inactiveRules)
            {
                AddRuleToInactive(kvp.Key.ToString(), kvp.Value.RuleName);
            }
        }

        public void AddValidationError(string ruleGuidString, string ruleName, ValidationErrorCollection errors)
        {
            if (_translatedRuleSet)
            {
                Guid ruleId = new Guid(ruleGuidString);
                _validationErrors.Add(ruleId, errors);
            }
            else
            {
                _nonTranslatedRulesValidationErrors.Add(ruleName, errors);
            }
        }


        public void AddExecutionResult(string ruleName, RuleExecution result)
        {
            if (_translatedRuleSet)
            {

                _evaluatedRules.Add(ruleName, result);

            }
            else
            {
                _nonTranslatedRulesExecutedRules.Add(ruleName, result);
            }
        }

        public void AddRuleSetValidationErrors(ValidationErrorCollection errors)
        {
            _ruleSetLevelValidationErrors = errors;
        }

        public void AddRuleToInactive(string ruleGuidString, string ruleName)
        {
            if (_translatedRuleSet)
            {
                Guid ruleId = new Guid(ruleGuidString);
                _inactiveRules.Add(ruleId, ruleName);
            }
            else
            {
                _nonTranslatedRulesInactiveRulesList.Add(ruleName);
            }
        }



        public DotNetNancy.Rules.RuleSet.RuleSetDefinition.Action[] GetThenActions(Guid guid, RuleExpressionResult ruleExpressionResult)
        {
            DotNetNancy.Rules.RuleSet.RuleSetDefinition.Action[] thenActions = null;

            if (_translatedRuleSet)
            {
                thenActions = _ruleSetTranslationResult.GetThenActionsByRuleID(guid);
            }
            else
            {
                
            }

            return thenActions;
        }

        public void AddExecutionError(string ruleGuidString, string ruleName, Exception rex)
        {
            if (_translatedRuleSet)
            {
                Guid ruleId = new Guid(ruleGuidString);
                _evaluationErrors.Add(ruleId, rex);
            }
            else
            {
                _nonTranslatedRuleExecutionErrors.Add(ruleName, rex);
            }
        }
    }
}
