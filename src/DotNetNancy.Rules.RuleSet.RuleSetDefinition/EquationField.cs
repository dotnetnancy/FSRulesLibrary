using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DotNetNancy.Rules.RuleSet.Translation;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{
    public class EquationField
    {
        public string PropertyName {get;set;}
        public SetTypes DataType { get; set; }
        public int Number { get; set; }

        public Field MetaDataField { get; set; }
    

        XmlNode _equationField = null;

        public XmlNode EquationFieldProperty
        {
            get { return _equationField; }
            set { _equationField = value; }
        }

        public EquationField()
        {
        }

        public EquationField(XmlNode field, RuleSetMetaDataDefinition ruleSetMetaDataDefinition)
        {
            _equationField = field;

            DataType = 
                Translation.TranslationHelper.SetTypeTranslation(field.Attributes[Constants.XmlRuleSetDefinitionConstants.DATA_TYPE].Value);
            PropertyName =
                field.Attributes[Constants.XmlRuleSetDefinitionConstants.PROPERTY_NAME].Value;
            Number = 
                Convert.ToInt32(field.Attributes["number"].Value);

            LoadFieldMetaData(ruleSetMetaDataDefinition);
        }

        private void LoadFieldMetaData(RuleSetMetaDataDefinition ruleSetMetaDataDefinition)
        {
            foreach (Field field in ruleSetMetaDataDefinition.RuleFieldsProperty.RuleFieldsDictionary[DataType])
            {
                if (field.PropertyName == PropertyName)
                {
                    //found my metadata so load it then break
                    MetaDataField = field;
                    ProcessInvocationMethodOverrideIfSpecified();
                    break;
                }
            }
        }

        private void ProcessInvocationMethodOverrideIfSpecified()
        {
            XmlElement element = _equationField as XmlElement;

            if (element.HasAttribute(Constants.XmlRuleElementConstants.INVOCATION_TYPE))
            {
                //then the ui has "overriden" the default behavior of the invocation type as specified by the rulefields.config
                //this could only happen if this was a member of a collection field and can only override the dynamic invocation type
                //not any other method invocation types - does not apply to a "methodCallWithParameter..." for example
                MetaDataField.InvocationType = 
                    TranslationHelper.InvocationTypesTranslation(element.Attributes[Constants.XmlRuleElementConstants.INVOCATION_TYPE].Value);
            }
        }
    }
}
