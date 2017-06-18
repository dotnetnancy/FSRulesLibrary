using System;
using System.Collections.Generic;
using System.Xml;
using DotNetNancy.Rules.RuleSet.Translation;
using System.Runtime.Serialization;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{
	[DataContract]
	public class Action : IStackable
	{
		public string MethodName { get; set; }
		public string DisplayName { get; set; }
        public int Number { get; set; }
		public RuleElementTypes Type { get; set; }
        public string CustomFieldName { get; set; }
        public string CustomFieldValue { get; set; }

        XmlNode _actionNode;

        public XmlNode ActionNode
        {
            get { return _actionNode; }
            set { _actionNode = value; }
        }

		public Action()
		{
            Type = RuleElementTypes.Action;
		}

        public Action(XmlNode actionNode)
            : this()
        {
            _actionNode = actionNode;
            MethodName = actionNode.Attributes[Constants.XmlRuleElementConstants.METHOD_NAME].Value;
            Number = Convert.ToInt32(actionNode.Attributes[Constants.XmlRuleElementConstants.NUMBER].Value);
            AssignCustomFieldNameAndValueIfExists();
        }

        private void AssignCustomFieldNameAndValueIfExists()
        {
            XmlElement element = _actionNode as XmlElement;

            if (element.HasAttribute(Constants.XmlRuleElementConstants.CUSTOM_FIELD_NAME))
            {
                CustomFieldName = element.Attributes[Constants.XmlRuleElementConstants.CUSTOM_FIELD_NAME].Value;
            }
            if (element.HasAttribute(Constants.XmlRuleElementConstants.CUSTOM_FIELD_VALUE))
            {
                CustomFieldValue = element.Attributes[Constants.XmlRuleElementConstants.CUSTOM_FIELD_VALUE].Value;
            }
        }
	}
}