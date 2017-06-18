using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{
    public class Equation : IStackable
    {
        public EquationField FieldProperty { get; set; }
        public EquationOperator OperatorProperty { get; set; }
        public EquationValue ValueProperty { get; set; }

        RuleSetMetaDataDefinition _ruleSetMetaDataDefinition = null;

        public Equation()
        {
        }

        public Equation(XmlNode fieldNode, XmlNode operatorNode, XmlNode valueNode, RuleSetMetaDataDefinition ruleSetMetaDataDefinition)
        {
            _ruleSetMetaDataDefinition = ruleSetMetaDataDefinition;

            FieldProperty = new EquationField(fieldNode, _ruleSetMetaDataDefinition);
            OperatorProperty = new EquationOperator(operatorNode, _ruleSetMetaDataDefinition);
            ValueProperty = new EquationValue(valueNode);
        }

        public Equation(XmlNode fieldNode, RuleSetMetaDataDefinition ruleSetMetaDataDefinition)
        {
            _ruleSetMetaDataDefinition = ruleSetMetaDataDefinition;

            FieldProperty = new EquationField(fieldNode, _ruleSetMetaDataDefinition);
            
            XmlNode equationOperatorNode = fieldNode.NextSibling;

            OperatorProperty = new EquationOperator(equationOperatorNode, _ruleSetMetaDataDefinition);

            XmlNode valueNode = equationOperatorNode.NextSibling;

            ValueProperty = new EquationValue(valueNode);

        }      
    }
}
