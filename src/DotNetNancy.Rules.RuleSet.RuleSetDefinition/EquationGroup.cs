using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using DotNetNancy.Rules.RuleSet.Translation;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{
    public class EquationGroup : IStackable
    {
        private Stack _conditionEquationGroupStack = new Stack();
        private Stack _actionStack = new Stack();
        private RuleSetMetaDataDefinition _ruleSetMetaDataDefinition = null;

        private string _memberOfCollectionString = null;
        private List<string> _fields = null;

        public string MemberOfCollectionString
        {
            get { return _memberOfCollectionString; }
            set { _memberOfCollectionString = value; }
        }

        //we make this the default and is only used at the equation group level if this is a square bracket equation group
        private InvocationTypes _invocationType = InvocationTypes.AsAnyOneDynamic;

        public InvocationTypes InvocationType
        {
            get { return _invocationType; }
            set { _invocationType = value; }
        }

        //before these items start getting popped off we should have a copy in case, since we want to reuse this object in memory
        //for more than one translation, in order to not have to do the recursion again over the xml document etc we just preserve them
        //before popping them off of the stack to "reload" this object after a translation which pops the items off of the stack

        IStackable[] _actionItemsCopiedToArray = null;
        IStackable[] _conditionItemsCopiedToArray = null;

        bool _isSquareBracketGrouping = false;

        public bool IsSquareBracketGrouping
        {
            get { return _isSquareBracketGrouping; }
            set { _isSquareBracketGrouping = value; }
        }

        public IStackable[] LIFOConditionItemsCopyOfOriginalStack
        {
            get { return _conditionItemsCopiedToArray; }
            set { _conditionItemsCopiedToArray = value; }
        }

        public IStackable[] LIFOActionItemsCopyOfOriginalStack
        {
            get { return _actionItemsCopiedToArray; }
            set { _actionItemsCopiedToArray = value; }
        }      


        public Stack ActionStackableItemsStack
        {
            get { return _actionStack; }
            set { _actionStack = value; }
        }
        public int Priority { get; set; }

        public Stack ConditionStackableItemsStack
        {
          get { return _conditionEquationGroupStack; }
          set { _conditionEquationGroupStack = value; }
        }

        public EquationGroup(ref List<string> fieldsUsedInRule)
        {
            _fields = fieldsUsedInRule;
        }

        public EquationGroup(RuleSetMetaDataDefinition ruleSetMetaDataDefinition, ref List<string> fieldsUsedInRule)
        {
            _ruleSetMetaDataDefinition = ruleSetMetaDataDefinition;
            _fields = fieldsUsedInRule;
        }

        public EquationGroup(XmlDocument doc, RuleSetMetaDataDefinition ruleSetMetaDataDefinition, ref List<string> fieldsUsedInRule)
        {
            _ruleSetMetaDataDefinition = ruleSetMetaDataDefinition;
            _fields = fieldsUsedInRule;
            LoadBottomUp(doc);
			fieldsUsedInRule = _fields;         
        }

        internal EquationGroup(XmlNode node, RuleSetMetaDataDefinition ruleSetMetaDataDefinition, ref List<string> fieldsUsedInRule)
        {
            _ruleSetMetaDataDefinition = ruleSetMetaDataDefinition;
            _fields = fieldsUsedInRule;
            Priority = GetPriority(node);
            LoadBottomUpByReferenceNode(node);
			fieldsUsedInRule = _fields;         
        }

        public void CopyStack()
        {
            _conditionItemsCopiedToArray = new IStackable[this.ConditionStackableItemsStack.Count];
            this.ConditionStackableItemsStack.CopyTo(_conditionItemsCopiedToArray, 0);

            _actionItemsCopiedToArray = new IStackable[this.ActionStackableItemsStack.Count];
           this.ActionStackableItemsStack.CopyTo(_actionItemsCopiedToArray, 0);

           foreach (IStackable stackable in this.ConditionStackableItemsStack)
           {
               if (stackable is EquationGroup)
               {
                   ((EquationGroup)stackable).CopyStack();
               }
           }

        }

        public void EquationGroupLoad(XmlNode parenthesesNode)
        {
            XmlNode referenceNode = parenthesesNode.LastChild;
            WalkUpFromLastChild(referenceNode);
            
        }      

        /// <summary>
        /// this is the entire rule set definition therefore we are going to number this EquationGroup as "0" or Top Equation Group aka Master EquationGroup
        /// </summary>
        /// <param name="doc"></param>
        public void LoadBottomUp(XmlDocument doc)
        {
            XmlNode rule = doc.SelectSingleNode("DotNetNancy/rule");

            Priority = GetPriority(rule);

            XmlNode node = rule.LastChild;

            WalkUpFromLastChild(node);           
        }

        public void LoadBottomUpByReferenceNode(XmlNode referenceNode)
        {
            XmlNode node = referenceNode.LastChild;
            WalkUpFromLastChild(node);
        }

        private void WalkUpFromLastChild(XmlNode node)
        {
           if (node == null)
            {               
                return;
            }
           else
               if(node.Name == "rule")
               {
                   return;
               }

            else
            {
                //add this one

                XmlNode previousSiblingForNextProcessing = null;                 

               if(node.Name == "action")
               {
                   Action action = new Action(node);
                   this.ActionStackableItemsStack.Push(action);
                   previousSiblingForNextProcessing = node.PreviousSibling;
               }

                if (node.Name == "clause")
                {
                    Clause clause = new Clause(node);
                    this.ConditionStackableItemsStack.Push(clause);
                    previousSiblingForNextProcessing = node.PreviousSibling;
                }

                if (node.Name == "parentheses")
                {   
                    EquationGroup equationGroup = new EquationGroup(_ruleSetMetaDataDefinition, ref _fields);
                    equationGroup.IsSquareBracketGrouping = IsSquareBrackets(node);
                    if (equationGroup.IsSquareBracketGrouping)
                    {
                        LoadSquareBracketGroupingProperties(node);
                    }
                    equationGroup.WalkUpFromLastChild(node.LastChild);
                    this.ConditionStackableItemsStack.Push(equationGroup);
                    previousSiblingForNextProcessing = node.PreviousSibling;                    
                }

                if (node.Name == "value")
                {
                    XmlNode valueNode = node;

                    XmlNode operatorNode = node.PreviousSibling;

                    XmlNode fieldNode = operatorNode.PreviousSibling;

                    Equation equation = new Equation(fieldNode, operatorNode, valueNode,_ruleSetMetaDataDefinition);
                	if (_fields != null)
                	{
						if (equation.FieldProperty.MetaDataField is EnumerableCollectionMemberField)
						{
							_fields.Add(((EnumerableCollectionMemberField)equation.FieldProperty.MetaDataField).MemberOfCollection);
						}
						else if (equation.FieldProperty.MetaDataField is DictionaryCollectionMemberField)
						{
							_fields.Add(((DictionaryCollectionMemberField)equation.FieldProperty.MetaDataField).MemberOfCollection);
						}
						else
                		 {
                		 	_fields.Add(equation.FieldProperty.PropertyName);
                		 }
                	}

                	this.ConditionStackableItemsStack.Push(equation);
                    previousSiblingForNextProcessing = fieldNode.PreviousSibling;
                }


                WalkUpFromLastChild(previousSiblingForNextProcessing);
                //call this function recursively with previous sibling
            }

           
            return;
        
        }

        private void LoadSquareBracketGroupingProperties(XmlNode node)
        {
            XmlElement element = node as XmlElement;

            if (element.HasAttribute(Constants.XmlRuleElementConstants.MEMBER_OF_COLLECTION))
            {
                _memberOfCollectionString = element.Attributes[Constants.XmlRuleElementConstants.MEMBER_OF_COLLECTION].Value;
            }
            if (element.HasAttribute(Constants.XmlRuleElementConstants.INVOCATION_TYPE))
            {
                _invocationType = 
                   TranslationHelper.InvocationTypesTranslation(element.Attributes[Constants.XmlRuleElementConstants.INVOCATION_TYPE].Value);
            }
        }

        private bool IsSquareBrackets(XmlNode node)
        {
            XmlElement element = node as XmlElement;

            bool isSquareBracket = false;

            if (element.HasAttribute(Constants.XmlRuleSetDefinitionConstants.SQUARE_BRACKET_GROUPING))
            {
                isSquareBracket = true;
            }

            return isSquareBracket;
        }

        private int GetPriority(XmlNode node)
        {
            //if no priority attribute found then just set the priority to 0 which is 
            //the default for the .net RuleSet Priority value
            XmlElement element = node as XmlElement;

            bool hasPriority = false;

            int priority = default(int);

            if (element.HasAttribute("priority"))
            {
                hasPriority = true;
            }

            if (hasPriority)
            {
                priority = Convert.ToInt32(node.Attributes["priority"].Value);                
            }
            return priority;
        }

        //need to recursively reload the stacks for this "top" level and any nested equation groups basically another recursive required here
        public void RestackForReuse()
        {
            IStackable[] actionItemsInLifoOrder = new IStackable[LIFOActionItemsCopyOfOriginalStack.Count()];

            IStackable[] conditionItemsInLifoOrder = new IStackable[LIFOConditionItemsCopyOfOriginalStack.Count()];

            LIFOActionItemsCopyOfOriginalStack.CopyTo(actionItemsInLifoOrder, 0);
            LIFOConditionItemsCopyOfOriginalStack.CopyTo(conditionItemsInLifoOrder, 0);

            IEnumerable<IStackable> reversedActions = actionItemsInLifoOrder.Reverse<IStackable>();
            IEnumerable<IStackable> reversedConditions = conditionItemsInLifoOrder.Reverse<IStackable>();
           
            this.ActionStackableItemsStack.Clear();
            this.ConditionStackableItemsStack.Clear();

            foreach (IStackable actionStackable in reversedActions)
            {
                this.ActionStackableItemsStack.Push(actionStackable);
            }
            foreach (IStackable conditionStackable in reversedConditions)
            {
                if (conditionStackable is EquationGroup)
                {
                    ((EquationGroup)conditionStackable).RestackForReuse();
                }
                this.ConditionStackableItemsStack.Push(conditionStackable);
            }
        }


    }
}

       

