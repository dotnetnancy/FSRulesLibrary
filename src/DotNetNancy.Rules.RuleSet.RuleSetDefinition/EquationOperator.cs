using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DotNetNancy.Rules.RuleSet.Translation;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{
    public class EquationOperator
    {
        XmlNode _equationOperator = null;

        public SetTypes DataType { get; set; }
        public OperatorTypes Operator { get; set; }
        public int Number { get; set; }

        Operator MetaDataOperator { get; set; }


        public XmlNode EquationOperatorProperty
        {
            get { return _equationOperator; }
            set { _equationOperator = value; }
        }

        public EquationOperator()
        {
        }

        public EquationOperator(XmlNode equationOperator, RuleSetMetaDataDefinition ruleSetMetaDataDefinition)
        {
            _equationOperator = equationOperator;

            DataType = 
                Translation.TranslationHelper.SetTypeTranslation(equationOperator.Attributes[Constants.XmlRuleSetDefinitionConstants.DATA_TYPE].Value);
            
            Operator = 
                Translation.TranslationHelper.OperatorTypeTranslation(equationOperator.Attributes[Constants.XmlRuleSetDefinitionConstants.TYPE].Value);

            Number = 
                Convert.ToInt32(equationOperator.Attributes["number"].Value);


            LoadOperatorMetaData(ruleSetMetaDataDefinition);


        }

        private void LoadOperatorMetaData(RuleSetMetaDataDefinition ruleSetMetaDataDefinition)
        {
            foreach (Operator operatorVar in ruleSetMetaDataDefinition.RuleOperatorsProperty.RuleOperatorsDictionary[DataType].Operators)
            {
                if (Translation.TranslationHelper.OperatorValueTranslation(Operator) == operatorVar.Value)
                {
                    //found my metadata so load it then break
                    MetaDataOperator = operatorVar;
                    break;
                }
            }
        }
    }
}
