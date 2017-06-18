using System;
using System.Collections.Generic;
using System.Xml;
using DotNetNancy.Rules.RuleSet.Translation;
using System.Runtime.Serialization;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{
	[DataContract]
	public class Field
	{
		public string PropertyName { get; set; }
		public string DisplayName { get; set; }
        public int? MaxLength { get; set; }
        public string DataType { get; set; }
		public RuleElementTypes Type { get; set; }
        XmlNode _fieldNode = null;
        public InvocationTypes InvocationType { get; set; }

        public XmlNode FieldNode
        {
            get { return _fieldNode; }
            set { _fieldNode = value; }
        }

        public Field()
        {
            Type = RuleElementTypes.Field;
            InvocationType = InvocationTypes.AsProperty;
        }

		public Field(XmlNode fieldNode):this()
		{
            _fieldNode = fieldNode;

		}
	}
}