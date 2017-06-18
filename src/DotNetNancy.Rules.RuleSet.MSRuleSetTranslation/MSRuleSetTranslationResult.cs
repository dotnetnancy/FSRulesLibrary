using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Activities.Rules;
using System.Runtime.Serialization;

namespace DotNetNancy.Rules.RuleSet.MSRuleSetTranslation
{
    [DataContract]
    public class MSRuleSetTranslationResult
    {
        System.Workflow.Activities.Rules.RuleSet _dotNetRuleSet;
        RuleSetDefinition.RuleSetDefinitions _ruleDefinitionsAfterTranslation;
        Dictionary<Guid,string> _ruleGuidToRuleName;
        CodeDomObject _referenceToCodeDomObject = null;
        Dictionary<Guid, DateTime> _ruleToLastUpdatedOrCreatedDateTime;
        Dictionary<Guid, RuleSetDefinition.RuleSetDefinition> _rulesRemovedFromRuleSetDueToException = 
            new Dictionary<Guid,DotNetNancy.Rules.RuleSet.RuleSetDefinition.RuleSetDefinition>();

		Dictionary<Guid, RuleSetDefinition.RuleSetDefinition> _rulesRemovedDueTranslationValidationError =
					new Dictionary<Guid, DotNetNancy.Rules.RuleSet.RuleSetDefinition.RuleSetDefinition>();

        Dictionary<Guid, TranslationValidationResult> _rulesTranslationErrors = null;

        public Dictionary<Guid, RuleSetDefinition.RuleSetDefinition> RulesRemovedFromRuleSetDueToException
        {
            get { return _rulesRemovedFromRuleSetDueToException; }
            set { _rulesRemovedFromRuleSetDueToException = value; }
        }

        public CodeDomObject ReferenceToCodeDomObject
        {
            get { return _referenceToCodeDomObject; }
            set { _referenceToCodeDomObject = value; }
        }

        public RuleSetDefinition.RuleSetDefinitions RuleDefinitionsAfterTranslation
        {
            get { return _ruleDefinitionsAfterTranslation; }
            set { _ruleDefinitionsAfterTranslation = value; }
        }


        public System.Workflow.Activities.Rules.RuleSet DotNetRuleSet
        {
            get { return _dotNetRuleSet; }
            set { _dotNetRuleSet = value; }
        }

		public Dictionary<Guid, RuleSetDefinition.RuleSetDefinition> RulesRemovedDueTranslationValidationErro
        {
			get { return _rulesRemovedDueTranslationValidationError; }
			set { _rulesRemovedDueTranslationValidationError = value; }
        }

		public Dictionary<Guid, TranslationValidationResult> RulesTranslationErrors
		{
			get { return _rulesTranslationErrors; }
			set { _rulesTranslationErrors = value; }
		}
		
        public MSRuleSetTranslationResult(System.Workflow.Activities.Rules.RuleSet dotNetRuleSet,
            RuleSetDefinition.RuleSetDefinitions ruleSetDefinitionsAfterTranslation, CodeDomObject codeDomObject,
			Dictionary<Guid, TranslationValidationResult> rulesTranslationErrors)
        {
            _dotNetRuleSet = dotNetRuleSet;
            _ruleDefinitionsAfterTranslation = ruleSetDefinitionsAfterTranslation;
            _referenceToCodeDomObject = codeDomObject;
			_rulesTranslationErrors = rulesTranslationErrors;

			LoadRuleGuidToRuleName();			
            LoadLastModifiedOrCreatedDictionary();
			RemoveRulesNotTranslatedToDotNetRuleSet(rulesTranslationErrors.Keys.ToList());
        }

		public MSRuleSetTranslationResult(CodeDomObject codeDomObject, Dictionary<Guid, TranslationValidationResult> rulesTranslationErrors)
        {
            _referenceToCodeDomObject = codeDomObject;
			_rulesTranslationErrors = rulesTranslationErrors;
        }

        private void LoadLastModifiedOrCreatedDictionary()
        {
            _ruleToLastUpdatedOrCreatedDateTime = new Dictionary<Guid, DateTime>();

            foreach (RuleSetDefinition.RuleSetDefinition ruleDefinition in _ruleDefinitionsAfterTranslation)
            {
                _ruleToLastUpdatedOrCreatedDateTime.Add(ruleDefinition.RuleID, ruleDefinition.DateCreated);
            }
        }

        public Dictionary<Guid,string> RuleSetNameToGuid
        {
            get
            {                
                return _ruleGuidToRuleName;
            }
        }

        private void LoadRuleGuidToRuleName()
        {
            _ruleGuidToRuleName = new Dictionary<Guid, string>();

            foreach (RuleSetDefinition.RuleSetDefinition ruleSetDefinition in _ruleDefinitionsAfterTranslation)
            {
                _ruleGuidToRuleName.Add(ruleSetDefinition.RuleID, ruleSetDefinition.RuleName);
            }
        }       


        public string GetRuleNameByRuleID(Guid guid)
        {
            if (_ruleGuidToRuleName.ContainsKey(guid))
            {
                return _ruleGuidToRuleName[guid];
            }

            return string.Empty;
        }
        
        public DotNetNancy.Rules.RuleSet.RuleSetDefinition.Action[] GetThenActionsByRuleID(Guid guid)
        {
            RuleSetDefinition.RuleSetDefinition ruleSetDefinitionFound = 
                _ruleDefinitionsAfterTranslation.Where(s => s.RuleID == guid).First();

            DotNetNancy.Rules.RuleSet.RuleSetDefinition.Action[] actions = new DotNetNancy.Rules.RuleSet.RuleSetDefinition.Action[ruleSetDefinitionFound.RuleThenActionsDefined.Count];

            for (int i = 0; i < ruleSetDefinitionFound.RuleThenActionsDefined.Count; i++)
            {
                actions[i] = ruleSetDefinitionFound.RuleThenActionsDefined[i + 1];
            }

            return actions;
        }

        public DateTime GetMaxTranslatedDate()
        {
            //return the max date for the rules that were translated
            DateTime maxDate = _ruleToLastUpdatedOrCreatedDateTime.Values.Max();
            return maxDate;
        }

		public void RemoveRulesNotTranslatedToDotNetRuleSet(List<Guid> list)
		{
			foreach (Guid ruleID in list)
			{
				DotNetNancy.Rules.RuleSet.RuleSetDefinition.RuleSetDefinition def =
					_ruleDefinitionsAfterTranslation.Where(s => s.RuleID == ruleID).First();

				_ruleDefinitionsAfterTranslation.Remove(def); //Remove rules not translated to dotNet rules
				_rulesRemovedDueTranslationValidationError.Add(ruleID, def);
			}
		}
		
        public void RemoveExceptionCausingRules(List<Guid> list)
        {
        	if (_ruleDefinitionsAfterTranslation.Count > 0)
        	{
        	foreach (Guid ruleID in list)
        	{

        		DotNetNancy.Rules.RuleSet.RuleSetDefinition.RuleSetDefinition def =
        			_ruleDefinitionsAfterTranslation.Where(s => s.RuleID == ruleID).First();

        		if (def != null)
        		{
        			this._rulesRemovedFromRuleSetDueToException.Add(ruleID, def);

        			this._ruleDefinitionsAfterTranslation.Remove(def);
        		}
        		this._ruleToLastUpdatedOrCreatedDateTime.Remove(ruleID);

        		Rule rule = this.DotNetRuleSet.Rules.Where(s => s.Name == ruleID.ToString()).First();

        		if (rule != null)
        		{
        			this.DotNetRuleSet.Rules.Remove(rule);
        		}
        	}
        }
    }

        public Dictionary<Guid, DotNetNancy.Rules.RuleSet.RuleSetDefinition.RuleSetDefinition> RemoveInactiveRules()
        {
            Dictionary<Guid,RuleSetDefinition.RuleSetDefinition> pausedRules = 
                new Dictionary<Guid,DotNetNancy.Rules.RuleSet.RuleSetDefinition.RuleSetDefinition>();

            foreach (RuleSetDefinition.RuleSetDefinition def in _ruleDefinitionsAfterTranslation)
            {
                if (def.Paused)
                {
                    pausedRules.Add(def.RuleID, def);                    
                }
            }

            RemoveInactiveRules(pausedRules);

            return pausedRules;
        }

        private void RemoveInactiveRules(Dictionary<Guid, DotNetNancy.Rules.RuleSet.RuleSetDefinition.RuleSetDefinition> pausedRules)
        {
            foreach (KeyValuePair<Guid, RuleSetDefinition.RuleSetDefinition> kvp in pausedRules)
            {
                this._ruleDefinitionsAfterTranslation.Remove(kvp.Value);

                this._ruleGuidToRuleName.Remove(kvp.Key);

                this._ruleToLastUpdatedOrCreatedDateTime.Remove(kvp.Key);

                Rule rule = this.DotNetRuleSet.Rules.Where(s => s.Name == kvp.Key.ToString()).First();

                this.DotNetRuleSet.Rules.Remove(rule);
            }
        }
    }
}
