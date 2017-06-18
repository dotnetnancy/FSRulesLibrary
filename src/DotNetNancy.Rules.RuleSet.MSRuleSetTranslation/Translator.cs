using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetNancy.Rules.RuleSet.RuleSetDefinition;
using System.Workflow.Activities.Rules;
using DotNetNancy.Rules.RuleSet.Translation;
using System.CodeDom;
using DotNetNancy.Rules.RuleSet.Common;
using System.Collections;
using DotNetNancy.Rules.RuleSet.DataAccess;


namespace DotNetNancy.Rules.RuleSet.MSRuleSetTranslation
{
    public class Translator<T>
    {
        CodeDomObject _codeDomObject = null;
        static log4net.ILog _log = null;
        Dictionary<Guid, TranslationValidationResult> _translationValidationFailures = new Dictionary<Guid, TranslationValidationResult>();

        static Translator()
        {
            log4net.Config.XmlConfigurator.Configure();
            _log = log4net.LogManager.GetLogger(Constants.CustomLoggers.DEFAULT_LOGGER);

        }

        public Translator()
        {

        }

        public MSRuleSetTranslationResult TranslateAndAssignCondition(RuleSetDefinition.RuleSetDefinition ruleSetDefinition)
        {
			TranslationValidationResult validationResult = CanBeTranslatedValidation(ruleSetDefinition);
			if (validationResult != null &&
				(validationResult.RulePropertyNamesNotInProcessor.Count > 0 ||
				validationResult.RuleFieldsNotInConfiguration.Count > 0))
			{
				_translationValidationFailures.Add(ruleSetDefinition.RuleID, validationResult);
			} 
			
            return BuildRuleSetAssignConditionAddThenActionsBasedOnRuleSetDefinition(ruleSetDefinition);
        }

        private TranslationValidationResult CanBeTranslatedValidation(RuleSetDefinition.RuleSetDefinition ruleSetDefinition)
        {
            TranslationValidationResult result = new TranslationValidationResult();
            if (typeof(T).IsSubclassOf(typeof(BaseRuleCollectionProcessingLibrary.BaseRuleCollectionProcessing)))
            {
                if (_codeDomObject == null)
                {
                    _codeDomObject = new CodeDomObject(typeof(T));
                }

                ValidateFieldsAgainstType(ruleSetDefinition, ref result);
            	ValidateFieldsAgainstConfigMeta(ruleSetDefinition, ref result);
  
                return (result);
            }
            else
            {
                throw new ApplicationException("This object does not derive from the BaseRuleCollectionProcessing class, not compatible with this translator");
            }
        }

		private void ValidateFieldsAgainstType(RuleSetDefinition.RuleSetDefinition ruleSetDefinition,ref TranslationValidationResult result)
        {
            //Check all fields in rule from Database with object defined 
			foreach (String fieldName in ruleSetDefinition.FieldsUsed)
			{
				if ( !(_codeDomObject.PropertyReferenceExpressions.ContainsKey(fieldName) ||
				    _codeDomObject.MethodReferenceExpressions.ContainsKey(fieldName)) ) 
				{
					//Add to TranslationValidationResult
					result.RulePropertyNamesNotInProcessor.Add(fieldName);
				}
			}
        }

		private void ValidateFieldsAgainstConfigMeta(RuleSetDefinition.RuleSetDefinition ruleSetDefinition, ref TranslationValidationResult result)
		{
			//Check all fields in rule from Database with object defined 
			bool found = false;
			RuleSetMetaDataDefinition ruleSetMetaDataDefinition = ruleSetDefinition.RuleSetMetaDataDefinitionProperty;

			foreach (string fieldName in ruleSetDefinition.FieldsUsed)
			{
				try
				{
					//Find fieldName in Metadata ie configuration for type in DB loaded in ruleSetMetaDataDefinition
					foreach (SetTypes fieldType in  ruleSetMetaDataDefinition.RuleFieldsProperty.RuleFieldsDictionary.Keys)
					{
						foreach (Field field in ruleSetMetaDataDefinition.RuleFieldsProperty.RuleFieldsDictionary[fieldType])
						{
							if (field.PropertyName == fieldName)
							{
								found = true;
								break;
							}
						}
					}
					if (!found)
					{
						//Add to TranslationValidationResult
						result.RuleFieldsNotInConfiguration.Add(fieldName);
					}
					found = false;
				}catch(SystemException ex)
				{
					string error = ex.StackTrace.ToString();
				}
			}
		}
		
		private bool ValidateRulesThenActions(RuleSetDefinition.RuleSetDefinition ruleSetDefinition)
		{
			int counter = 1;
			bool validActions = true;

			while (ruleSetDefinition.ActionsStack.Count > 0)
			{
				DotNetNancy.Rules.RuleSet.RuleSetDefinition.Action action =
					(DotNetNancy.Rules.RuleSet.RuleSetDefinition.Action)ruleSetDefinition.ActionsStack.Pop();

				if ( !_codeDomObject.MethodReferenceExpressions.ContainsKey(action.MethodName) )
				{
					if (!_translationValidationFailures.ContainsKey(ruleSetDefinition.RuleID))
					{
						_translationValidationFailures.Add(ruleSetDefinition.RuleID, new TranslationValidationResult());
						validActions = ruleSetDefinition.CanBeRuntimeExecuted = false;
					}
					_translationValidationFailures[ruleSetDefinition.RuleID].RuleActionsNotInProcessor.Add(action.MethodName);
					_translationValidationFailures[ruleSetDefinition.RuleID].CanBeRuntimeExecuted = false;
					ruleSetDefinition.CanBeRuntimeExecuted = false;
				}
				counter++;
			}
			validActions = ruleSetDefinition.CanBeRuntimeExecuted;
			
			return validActions;
		}
		
        public MSRuleSetTranslationResult TranslateAndAssignCondition(RuleSetDefinitions ruleSetDefinitions)
        {
            int counter=0;
            MSRuleSetTranslationResult translationResult = null;
            for (counter=0; counter<ruleSetDefinitions.Count(); counter++)
            {
                //Add result to _translationValidationFailures 
				TranslationValidationResult validationResult = CanBeTranslatedValidation(ruleSetDefinitions[counter]);
				if (validationResult != null && 
					(validationResult.RulePropertyNamesNotInProcessor.Count > 0 || 
					validationResult.RuleFieldsNotInConfiguration.Count>0))
                {
                   _translationValidationFailures.Add(ruleSetDefinitions[counter].RuleID, validationResult );
                }  
            }

            translationResult = BuildRuleSetAssignConditionAddThenActionsBasedOnRuleSetDefinition(ruleSetDefinitions);    
            return (translationResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleSetDefinition"></param>
        /// <returns>sequence and code expression for setting of the .net ThenActions property on a rule set</returns>
        private SortedDictionary<int, MethodInvocationAction> GetThenActionsByDefinition(RuleSetDefinition.RuleSetDefinition ruleSetDefinition)
        {
            //the sequence is the order that they were defined in, today from the ui only one then action can be defined thus you see 
            //the counter being incremented and the retrieval in the order defined in the xml to support multiple, but multiple
            //not exposed by the ui today.
            int counter = 1;

            SortedDictionary<int, MethodInvocationAction> thenActionsDefinedBySequence = new SortedDictionary<int, MethodInvocationAction>();
            while(ruleSetDefinition.ActionsStack.Count > 0)
            {
                DotNetNancy.Rules.RuleSet.RuleSetDefinition.Action action = 
                    (DotNetNancy.Rules.RuleSet.RuleSetDefinition.Action)ruleSetDefinition.ActionsStack.Pop();
                    
				if (_codeDomObject.MethodReferenceExpressions.ContainsKey(action.MethodName))
				{
					thenActionsDefinedBySequence.Add(counter, GetRuleStatementAction(action));
				}
				counter++;
            }

            return thenActionsDefinedBySequence;
        }

        /// <summary>
        /// currently we do not define parameters in the xml for a method invocation action therefore we only call methods
        /// with no parameters at this time, however the MethodInvocationAction object does take in parameters,
        /// if added to the xml we could support parameters another overload of this method required for that
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private MethodInvocationAction GetRuleStatementAction(DotNetNancy.Rules.RuleSet.RuleSetDefinition.Action action)
        {
            MethodInvocationAction methodInvocationAction = new MethodInvocationAction(
                _codeDomObject.ThisReferenceExpression,
                _codeDomObject.MethodReferenceExpressions[action.MethodName],
                new CodeExpression[] { });

            return methodInvocationAction;
        }


        private void AddRuleSetActions(System.Workflow.Activities.Rules.Rule rule, System.Workflow.Activities.Rules.RuleAction thenAction, System.Workflow.Activities.Rules.RuleAction elseAction)
        {
            if (thenAction != null)
            {
                rule.ThenActions.Add(thenAction);
            }

            if (elseAction != null)
            {
                rule.ElseActions.Add(elseAction);
            }
        }

        private MSRuleSetTranslationResult BuildRuleSetAssignConditionAddThenActionsBasedOnRuleSetDefinition(DotNetNancy.Rules.RuleSet.RuleSetDefinition.RuleSetDefinitions ruleSetDefinitions)
        {
            System.Workflow.Activities.Rules.RuleSet ruleSet = new System.Workflow.Activities.Rules.RuleSet(ruleSetDefinitions.RuleSetName);

            if (_codeDomObject == null)
            {
                _codeDomObject = new CodeDomObject(typeof(T));
            }

            foreach (DotNetNancy.Rules.RuleSet.RuleSetDefinition.RuleSetDefinition ruleSetDefinition in ruleSetDefinitions)
            {
				if ( !_translationValidationFailures.ContainsKey(ruleSetDefinition.RuleID) )
				{
					AddRuleByDefinition(ruleSetDefinition, ref ruleSet);
                }
            }

			MSRuleSetTranslationResult ruleSetTranslationResult = new MSRuleSetTranslationResult(ruleSet, ruleSetDefinitions, _codeDomObject, _translationValidationFailures);

            return ruleSetTranslationResult;
        }

        private MSRuleSetTranslationResult BuildRuleSetAssignConditionAddThenActionsBasedOnRuleSetDefinition(DotNetNancy.Rules.RuleSet.RuleSetDefinition.RuleSetDefinition ruleSetDefinition)
        {           
            System.Workflow.Activities.Rules.RuleSet ruleSet = 
                new System.Workflow.Activities.Rules.RuleSet(ruleSetDefinition.RuleName);

            if (_codeDomObject == null)
            {
                _codeDomObject = new CodeDomObject(typeof(T));
            }

			if (_translationValidationFailures == null || !(_translationValidationFailures.ContainsKey(ruleSetDefinition.RuleID)))
			{
                AddRuleByDefinition(ruleSetDefinition, ref ruleSet);
			}

            MSRuleSetTranslationResult ruleSetTranslationResult = new MSRuleSetTranslationResult(ruleSet, new RuleSetDefinitions(ruleSetDefinition.RuleSetMetaDataDefinitionProperty) {ruleSetDefinition}, _codeDomObject,  _translationValidationFailures);

            return ruleSetTranslationResult;
        }

        private void AddRuleByDefinition(
            DotNetNancy.Rules.RuleSet.RuleSetDefinition.RuleSetDefinition ruleSetDefinition, 
            ref System.Workflow.Activities.Rules.RuleSet ruleSet)
        {
            CodeExpression condition = BuildCondition(ruleSetDefinition.ConditionProperty.MasterEquationGroupProperty);

            if (condition != null)
            {
                System.Workflow.Activities.Rules.Rule rule = new System.Workflow.Activities.Rules.Rule();
                //the description property will be the actual textual name of the rule and the Name property will contain the guid 
                //as a string

                rule.Name = ruleSetDefinition.RuleID.ToString();
                rule.Description = ruleSetDefinition.RuleName;
                rule.Condition = new RuleExpressionCondition(condition);

                rule.Active = !ruleSetDefinition.Paused;
                rule.Priority = ruleSetDefinition.Priority;

                AddRuleStatsActionAndThenActions(rule, ruleSetDefinition);

                ruleSet.Rules.Add(rule);
            }
            else
            {
                //the condition was null probably an error in logic during construction
                //do not add this rule, and log it but do not throw exception as we want to continue processing
                //subsequent calls to this method, we are building up one rule here but adding that rule to a rule set
                //this method called once for each rule set definition that is passed in
                _log.Error("MSRuleSetTranslation.Translator, BuildCondition(...)" + "Rule Name:  " + ruleSetDefinition.RuleName + " a call to BuildCondition returned null, this rule was not added to the rule set, see log for previous exceptions that may have been logged for help debugging");
            }

        }

        public void AddRuleStatsActionAndThenActions(System.Workflow.Activities.Rules.Rule rule, DotNetNancy.Rules.RuleSet.RuleSetDefinition.RuleSetDefinition ruleSetDefinition)
        {
            //basically a rule definition has an applicationID and a typeID so each rule stat that is written will vary by ruleid applicationid and typeid
            //also just by virtue of the type that is sent into this method having implemented IRuleStats says that you want to have rule stats 
            //written else do not implement the IRuleStats interface to "turn off" this behaviour

            if (typeof(T).GetInterface("DotNetNancy.Rules.RuleSet.RuleSetDefinition.IRuleStats") != null)
            {
                CodePrimitiveExpression applicationID = new CodePrimitiveExpression(ruleSetDefinition.ApplicationID);
                    CodePrimitiveExpression typeID = new CodePrimitiveExpression(ruleSetDefinition.TypeID);
                    CodePrimitiveExpression ruleID = new CodePrimitiveExpression(ruleSetDefinition.RuleID);
                    CodePrimitiveExpression ruleName = new CodePrimitiveExpression(ruleSetDefinition.RuleName);
                    CodePrimitiveExpression result = new CodePrimitiveExpression(true);
                    CodePrimitiveExpression insertDate = new CodePrimitiveExpression(DateTime.Now);

                //Then Action for rule stat record (if it evaluates to true, then action is performed)
                InsertRuleStatsAction insertThenRuleStatsAction = new InsertRuleStatsAction(                    
                    applicationID,
                    typeID,
                    ruleID,
                    ruleName,
                    result,
                    insertDate,
                   _codeDomObject.PropertyReferenceExpressions[DataAccessConstants.RuleStatisticTable.REFERENCE_ID]);

                rule.ThenActions.Add(insertThenRuleStatsAction);


                //if it is an Else, then we know that the condition evalutated to false, we have execution time ways of know this also
                result = new CodePrimitiveExpression(false);

                InsertRuleStatsAction insertElseRuleStatsAction = new InsertRuleStatsAction(
                    applicationID,
                    typeID,
                    ruleID,
                    ruleName,
                    //if it is a Then, then we know that the condition evalutated to True, we have execution time ways of know this also
                    //but since this is going to be added to the ThenActions, then it will only be executed if the condition evaluated to true
                    //if this action was added to the else actions then we would know that it evaluated to false and woudl instead pass
                    //false as the result
                    //also this would only be called if the execute method was called on the rule set otherwise if you just use evaluate this
                    //would not execute
                    result,
                    insertDate,
                   _codeDomObject.PropertyReferenceExpressions[DataAccessConstants.RuleStatisticTable.REFERENCE_ID]);

                rule.ElseActions.Add(insertElseRuleStatsAction);
            }

            //only if for this definition shoudl the methods be invoked right at the time of execution of the rule evaluations
            //would we add them to the "ThenActions" else we do something else
            if (ruleSetDefinition.ExecuteActionsInRule)
			{  
				bool canBeRunTimeExecuted = ValidateRulesThenActions(ruleSetDefinition);
				if (canBeRunTimeExecuted)
            	{
					SortedDictionary<int, MethodInvocationAction> definedThenActions = GetThenActionsByDefinition(ruleSetDefinition);
					foreach (KeyValuePair<int, MethodInvocationAction> kvp in definedThenActions)
					{
						//since this is a sorted dictionary we know that they are in the correct order (that they were defined in in the xml)
						//of the key
						rule.ThenActions.Add(kvp.Value);
					}
                }
            }
            else
            {
                SortedDictionary<int, DotNetNancy.Rules.RuleSet.RuleSetDefinition.Action> postProcessingActions = GetPostProcessingActions(ruleSetDefinition);
                ruleSetDefinition.RuleThenActionsDefined = postProcessingActions;
            }

        }

        private SortedDictionary<int, DotNetNancy.Rules.RuleSet.RuleSetDefinition.Action> GetPostProcessingActions(DotNetNancy.Rules.RuleSet.RuleSetDefinition.RuleSetDefinition ruleSetDefinition)
        {
            int counter = 1;

            SortedDictionary<int, DotNetNancy.Rules.RuleSet.RuleSetDefinition.Action> postProcessingActionsBySequence = new SortedDictionary<int, DotNetNancy.Rules.RuleSet.RuleSetDefinition.Action>();

            while (ruleSetDefinition.ActionsStack.Count > 0)
            {
                DotNetNancy.Rules.RuleSet.RuleSetDefinition.Action action =
                    (DotNetNancy.Rules.RuleSet.RuleSetDefinition.Action)ruleSetDefinition.ActionsStack.Pop();

                postProcessingActionsBySequence.Add(counter,action);
                counter++;
            }

            return postProcessingActionsBySequence;
        }

        private CodeExpression BuildCondition(MasterEquationGroup masterEquationGroup)
        {
            CodeExpression mainRuleExpression = null;
           
            LoadEquationGroupRuleExpression(masterEquationGroup.FinalEquationGroup, ref mainRuleExpression);

            masterEquationGroup.FinalEquationGroup.RestackForReuse();

            return mainRuleExpression;
        }

     
        private void LoadEquationGroupRuleExpression(EquationGroup equationGroup, ref CodeExpression mainRuleExpression)
        {            
            //so we need to pop things off of the stack
            while(equationGroup.ConditionStackableItemsStack.Count > 0)
            {
                IStackable firstStackable = null;

                CodeExpression ruleExpression = null;

                //if the first one comes through this loop as an equation or equation group then we process that item as the 
                //start of the main rule expression, then we should always start with the next clause
                try
                {
                    firstStackable = (IStackable)equationGroup.ConditionStackableItemsStack.Pop();

                    if (firstStackable is Equation)
                    {                     
                       ProcessEquation(firstStackable, ref ruleExpression, equationGroup);
                        //at this point ruleExpression should have an initial value, now we need to get our next clause 

                       mainRuleExpression = ruleExpression;
                       
                        //this loops again                        
                        continue;
                        
                    }
                    else
                        if(firstStackable is EquationGroup)
                    {                        
                        ProcessEquationGroup(firstStackable, ref ruleExpression, equationGroup);

                        //at this point ruleExpression should have an initial value, now we need to get our next clause 

                        mainRuleExpression = ruleExpression;

                        continue;                        
                    }
                        else
                            //now we always want to continue in this loop starting with Clause
                            if (firstStackable is Clause)
                            {
                                if (((Clause)(IStackable)firstStackable).ClauseType != ClauseTypes.Then)
                                {
                                    ProcessByClause(firstStackable, ref mainRuleExpression, equationGroup);
                                }
                                else
                                {
                                    //we have hit our "then" which is not a logical operator to be processed
                                    break;
                                }
                                continue;
                            }              

                }
                catch (Exception ex)
                {
                    _log.Error("MSRuleSetTranslation.Translator, LoadEquationGroupRuleExpression(...) caught an exception", ex);

                    break;
                } 
            }
        }

        private void LoadEquationGroupRuleExpressionNested(EquationGroup equationGroup, ref CodeExpression mainRuleExpression)
        {
            //this is a nested expression group and needs to be processed a little differently than the regular top
            //level expression group

            CodeExpression ruleExpression = null;

            //so we need to pop things off of the stack
            while (equationGroup.ConditionStackableItemsStack.Count > 0)
            {
                IStackable firstStackable = null;               

                //if the first one comes through this loop as an equation or equation group then we process that item as the 
                //start of the main rule expression, then we should always start with the next clause
                try
                {
                    firstStackable = (IStackable)equationGroup.ConditionStackableItemsStack.Pop();

                    if (firstStackable is Equation)
                    {
                        //process a single equation make that the beginning of this mainRuleExpression
                        ProcessSingleEquation(firstStackable, ref ruleExpression, equationGroup);
                        //at this point ruleExpression should have an initial value, now we need to get our next clause                       
                        mainRuleExpression = ruleExpression;
                        //this loops again                        
                        continue;

                    }
                    else
                        if (firstStackable is EquationGroup)
                        {
                            ProcessEquationGroup(firstStackable, ref ruleExpression, equationGroup);

                            //at this point ruleExpression should have an initial value, now we need to get our next clause                            
                            mainRuleExpression = ruleExpression;
                            continue;
                        }
                        else
                            //now we always want to continue in this loop starting with Clause
                            if (firstStackable is Clause)
                            {
                                if (((Clause)(IStackable)firstStackable).ClauseType != ClauseTypes.Then)
                                {
                                    ProcessByClause(firstStackable, ref mainRuleExpression, equationGroup);
                                }
                                else
                                {
                                    //we have hit our "then" which is not a logical operator to be processed
                                    break;
                                }
                                continue;
                            }

                }
                catch (Exception ex)
                {
                    _log.Error("MSRuleSetTranslation.Translator, LoadEquationGroupRuleExpressionNested(...)", ex);

                    break;
                }
            }
            return;
        }

       

        private void ProcessSingleEquation(IStackable firstStackable, ref CodeExpression ruleExpression, EquationGroup equationGroup)
        {
            Equation equation = (Equation)firstStackable;

            CodeExpression equationExpression = GetComparisonEquation(equation);

            ruleExpression = equationExpression;
        }


        private void ProcessByClause(IStackable firstStackable, ref CodeExpression mainRuleExpression, EquationGroup equationGroup)
        {
            //main rule expression should already have been initalized so now we want to get the next equation or equation group 
            //then use main rule expression as left then clause the next item on the stack either an equation or equation group as 
            //the right

            Clause clause = (Clause)firstStackable;

            IStackable equationOrEquationGroup = (IStackable)equationGroup.ConditionStackableItemsStack.Pop();

            if (equationOrEquationGroup is Equation)
            {
                Equation equationRight = (Equation)equationOrEquationGroup;

                CodeExpression rightEquationExpression = GetComparisonEquation(equationRight);

                CodeExpression resultExpression = GetClauseExpression(mainRuleExpression, clause, rightEquationExpression);

                mainRuleExpression = resultExpression;

                return;
            }
            else
                if (equationOrEquationGroup is EquationGroup)
                {
                    EquationGroup equationGroupRight = (EquationGroup)equationOrEquationGroup;

                    CodeExpression rightEquationGroupExpression = null;

                    LoadEquationGroupRuleExpressionNested(equationGroupRight, ref rightEquationGroupExpression);

                    if (rightEquationGroupExpression != null)
                    {

                        CodeExpression resultExpression = GetClauseExpression(mainRuleExpression, clause, rightEquationGroupExpression);

                        mainRuleExpression = resultExpression;
                    }
                    else
                    {

                        //we have reached the end so stop processing, not sure about this one, TODO: keep an eye on this permutation
                        return;
                    }
                }


        }

        private void ProcessEquationGroup(IStackable firstStackable, ref CodeExpression ruleExpression, EquationGroup equationGroup)
        {
            //we have an equation group with no previous clause so all we can do is process this equation group, its clause that should follow and
            //either the equation or equation group that proceeds it

            if (equationGroup.ConditionStackableItemsStack.Count > 0)
            {
                EquationGroup equationGroupLeft = (EquationGroup)firstStackable;

                if (equationGroupLeft.IsSquareBracketGrouping)
                {
                    CodeExpression resultExpression = GetSquareBracketEquationGroupExpression(equationGroupLeft);
                    ruleExpression = resultExpression;
                    return;

                }
                else
                {

                    IStackable clauseStackable = (IStackable)equationGroup.ConditionStackableItemsStack.Pop();

                    if (clauseStackable is Clause)
                    {
                        Clause clause = (Clause)(IStackable)clauseStackable;
                        if (clause.ClauseType != ClauseTypes.Then)
                        {
                            IStackable equationOrEquationGroupStackable = (IStackable)equationGroup.ConditionStackableItemsStack.Pop();

                            if (equationOrEquationGroupStackable is Equation)
                            {
                                Equation equation = (Equation)(IStackable)equationOrEquationGroupStackable;

                                CodeExpression resultExpression = GetEquationGroupClauseEquationExpression(equationGroupLeft, clause, equation);

                                ruleExpression = resultExpression;

                                return;
                            }
                            else
                                if (equationOrEquationGroupStackable is EquationGroup)
                                {
                                    EquationGroup equationGroupRight = (EquationGroup)(IStackable)equationOrEquationGroupStackable;

                                    CodeExpression resultExpression = GetEquationGroupClauseEquationGroupExpression(equationGroupLeft, clause,
                                        equationGroupRight);

                                    ruleExpression = resultExpression;

                                    return;
                                }
                        }
                        else
                        {
                            //it may be the only equationgroup on the stack
                            CodeExpression resultExpression = GetSingleEquationGroupExpression(equationGroupLeft);

                            ruleExpression = resultExpression;

                        }
                    }

                }
            }
        }

        private CodeExpression GetSquareBracketEquationGroupExpression(EquationGroup equationGroup)
        {
            ///in a square bracket equation group expression you may not have nested groupings no parentheses and also no
            ///nested square bracket parentheses, also each of the equations in this grouping must target members of the same collection,
            ///each equation must be separated by a clause and i think that the items within this group cannot end with a clause, we expect that
            ///the clause must follow outside of the end square bracket

            //get all items and put them into sequenced dictionary

            int equationSequence = 1;
            int clauseSequence = 1;            

            Dictionary<int, Equation> sequencedEquations = new Dictionary<int, Equation>();
            Dictionary<int, Clause> sequencedClauses = new Dictionary<int, Clause>();

            string memberOfCollection = GetSquareBracketMemberOfCollection(equationGroup);
            InvocationTypes invocationType = GetSquareBracketInvocationType(equationGroup);

            for (int i = 0; i < equationGroup.ConditionStackableItemsStack.Count; i++)
            {

                if (equationGroup.ConditionStackableItemsStack.Peek() is Equation)
                {
                    Equation equation = (Equation)(IStackable)equationGroup.ConditionStackableItemsStack.Pop();
                    sequencedEquations.Add(equationSequence, equation);
                    equationSequence++;
                }
                else
                    if (equationGroup.ConditionStackableItemsStack.Peek() is Clause)
                    {
                        Clause clause = (Clause)(IStackable)equationGroup.ConditionStackableItemsStack.Pop();
                        sequencedClauses.Add(clauseSequence, clause);
                        clauseSequence++;
                    }
            }           

            return GetSquareBracketEquationGroupExpression(memberOfCollection, invocationType,sequencedEquations, sequencedClauses);
        }

        private InvocationTypes GetSquareBracketInvocationType(EquationGroup equationGroup)
        {
            //none of these are valid for this
            if (equationGroup.InvocationType == InvocationTypes.AsMethodWithDefinedValueAsParameter ||
                equationGroup.InvocationType == InvocationTypes.AsProperty ||
                equationGroup.InvocationType == InvocationTypes.NotSupported)
            {
                //this is the default anyway but just make sure
                return InvocationTypes.AsAnyOneDynamic;
            }
            else
            {
                return equationGroup.InvocationType;
            }
            
        }

        private string GetSquareBracketMemberOfCollection(EquationGroup equationGroup)
        {
           //there should only be one level of equations separated by clauses in this square brackets equation group
            //find the first one to derive it if the ui did not write an attribute as to which collection the properties are member's of
            if (!String.IsNullOrEmpty(equationGroup.MemberOfCollectionString))
            {
                return equationGroup.MemberOfCollectionString;
            }
            else
            {
                Equation equation = (Equation)equationGroup.ConditionStackableItemsStack.Peek();
                if (equation.FieldProperty.MetaDataField is EnumerableCollectionMemberField)
                {
                    return ((EnumerableCollectionMemberField)equation.FieldProperty.MetaDataField).CollectionField.PropertyName;
                }
            }

            return null;
        }

        private CodeExpression GetSquareBracketEquationGroupExpression(string memberOfCollection, InvocationTypes invocationType, Dictionary<int, Equation> sequencedEquations, Dictionary<int, Clause> sequencedClauses)
        {
            string[] fields = new string[sequencedEquations.Count];
            string[] operators = new string[sequencedEquations.Count];
            object[] values = new object[sequencedEquations.Count];
            string[] clauses = new string[sequencedClauses.Count];
            string[] setTypes = new string[sequencedEquations.Count];

            int counter = 0;

            foreach (KeyValuePair<int, Equation> kvp in sequencedEquations)
            {
                fields[counter] = kvp.Value.FieldProperty.PropertyName;
                operators[counter] = TranslationHelper.OperatorValueTranslation(kvp.Value.OperatorProperty.Operator);
                values[counter] = kvp.Value.ValueProperty.Value;
                setTypes[counter] = kvp.Value.FieldProperty.MetaDataField.DataType.ToString();
                if (sequencedClauses.ContainsKey(kvp.Key))
                {
                    clauses[counter] = sequencedClauses[kvp.Key].ClauseType.ToString();
                }

                counter++;

            }

            return GetSquareBracketEquationGroupExpression(memberOfCollection,invocationType,fields, operators, values, clauses, setTypes);


        }

        private CodeExpression GetSquareBracketEquationGroupExpression(CodeIndexerExpression codeIndexerExpression, InvocationTypes invocationType, string[] fields, string[] operators, object[] values, string[] clauses, string[] setTypes)
        {
            CodePrimitiveExpression caseSensitive = new CodePrimitiveExpression(false);

            CodeExpression[] fieldPrimitiveExpressions = new CodeExpression[fields.Count()];
            CodeExpression[] operatorPrimitiveExpressions = new CodeExpression[operators.Count()];
            CodeExpression[] valuesPrimitiveExpressions = new CodeExpression[values.Count()];
            CodeExpression[] clausePrimitiveExpressions = new CodeExpression[clauses.Count()];
            CodeExpression[] setTypePrimitiveExpressions = new CodeExpression[setTypes.Count()];

            //we should always have one more equation than clauses

            for (int i = 0; i < fields.Count(); i++)
            {
                CodePrimitiveExpression fieldExpression = new CodePrimitiveExpression(fields[i]);
                CodePrimitiveExpression operatorExpression = new CodePrimitiveExpression(operators[i]);
                CodePrimitiveExpression valueExpression = new CodePrimitiveExpression(values[i]);
                CodePrimitiveExpression setTypeExpression = new CodePrimitiveExpression(setTypes[i]);

                fieldPrimitiveExpressions[i] = fieldExpression;
                operatorPrimitiveExpressions[i] = operatorExpression;
                valuesPrimitiveExpressions[i] = valueExpression;
                setTypePrimitiveExpressions[i] = setTypeExpression;

                if (i < fields.Count() - 1)
                {
                    CodePrimitiveExpression clauseExpression = new CodePrimitiveExpression(clauses[i]);
                    //then try to add clause
                    clausePrimitiveExpressions[i] = clauseExpression;
                }
            }


            CodeArrayCreateExpression propertiesArrayCreateAndInitalize = new CodeArrayCreateExpression(typeof(string), fieldPrimitiveExpressions);
            CodeArrayCreateExpression operatorsArrayCreateAndInitialize = new CodeArrayCreateExpression(typeof(string), operatorPrimitiveExpressions);
            CodeArrayCreateExpression valuesArrayCreateAndInitialize = new CodeArrayCreateExpression(typeof(object), valuesPrimitiveExpressions);
            CodeArrayCreateExpression setTypesArrayCreateAndInitialize = new CodeArrayCreateExpression(typeof(string), setTypePrimitiveExpressions);
            CodeArrayCreateExpression clausesArrayCreateAndInitialize = new CodeArrayCreateExpression(typeof(string), clausePrimitiveExpressions);

            //this is the extension method syntax this extension method is defined in the knowntypes class with a signature of (this List<Segements>)
            CodeExpression invoke = null;

            if (invocationType == InvocationTypes.AsAnyOneDynamic)
            {

                invoke = new CodeMethodInvokeExpression(codeIndexerExpression,
                    Constants.InvocationTypeMethodNameConstants.ANY_ONE_DYNAMIC, new CodeExpression[] {propertiesArrayCreateAndInitalize,operatorsArrayCreateAndInitialize,
                            valuesArrayCreateAndInitialize, clausesArrayCreateAndInitialize, setTypesArrayCreateAndInitialize,
                                        caseSensitive});
            }
            else
                if (invocationType == InvocationTypes.AsAllDynamic)
                {
                    invoke = new CodeMethodInvokeExpression(codeIndexerExpression,
                    Constants.InvocationTypeMethodNameConstants.ALL_DYNAMIC, new CodeExpression[] {propertiesArrayCreateAndInitalize,operatorsArrayCreateAndInitialize,
                            valuesArrayCreateAndInitialize, clausesArrayCreateAndInitialize, setTypesArrayCreateAndInitialize,
                                        caseSensitive});
                }

            CodeExpression expression = new SingleBooleanResultExpression(invoke);

            return expression;
        }


        private CodeExpression GetSquareBracketEquationGroupExpression(string memberOfCollection, InvocationTypes invocationType,string[] fields, string[] operators, object[] values, string[] clauses, string[] setTypes)
        {
            CodePrimitiveExpression caseSensitive = new CodePrimitiveExpression(false);

            CodeExpression[] fieldPrimitiveExpressions = new CodeExpression[fields.Count()];
            CodeExpression[] operatorPrimitiveExpressions = new CodeExpression[operators.Count()];
            CodeExpression[] valuesPrimitiveExpressions = new CodeExpression[values.Count()];
            CodeExpression[] clausePrimitiveExpressions = new CodeExpression[clauses.Count()];
            CodeExpression[] setTypePrimitiveExpressions = new CodeExpression[setTypes.Count()];

            //we should always have one more equation than clauses

            for (int i = 0; i < fields.Count(); i++)
            {
                CodePrimitiveExpression fieldExpression = new CodePrimitiveExpression(fields[i]);
                CodePrimitiveExpression operatorExpression = new CodePrimitiveExpression(operators[i]);
                CodePrimitiveExpression valueExpression = new CodePrimitiveExpression(values[i]);
                CodePrimitiveExpression setTypeExpression = new CodePrimitiveExpression(setTypes[i]);

                fieldPrimitiveExpressions[i] = fieldExpression;
                operatorPrimitiveExpressions[i] = operatorExpression;
                valuesPrimitiveExpressions[i] = valueExpression;
                setTypePrimitiveExpressions[i] = setTypeExpression;

                if (i <= fields.Count() - 1)
                {
                    CodePrimitiveExpression clauseExpression = new CodePrimitiveExpression(clauses[i]);
                    //then try to add clause
                    clausePrimitiveExpressions[i] = clauseExpression;
                }
                
            }    


            CodeArrayCreateExpression propertiesArrayCreateAndInitalize = new CodeArrayCreateExpression(typeof(string), fieldPrimitiveExpressions);
            CodeArrayCreateExpression operatorsArrayCreateAndInitialize = new CodeArrayCreateExpression(typeof(string), operatorPrimitiveExpressions);
            CodeArrayCreateExpression valuesArrayCreateAndInitialize = new CodeArrayCreateExpression(typeof(object), valuesPrimitiveExpressions);
            CodeArrayCreateExpression setTypesArrayCreateAndInitialize = new CodeArrayCreateExpression(typeof(string), setTypePrimitiveExpressions);
            CodeArrayCreateExpression clausesArrayCreateAndInitialize = new CodeArrayCreateExpression(typeof(string),clausePrimitiveExpressions);

            //this is the extension method syntax this extension method is defined in the knowntypes class with a signature of (this List<Segements>)
            CodeExpression invoke = null;

            if (invocationType == InvocationTypes.AsAnyOneDynamic)
            {

                invoke = new CodeMethodInvokeExpression(_codeDomObject.PropertyReferenceExpressions[memberOfCollection],
                    Constants.InvocationTypeMethodNameConstants.ANY_ONE_DYNAMIC, new CodeExpression[] {propertiesArrayCreateAndInitalize,operatorsArrayCreateAndInitialize,
                            valuesArrayCreateAndInitialize, clausesArrayCreateAndInitialize, setTypesArrayCreateAndInitialize,
                                        caseSensitive});
            }
            else
                if (invocationType == InvocationTypes.AsAllDynamic)
                {
                    invoke = new CodeMethodInvokeExpression(_codeDomObject.PropertyReferenceExpressions[memberOfCollection],
                    Constants.InvocationTypeMethodNameConstants.ALL_DYNAMIC, new CodeExpression[] {propertiesArrayCreateAndInitalize,operatorsArrayCreateAndInitialize,
                            valuesArrayCreateAndInitialize, clausesArrayCreateAndInitialize, setTypesArrayCreateAndInitialize,
                                        caseSensitive});
                }

            CodeExpression expression = new SingleBooleanResultExpression(invoke);

            return expression;
        }

        private CodeExpression GetSingleEquationGroupExpression(EquationGroup equationGroupLeft)
        {
            CodeExpression singleEquationGroupExpression = null;

            LoadEquationGroupRuleExpressionNested(equationGroupLeft, ref singleEquationGroupExpression);

            return singleEquationGroupExpression;
        }

        private CodeExpression GetEquationGroupClauseEquationGroupExpression(EquationGroup equationGroupLeft, Clause clause, EquationGroup equationGroupRight)
        {
            CodeExpression leftEquationGroupExpression = null;

            LoadEquationGroupRuleExpressionNested(equationGroupLeft, ref leftEquationGroupExpression);

            CodeExpression rightEquationGroupExpression = null;

            LoadEquationGroupRuleExpressionNested(equationGroupRight, ref rightEquationGroupExpression);

            CodeExpression resultExpression = GetClauseExpression(leftEquationGroupExpression, clause, rightEquationGroupExpression);

            return resultExpression;
        }

        private CodeExpression GetEquationGroupClauseEquationExpression(EquationGroup equationGroupLeft, Clause clause, Equation equation)
        {
            CodeExpression leftEquationGroupExpression = null;

            LoadEquationGroupRuleExpressionNested(equationGroupLeft, ref leftEquationGroupExpression);

            CodeExpression rightEquationExpression = GetComparisonEquation(equation);

            CodeExpression resultExpression = GetClauseExpression(leftEquationGroupExpression, clause, rightEquationExpression);

            return resultExpression;
        }

        private void ProcessEquation(IStackable firstStackable, ref CodeExpression ruleExpression, EquationGroup equationGroup)
        {
            //here we are going to assume that we are assigning the result of this to the ruleExpression that was passed in
            //there is no previous clause etc so that is all that we can do, process this equation top down and assign to rule expression
            //generate left, right, and clause in between

            Equation equation = (Equation)firstStackable;

            if (equationGroup.ConditionStackableItemsStack.Count > 0)
            {
                //only a clause should proceed an equation
                IStackable clauseStackable = (IStackable)equationGroup.ConditionStackableItemsStack.Pop();

                if (clauseStackable is Clause)
                {
                    Clause clause = (Clause)clauseStackable;

                    if (clause.ClauseType != ClauseTypes.Then)
                    {

                        //then after a clause can always come either an equation or an equation group

                        IStackable equationOrEquationGroup = (IStackable)equationGroup.ConditionStackableItemsStack.Pop();

                        if (equationOrEquationGroup is Equation)
                        {
                            Equation nextEquation = (Equation)equationOrEquationGroup;

                            CodeExpression expressionResult = GetEquationClauseEquationExpression(equation, clause, nextEquation);

                            ruleExpression = expressionResult;
                            return;
                        }
                        else
                            if (equationOrEquationGroup is EquationGroup)
                            {
                                EquationGroup nextEquationGroup = (EquationGroup)equationOrEquationGroup;

                                CodeExpression expressionResult = GetEquationClauseEquationGroupExpression(equation, clause, nextEquationGroup);


                                ruleExpression = expressionResult;
                                return;
                            }
                    }
                    else
                    {
                        //maybe it was the only item on the stackableItems
                        CodeExpression expressionResult = GetComparisonEquation(equation);

                        ruleExpression = expressionResult;
                    }
                }

            }           
            
        }

        private CodeExpression GetEquationClauseEquationGroupExpression(Equation leftEquation, Clause clause, EquationGroup rightEquationGroup)
        {
            CodeExpression leftExpression = GetComparisonEquation(leftEquation);

            CodeExpression rightExpression = null;
            
            LoadEquationGroupRuleExpressionNested(rightEquationGroup, ref rightExpression);

            CodeExpression clauseExpression = GetClauseExpression(leftExpression, clause, rightExpression);

            return clauseExpression;

        }

        private CodeExpression GetEquationClauseEquationExpression(Equation leftEquation, Clause clause, Equation rightEquation)
        {
            CodeExpression leftExpression = GetComparisonEquation(leftEquation);

            CodeExpression rightExpression = GetComparisonEquation(rightEquation);

            CodeExpression clauseExpression = GetClauseExpression(leftExpression, clause, rightExpression);

            return clauseExpression;
        }


        private CodeExpression GetClauseExpression(CodeExpression leftEquationExpression, Clause clause, CodeExpression rightEquationExpression)
        {
            switch (clause.ClauseType)
            {
                case ClauseTypes.And:
                    {
                        return  new LogicalAND(leftEquationExpression, rightEquationExpression);
                    }
                case ClauseTypes.Or:
                    {
                        return new LogicalOR(leftEquationExpression, rightEquationExpression);
                    }
                default:
                    {
                        //anything else not supported
                        return null;
                    }
            }
        }

        private CodeExpression GetEquationRuleExpression(Equation equation)
        {
            switch (equation.OperatorProperty.Operator)
            {
                    //equals, less than, less than or equal, greater than, greater than or equal
                case OperatorTypes.Is:
                case OperatorTypes.Less:
                case OperatorTypes.LessIs:
                case OperatorTypes.More:
                case OperatorTypes.MoreIs:
                case OperatorTypes.Not:
                    {
                        return GetComparisonEquation(equation);
                    }                   
                default:
                    {
                        return null;
                    }
            }
        }

        private CodeExpression GetComparisonEquation(Equation equation)
        {
            //it can be part of the set of string manipulation type comparisons or just regular CompareTo comparisons

            if (TranslationHelper.IsCompareToOperator(equation.OperatorProperty.Operator))
            {
                return GetCompareToEquation(equation);
            }
            else
                if(TranslationHelper.IsStringComparisonOperator(equation.OperatorProperty.Operator))
            {
                return GetStringComparisonEquation(equation);
            }

            return null;
        }

        private CodeExpression GetStringComparisonEquation(Equation equation)
        {
            StringComparisonType stringComparisonType = TranslationHelper.StringComparisonValidTypes(equation.OperatorProperty.Operator);

            if (stringComparisonType == StringComparisonType.NotSupported)
            {
                throw new ApplicationException("string comparison type not supported");
            }
            else
                if ((equation.FieldProperty.MetaDataField is CollectionField
               || equation.FieldProperty.MetaDataField is DictionaryCollectionMemberField 
               || equation.FieldProperty.MetaDataField is EnumerableCollectionMemberField)
               &&
               (equation.FieldProperty.MetaDataField.InvocationType == InvocationTypes.AsAnyOneDynamic ||
                    equation.FieldProperty.MetaDataField.InvocationType == InvocationTypes.AsAllDynamic ||
                    equation.FieldProperty.MetaDataField.InvocationType == InvocationTypes.AsProperty))
                {
                    return ProcessCollection(equation);
                }
                else
                    if (equation.FieldProperty.MetaDataField.InvocationType == InvocationTypes.AsMethodWithDefinedValueAsParameter)
                    {
                        CodeMethodReferenceExpression methodReferenceExpression = _codeDomObject.MethodReferenceExpressions[equation.FieldProperty.PropertyName];
                        return ProcessStringMethodCallWithValueAsParameter(methodReferenceExpression, equation.ValueProperty.Value.ToString());

                    }
                    else
                        switch (stringComparisonType)
                        {
                            case StringComparisonType.Contains:
                                {
                                    return ContainsEquation(equation);
                                }
                            case StringComparisonType.NotContains:
                                {
                                    return NotContainsEquation(equation);
                                }
                            case StringComparisonType.EndsWith:
                                {
                                    return EndsWithEquation(equation);
                                }
                            case StringComparisonType.NotEndsWith:
                                {
                                    return NotEndsWithEquation(equation);
                                }
                            case StringComparisonType.StartsWith:
                                {
                                    return StartsWithEquation(equation);
                                }
                            case StringComparisonType.NotStartsWith:
                                {
                                    return NotStartsWithEquation(equation);
                                }
                        }

            return null;
        }


        private CodeExpression NotStartsWithEquation(Equation equation)
        {
            CodeExpression expression = null;

            CodeExpression propertyReferenceExpression =
                _codeDomObject.PropertyReferenceExpressions[equation.FieldProperty.PropertyName];

            expression = NotStartsWithEquation(equation, propertyReferenceExpression);

            return expression;
        }

        private CodeExpression NotStartsWithEquation(Equation equation, CodeExpression itemToOperateOn)
        {
            CodeExpression expression = null;

            string stringValueToLookFor = equation.ValueProperty.Value.ToString();

            bool caseSensitive = false; //true is the default .net behaviour if you do not specify 

            CodeExpression startsWithExpression = new StartsWithString(itemToOperateOn, stringValueToLookFor, caseSensitive);

            expression = new LogicalNOT(startsWithExpression);

            return expression;
        }

        private CodeExpression StartsWithEquation(Equation equation)
        {
            CodeExpression expression = null;

            CodeExpression propertyReferenceExpression =
                _codeDomObject.PropertyReferenceExpressions[equation.FieldProperty.PropertyName];

            expression = StartsWithEquation(equation, propertyReferenceExpression);

            return expression;
        }

        private CodeExpression StartsWithEquation(Equation equation, CodeExpression itemToOperateOn)
        {
            CodeExpression expression = null;

            string stringValueToLookFor = equation.ValueProperty.Value.ToString();

            bool caseSensitive = false; //true is the default .net behaviour if you do not specify 

            expression = new StartsWithString(itemToOperateOn, stringValueToLookFor, caseSensitive);

            return expression;
        }

        private CodeExpression NotEndsWithEquation(Equation equation)
        {
            CodeExpression expression = null;

            CodeExpression propertyReferenceExpression =
                _codeDomObject.PropertyReferenceExpressions[equation.FieldProperty.PropertyName];

            expression = NotEndsWithEquation(equation, propertyReferenceExpression);

            return expression;
        }

        private CodeExpression NotEndsWithEquation(Equation equation, CodeExpression itemToOperateOn)
        {
            CodeExpression expression = null;

            string stringValueToLookFor = equation.ValueProperty.Value.ToString();

            bool caseSensitive = false; //true is the default .net behaviour if you do not specify 

            CodeExpression endsWithExpression = new EndsWithString(itemToOperateOn, stringValueToLookFor, caseSensitive);

            expression = new LogicalNOT(endsWithExpression);

            return expression;
        }

        private CodeExpression EndsWithEquation(Equation equation)
        {
            CodeExpression expression = null;

            CodeExpression propertyReferenceExpression =
                _codeDomObject.PropertyReferenceExpressions[equation.FieldProperty.PropertyName];

            expression = EndsWithEquation(equation, propertyReferenceExpression);

            return expression;
        }

        private CodeExpression EndsWithEquation(Equation equation, CodeExpression propertyReferenceExpression)
        {
            CodeExpression expression = null;

            string stringValueToLookFor = equation.ValueProperty.Value.ToString();

            bool caseSensitive = false; //true is the default .net behaviour if you do not specify 

            expression = new EndsWithString(propertyReferenceExpression, stringValueToLookFor, caseSensitive);

            return expression;
        }

        private CodeExpression NotContainsEquation(Equation equation)
        {
            CodeExpression expression = null;

            CodeExpression propertyReferenceExpression =
                _codeDomObject.PropertyReferenceExpressions[equation.FieldProperty.PropertyName];


            expression = NotContainsEquation(equation, propertyReferenceExpression);

            return expression;
        }

        private CodeExpression NotContainsEquation(Equation equation, CodeExpression itemToOperateOn)
        {
            CodeExpression expression = null;
            CodeExpression containsExpression = null;

            string stringValueToLookFor = equation.ValueProperty.Value.ToString();

            bool caseSensitive = false; //true is the default .net behaviour if you do not specify 

            StringComparisonType stringComparisonType =
               TranslationHelper.StringComparisonValidTypes(equation.OperatorProperty.Operator);

            if (stringValueToLookFor.Contains('*') ||
                                           stringValueToLookFor.Contains('?') && stringComparisonType == StringComparisonType.Contains)
            {
                //then this is a wild card search expression
                containsExpression = new WildCardSearchExpression(itemToOperateOn,
                    stringValueToLookFor,
                    stringComparisonType,
                    caseSensitive);
            }
            else
            {
                //there are no wildcard characters so just do a regular .net contains
                containsExpression = new ContainsString(itemToOperateOn, stringValueToLookFor, caseSensitive);
            }

            expression = new LogicalNOT(containsExpression);

            return expression;

        }

        private CodeExpression ContainsEquation(Equation equation)
        {
            CodeExpression expression = null;

            CodeExpression propertyReferenceExpression =
                _codeDomObject.PropertyReferenceExpressions[equation.FieldProperty.PropertyName];

            expression = ContainsEquation(equation, propertyReferenceExpression);


            return expression;
        }

        private CodeExpression ContainsEquation(Equation equation, CodeExpression itemToOperateOn)
        {
            CodeExpression expression = null;

            string stringValueToLookFor = equation.ValueProperty.Value.ToString();

            bool caseSensitive = false; //true is the default .net behaviour if you do not specify 

            StringComparisonType stringComparisonType =
                TranslationHelper.StringComparisonValidTypes(equation.OperatorProperty.Operator);

            if (stringValueToLookFor.Contains('*') ||
                               stringValueToLookFor.Contains('?') && stringComparisonType == StringComparisonType.Contains)
            {
                //then this is a wild card search expression
                expression = new WildCardSearchExpression(itemToOperateOn,
                    stringValueToLookFor,
                    stringComparisonType,
                    caseSensitive);
            }
            else
            {
                //there are no wildcard characters so just do a regular .net contains
                expression = new ContainsString(itemToOperateOn, stringValueToLookFor, caseSensitive);
            }

            return expression;
        }

        private CodeExpression GetCompareToEquation(Equation equation)
        {
            CodeExpression expression = null;

             ComparisonType comparisonType = Translation.TranslationHelper.CompareToValidEqualityTypes(equation.OperatorProperty.Operator);

            if (comparisonType == ComparisonType.NotSupported)                    
                throw new ApplicationException("comparison type not supported");         

            bool isEnum = false;
            bool isDate = false;
            bool isTime = false;
            bool isString = false;         

            DetermineType(ref isEnum, ref isDate, ref isTime, ref isString, equation);

            if ((equation.FieldProperty.MetaDataField is CollectionField
                || equation.FieldProperty.MetaDataField is DictionaryCollectionMemberField
                || equation.FieldProperty.MetaDataField is EnumerableCollectionMemberField)
                && 
                (equation.FieldProperty.MetaDataField.InvocationType == InvocationTypes.AsAnyOneDynamic ||
                    equation.FieldProperty.MetaDataField.InvocationType == InvocationTypes.AsAllDynamic ||
                    equation.FieldProperty.MetaDataField.InvocationType == InvocationTypes.AsProperty))
            {
                expression = ProcessCollection(equation);
            }
            
            else
                {                    
                    expression = ProcessNonCollection(equation,comparisonType,
                        isEnum, isDate, isTime, isString);
                }

           return expression;
        }

        private CodeExpression ProcessCollection(Equation equation)
        {

            CodeExpression expression = null;

            if (equation.FieldProperty.MetaDataField is CollectionField)
            {
                CollectionField collectionField = (CollectionField)equation.FieldProperty.MetaDataField;
                if (collectionField.CollectionType == CollectionTypes.IEnumerable)
                {
                    if (equation.OperatorProperty.Operator == OperatorTypes.In)
                    {
                        expression = ProcessIEnumerableInOperator(collectionField, equation);
                    }
                    else
                        if (equation.OperatorProperty.Operator == OperatorTypes.NotIn)
                        {
                            expression = ProcessIEnumerableInOperator(collectionField, equation);
                            expression = new LogicalNOT(expression);
                        }
                        else
                        {
                            expression = ProcessIEnumerableCollection(collectionField, equation);
                        }
                }
                else
                {
                    //this is a dictionary type of field which is the whole dictionary, the idea is to conver the dictionary values
                    //to a list and then use the usual list processing, must have an extension method with the Dictionary type signature
                    //i should be able to call the same 

                    //need to take the comma separated list of values and pass that in
                    if (equation.OperatorProperty.Operator == OperatorTypes.In)
                    {
                        expression = ProcessIDictionaryInCollection(collectionField, equation);
                    }
                    else
                        if (equation.OperatorProperty.Operator == OperatorTypes.NotIn)
                        {
                            expression = ProcessIDictionaryInCollection(collectionField, equation);
                            expression = new LogicalNOT(expression);
                        }
                        else
                        {
                            expression = ProcessIDictionaryCollection(collectionField, equation);
                        }

                }
            }
            else
                if (equation.FieldProperty.MetaDataField is EnumerableCollectionMemberField)
                {
                    EnumerableCollectionMemberField enumerableCollectionMemberField =
                        (EnumerableCollectionMemberField)equation.FieldProperty.MetaDataField;

                    if (equation.OperatorProperty.Operator == OperatorTypes.In)
                    {
                        expression = ProcessIEnumerableInOperator(enumerableCollectionMemberField, equation);
                    }
                    else
                        if (equation.OperatorProperty.Operator == OperatorTypes.NotIn)
                        {
                            expression = ProcessIEnumerableInOperator(enumerableCollectionMemberField, equation);
                            expression = new LogicalNOT(expression);
                        }
                        else
                        {

                            expression = ProcessIEnumerableCollectionMember(enumerableCollectionMemberField, equation);
                        }
                }
                else
                    if (equation.FieldProperty.MetaDataField is DictionaryCollectionMemberField)
                    {
                        DictionaryCollectionMemberField dictionaryCollectionMemberField = (DictionaryCollectionMemberField)equation.FieldProperty.MetaDataField;

                        if (equation.OperatorProperty.Operator == OperatorTypes.In)
                        {
                            expression = ProcessIDictionaryInCollectionWithKey(dictionaryCollectionMemberField, equation);
                        }
                        else
                            if (equation.OperatorProperty.Operator == OperatorTypes.NotIn)
                            {
                                expression = ProcessIDictionaryInCollectionWithKey(dictionaryCollectionMemberField, equation);
                                expression = new LogicalNOT(expression);
                            }
                            else
                            {
                                expression = ProcessIDictionaryCollectionWithKey(dictionaryCollectionMemberField, equation);
                            }
                    }

            return expression;
        }

        private CodeExpression ProcessIDictionaryInCollectionWithKey(DictionaryCollectionMemberField dictionaryCollectionMemberField, Equation equation)
        {
            //the In operator only supports string types at this time

            CodePropertyReferenceExpression propertyReferenceExpression = _codeDomObject.PropertyReferenceExpressions[dictionaryCollectionMemberField.CollectionField.PropertyName];
            CodeExpression expression = null;

            if (dictionaryCollectionMemberField.Key != null)
            {
                System.CodeDom.CodeIndexerExpression indexerExpression = new CodeIndexerExpression(propertyReferenceExpression, new CodePrimitiveExpression(dictionaryCollectionMemberField.Key));
                Type[] types =
            _codeDomObject.PropertyNameToConcretePropertyType[dictionaryCollectionMemberField.CollectionField.PropertyName].GetGenericArguments();

                //types[0] is the key type
                //types[1] is the value type

                Type dictionaryValuesType = types[1];

                bool isEnum = false;
                bool isDate = false;
                bool isTime = false;
                bool isString = false;

                DetermineType(ref isEnum, ref isDate, ref isTime, ref isString, equation);
                expression = ProcessDictionaryInWithKey(dictionaryCollectionMemberField, indexerExpression, equation, dictionaryValuesType, propertyReferenceExpression, isEnum, isDate, isTime, isString);
            }

            expression = new SingleBooleanResultExpression(expression);

            return expression;

        }

        private CodeExpression ProcessIEnumerableInOperator(CollectionField collectionField, Equation equation)
        {
            //split the values first

            string commaSeparatedValues = equation.ValueProperty.Value.ToString();

            if (String.IsNullOrEmpty(commaSeparatedValues))
            {
                throw new ApplicationException("In expects a string with comma separated values, this string is null or empty, cannot process In operator for collection");
            }

            string [] splitStringvalues = commaSeparatedValues.Split(TranslationHelper.IN_DELIMITER);

            string [] repeatPropertyName = new string[splitStringvalues.Length];

            int counter = 0;

            // then just repeat the property name because we need to do a "if this item.Property = value1 OR item.SameProperty = value2 OR item.SameProperty = value3 etc..."
            foreach (string value in splitStringvalues)
            {
                repeatPropertyName[counter] = equation.FieldProperty.PropertyName;
                counter++;
            }

            //there will always be one less clause than there are equations
            int clauseCount = repeatPropertyName.Length - 1;

            string[] clauses = new string[clauseCount];

            for (int i = 0; i < clauseCount; i++)
            {
                clauses[i] = ClauseTypes.Or.ToString();
            }

            int setTypeCount = repeatPropertyName.Length;

            string[] setTypes = new string[setTypeCount];

            for (int i = 0; i < setTypeCount; i++)
            {
                setTypes[i] = equation.FieldProperty.MetaDataField.DataType.ToString();
            }

            int operatorTypeCount = repeatPropertyName.Length;
            
            string [] operatorTypes = new string[repeatPropertyName.Length];

            for(int i = 0; i < operatorTypeCount; i++)
            {
                operatorTypes[i] = TranslationHelper.OperatorValueTranslation(equation.OperatorProperty.Operator);
            }

            string memberOfCollection = string.Empty;

            if (equation.FieldProperty.MetaDataField is EnumerableCollectionMemberField)
            {
                memberOfCollection = ((EnumerableCollectionMemberField)equation.FieldProperty.MetaDataField).CollectionField.PropertyName;
            }
            else
            {
                memberOfCollection = equation.FieldProperty.MetaDataField.PropertyName;
            }

            //invocation type is any one dynamic if any one of the property that we are inspecting has the value value1 OR value2 OR value3 then
            //return true else return false
            CodeExpression expression = GetSquareBracketEquationGroupExpression(memberOfCollection,
                InvocationTypes.AsAnyOneDynamic,
                repeatPropertyName,
                operatorTypes,
                splitStringvalues,
                clauses,
                setTypes);


            return expression;

        }

        private CodeExpression ProcessIEnumerableInOperator(EnumerableCollectionMemberField enumerableMemberCollectionField, Equation equation)
        {
            //split the values first

            string commaSeparatedValues = equation.ValueProperty.Value.ToString();

            if (String.IsNullOrEmpty(commaSeparatedValues))
            {
                throw new ApplicationException("In expects a string with comma separated values, this string is null or empty, cannot process In operator for collection");
            }

            string[] splitStringvalues = commaSeparatedValues.Split(TranslationHelper.IN_DELIMITER);

            string[] repeatPropertyName = new string[splitStringvalues.Length];

            int counter = 0;

            // then just repeat the property name because we need to do a "if this item.Property = value1 OR item.SameProperty = value2 OR item.SameProperty = value3 etc..."
            foreach (string value in splitStringvalues)
            {
                repeatPropertyName[counter] = equation.FieldProperty.PropertyName;
                counter++;
            }

            //there will always be one less clause than there are equations
            int clauseCount = repeatPropertyName.Length - 1;

            string[] clauses = new string[clauseCount];

            for (int i = 0; i < clauseCount; i++)
            {
                clauses[i] = ClauseTypes.Or.ToString();
            }

            int setTypeCount = repeatPropertyName.Length;

            string[] setTypes = new string[setTypeCount];

            for (int i = 0; i < setTypeCount; i++)
            {
                setTypes[i] = equation.FieldProperty.MetaDataField.DataType.ToString();
            }

            int operatorTypeCount = repeatPropertyName.Length;

            string[] operatorTypes = new string[repeatPropertyName.Length];

            for (int i = 0; i < operatorTypeCount; i++)
            {
                operatorTypes[i] = TranslationHelper.OperatorValueTranslation(equation.OperatorProperty.Operator);
            }

            string memberOfCollection = string.Empty;

            if (equation.FieldProperty.MetaDataField is EnumerableCollectionMemberField)
            {
                memberOfCollection = ((EnumerableCollectionMemberField)equation.FieldProperty.MetaDataField).CollectionField.PropertyName;
            }
            else
            {
                memberOfCollection = equation.FieldProperty.MetaDataField.PropertyName;
            }

            //invocation type is any one dynamic if any one of the property that we are inspecting has the value value1 OR value2 OR value3 then
            //return true else return false
            CodeExpression expression = GetSquareBracketEquationGroupExpression(memberOfCollection,
                InvocationTypes.AsAnyOneDynamic,
                repeatPropertyName,
                operatorTypes,
                splitStringvalues,
                clauses,
                setTypes);


            return expression;

        }


        private CodeExpression ProcessIDictionaryInCollection(CollectionField collectionField, Equation equation)
        {
            CodeExpression expression = null;

            Type listMembersType = null;

            CodePropertyReferenceExpression propertyReferenceExpression = _codeDomObject.PropertyReferenceExpressions[collectionField.PropertyName];
            Type propertyType = _codeDomObject.PropertyNameToConcretePropertyType[collectionField.PropertyName];

            Type[] types =
               _codeDomObject.PropertyNameToConcretePropertyType[propertyReferenceExpression.PropertyName].GetGenericArguments();


            listMembersType = types[1];

            CodeFieldReferenceExpression operatorTypeRef = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(DotNetNancy.Rules.RuleSet.Translation.OperatorTypes)),
    equation.OperatorProperty.Operator.ToString());

            CodeFieldReferenceExpression setTypeRef = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(DotNetNancy.Rules.RuleSet.Translation.SetTypes)),
equation.FieldProperty.DataType.ToString());


            CodePrimitiveExpression caseSensitive = new CodePrimitiveExpression(false);

            //so today we are using "method extensions" which are defined for each type of collection that we want to process in our "known types" assembly
            //in the future i would much rather use the base class implementation 
            CodeMethodInvokeExpression invoke = GetDynamicInvocationInExpressionByInvocationType(collectionField,
                equation,
                propertyReferenceExpression,
                operatorTypeRef,
                caseSensitive,
                setTypeRef);

            #region preferred way to do this, but not supported in .net RuleSet today, .net rule set only supports limited dom expressions
            //does not support generics today which sucks but not much we can do

            //There is also a class that all rule processable classes derive from which is DotNetNancy.Rules.RuleSet.BaseRuleCollectionProcessingLibrary.BaseRuleCollectionProcessing
            //DO NOT FORGET TO CAST TO BASE CLASS because that is where these methods are implemented

            //CodeCastExpression baseClassType = new CodeCastExpression(typeof(BaseRuleCollectionProcessingLibrary.BaseRuleCollectionProcessing), codeDomObject.ThisReferenceExpression);

            // CodeMethodReferenceExpression methodRefExpression = 
            //     new CodeMethodReferenceExpression(baseClassType,Constants.InvocationTypeMethodNameConstants.ANY_ONE_DYNAMIC);

            //methodRefExpression.TypeArguments.Add(new CodeTypeReference(listMembersType));

            //CodeExpression [] parameters = new CodeExpression[] {new CodePrimitiveExpression(equation.ValueProperty.Value),
            //operatorTypeRef,
            //caseSensitive};

            //CodeMethodInvokeExpression invoke = new CodeMethodInvokeExpression(methodRefExpression, parameters);

            #endregion preferred way to do this, but not supported in RuleSet today

            expression = new SingleBooleanResultExpression(invoke);

            return expression;
        }

        private CodeMethodInvokeExpression GetDynamicInvocationInExpressionByInvocationType(CollectionField collectionField, Equation equation, CodePropertyReferenceExpression propertyReferenceExpression, CodeFieldReferenceExpression operatorTypeRef, CodePrimitiveExpression caseSensitive, CodeFieldReferenceExpression setTypeRef)
        {                  
            CodeMethodInvokeExpression invoke = null;

            //we only support a comma separated list of values so use the split function to pass that to the AnyOne overload that takes
            //in an array of strings

            string[] multipleValuesToCompare = equation.ValueProperty.Value.ToString().Split(TranslationHelper.IN_DELIMITER);

            CodeExpression[] mulitpleValuePrimitiveExpressions = new CodeExpression[multipleValuesToCompare.Count()];
            
            //we should always have one more equation than clauses

            for (int i = 0; i < multipleValuesToCompare.Count(); i++)
            {
                CodePrimitiveExpression valueToCompareExpression = new CodePrimitiveExpression(multipleValuesToCompare[i]);
                
                mulitpleValuePrimitiveExpressions[i] = valueToCompareExpression;             
            }

            CodeArrayCreateExpression multipleValuesArrayCreateAndInitalize = new CodeArrayCreateExpression(typeof(string), mulitpleValuePrimitiveExpressions);

            if (collectionField.InvocationType == InvocationTypes.AsAnyOneDynamic)
            {
                invoke = new CodeMethodInvokeExpression(propertyReferenceExpression,
                   Constants.InvocationTypeMethodNameConstants.ANY_ONE_DYNAMIC, new CodeExpression[] {multipleValuesArrayCreateAndInitalize,
               operatorTypeRef,
               caseSensitive,
                   setTypeRef});
            }           

            return invoke;
        }

        


        private CodeExpression ProcessIDictionaryCollection(CollectionField collectionField, Equation equation)
        {
            CodeExpression expression = null;

            Type listMembersType = null;

            CodePropertyReferenceExpression propertyReferenceExpression = _codeDomObject.PropertyReferenceExpressions[collectionField.PropertyName];
            Type propertyType = _codeDomObject.PropertyNameToConcretePropertyType[collectionField.PropertyName];

            Type[] types =
               _codeDomObject.PropertyNameToConcretePropertyType[propertyReferenceExpression.PropertyName].GetGenericArguments();

          
            listMembersType = types[1];

            CodeFieldReferenceExpression operatorTypeRef = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(DotNetNancy.Rules.RuleSet.Translation.OperatorTypes)),
    equation.OperatorProperty.Operator.ToString());

            CodeFieldReferenceExpression setTypeRef = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(DotNetNancy.Rules.RuleSet.Translation.SetTypes)),
equation.FieldProperty.DataType.ToString());


            CodePrimitiveExpression caseSensitive = new CodePrimitiveExpression(false);

           //so today we are using "method extensions" which are defined for each type of collection that we want to process in our "known types" assembly
            //in the future i would much rather use the base class implementation 
            CodeMethodInvokeExpression invoke = GetDynamicInvocationExpressionByInvocationType(collectionField,
                equation,
                propertyReferenceExpression,                
                operatorTypeRef,
                caseSensitive,
                setTypeRef);

            #region preferred way to do this, but not supported in .net RuleSet today, .net rule set only supports limited dom expressions
            //does not support generics today which sucks but not much we can do

            //There is also a class that all rule processable classes derive from which is DotNetNancy.Rules.RuleSet.BaseRuleCollectionProcessingLibrary.BaseRuleCollectionProcessing
            //DO NOT FORGET TO CAST TO BASE CLASS because that is where these methods are implemented

            //CodeCastExpression baseClassType = new CodeCastExpression(typeof(BaseRuleCollectionProcessingLibrary.BaseRuleCollectionProcessing), codeDomObject.ThisReferenceExpression);

            // CodeMethodReferenceExpression methodRefExpression = 
            //     new CodeMethodReferenceExpression(baseClassType,Constants.InvocationTypeMethodNameConstants.ANY_ONE_DYNAMIC);

            //methodRefExpression.TypeArguments.Add(new CodeTypeReference(listMembersType));

            //CodeExpression [] parameters = new CodeExpression[] {new CodePrimitiveExpression(equation.ValueProperty.Value),
            //operatorTypeRef,
            //caseSensitive};

            //CodeMethodInvokeExpression invoke = new CodeMethodInvokeExpression(methodRefExpression, parameters);

            #endregion preferred way to do this, but not supported in RuleSet today

            expression = new SingleBooleanResultExpression(invoke);

            return expression;
        }

        private CodeMethodInvokeExpression GetDynamicInvocationExpressionByInvocationType(CollectionField collectionField, Equation equation, CodePropertyReferenceExpression propertyReferenceExpression, CodeFieldReferenceExpression operatorTypeRef, CodePrimitiveExpression caseSensitive,
            CodeFieldReferenceExpression setTypeRef)
        {
            CodeMethodInvokeExpression invoke = null;

            if (collectionField.InvocationType == InvocationTypes.AsAnyOneDynamic)
            {
                invoke = new CodeMethodInvokeExpression(propertyReferenceExpression,
                   Constants.InvocationTypeMethodNameConstants.ANY_ONE_DYNAMIC, new CodeExpression[] {new CodePrimitiveExpression(equation.ValueProperty.Value),
               operatorTypeRef,
               caseSensitive,
                   setTypeRef});
            }
            else
                if (collectionField.InvocationType == InvocationTypes.AsAllDynamic)
                {
                    invoke = new CodeMethodInvokeExpression(propertyReferenceExpression,
                   Constants.InvocationTypeMethodNameConstants.ALL_DYNAMIC, new CodeExpression[] { new CodePrimitiveExpression(equation.ValueProperty.Value),
               operatorTypeRef,
               caseSensitive,
                   setTypeRef});
                }

            return invoke;
        }

        private CodeExpression ProcessIDictionaryCollectionWithKey(DictionaryCollectionMemberField dictionaryCollectionMemberField, Equation equation)
        {
            CodePropertyReferenceExpression propertyReferenceExpression = _codeDomObject.PropertyReferenceExpressions[dictionaryCollectionMemberField.CollectionField.PropertyName];
            CodeExpression expression = null;

            if (dictionaryCollectionMemberField.Key != null)
            {
                System.CodeDom.CodeIndexerExpression indexerExpression = new CodeIndexerExpression(propertyReferenceExpression, new CodePrimitiveExpression(dictionaryCollectionMemberField.Key));
                   Type[] types =
               _codeDomObject.PropertyNameToConcretePropertyType[dictionaryCollectionMemberField.CollectionField.PropertyName].GetGenericArguments();

                //types[0] is the key type
                //types[1] is the value type
            
            Type dictionaryValuesType = types[1];               

            bool isEnum = false;
            bool isDate = false;
            bool isTime = false;
            bool isString = false;         

            DetermineType(ref isEnum, ref isDate, ref isTime, ref isString, equation);
            expression = ProcessDictionaryWithKey(dictionaryCollectionMemberField,indexerExpression, equation, dictionaryValuesType, propertyReferenceExpression, isEnum, isDate, isTime, isString);
            }

            return expression;
        }

        private CodeExpression ProcessIEnumerableCollectionMember(EnumerableCollectionMemberField enumerableCollectionMemberField, Equation equation)
        {
            CodePropertyReferenceExpression propertyReferenceExpression = _codeDomObject.PropertyReferenceExpressions[enumerableCollectionMemberField.CollectionField.PropertyName];
            Type propertyType = _codeDomObject.PropertyNameToConcretePropertyType[enumerableCollectionMemberField.CollectionField.PropertyName];

            CodeExpression expression = null;

            Type listMembersType = null;

            Type[] types =
               _codeDomObject.PropertyNameToConcretePropertyType[propertyReferenceExpression.PropertyName].GetGenericArguments();

            foreach (Type type in types)
            {
                if (!type.IsGenericParameter)
                {
                    listMembersType = type;
                }
            }

            CodeFieldReferenceExpression operatorTypeRef = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(DotNetNancy.Rules.RuleSet.Translation.OperatorTypes)),
                equation.OperatorProperty.Operator.ToString());

            CodeFieldReferenceExpression setTypeRef = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(DotNetNancy.Rules.RuleSet.Translation.SetTypes)),
equation.FieldProperty.DataType.ToString());


            CodePrimitiveExpression caseSensitive = new CodePrimitiveExpression(false);

            //so here the Constants.InvocationTypeMethodNameConstants.ANY_ONE_DYNAMIC method needs to do the comparison that the user specified and then the result of that
            //will be a boolean so collection.item.property > or < or == or != etc, so we use the result
            CodeMethodInvokeExpression invoke = GetDynamicInvocationExpressionByInvocationType(enumerableCollectionMemberField,
                equation, propertyReferenceExpression, listMembersType, operatorTypeRef, caseSensitive,setTypeRef);

            #region preferred way to do this, but not supported in .net RuleSet today, .net rule set only supports limited dom expressions
            //does not support generics today which sucks but not much we can do

            //There is also a class that all rule processable classes derive from which is DotNetNancy.Rules.RuleSet.BaseRuleCollectionProcessingLibrary.BaseRuleCollectionProcessing
            //DO NOT FORGET TO CAST TO BASE CLASS because that is where these methods are implemented

            //CodeCastExpression baseClassType = new CodeCastExpression(typeof(DotNetNancy.Rules.RuleSet.BaseRuleCollectionProcessingLibrary.BaseRuleCollectionProcessing), codeDomObject.ThisReferenceExpression);

            // CodeMethodReferenceExpression methodRefExpression = 
            //     new CodeMethodReferenceExpression(baseClassType,Constants.InvocationTypeMethodNameConstants.ANY_ONE_DYNAMIC);

            //methodRefExpression.TypeArguments.Add(new CodeTypeReference(listMembersType));

            //CodeExpression [] parameters = new CodeExpression[] {new CodePrimitiveExpression(equation.ValueProperty.Value),
            //operatorTypeRef,
            //caseSensitive};

            //CodeMethodInvokeExpression invoke = new CodeMethodInvokeExpression(methodRefExpression, parameters);

            #endregion preferred way to do this, but not supported in RuleSet today

            expression = new SingleBooleanResultExpression(invoke);

            return expression;
        }


        private CodeExpression ProcessIEnumerableCollection(CollectionField collectionField, Equation equation)
        {
                           
            CodePropertyReferenceExpression propertyReferenceExpression = _codeDomObject.PropertyReferenceExpressions[collectionField.PropertyName];
            Type propertyType = _codeDomObject.PropertyNameToConcretePropertyType[collectionField.PropertyName];

            CodeExpression expression = null;

            Type listMembersType = null;

            Type[] types =
               _codeDomObject.PropertyNameToConcretePropertyType[propertyReferenceExpression.PropertyName].GetGenericArguments();

            foreach (Type type in types)
            {
                if (!type.IsGenericParameter)
                {
                    listMembersType = type;
                }
            }

            CodeFieldReferenceExpression operatorTypeRef = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(DotNetNancy.Rules.RuleSet.Translation.OperatorTypes)),
                equation.OperatorProperty.Operator.ToString());
            CodeFieldReferenceExpression setTypeRef = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(DotNetNancy.Rules.RuleSet.Translation.SetTypes)),
equation.FieldProperty.DataType.ToString());


            CodePrimitiveExpression caseSensitive = new CodePrimitiveExpression(false);

            //so here the Constants.InvocationTypeMethodNameConstants.ANY_ONE_DYNAMIC method needs to do the comparison that the user specified and then the result of that
            //will be a boolean so collection.item.property > or < or == or != etc, so we use the result
            CodeMethodInvokeExpression invoke = GetDynamicInvocationExpressionByInvocationType(collectionField,
                equation,
                propertyReferenceExpression,
                listMembersType,
                operatorTypeRef,
                caseSensitive,
                setTypeRef);

            #region preferred way to do this, but not supported in .net RuleSet today, .net rule set only supports limited dom expressions
            //does not support generics today which sucks but not much we can do

            //There is also a class that all rule processable classes derive from which is DotNetNancy.Rules.RuleSet.BaseRuleCollectionProcessingLibrary.BaseRuleCollectionProcessing
            //DO NOT FORGET TO CAST TO BASE CLASS because that is where these methods are implemented

            //CodeCastExpression baseClassType = new CodeCastExpression(typeof(DotNetNancy.Rules.RuleSet.BaseRuleCollectionProcessingLibrary.BaseRuleCollectionProcessing), codeDomObject.ThisReferenceExpression);

            // CodeMethodReferenceExpression methodRefExpression = 
            //     new CodeMethodReferenceExpression(baseClassType,Constants.InvocationTypeMethodNameConstants.ANY_ONE_DYNAMIC);

            //methodRefExpression.TypeArguments.Add(new CodeTypeReference(listMembersType));

            //CodeExpression [] parameters = new CodeExpression[] {new CodePrimitiveExpression(equation.ValueProperty.Value),
            //operatorTypeRef,
            //caseSensitive};

            //CodeMethodInvokeExpression invoke = new CodeMethodInvokeExpression(methodRefExpression, parameters);

            #endregion preferred way to do this, but not supported in RuleSet today

            expression = new SingleBooleanResultExpression(invoke);

           return expression;
        }

        private CodeMethodInvokeExpression GetDynamicInvocationExpressionByInvocationType(CollectionField collectionField,
            Equation equation,
            CodePropertyReferenceExpression propertyReferenceExpression,
            Type listMembersType,
            CodeFieldReferenceExpression operatorTypeRef,
            CodePrimitiveExpression caseSensitive,
            CodeFieldReferenceExpression setTypeRef)
        {
            CodeMethodInvokeExpression invoke = null;

            if (collectionField.InvocationType == InvocationTypes.AsAnyOneDynamic)
            {
                invoke = new CodeMethodInvokeExpression(propertyReferenceExpression,
                   Constants.InvocationTypeMethodNameConstants.ANY_ONE_DYNAMIC, new CodeExpression[] {new CodePrimitiveExpression(propertyReferenceExpression.PropertyName)
                    , new CodePrimitiveExpression(listMembersType.FullName), new CodePrimitiveExpression(equation.ValueProperty.Value),
               operatorTypeRef,
               caseSensitive,
                   setTypeRef});              
            }
            else
                if (collectionField.InvocationType == InvocationTypes.AsAllDynamic)
                {
                    invoke = new CodeMethodInvokeExpression(propertyReferenceExpression,
                   Constants.InvocationTypeMethodNameConstants.ALL_DYNAMIC, new CodeExpression[] {new CodePrimitiveExpression(propertyReferenceExpression.PropertyName)
                    , new CodePrimitiveExpression(listMembersType.FullName), new CodePrimitiveExpression(equation.ValueProperty.Value),
               operatorTypeRef,
               caseSensitive,
                   setTypeRef});
                }

            return invoke;
        }

        private CodeMethodInvokeExpression GetDynamicInvocationExpressionByInvocationType(EnumerableCollectionMemberField enumerableCollectionMemberField,
            Equation equation,
            CodePropertyReferenceExpression propertyReferenceExpression,
            Type listMembersType,
            CodeFieldReferenceExpression operatorTypeRef,
            CodePrimitiveExpression caseSensitive,
            CodeFieldReferenceExpression setTypeRef)
        {
            CodeMethodInvokeExpression invoke = null;

            if (enumerableCollectionMemberField.InvocationType == InvocationTypes.AsAnyOneDynamic)
            {
                invoke = new CodeMethodInvokeExpression(propertyReferenceExpression,
               Constants.InvocationTypeMethodNameConstants.ANY_ONE_DYNAMIC, new CodeExpression[] {new CodePrimitiveExpression(enumerableCollectionMemberField.PropertyName)
                    , new CodePrimitiveExpression(listMembersType.FullName), new CodePrimitiveExpression(equation.ValueProperty.Value),
               operatorTypeRef,
               caseSensitive,
               setTypeRef});
            }
            else
                if (enumerableCollectionMemberField.InvocationType == InvocationTypes.AsAllDynamic)
                {
                    invoke = new CodeMethodInvokeExpression(propertyReferenceExpression,
               Constants.InvocationTypeMethodNameConstants.ALL_DYNAMIC, new CodeExpression[] {new CodePrimitiveExpression(enumerableCollectionMemberField.PropertyName)
                    , new CodePrimitiveExpression(listMembersType.FullName), new CodePrimitiveExpression(equation.ValueProperty.Value),
               operatorTypeRef,
               caseSensitive,
               setTypeRef});
                }

            return invoke;
        }

        private CodeExpression ProcessDictionaryWithKey(DictionaryCollectionMemberField dictionaryCollectionMemberField,
            CodeIndexerExpression codeIndexerExpression, Equation equation, Type propertyType, CodePropertyReferenceExpression propertyReferenceExpression,
     bool isEnum, bool isDate, bool isTime, bool isString)
        {
            CodeExpression expression = null;

            ComparisonType comparisonType = TranslationHelper.CompareToValidEqualityTypes(equation.OperatorProperty.Operator);

            StringComparisonType stringComparisonType = TranslationHelper.StringComparisonValidTypes(equation.OperatorProperty.Operator);

            bool isIenumerable = false;

            Type[] listInterfaces = propertyType.GetInterfaces();

            foreach (Type t in listInterfaces)
            {
                if (t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    isIenumerable = true;
                    break;
                }
            }

            if (comparisonType != ComparisonType.NotSupported)
            {           
                if (!isIenumerable)
                {
                    expression = GetDictionaryWithKeyCompareToExpression(codeIndexerExpression, equation, equation.ValueProperty.Value,
                        isEnum, isDate, isTime, isString, comparisonType, propertyType);
                }
                else
                {

                    CodeFieldReferenceExpression operatorTypeRef = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(DotNetNancy.Rules.RuleSet.Translation.OperatorTypes)),
                        equation.OperatorProperty.Operator.ToString());
                    CodeFieldReferenceExpression setTypeRef = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(DotNetNancy.Rules.RuleSet.Translation.SetTypes)),
                    equation.FieldProperty.DataType.ToString());

                    CodePrimitiveExpression caseSensitive = new CodePrimitiveExpression(false);

                    expression = new CodeMethodInvokeExpression(codeIndexerExpression,
                   Constants.InvocationTypeMethodNameConstants.ANY_ONE_DYNAMIC, new CodeExpression[] {new CodePrimitiveExpression(equation.ValueProperty.Value),
               operatorTypeRef,
               caseSensitive,
                   setTypeRef});
                }

            }
            else
                if (stringComparisonType != StringComparisonType.NotSupported)
                {
                    if (!isIenumerable)
                    {
                        expression = GetDictionaryWithKeyStringComparisonExpression(codeIndexerExpression, equation,
                            stringComparisonType);
                    }
                    else
                    {
                        CodeFieldReferenceExpression operatorTypeRef = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(DotNetNancy.Rules.RuleSet.Translation.OperatorTypes)),
                       equation.OperatorProperty.Operator.ToString());
                        CodeFieldReferenceExpression setTypeRef = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(DotNetNancy.Rules.RuleSet.Translation.SetTypes)),
                        equation.FieldProperty.DataType.ToString());

                        CodePrimitiveExpression caseSensitive = new CodePrimitiveExpression(false);

                        expression = new CodeMethodInvokeExpression(codeIndexerExpression,
                       Constants.InvocationTypeMethodNameConstants.ANY_ONE_DYNAMIC, new CodeExpression[] {new CodePrimitiveExpression(equation.ValueProperty.Value),
               operatorTypeRef,
               caseSensitive,
                   setTypeRef});
                    }
                }
            return expression;
        }

        private CodeExpression ProcessDictionaryInWithKey(DictionaryCollectionMemberField dictionaryCollectionMemberField,
    CodeIndexerExpression codeIndexerExpression, Equation equation, Type propertyType, CodePropertyReferenceExpression propertyReferenceExpression,
bool isEnum, bool isDate, bool isTime, bool isString)
        {
            CodeExpression expression = null;

            ComparisonType comparisonType = TranslationHelper.CompareToValidEqualityTypes(equation.OperatorProperty.Operator);

            StringComparisonType stringComparisonType = TranslationHelper.StringComparisonValidTypes(equation.OperatorProperty.Operator);

            bool isIenumerable = false;

            Type[] listInterfaces = propertyType.GetInterfaces();

            foreach (Type t in listInterfaces)
            {
                if (t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    isIenumerable = true;
                    break;
                }
            }

            if (comparisonType != ComparisonType.NotSupported)
            {
                if (!isIenumerable)
                {
                    expression = GetDictionaryWithKeyCompareToInExpression(codeIndexerExpression, equation, equation.ValueProperty.Value,
                        isEnum, isDate, isTime, isString, comparisonType, propertyType);
                }
                else
                {

                    string[] multipleValuesToCompare = equation.ValueProperty.Value.ToString().Split(TranslationHelper.IN_DELIMITER);

                    CodeExpression[] mulitpleValuePrimitiveExpressions = new CodeExpression[multipleValuesToCompare.Count()];

                    //we should always have one more equation than clauses

                    for (int i = 0; i < multipleValuesToCompare.Count(); i++)
                    {
                        CodePrimitiveExpression valueToCompareExpression = new CodePrimitiveExpression(multipleValuesToCompare[i]);

                        mulitpleValuePrimitiveExpressions[i] = valueToCompareExpression;
                    }

                    CodeArrayCreateExpression multipleValuesArrayCreateAndInitalize = new CodeArrayCreateExpression(typeof(string), mulitpleValuePrimitiveExpressions);

                    CodeFieldReferenceExpression operatorTypeRef = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(DotNetNancy.Rules.RuleSet.Translation.OperatorTypes)),
                        equation.OperatorProperty.Operator.ToString());
                    CodeFieldReferenceExpression setTypeRef = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(DotNetNancy.Rules.RuleSet.Translation.SetTypes)),
                    equation.FieldProperty.DataType.ToString());

                    CodePrimitiveExpression caseSensitive = new CodePrimitiveExpression(false);

                    expression = new CodeMethodInvokeExpression(codeIndexerExpression,
                   Constants.InvocationTypeMethodNameConstants.ANY_ONE_DYNAMIC, new CodeExpression[] {multipleValuesArrayCreateAndInitalize,
               operatorTypeRef,
               caseSensitive,
                   setTypeRef});
                }

            }
            else
                if (stringComparisonType != StringComparisonType.NotSupported)
                {
                    if (!isIenumerable)
                    {
                        expression = GetDictionaryWithKeyStringComparisonExpression(codeIndexerExpression, equation,
                            stringComparisonType);
                    }
                    else
                    {
                        CodeFieldReferenceExpression operatorTypeRef = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(DotNetNancy.Rules.RuleSet.Translation.OperatorTypes)),
                       equation.OperatorProperty.Operator.ToString());
                        CodeFieldReferenceExpression setTypeRef = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(DotNetNancy.Rules.RuleSet.Translation.SetTypes)),
                        equation.FieldProperty.DataType.ToString());

                        CodePrimitiveExpression caseSensitive = new CodePrimitiveExpression(false);

                        expression = new CodeMethodInvokeExpression(codeIndexerExpression,
                       Constants.InvocationTypeMethodNameConstants.ANY_ONE_DYNAMIC, new CodeExpression[] {new CodePrimitiveExpression(equation.ValueProperty.Value),
               operatorTypeRef,
               caseSensitive,
                   setTypeRef});
                    }
                }
            return expression;
        }


        private CodeExpression GetDictionaryWithKeyStringComparisonExpression(CodeIndexerExpression codeIndexerExpression, Equation equation,StringComparisonType stringComparisonType)
        {
            switch (stringComparisonType)
            {
                case StringComparisonType.Contains:
                    {
                        return ContainsEquation(equation, codeIndexerExpression);
                    }
                case StringComparisonType.NotContains:
                    {
                        return NotContainsEquation(equation,codeIndexerExpression);
                    }
                case StringComparisonType.EndsWith:
                    {
                        return EndsWithEquation(equation,codeIndexerExpression);
                    }
                case StringComparisonType.NotEndsWith:
                    {
                        return NotEndsWithEquation(equation,codeIndexerExpression);
                    }
                case StringComparisonType.StartsWith:
                    {
                        return StartsWithEquation(equation,codeIndexerExpression);
                    }
                case StringComparisonType.NotStartsWith:
                    {
                        return NotStartsWithEquation(equation,codeIndexerExpression);
                    }
            }

            return null;
        }

        private CodeExpression GetDictionaryWithKeyCompareToExpression(CodeIndexerExpression codeIndexerExpression,Equation equation,
            object value, bool isEnum, bool isDate, bool isTime, bool isString, ComparisonType comparisonType, Type propertyType)
        {

            CodeExpression expression = null;

            string valueToCompare = equation.ValueProperty.Value.ToString();

            if (isEnum)
            {
                expression = ProcessEnumDictionaryWithKey(codeIndexerExpression, valueToCompare,propertyType, equation);
            }
            else
            {
                if (isDate || isTime)
                {
                    expression = ProcessDateOrTimeDictionaryWithKey(codeIndexerExpression, valueToCompare, isDate, isTime, comparisonType);
                }
                else
                    if (isString)
                    {
                        expression = ProcessStringDictionaryWithKey(codeIndexerExpression, valueToCompare, comparisonType);
                    }
                    //any other type we don't need to handle any special way yet, however casting may be an issue
                    //we will see at execution time.
                    else
                    {
                        expression = new CompareTo(codeIndexerExpression,
                           GetConvertMethodInvocation(propertyType,valueToCompare),
                           TranslationHelper.CompareToValidEqualityTypes(equation.OperatorProperty.Operator));
                    }
            }

            return expression;
        }

        private CodeExpression GetDictionaryWithKeyCompareToInExpression(CodeIndexerExpression codeIndexerExpression, Equation equation,
    object value, bool isEnum, bool isDate, bool isTime, bool isString, ComparisonType comparisonType, Type propertyType)
        {
            CodeExpression expression = null;

            //not supported for In operator only string type
            if (isEnum)
            {
                expression = ProcessEnumInDictionaryWithKey(codeIndexerExpression, equation, propertyType);
            }
            else
            {
                //not supported for In operator only string type
                if (isDate || isTime)
                {
                    expression = ProcessDateOrTimeInDictionaryWithKey(codeIndexerExpression, equation, isDate, isTime, comparisonType);
                }
                else
                    if (isString)
                    {
                        expression = ProcessStringInDictionaryWithKey(codeIndexerExpression, equation, comparisonType);
                    }
                    else
                    {
                        //not supported for In operator only string types
                        expression = new CompareTo(codeIndexerExpression,
                           GetConvertInMethodInvocation(propertyType, equation),
                           TranslationHelper.CompareToValidEqualityTypes(equation.OperatorProperty.Operator));
                    }
            }            

            return expression;
        }

        private CodeExpression GetConvertInMethodInvocation(Type propertyType,Equation equation)
        {
            throw new NotImplementedException();
        }

        private CodeExpression ProcessStringInDictionaryWithKey(CodeIndexerExpression codeIndexerExpression, Equation equation, ComparisonType comparisonType)
        {
            //split the values first

            string commaSeparatedValues = equation.ValueProperty.Value.ToString();

            if (String.IsNullOrEmpty(commaSeparatedValues))
            {
                throw new ApplicationException("In expects a string with comma separated values, this string is null or empty, cannot process In operator for collection");
            }

            string[] splitStringvalues = commaSeparatedValues.Split(TranslationHelper.IN_DELIMITER);

            string[] repeatPropertyName = new string[splitStringvalues.Length];

            int counter = 0;

            // then just repeat the property name because we need to do a "if this item.Property = value1 OR item.SameProperty = value2 OR item.SameProperty = value3 etc..."
            foreach (string value in splitStringvalues)
            {
                repeatPropertyName[counter] = equation.FieldProperty.PropertyName;
                counter++;
            }

            //there will always be one less clause than there are equations
            int clauseCount = repeatPropertyName.Length - 1;

            string[] clauses = new string[clauseCount];

            for (int i = 0; i < clauseCount; i++)
            {
                clauses[i] = ClauseTypes.Or.ToString();
            }

            int setTypeCount = repeatPropertyName.Length;

            string[] setTypes = new string[setTypeCount];

            for (int i = 0; i < setTypeCount; i++)
            {
                setTypes[i] = equation.FieldProperty.MetaDataField.DataType.ToString();
            }

            int operatorTypeCount = repeatPropertyName.Length;

            string[] operatorTypes = new string[repeatPropertyName.Length];

            for (int i = 0; i < operatorTypeCount; i++)
            {
                operatorTypes[i] = TranslationHelper.OperatorValueTranslation(equation.OperatorProperty.Operator);
            }

           
            //invocation type is any one dynamic if any one of the property that we are inspecting has the value value1 OR value2 OR value3 then
            //return true else return false
            CodeExpression expression = GetSquareBracketEquationGroupExpression(codeIndexerExpression,
                InvocationTypes.AsAnyOneDynamic,
                repeatPropertyName,
                operatorTypes,
                splitStringvalues,
                clauses,
                setTypes);


            return expression;
        }

        private CodeExpression ProcessDateOrTimeInDictionaryWithKey(CodeIndexerExpression codeIndexerExpression, Equation equation, bool isDate, bool isTime, ComparisonType comparisonType)
        {
            throw new NotImplementedException();
        }

        private CodeExpression ProcessEnumInDictionaryWithKey(CodeIndexerExpression codeIndexerExpression, Equation equation, Type propertyType)
        {
            throw new NotImplementedException();
        }


        private CodeExpression ProcessStringDictionaryWithKey(CodeIndexerExpression codeIndexerExpression, string valueToCompare, ComparisonType comparisonType)
        {
            //we only do wildcard searches for the Contains operator at this time
            CodeExpression expression = null;
            bool caseSensitive = false;
                               
            expression = new StringCompare(codeIndexerExpression,
               new CodePrimitiveExpression(valueToCompare),
               caseSensitive,
               comparisonType);            

            return expression;
        }

        private CodeExpression ProcessDateOrTimeDictionaryWithKey(CodeIndexerExpression codeIndexerExpression, string valueToCompare, bool isDate, bool isTime, ComparisonType comparisonType)
        {
            CodeMethodInvokeExpression invokeDateTimeParseValueToCompare = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(System.DateTime)),
               "Parse",
               new CodeExpression[] { new CodePrimitiveExpression(valueToCompare) });

            CodeExpression expression = null;

            if (isDate)
            {

                CodePropertyReferenceExpression dateProperty = new CodePropertyReferenceExpression(invokeDateTimeParseValueToCompare, "Date");

                CodePropertyReferenceExpression codeIndexerRefDateProperty = new CodePropertyReferenceExpression(codeIndexerExpression,
                    "Date");

                expression = new CompareTo(codeIndexerRefDateProperty, dateProperty, comparisonType);
            }

            if (isTime)
            {
                CodePropertyReferenceExpression codeIndexerTimeOfDayPropertyRef = new CodePropertyReferenceExpression(codeIndexerExpression,
                    "TimeOfDay");
                CodePropertyReferenceExpression valueToCompareTimeOfDayPropertyRef = new CodePropertyReferenceExpression(invokeDateTimeParseValueToCompare,
                    "TimeOfDay");

                expression = new CompareTo(codeIndexerTimeOfDayPropertyRef, valueToCompareTimeOfDayPropertyRef, comparisonType);
            }

            return expression;
        }

        private CodeExpression ProcessEnumDictionaryWithKey(CodeIndexerExpression codeIndexerExpression, string valueToCompare, Type propertyType, Equation equation)
        {
            ComparisonType comparisonType = TranslationHelper.CompareToValidEqualityTypes(equation.OperatorProperty.Operator);
            valueToCompare = GetEnumValueToCompareIfStringEnum(propertyType.AssemblyQualifiedName, equation);

            CodeExpression leftExpression = codeIndexerExpression;

            CodeExpression rightExpression =
                   new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(propertyType),
                       Enum.Parse(propertyType, valueToCompare).ToString());

            CodeExpression expression = new CompareTo(leftExpression,
             rightExpression, comparisonType);

            return expression;
        }

       
        private CodeExpression ProcessNonCollection(Equation equation,
            ComparisonType comparisonType, bool isEnum, bool isDate, bool isTime, bool isString)
        {
            CodeExpression expression = null;           

            string valueToCompare = equation.ValueProperty.Value.ToString();

            string propertyName = equation.FieldProperty.PropertyName;

            if (isEnum)
            {
                if (equation.FieldProperty.MetaDataField.InvocationType == InvocationTypes.AsProperty)
                {
                    CodePropertyReferenceExpression propertyReferenceExpression = _codeDomObject.PropertyReferenceExpressions[propertyName];
                    Type propertyType = _codeDomObject.PropertyNameToConcretePropertyType[propertyName];
                    expression = ProcessEnumProperty(valueToCompare, propertyType, equation, propertyReferenceExpression, comparisonType);
                }
                else
                {
                    if (equation.FieldProperty.MetaDataField.InvocationType == InvocationTypes.AsMethodWithDefinedValueAsParameter)
                    {
                        throw new ApplicationException("we do not support an enum as a parameter to a method at this time");
                    }
                }
            }
            else
            {
                if (isDate || isTime)
                {
                    if (equation.FieldProperty.MetaDataField.InvocationType == InvocationTypes.AsProperty)
                    {
                        CodePropertyReferenceExpression propertyReferenceExpression = _codeDomObject.PropertyReferenceExpressions[propertyName];
                        Type propertyType = _codeDomObject.PropertyNameToConcretePropertyType[propertyName];
                        expression = ProcessDateOrTimeProperty(propertyReferenceExpression, valueToCompare, comparisonType, isDate, isTime);
                    }
                    else
                        if (equation.FieldProperty.MetaDataField.InvocationType == InvocationTypes.AsMethodWithDefinedValueAsParameter)
                    {                    
                        CodeMethodReferenceExpression methodReferenceExpression = _codeDomObject.MethodReferenceExpressions[propertyName];
                        expression = ProcessDateOrTimeMethodCallWithValueAsParameter(methodReferenceExpression, valueToCompare, isDate, isTime);
                    }
                }
                
                else
                    if (isString)
                    {
                        //only string type fields can be used with the In comparison type, and it must be a "comparable" rather than "contains,startswith,endswith etc"

                        if (comparisonType == ComparisonType.In || comparisonType == ComparisonType.NotIn)
                        {
                            if (equation.FieldProperty.MetaDataField.InvocationType == InvocationTypes.AsProperty)
                            {
                                CodePropertyReferenceExpression propertyReferenceExpression = _codeDomObject.PropertyReferenceExpressions[propertyName];
                                Type propertyType = _codeDomObject.PropertyNameToConcretePropertyType[propertyName];
                                expression = new In(propertyReferenceExpression, valueToCompare);                                
                            }
                            else
                                if (equation.FieldProperty.MetaDataField.InvocationType == InvocationTypes.AsMethodWithDefinedValueAsParameter)
                                {
                                   CodeMethodReferenceExpression methodReferenceExpression = _codeDomObject.MethodReferenceExpressions[propertyName];

                                   expression = new In(methodReferenceExpression, valueToCompare);
                                }
                            if (comparisonType == ComparisonType.NotIn)
                            {
                                expression = new LogicalNOT(expression);
                            }

                        }
                        else
                        {
                            if (equation.FieldProperty.MetaDataField.InvocationType == InvocationTypes.AsProperty)
                            {
                                CodePropertyReferenceExpression propertyReferenceExpression = _codeDomObject.PropertyReferenceExpressions[propertyName];
                                Type propertyType = _codeDomObject.PropertyNameToConcretePropertyType[propertyName];
                                expression = ProcessStringProperty(propertyReferenceExpression, valueToCompare, comparisonType);
                            }
                            else
                                if (equation.FieldProperty.MetaDataField.InvocationType == InvocationTypes.AsMethodWithDefinedValueAsParameter)
                                {
                                    CodeMethodReferenceExpression methodReferenceExpression = _codeDomObject.MethodReferenceExpressions[propertyName];
                                    expression = ProcessStringMethodCallWithValueAsParameter(methodReferenceExpression, valueToCompare);

                                }
                        }
                    }
                    else
                    {
                        if (equation.FieldProperty.MetaDataField.InvocationType == InvocationTypes.AsProperty)
                        {
                            CodePropertyReferenceExpression propertyReferenceExpression = _codeDomObject.PropertyReferenceExpressions[propertyName];
                            Type propertyType = _codeDomObject.PropertyNameToConcretePropertyType[propertyName];

                            expression = new CompareTo(propertyReferenceExpression,
                               GetConvertMethodInvocation(propertyType,valueToCompare),
                               comparisonType);
                        }

                         else
                            if (equation.FieldProperty.MetaDataField.InvocationType == InvocationTypes.AsMethodWithDefinedValueAsParameter)
                            {
                                CodeMethodReferenceExpression methodReferenceExpression = _codeDomObject.MethodReferenceExpressions[propertyName];
                                //a method invoke expression with the parameter should only return a boolean
                                CodeMethodInvokeExpression invoke = new CodeMethodInvokeExpression(methodReferenceExpression,
                                    new CodeExpression[] { new CodePrimitiveExpression(valueToCompare) });

                                expression = new SingleBooleanResultExpression(invoke);
                            }                            
                    }
            }
            return expression;
        }

        private CodeExpression GetConvertMethodInvocation(Type propertyType, string valueToCompare)
        {
            CodeExpression expression = null;

            string convert = "System.Convert";
            string convertMethodBasedOnType = null;

            if (propertyType.Equals(typeof(String)) ||
                propertyType.Equals(typeof(string)))
            {
                convertMethodBasedOnType = "ToString";
            }
            else
                if (propertyType.Equals(typeof(Boolean)) ||
                    propertyType.Equals(typeof(bool)))
                {
                    convertMethodBasedOnType = "ToBoolean";
                }
                else
                    if (propertyType.Equals(typeof(Decimal)) ||
                        propertyType.Equals(typeof(decimal)))
                    {
                        convertMethodBasedOnType = "ToDecimal";
                    }
                    else
                        if (propertyType.Equals(typeof(Double)) ||
                            propertyType.Equals(typeof(double)))
                        {
                            convertMethodBasedOnType = "ToDouble";
                        }
                        else
                            if (propertyType.Equals(typeof(short ))||
                                propertyType.Equals(typeof(Int16)))
                            {
                                convertMethodBasedOnType = "ToInt16";
                            }
                            else
                                if (propertyType.Equals(typeof(int ))||
                                    propertyType.Equals(typeof(Int32)))
                                {
                                    convertMethodBasedOnType = "ToInt32";
                                }
                                else
                                    if (propertyType.Equals(typeof(Int64)) ||
                                        propertyType.Equals(typeof(long)))
                                    {
                                        convertMethodBasedOnType = "ToInt64";
                                    }
                                    else
                                        if (propertyType.Equals(typeof(Single)) ||
                                            propertyType.Equals(typeof(float)))
                                        {
                                            convertMethodBasedOnType = "ToSingle";
                                        }

            CodePrimitiveExpression valuePrimitiveExpression = new CodePrimitiveExpression(valueToCompare);

            CodeMethodInvokeExpression invoke = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(convert),
                convertMethodBasedOnType,
                new CodeExpression[] { valuePrimitiveExpression });

            expression = invoke;

            return expression;
        }

        private CodeExpression ProcessStringMethodCallWithValueAsParameter(CodeMethodReferenceExpression methodReferenceExpression, string valueToCompare)
        {
            CodeMethodInvokeExpression invoke = new CodeMethodInvokeExpression(methodReferenceExpression, new CodeExpression[] { new CodePrimitiveExpression(valueToCompare) });
            SingleBooleanResultExpression expression = new SingleBooleanResultExpression(invoke);
            return expression;
        }

        private CodeExpression ProcessDateOrTimeMethodCallWithValueAsParameter(CodeMethodReferenceExpression methodReferenceExpression, string valueToCompare, bool isDate, bool isTime)
        {
            CodeMethodInvokeExpression invokeDateTimeParseValueToCompare = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(System.DateTime)),
     "Parse",
     new CodeExpression[] { new CodePrimitiveExpression(valueToCompare) });

            CodeExpression expression = null;
                        
            CodeMethodInvokeExpression invoke = new CodeMethodInvokeExpression(methodReferenceExpression, new CodeExpression[] { invokeDateTimeParseValueToCompare });
            expression = new SingleBooleanResultExpression(invoke);
           
            return expression;
        }

       

        private CodeExpression ProcessStringProperty(CodePropertyReferenceExpression propertyReferenceExpression, string valueToCompare, ComparisonType comparisonType)
        {
            CodeExpression expression = null;
            
            //we do not support wildcard search for equals, starts with ends with etc only contains is processed using a wildcard search
            //this is just a business rule for Centris really and so the following code would process this if it was equal comparison
            //type using the WildCardSearchExpression - keeping this here as an example

            #region example of wildcard search

            ////this check makes sure it is compatible with wildcard search expression
            //if (valueToCompare.Contains('*') ||
            //    valueToCompare.Contains('?') && comparisonType == ComparisonType.Equal)
            //{
            //    bool caseSensitive = false;
            //    //then this is a wild card search expression
            //    expression = new WildCardSearchExpression(propertyReferenceExpression,
            //        valueToCompare,
            //        comparisonType,
            //        caseSensitive);
            //}
            //else
            //{

            //code cast expression not necessary valueToCompare is a string type                           
            //expression = new CompareTo(propertyReferenceExpression,
            //   new CodePrimitiveExpression(valueToCompare),
            //   comparisonType);
            //}

            #endregion example of wildcard search

            bool caseSensitive = false;

            //code cast expression not necessary valueToCompare is a string type                           
            expression = new StringCompare(propertyReferenceExpression,
               new CodePrimitiveExpression(valueToCompare),
               caseSensitive,
               comparisonType);          

            return expression;
        }

        private CodeExpression ProcessDateOrTimeProperty(CodePropertyReferenceExpression propertyReferenceExpression,
            string valueToCompare, 
            ComparisonType comparisonType,bool isDate, bool isTime)
        {

            CodeMethodInvokeExpression invokeDateTimeParseValueToCompare = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(System.DateTime)),
                "Parse",
                new CodeExpression[] { new CodePrimitiveExpression(valueToCompare) });

            CodeExpression expression = null;

            if(isDate)
            {
                CodePropertyReferenceExpression dateProperty = new CodePropertyReferenceExpression(invokeDateTimeParseValueToCompare, "Date");

                CodePropertyReferenceExpression propertyRefDateProperty = new CodePropertyReferenceExpression(propertyReferenceExpression,
                    "Date");

                expression = new CompareTo(propertyRefDateProperty, dateProperty, comparisonType);
            }

            if (isTime)
            {
                CodePropertyReferenceExpression propertyTimeOfDayPropertyRef = new CodePropertyReferenceExpression(propertyReferenceExpression,
                    "TimeOfDay");
                CodePropertyReferenceExpression valueToCompareTimeOfDayPropertyRef = new CodePropertyReferenceExpression(invokeDateTimeParseValueToCompare,
                    "TimeOfDay");

                expression = new CompareTo(propertyTimeOfDayPropertyRef, valueToCompareTimeOfDayPropertyRef, comparisonType);
            }

            return expression;
        }

        private void DetermineType(ref bool isEnum, ref bool isDate, ref bool isTime, ref bool isString,  Equation equation)
        {
           
            if (equation.FieldProperty.DataType == SetTypes.Enum)
            {
                isEnum = true;
                return;
            }
            if (equation.FieldProperty.DataType == SetTypes.Date)
            {
                isDate = true;
                return;
            }
            if (equation.FieldProperty.DataType == SetTypes.Time)
            {
                isTime = true;
                return;
            }
            if (equation.FieldProperty.DataType == SetTypes.String)
            {
                isString = true;
                return;
            }
        }

        private CodeExpression ProcessEnumProperty(string valueToCompare, Type propertyType, Equation equation, CodePropertyReferenceExpression propertyReferenceExpression,
            ComparisonType comparisonType)
        {
            valueToCompare = GetEnumValueToCompareIfStringEnum(propertyType.AssemblyQualifiedName, equation);

            CodeExpression leftExpression = propertyReferenceExpression;

            CodeExpression rightExpression =
                   new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(propertyType),
                       Enum.Parse(propertyType, valueToCompare).ToString());

            CodeExpression expression = new CompareTo(leftExpression,
             rightExpression, comparisonType);

            return expression;
        }

        private string GetEnumValueToCompareIfStringEnum( string  enumTypeName,
            Equation equation)
        {
            string valueToCompare = equation.ValueProperty.Value.ToString();
            //then this is an enum type

            //for information about how to use the StringEnum class please see the region in the StringEnum class labelled
            //"examples of how to use this class" in general this is how we would allow a developer to create a string enumeration
            //.net does not natively support this so we have to use a custom implementation, developer would dress each
            //enum value in their enum with the "string" attribute that would be found in the XML written from the UI
            if (StringEnum.IsStringDefined(Type.GetType(enumTypeName), valueToCompare))
            {
                //if this is true then this is a string enumeration which means that the attribute on the enumerated value is
                //what would be found in the xml - .net does not support string enums just numeric enums

                //converts back to an enum value from String Value (case insensitive)
                //and now shows the actual enum string from the attribute which is what is fed to the fieldreferenceexpression

                //valueToCompare = StringEnum.Parse(Type.GetType(enumTypeName),
                //    equation.ValueProperty.Value.ToString(), false).ToString();

                //if we want the string value
                valueToCompare = StringEnum.Parse(Type.GetType(enumTypeName),
                    equation.ValueProperty.Value.ToString(), false).ToString();

                //if we are in this block it is because there is a string attribute defined and this is a string
                //enum in that case we want the numeric value as a string 

                valueToCompare = Enum.Parse(Type.GetType(enumTypeName), valueToCompare).ToString();
              
            }           

            return valueToCompare;
        }       
    }

    public class TranslationValidationResult
    {
    	private bool ruleCanBeEvaluated;
		public bool CanBeEvaluated
		{
			get
			{
				return ruleCanBeEvaluated;
			}
			set
			{
				ruleCanBeEvaluated = value;
			}
		}

		private bool ruleCanBeRuntimeExecuted;
		public bool CanBeRuntimeExecuted
		{
			get
			{
				return ruleCanBeRuntimeExecuted;
			}
			set
			{
				ruleCanBeRuntimeExecuted = value;
			}
		}
		    	
		List<string> _rulePropertyNamesNotInProcessor = null;
		public List<string> RulePropertyNamesNotInProcessor
		{
			get { return _rulePropertyNamesNotInProcessor; }
			set { _rulePropertyNamesNotInProcessor = value; }
		}
		
		List<string> _ruleFieldsNotInConfiguration = null;
		public List<string> RuleFieldsNotInConfiguration
		{
			get { return _ruleFieldsNotInConfiguration; }
			set { _ruleFieldsNotInConfiguration = value; }
		}

		List<string> _ruleActionsNotInProcessor = null;
		public List<string> RuleActionsNotInProcessor
		{
			get { return _ruleActionsNotInProcessor; }
			set { _ruleActionsNotInProcessor = value; }
		}
		
        public TranslationValidationResult()
        {
			ruleCanBeEvaluated = true;
			ruleCanBeRuntimeExecuted = true;
        	
			_rulePropertyNamesNotInProcessor = new List<string>();
			_ruleFieldsNotInConfiguration = new List<string>();
			_ruleActionsNotInProcessor = new List<string>();
        }
    }
}
