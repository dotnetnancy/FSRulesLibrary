using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DotNetNancy.Rules.RuleSet.Translation;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{
    public class EquationValue
    {
        public SetTypes DataType { get; set; }
        public int Number { get; set; }
        public object Value { get; set; }

        XmlNode _valueNode = null;

        public XmlNode ValueNode
        {
            get { return _valueNode; }
            set { _valueNode = value; }
        }

        public EquationValue()
        {
        }

        public EquationValue(XmlNode valueNode)
        {
            _valueNode = valueNode;          

            DataType =
                Translation.TranslationHelper.SetTypeTranslation(valueNode.Attributes[Constants.XmlRuleSetDefinitionConstants.DATA_TYPE].Value);

            Value =
                valueNode.InnerText;

            Number = 
                Convert.ToInt32(valueNode.Attributes["number"].Value);

        }
    }
}
