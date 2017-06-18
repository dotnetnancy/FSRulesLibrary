using System;
using System.Collections.Generic;
using System.Xml;
using DotNetNancy.Rules.RuleSet.Translation;
using System.Runtime.Serialization;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{
	[DataContract]
	public class Clause : IStackable
	{
		public string Name { get; set; }
		public string Value { get; set; }
		public RuleElementTypes RuleElementType { get; set; }      
        public ClauseTypes ClauseType { get; set; }
        public int Number { get; set; }

        public Clause(XmlNode node): this()
        {
            ClauseType = Translation.TranslationHelper.ClauseTypeTranslation(node.Attributes["type"].Value);
            Number = Convert.ToInt32(node.Attributes["number"].Value);
           
        }
		public Clause()
		{
            RuleElementType = RuleElementTypes.Clause;
		}
	}
}