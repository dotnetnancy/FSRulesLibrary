using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetNancy.Rules.RuleSet.DataAccess;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{
    public class RuleSetDefinitions : List<RuleSetDefinition>
    {
        List<DotNetNancy.Rules.RuleSet.DataAccess.DTO.RuleDefinition> _ruleSetDefinitionsDtos = null;
        RuleSetMetaDataDefinition _ruleSetMetaDataDefinition = null;            
        List<string> _fieldsUsedInRules = new List<string>();

        string _ruleSetName = "RuleSetNotExplicitlyNamed";

        public string RuleSetName
        {
            get { return _ruleSetName; }
            set { _ruleSetName = value; }
        }

        public List<string> FieldsUsedInRules
        {
            get { return _fieldsUsedInRules; }
        }

        public RuleSetDefinitions(RuleSetMetaDataDefinition ruleSetMetaDataDefinition)
        {
            _ruleSetMetaDataDefinition = ruleSetMetaDataDefinition;
        }
      

        public void LoadByExistingDefinitions(List<RuleSetDefinition> listOfExistingRuleSetDefinitions)
        {
            Load(listOfExistingRuleSetDefinitions);
        }   
      
        public void LoadAll(Guid applicationID, Guid typeID)
        {
            foreach (RuleSetDefinitionsProvider provider in DataAccess.InitRuleSetDefinitionsProvider.Providers)
            {
                if (provider.IsCurrentBusinessObjectProvider)
                {
                    Load(provider.GetRuleDefinitions(applicationID,typeID));
                    break;
                }
            }
        }  

        private void Load(DotNetNancy.Rules.RuleSet.DataAccess.DTO.RuleDefinition ruleSetDefinitionDto)
        {            
            _ruleSetDefinitionsDtos = new List<DotNetNancy.Rules.RuleSet.DataAccess.DTO.RuleDefinition>();
            _ruleSetDefinitionsDtos.Add(ruleSetDefinitionDto);
            this.Clear();
            RuleSetDefinition rule = new RuleSetDefinition(ruleSetDefinitionDto, _ruleSetMetaDataDefinition);

			this.Add(rule);
			foreach (var fieldName in rule.FieldsUsed)
			{
				if (!this._fieldsUsedInRules.Contains(fieldName))
				{
					this._fieldsUsedInRules.Add(fieldName);
				}
			}
        }

        private void Load(List<DotNetNancy.Rules.RuleSet.DataAccess.DTO.RuleDefinition> ruleSetDefinitionsDtos)
        {
            _ruleSetDefinitionsDtos = ruleSetDefinitionsDtos;
            this.Clear();

			foreach (DotNetNancy.Rules.RuleSet.DataAccess.DTO.RuleDefinition ruleSetDefintionDto in ruleSetDefinitionsDtos)
			{
				RuleSetDefinition rule = new RuleSetDefinition(ruleSetDefintionDto, _ruleSetMetaDataDefinition);
				this.Add(rule);
				foreach (var fieldName in rule.FieldsUsed)
				{
					if (!this._fieldsUsedInRules.Contains(fieldName))
					{
						this._fieldsUsedInRules.Add(fieldName);
					}
				} 
			}

        }

        private void Load(List<RuleSetDefinition> listOfExistingRuleSetDefinitions)
        {
            _ruleSetDefinitionsDtos = new List<DotNetNancy.Rules.RuleSet.DataAccess.DTO.RuleDefinition>();
            this.Clear();

            foreach (RuleSetDefinition ruleSetDefinition in listOfExistingRuleSetDefinitions)
            {
                _ruleSetDefinitionsDtos.Add(ruleSetDefinition.Dto);
                this.Add(ruleSetDefinition);
				foreach (var fieldName in ruleSetDefinition.FieldsUsed)
				{
					if (!this._fieldsUsedInRules.Contains(fieldName))
					{
						this._fieldsUsedInRules.Add(fieldName);
					}
				}
            }
        }

       
    }
}
