using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DotNetNancy.Rules.RuleSet.Translation;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{
    public class Operators : Dictionary<SetTypes,RuleOperatorSet>
    {
        XmlDocument _operatorsDefinitionXml = null;

        public Operators(XmlDocument sourceXml)
            : base()
        {
            _operatorsDefinitionXml = sourceXml;
            foreach (XmlNode node in _operatorsDefinitionXml.DocumentElement.SelectSingleNode(Translation.Constants.XmlRuleElementConstants.OPERATOR_PATH).ChildNodes)
            {
                Operator op = new Operator
                    {
                        Name = node.Attributes[Translation.Constants.XmlRuleElementConstants.NAME].Value,
                        Value = node.Attributes[Translation.Constants.XmlRuleElementConstants.VALUE].Value,
                        OperatorSet = node.Attributes[Translation.Constants.XmlRuleElementConstants.SET].Value,
                        Type = RuleElementTypes.Operator
                    };

                SetTypes setType = Translation.TranslationHelper.SetTypeTranslation(op.OperatorSet);

                if (!this.ContainsKey(setType))
                {
                    RuleOperatorSet ruleOperatorSet = new RuleOperatorSet();
                    ruleOperatorSet.Set = op.Value;
                    ruleOperatorSet.Operators.Add(op);

                    this.Add(setType, ruleOperatorSet);
                }
                else
                {
                    this[setType].Operators.Add(op);
                }
            }
        }
    }
}

