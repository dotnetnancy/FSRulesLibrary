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
    public class MSRuleSetEvaluationResult
    {
        private log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        MSRuleSetTranslationResult _ruleSetTranslationResult = null;
        Dictionary<Guid, ValidationErrorCollection> _validationErrors = new Dictionary<Guid, ValidationErrorCollection>();
        Dictionary<Guid, RuleExpressionResult> _evaluatedRules = new Dictionary<Guid, RuleExpressionResult>();
        ValidationErrorCollection _ruleSetLevelValidationErrors = new ValidationErrorCollection();

        Dictionary<Guid, Exception> _evaluationErrors = new Dictionary<Guid, Exception>();

        public Dictionary<Guid, Exception> EvaluationErrors
        {
            get { return _evaluationErrors; }
            set { _evaluationErrors = value; }
        }

        Dictionary<string, Exception> _nonTranslatedRuleEvaluationErrors = new Dictionary<string, Exception>();

        public Dictionary<string, Exception> NonTranslatedRuleEvaluationErrors
        {
            get { return _nonTranslatedRuleEvaluationErrors; }
            set { _nonTranslatedRuleEvaluationErrors = value; }
        }

        bool _translatedRuleSet = false;
        Dictionary<string, ValidationErrorCollection> _nonTranslatedRulesValidationErrors = new Dictionary<string, ValidationErrorCollection>();
        Dictionary<string, RuleExpressionResult> _nonTranslatedRulesEvaluatedRules = new Dictionary<string, RuleExpressionResult>();
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

        public Dictionary<string, RuleExpressionResult> NonTranslatedRulesEvaluatedRules
        {
            get { return _nonTranslatedRulesEvaluatedRules; }
        }

        public Dictionary<Guid, RuleExpressionResult> EvaluatedRules
        {
            get { return _evaluatedRules; }
        }

        public Dictionary<Guid, ValidationErrorCollection> ValidationErrors
        {
            get { return _validationErrors; }            
        }

        public MSRuleSetEvaluationResult()
        {
        }

        public MSRuleSetEvaluationResult(MSRuleSetTranslationResult ruleSetTranslationResult)
        {
            _translatedRuleSet = true;
            _ruleSetTranslationResult = ruleSetTranslationResult;
        }

        public void AddValidationError(string ruleIdGuidString, string ruleName,ValidationErrorCollection errors)
        {
            if (_translatedRuleSet)
            {
                Guid ruleId = new Guid(ruleIdGuidString);
                _validationErrors.Add(ruleId, errors);
            }
            else
            {
                _nonTranslatedRulesValidationErrors.Add(ruleName, errors);
            }
        }


        public void AddEvaluationResult(string ruleIdGuidString, string ruleName, RuleExpressionResult result)
        {
            if (_translatedRuleSet)
            {
                Guid ruleId = new Guid(ruleIdGuidString);
                _evaluatedRules.Add(ruleId, result);

            }
            else
            {
                _nonTranslatedRulesEvaluatedRules.Add(ruleName, result);
            }
        }

        public void AddRuleSetValidationErrors(ValidationErrorCollection errors)
        {
            _ruleSetLevelValidationErrors = errors;
        }

        public void AddRuleToInactive(string ruleIdGuidString, string ruleName)
        {
            if (_translatedRuleSet)
            {
                Guid ruleId = new Guid(ruleIdGuidString);
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

            return thenActions;
        }

        public void AddEvaluationError(string ruleGuidString, string ruleName, Exception rex)
        {
            if (_translatedRuleSet)
            {
                Guid ruleId = new Guid(ruleGuidString);
                _evaluationErrors.Add(ruleId, rex);
            }
            else
            {
                _nonTranslatedRuleEvaluationErrors.Add(ruleName, rex);
            }
        }
    }
}
