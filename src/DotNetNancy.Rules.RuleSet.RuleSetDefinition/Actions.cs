using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{
    public class Actions : Dictionary<string,Action>
    {
        XmlDocument _definedActionsXml = null;

        public Actions(XmlDocument sourceXml)
            : base()
        {
            _definedActionsXml = sourceXml;

            foreach (XmlNode node in _definedActionsXml.DocumentElement.SelectSingleNode(Translation.Constants.XmlRuleElementConstants.ACTION_PATH).ChildNodes)
            {
                Action action = new Action
                {
                    DisplayName = node.Attributes[Translation.Constants.XmlRuleElementConstants.DISPLAY_NAME].Value,
                    MethodName = node.Attributes[Translation.Constants.XmlRuleElementConstants.METHOD_NAME].Value,
                    Type = RuleElementTypes.Action
                };

                 //if there is more than one with the same method name there is a problem... but we just take the one lower in the list
                if (this.ContainsKey(action.MethodName))
                {
                    this[action.MethodName] = action;
                }
                else
                {
                    this.Add(action.MethodName, action);
                }
            }
            
           


        }
    }
}
