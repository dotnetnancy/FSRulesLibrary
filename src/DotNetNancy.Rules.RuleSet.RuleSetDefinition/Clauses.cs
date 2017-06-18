using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetNancy.Rules.RuleSet.Translation;
using System.Xml;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{
    public class Clauses : Dictionary<ClauseTypes,Clause>
    {
        XmlDocument _clausesDefinitionXml = null;

        public Clauses(XmlDocument sourceXml)
            : base()
        {
            _clausesDefinitionXml = sourceXml;
            foreach (XmlNode node in _clausesDefinitionXml.DocumentElement.SelectSingleNode(Translation.Constants.XmlRuleElementConstants.CLAUSE_PATH).ChildNodes)
            {
                Clause clause = new Clause
                {
                    Name = node.Attributes[Translation.Constants.XmlRuleElementConstants.NAME].Value,
                    Value = node.Attributes[Translation.Constants.XmlRuleElementConstants.VALUE].Value,
                    RuleElementType = RuleElementTypes.Clause
                };

                ClauseTypes clauseType = Translation.TranslationHelper.ClauseTypeTranslation(clause.Value);

                if (!this.ContainsKey(clauseType))
                {                  
                    this.Add(clauseType, clause);
                }
                else
                {
                    this[clauseType] = clause;
                }
            }
        }
    }

}
