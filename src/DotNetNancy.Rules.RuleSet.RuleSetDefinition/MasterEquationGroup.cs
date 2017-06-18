using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DotNetNancy.Rules.RuleSet.Translation;
using System.Collections;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{
    public class MasterEquationGroup //: EquationGroup, IStackable
    {
        XmlDocument _ruleSetDefinitionXml = null;

        SortedDictionary<int, IStackable> _topLevelStackableItems =
                   new SortedDictionary<int, IStackable>();

        EquationGroup _finalEquationGroup = null;

        Stack _actionsStack = null;

        RuleSetMetaDataDefinition _ruleSetMetaDataDefinition = null;

        public EquationGroup FinalEquationGroup
        {
            get { return _finalEquationGroup; }
            set { _finalEquationGroup = value; }
        }
         
      /// <summary>
        /// recommended you pass the entire rule set definition xml document to this constructor as the top level 
        /// is rule, this class is meant to be the "Master Equation Group" made up of many other Equation groups
        /// but the nodes selected are all top level parentheses nodes to load this particular object
      /// </summary>
      /// <param name="ruleSetDefinitionXmlDocumentAfterCustomTransformations"></param>
        public MasterEquationGroup(XmlDocument ruleSetDefinitionXmlDocumentAfterCustomTransformations,
            RuleSetMetaDataDefinition ruleSetMetaDataDefinition, ref List<string> fieldsUsedInRule)
        {
            _ruleSetMetaDataDefinition = ruleSetMetaDataDefinition;

            _ruleSetDefinitionXml = 
                ruleSetDefinitionXmlDocumentAfterCustomTransformations;

            Load(_ruleSetDefinitionXml, ref fieldsUsedInRule);          

        }



        private void Load(XmlDocument ruleSetDefinitionXml, ref List<string> fieldsUsedInRule)
        {         
            LoadEquationGroup(ref fieldsUsedInRule);
        }

        /// <summary>
        /// </summary>
        /// <param name="levelCounter"></param>
        /// <param name="terminate"></param>
        private void LoadEquationGroup(ref List<string> fieldsUsedInRule)
        {
            int topLevel = 0;

            string root = "rule";
            string repeater = "parentheses";
            string path = GetRecursiveLevelledPath(root, repeater, topLevel);

            string parenthesesXpath = path + "[@level='" + topLevel.ToString() + "']";

            XmlNodeList listSiblingsAtThisLevel =
              _ruleSetDefinitionXml.SelectNodes(parenthesesXpath);

            EquationGroup equationGroup = new EquationGroup(_ruleSetDefinitionXml,_ruleSetMetaDataDefinition, ref fieldsUsedInRule);
            //this cannot be a square bracket grouping because it is the top level, but the top level can contain square bracket groupings
            //this is the default on the equation group object set to false just for clarity of intention
            equationGroup.IsSquareBracketGrouping = false;
            this._finalEquationGroup = equationGroup;
            this._actionsStack = _finalEquationGroup.ActionStackableItemsStack;
            equationGroup.CopyStack();
          
        }

        private void GetLowestLevel(XmlDocument ruleSetDefinitionXml, string root, string repeater, ref int levelCounter, ref bool terminate)
        {
            if (terminate == true)
            {
                return;
            }                 

            string parenthesesXpath = "//parentheses" + "[@level='" + levelCounter.ToString() + "']";

            XmlNodeList listSiblingsAtThisLevel =
              _ruleSetDefinitionXml.SelectNodes(parenthesesXpath);

            if (listSiblingsAtThisLevel.Count > 0)
            {               
                levelCounter++;
                GetLowestLevel(ruleSetDefinitionXml, root,  repeater, ref levelCounter, ref terminate);
            }
            else
            {
                terminate = true;
                levelCounter--;               
            }


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
                    //AddLevelAttribute(actionNode, levelCounter);
                    //AddSequenceAttribute(actionNode, sequence);
                    Action action = new Action(actionNode);

                    //this.EquationGroupStack.Push(action);
                    _topLevelStackableItems.Add(sequence, action);
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



        private void LoadEquationGroupAndClause(XmlNode parenthesesNode, ref int level)
        {
            
            XmlNodeList subLevels = parenthesesNode.SelectNodes("parentheses[@level='" + level.ToString() + "']");

        }

        private int GetSequence(XmlNode node)
        {
            int sequence = Convert.ToInt32(node.Attributes["sequence"].Value);
            return sequence;
        }

        private int GetLevel(XmlNode node)
        {
            int level = Convert.ToInt32(node.Attributes["level"].Value);
            return level;
        }

        #region old code some helper stuff if needed later on

        //XmlNode _nodeWithChildren = null;

        //public XmlNode NodeWithChildren
        //{
        //    get { return _nodeWithChildren; }
        //    set { _nodeWithChildren = value; }
        //}

        //Stack<Stackable> _stackableItems = new Stack<Stackable>();
        //SortedDictionary<int, SortedDictionary<int, EquationGroup>> _levelledEquationGroups =
        //    new SortedDictionary<int, SortedDictionary<int, EquationGroup>>();

        //public SortedDictionary<int, SortedDictionary<int, EquationGroup>> LevelledEquationGroups
        //{
        //    get { return _levelledEquationGroups; }
        //    set { _levelledEquationGroups = value; }
        //}

        //public Stack<Stackable> StackableItems
        //{
        //    get 
        //    {
        //        if (_levelledEquationGroups.Count > 0)
        //        {
        //            foreach(KeyValuePair<int,SortedDictionary<int,EquationGroup>> kvp in _levelledEquationGroups)
        //            {
        //                foreach(KeyValuePair<int,EquationGroup> kvpEquations in kvp.Value)
        //                {
        //                    this.AddStackedItems(kvpEquations.Value.StackableItems);
        //                }
        //            }
        //        }
        //        return _stackableItems; 
        //    }
        //    set 
        //    { 
        //        _stackableItems = value; 
        //    }
        //}

        //public EquationGroup()
        //{
        //}

        //public EquationGroup(SortedDictionary<int, XmlNode> allParenthesesNodes)
        //{
        //    foreach (KeyValuePair<int, XmlNode> kvp in allParenthesesNodes)
        //    {
        //        bool foundchildnodes = false;
        //        foreach (XmlNode childNode in kvp.Value.ChildNodes)
        //        {
        //            if (childNode.Name == "parentheses")
        //            {
        //                foundchildnodes = true;
        //                EquationGroup group = new EquationGroup(kvp.Value);
                        
        //            }
        //        }
        //    }

        //}

        //public EquationGroup(Stack<Stackable> stack)
        //{
        //    _stackableItems = stack;
        //}
        //internal EquationGroup(XmlNode nodeWithChildren)
        //{
        //    _nodeWithChildren = nodeWithChildren;
        //    SortedDictionary<int, SortedDictionary<int, XmlNode>> levelledParenthesesNodes = 
        //        LevelParenthesesNodes(nodeWithChildren);

        //    //reverse them so that the lowest level is on top

        //    List<int> levels = new List<int>();
        //    levels.AddRange(levelledParenthesesNodes.Keys);

        //    int lowestLevel = levels[levels.Count-1];

        //    for (int i = lowestLevel; i <= lowestLevel; i--)
        //    {
        //        if (i < 1)
        //            break;
        //        if (levelledParenthesesNodes.ContainsKey(i))
        //        {
        //            foreach(KeyValuePair<int,XmlNode> kvp in levelledParenthesesNodes[i])
        //            {
        //                this.AddLevelledEquationGroup(i, LoadObjectsOld(kvp.Value));
        //            }
        //        }
        //    }

        //}

        //private Stack<Stackable> LoadObjectsOld(XmlNode parenthesesNode)
        //{
        //    int mainStackableSequence = 1;

        //    SortedDictionary<int, Stackable> mainItemsToStack = new SortedDictionary<int, Stackable>();





        //    EquationGroup equationGroup = null;

        //    XmlNodeList fieldNodes = parenthesesNode.SelectNodes("field");

        //    SortedDictionary<int, Stackable> itemsToStack = new SortedDictionary<int, Stackable>();
        //    if (fieldNodes.Count > 0)
        //    {
        //        int stackedItemSequence = 1;

        //        foreach (XmlNode fieldNode in fieldNodes)
        //        {
        //            XmlNode operatorNode = fieldNode.NextSibling;
        //            XmlNode valueNode = operatorNode.NextSibling;

        //            Equation equation = new Equation();
        //            equation.FieldProperty = new Field(fieldNode);
        //            equation.OperatorProperty = new EquationOperator(operatorNode);
        //            equation.ValueProperty = new Value(valueNode);

        //            XmlNode clauseNode = null;

        //            if (parenthesesNode.PreviousSibling != null)
        //            {
        //                if (parenthesesNode.PreviousSibling.Name == "clause")
        //                {
        //                    clauseNode = parenthesesNode.PreviousSibling;
        //                    itemsToStack.Add(stackedItemSequence, new Clause(clauseNode));
        //                    stackedItemSequence++;
        //                }
        //            }
                    
                    
        //            itemsToStack.Add(stackedItemSequence, equation);
        //            stackedItemSequence++;            

        //            if (valueNode.NextSibling != null)
        //            {

        //                clauseNode = valueNode.NextSibling;


        //                if (clauseNode.Name == "clause")
        //                {
        //                    Clause clause = new Clause(clauseNode);
        //                    itemsToStack.Add(stackedItemSequence, clause);
        //                    stackedItemSequence++;
        //                }
        //            }

        //            if (clauseNode == null)
        //            {
        //                if (parenthesesNode.NextSibling != null)
        //                {
        //                    if (parenthesesNode.NextSibling.Name == "clause")
        //                    {
        //                        clauseNode = parenthesesNode.NextSibling;
        //                        Clause clause = new Clause(clauseNode);
        //                        itemsToStack.Add(stackedItemSequence, clause);
        //                        stackedItemSequence++;
        //                    }
        //                }
        //            }

        //        }

        //        Stack<Stackable> stack = FlipSequentialEquationAndClausesStackBottomToTop(itemsToStack);
        //        equationGroup = new EquationGroup(stack);

        //    }

        //    if (equationGroup != null)
        //    {
        //        mainItemsToStack.Add(mainStackableSequence, equationGroup);
        //        mainStackableSequence++;
        //    }

        //    if (parenthesesNode.NextSibling != null)
        //    {
        //        if (parenthesesNode.NextSibling.Name == "clause")
        //        {
        //            Clause clause = new Clause(parenthesesNode.NextSibling);
        //            mainItemsToStack.Add(mainStackableSequence, clause);
        //            mainStackableSequence++;
        //        }
        //    }

        //    return FlipSequentialEquationAndClausesStackBottomToTop(mainItemsToStack);
        //}

        //private Stack<Stackable> FlipSequentialEquationAndClausesStackBottomToTop(SortedDictionary<int, Stackable> itemsToStack)
        //{
        //    Stack<Stackable> stack = new Stack<Stackable>();

        //    //bottom to top now
        //    itemsToStack.Reverse();

        //    foreach (KeyValuePair<int, Stackable> kvp in itemsToStack)
        //    {
        //        stack.Push(kvp.Value);

        //    }
        //    return stack;
        //}



        //private SortedDictionary<int, SortedDictionary<int, XmlNode>> LevelParenthesesNodes(XmlNode nodeWithChildren)
        //{

        //    int depth = 0;

        //    int searchBeginLevel = 1;

        //    SortedDictionary<int, SortedDictionary<int, XmlNode>> levelledParentheses = new SortedDictionary<int, SortedDictionary<int, XmlNode>>();


        //    SortedDictionary<int, string> paths = new SortedDictionary<int, string>();

        //    GetToBottomLevelParentheses(ref depth, searchBeginLevel, ref paths, nodeWithChildren);


        //    foreach (KeyValuePair<int, string> kvp in paths)
        //    {
        //        //get the list of nodes at that depth = level, path = xpath string
        //        XmlNodeList list = nodeWithChildren.SelectNodes(kvp.Value);

        //        int sequence = 1;

        //        foreach (XmlNode node in list)
        //        {
        //            if (levelledParentheses.ContainsKey(kvp.Key))
        //            {
        //                levelledParentheses[kvp.Key].Add(sequence, node);


        //            }
        //            else
        //            {
        //                SortedDictionary<int, XmlNode> value = new SortedDictionary<int, XmlNode>();
        //                value.Add(sequence, node);
        //                levelledParentheses.Add(kvp.Key, value);
        //            }
                   

        //            //if the level is greater than 1 then it is nested
        //            if (kvp.Key > 1)
        //            {
        //                //ProcessSequenceInParentParenthesesNode(node, kvp.Key);
        //            }

        //            sequence++;
        //        }
        //    }

        //    return levelledParentheses;





        //}

        //private void GetToBottomLevelParentheses(ref int depth, int searchBeginLevel, ref SortedDictionary<int, string> paths, XmlNode nodeWithChildren)
        //{
        //    string path = GetRecursiveLevelledPath("parentheses"
        //        , searchBeginLevel);

        //    if (nodeWithChildren.SelectNodes(path).Count > 0)
        //    {
        //        paths.Add(searchBeginLevel, path);
        //        depth = paths.Count;
        //        searchBeginLevel++;
        //        GetToBottomLevelParentheses(ref depth, searchBeginLevel, ref paths,nodeWithChildren);
        //    }
        //    else
        //    {

        //        //end recursion
        //        return;
        //    }

        //}

        //private string GetRecursiveLevelledPath(string name, int currentLevel)
        //{
        //    StringBuilder pathBuilder = new StringBuilder();

        //    int count = currentLevel;

        //    pathBuilder.Append("//");

        //    for (int i = 1; i <= currentLevel; i++)
        //    {
        //        if (i < currentLevel)
        //        {
        //            pathBuilder.Append(name + "/");
        //        }
        //        else
        //        {
        //            pathBuilder.Append(name);
        //        }

        //    }

        //    return pathBuilder.ToString();
        //}






        //internal void AddStackedItems(Stack<Stackable> stack)
        //{
        //    foreach (Stackable stackedItem in stack)
        //    {
        //        _stackableItems.Push(stackedItem);
        //    }
        //}

        //internal void AddLevelledEquationGroup(int level, Stack<Stackable> stack)
        //{
        //    List<int> keys = new List<int>();

        //    if (_levelledEquationGroups.ContainsKey(level))
        //    {
        //        keys.AddRange(_levelledEquationGroups[level].Keys);
        //        int topKey = keys[keys.Count-1];
        //        _levelledEquationGroups[level].Add(topKey + 1, new EquationGroup(stack));
        //    }
        //    else
        //    {
        //        int key = 1;
        //        SortedDictionary<int,EquationGroup> value = new SortedDictionary<int,EquationGroup>();
        //        value.Add(key,new EquationGroup(stack));
        //        _levelledEquationGroups.Add(level, value);
        //    }

        //}
    #endregion old code helper stuff

        
    }
}
