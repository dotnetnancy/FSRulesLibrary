using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DotNetNancy.Rules.RuleSet.Translation;
using System.Collections;
using System.IO;
using DotNetNancy.Rules.RuleSet.RuleSetDefinition;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{
    public class Condition
    {
        XmlDocument _sourceXmlDocument = null;

        MasterEquationGroup masterEquationGroup = null;

        RuleSetMetaDataDefinition _ruleSetMetaDataDefinition = null;

        public MasterEquationGroup MasterEquationGroupProperty
        {
            get { return masterEquationGroup; }
            set { masterEquationGroup = value; }
        }

        public XmlDocument SourceXmlDocument
        {
            get { return _sourceXmlDocument; }
            set { _sourceXmlDocument = value; }
        }

		public Condition(XmlDocument sourceXmlDocument, RuleSetMetaDataDefinition ruleSetMetaDataDefinition, ref List<string> fieldsUsedInRule)
        {
            _sourceXmlDocument = sourceXmlDocument;

            _ruleSetMetaDataDefinition = ruleSetMetaDataDefinition;

            LoadAndLevelXmlDocument();

			masterEquationGroup = new MasterEquationGroup(_sourceXmlDocument, _ruleSetMetaDataDefinition, ref fieldsUsedInRule);            
        }

        /// <summary>
        /// we are preparing the xml document so that we can we can walk the items
        /// and build the object model in another set of steps
        /// </summary>
        private void LoadAndLevelXmlDocument()
        {
            //remove the newlines position is important so we remove anything that we do not need in the xml 
            StripOutUnwantedNodes();

            #region notes

            //each equation "group" can be made up of one or more equations so any standalone equations can have parentheses around it
            //without changing the result (redundant yes, but needs to make this xml "uniform" in pattern), this will make the processing
            //of the xml very light and allow us to build the object model to .net exactly the way the user defined it, when it gets to the engine
            //we can keep the exact way the user wants it processed and we can also evaluate and know each equation that makes up the final condition
            //for stats monitoring etc

            #endregion notes

            //AddParenthesesToStandaloneEquations();
            //AddParenthesesAroundEntireSet();

            //now that we have parentheses around everything we put a level and sequence property on item
            //LevelAndSequenceXml(_sourceXmlDocument);    
        }

        private void AddParenthesesAroundEntireSet()
        {
            XmlNodeList ruleNode = _sourceXmlDocument.SelectNodes("//rule");

            Dictionary<XmlNode, XmlDocumentFragment> ruleToDocFrag = new Dictionary<XmlNode, XmlDocumentFragment>();

            //there should only be one
            foreach (XmlNode rule in ruleNode)
            {
                {

                    XmlDocumentFragment docFragment = _sourceXmlDocument.CreateDocumentFragment();

                    docFragment.InnerXml = @"<rule><parentheses>" +
                        rule.InnerXml +
                        @"</parentheses></rule>";


                    ruleToDocFrag.Add(rule, docFragment);
                }
            }

            foreach (KeyValuePair<XmlNode, XmlDocumentFragment> kvp in ruleToDocFrag)
            {
                XmlNode parenthesesNodeAdded = kvp.Value;
                XmlNode parentOfRuleNode = kvp.Key.ParentNode;


                parentOfRuleNode.ReplaceChild(parenthesesNodeAdded, kvp.Key);

            }
        }

        private void LevelAndSequenceXml(XmlDocument _sourceXmlDocument)
        {
            int beginLevel = 0;

            int counter = beginLevel;
            bool terminate = false;


            RecursiveSequenceSiblingsAtGivenNestedLevel(ref counter, ref terminate);

        }

        private void RecursiveSequenceSiblingsAtGivenNestedLevel(ref int levelCounter, ref bool terminate)
        {
            if (terminate)
            {
                return;
            }

            string root = "rule";
            string repeater = "parentheses";
            string path = GetRecursiveLevelledPath(root, repeater, levelCounter);


            string parenthesesXpath = path;

            //string parenthesesXpath = path + "/" + repeater + "[@level='" + levelCounter.ToString() + "']";
            //string clauseXpath = parenthesesXpath + "/clause" + "[@level='" + levelCounter.ToString() + "']";

            XmlNodeList listSiblingsAtThisLevel =
              _sourceXmlDocument.SelectNodes(parenthesesXpath);



            if (listSiblingsAtThisLevel.Count > 0)
            //if (listSiblingsAtThisLevel.Count > 1)
            {
                int sequence = 0;


                foreach (XmlNode siblingNode in listSiblingsAtThisLevel)
                {
                    AddLevelAttribute(siblingNode, levelCounter);
                    AddSequenceAttribute(siblingNode, sequence);
                    sequence++;

                    ////if (levelCounter > 0)
                    ////{

                    int childSequence = 1;
                    foreach (XmlNode childNode in siblingNode)
                    {
                        AddLevelAttribute(childNode, levelCounter);
                        AddSequenceAttribute(childNode, childSequence);
                        childSequence++;
                    }
                    ////}

                    XmlNode clauseNode = siblingNode.NextSibling;

                    if (clauseNode != null)
                    {
                        if (clauseNode.Name == "clause")
                        {                         
                            AddLevelAttribute(clauseNode, levelCounter);
                            AddSequenceAttribute(clauseNode, sequence);
                            sequence++;

                            if (clauseNode.Attributes["type"].Value == "then")
                            {
                                XmlNode actionNode = clauseNode.NextSibling;

                                bool endRecursion = false;

                                RecursivelyProcessActions(actionNode, ref levelCounter, ref sequence, ref endRecursion);
                            }
                        }
                    }                   

                }

                levelCounter++;

                RecursiveSequenceSiblingsAtGivenNestedLevel(ref levelCounter, ref terminate);
            }
            else
            {
                terminate = true;
            }
        }

        private void RecursivelyProcessActions(XmlNode actionNode, ref int levelCounter, ref int sequence, ref bool terminate)
        {
            if (terminate == true)
            {
                return;
            }

            if (actionNode != null)
            {
                if (actionNode.Name == "action")
                {
                    AddLevelAttribute(actionNode, levelCounter);
                    AddSequenceAttribute(actionNode, sequence);
                    sequence++;
                    RecursivelyProcessActions(actionNode.NextSibling, ref levelCounter, ref sequence, ref terminate);
                }
                else
                {
                    terminate = true;
                }
            }
            else
            {
                terminate = true;
            }
        }

        private void AddLevelAttribute(XmlNode childNode, int currentLevel)
        {            
            XmlAttribute attrib =
                childNode.OwnerDocument.CreateAttribute("level");
            attrib.Value = currentLevel.ToString();
            childNode.Attributes.Append(attrib);
        }

        private void AddSequenceAttribute(XmlNode childNode, int sequence)
        {           
            XmlAttribute attrib =
                childNode.OwnerDocument.CreateAttribute("sequence");
            attrib.Value = sequence.ToString();
            childNode.Attributes.Append(attrib);
        }
        
        private void StripOutUnwantedNodes()
        {
            foreach (XmlNode node in _sourceXmlDocument.SelectNodes("//newline"))
            {
                node.ParentNode.RemoveChild(node);
            }           
        }

        private XmlNode GetThenClauseNode()
        {
            XmlNode thenClauseNode = _sourceXmlDocument.SelectSingleNode("//rule/clause[@type='then']");
            return thenClauseNode;
        }

        private void AddParenthesesToStandaloneEquations()
        {
            XmlNodeList fieldsNodes = _sourceXmlDocument.SelectNodes("//field");

            Dictionary<XmlNode, XmlDocumentFragment> referenceFieldToNewElement = new Dictionary<XmlNode, XmlDocumentFragment>();

            foreach (XmlNode field in fieldsNodes)
            {
                if (field.ParentNode.Name != "parentheses")
                {
                    XmlNode operatorNode = field.NextSibling;

                    XmlNode valueNode = field.NextSibling.NextSibling;

                    XmlDocumentFragment docFragment = _sourceXmlDocument.CreateDocumentFragment();

                    docFragment.InnerXml = @"<parentheses>" +
                        field.OuterXml +
                        operatorNode.OuterXml +
                        valueNode.OuterXml +
                        @"</parentheses>";


                    referenceFieldToNewElement.Add(field, docFragment);
                }
            }

            foreach (KeyValuePair<XmlNode, XmlDocumentFragment> kvp in referenceFieldToNewElement)
            {
                XmlNode parenthesesNodeAdded = kvp.Value;
                XmlNode parentNodeOfField = kvp.Key.ParentNode;
                XmlNode operatorNode = kvp.Key.NextSibling;
                XmlNode valueNode = kvp.Key.NextSibling.NextSibling;

                //remove existing operator node
                parentNodeOfField.RemoveChild(operatorNode);
                //remove existing value node
                parentNodeOfField.RemoveChild(valueNode);
                //replace existing field node with the new parentheses grouping of field operator value
                parentNodeOfField.ReplaceChild(parenthesesNodeAdded, kvp.Key);

            }
        }

        private int GetHighestKeyValueFromSortedDictionary(SortedDictionary<int, SortedDictionary<int, XmlNode>> SortedDictionary)
        {
            List<int> keys = new List<int>();
            keys.AddRange(SortedDictionary.Keys);

            int count = keys.Count;

            return keys[count - 1];
        }

        private int GetHighestKeyValueFromSortedDictionary(SortedDictionary<int, XmlNode> SortedDictionary)
        {
            List<int> keys = new List<int>();
            keys.AddRange(SortedDictionary.Keys);

            int count = keys.Count;

            return keys[count - 1];
        }

        private string GetRecursiveLevelledPath(string root, string name, int currentLevel)
        {
            StringBuilder pathBuilder = new StringBuilder();

            int count = currentLevel;

            pathBuilder.Append("//" + root + "/");

            for (int i = 0; i <= currentLevel; i++)
            {
                if (i < currentLevel)
                {
                    pathBuilder.Append(name + "/");
                }
                else
                {
                    pathBuilder.Append(name);
                }

            }

            return pathBuilder.ToString();
        }

    }
}
