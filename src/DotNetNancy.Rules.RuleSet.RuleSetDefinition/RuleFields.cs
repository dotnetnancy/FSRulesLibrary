using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DotNetNancy.Rules.RuleSet.DataAccess;
using System.Xml;
using DotNetNancy.Rules.RuleSet.Translation;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{
    public class RuleFields
    {
        DataAccess.DTO.RuleFields _ruleFieldsDto = null;
        Dictionary<SetTypes, List<Field>> _ruleFieldsDictionary = null;

        public Dictionary<SetTypes, List<Field>> RuleFieldsDictionary
        {
            get { return _ruleFieldsDictionary; }
            set { _ruleFieldsDictionary = value; }
        }

        Dictionary<string, Action> _ruleActionDictionary = null;

        public RuleFields(Guid applicationID, Guid typeID,
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
            foreach (RuleFieldsProvider provider in DataAccess.InitRuleFieldsProvider.Providers)
            {
                if (provider.IsCurrentBusinessObjectProvider)
                {
                    Load(provider.GetData(applicationID, typeID,configurationType));
                    break;
                }
            }

        }

        private void Load(DotNetNancy.Rules.RuleSet.DataAccess.DTO.RuleFields ruleOperators)
        {
            _ruleFieldsDto = ruleOperators;
            _ruleFieldsDictionary = new Fields(_ruleFieldsDto.SourceXmlDocument);
            _ruleActionDictionary = new Actions(_ruleFieldsDto.SourceXmlDocument);           
        }

        public XmlDocument GetSourceXml()
        {
            return _ruleFieldsDto.SourceXmlDocument;
        }

    }
}
