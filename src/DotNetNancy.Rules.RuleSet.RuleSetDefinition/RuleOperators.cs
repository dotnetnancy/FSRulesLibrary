using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DotNetNancy.Rules.RuleSet.DataAccess;
using System.Xml;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{
    public class RuleOperators
    {
        DataAccess.DTO.RuleOperators _ruleOperatorsDto = null;
        Operators _ruleOperatorsDictionary = null;

        public Operators RuleOperatorsDictionary
        {
            get { return _ruleOperatorsDictionary; }
            set { _ruleOperatorsDictionary = value; }
        }
        Clauses _ruleClausesDictionary = null;

        public Clauses RuleClausesDictionary
        {
            get { return _ruleClausesDictionary; }
            set { _ruleClausesDictionary = value; }
        }

        public RuleOperators(Guid applicationID, Guid typeID,
            DotNetNancy.Rules.RuleSet.DataAccess.Enumerations.ConfigurationTypes configurationType)
        {
            Load(applicationID, typeID,configurationType);
        }
        
        /// <summary>
        /// load this object
        /// </summary>
        public void Load(Guid applicationID, Guid typeID,
            DotNetNancy.Rules.RuleSet.DataAccess.Enumerations.ConfigurationTypes configurationType)
        {
            foreach (RuleOperatorsProvider provider in DataAccess.InitRuleOperatorsProvider.Providers)
            {
                if (provider.IsCurrentBusinessObjectProvider)
                {
                    Load(provider.GetData(applicationID,typeID,configurationType));
                    break;
                }
            }          
        }


        private void Load(DotNetNancy.Rules.RuleSet.DataAccess.DTO.RuleOperators ruleOperators)
        {
            _ruleOperatorsDto = ruleOperators;
            _ruleOperatorsDictionary = new Operators(ruleOperators.SourceXmlDocument);
            _ruleClausesDictionary = new Clauses(ruleOperators.SourceXmlDocument);
        }

        public XmlDocument GetSourceXml()
        {
            return _ruleOperatorsDto.SourceXmlDocument;
        }
        
    }
}
