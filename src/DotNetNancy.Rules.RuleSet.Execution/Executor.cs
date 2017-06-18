using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetNancy.Rules.RuleSet.MSRuleSetTranslation;
using System.Workflow.Activities.Rules;
using System.Workflow.ComponentModel.Compiler;
using System.CodeDom;
using System.Reflection;

namespace DotNetNancy.Rules.RuleSet.MSRuleSetExecution
{
    public class Executor<T>
    {
        private log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //may or may not be needed but because it was already instantiated and the reflection has already been done i just pass
        //it around so that the reflection does not need to be done again if we need to inspect the object
        CodeDomObject _codeDomObject = null;
        MSRuleSetTranslationResult _msRuleSetTranslationResult = null;
        T _instanceOfObject;

        public Executor()
        {

        }

        public MSRuleSetExecutionResult Execute(MSRuleSetTranslationResult ruleSetTranslationResult, T instanceOfObject)
        {
            _msRuleSetTranslationResult = ruleSetTranslationResult;
            _instanceOfObject = instanceOfObject;
            _codeDomObject = _msRuleSetTranslationResult.ReferenceToCodeDomObject;

            return ExecuteRuleSet(ruleSetTranslationResult.DotNetRuleSet, true);
        }

        public MSRuleSetExecutionResult ExecuteRuleSet(System.Workflow.Activities.Rules.RuleSet ruleSet, bool translatedRule)
        {
            MSRuleSetExecutionResult ruleSetExecutionResult = null;

            if (translatedRule)
            {
                ruleSetExecutionResult = new MSRuleSetExecutionResult(_msRuleSetTranslationResult);
            }
            else
            {
                ruleSetExecutionResult = new MSRuleSetExecutionResult();
            }

            RuleValidation rv = new RuleValidation(typeof(T), null);

            ruleSet.Validate(rv);

            ValidationErrorCollection errors = rv.Errors;

            if (errors.Count > 0)
            {
                ruleSetExecutionResult.AddRuleSetValidationErrors(errors);
            }
            else
            {
                try
                {
                    ExecuteRuleSet(ruleSet, rv, ref ruleSetExecutionResult);
                }
                catch (RuleException rex)
                {
                    _log.Error("RuleSet Name:  " + ruleSet.Name + "threw a Rule Exception during its execution.  ", rex);
                    ruleSetExecutionResult.AddExecutionError(Guid.Empty.ToString(), ruleSet.Name, rex);
                }
                catch (TargetInvocationException tex)
                {
                    _log.Error("RuleSetName:  " + ruleSet.Name +
                        ", threw a Target Invocation Exception which means that the Wrapper object itself probably threw an error, maybe the rule is targeting a property that is null or not a valid value for comparison", tex);
                    ruleSetExecutionResult.AddExecutionError(Guid.Empty.ToString(), ruleSet.Name, tex);

                }
                catch (Exception ex)
                {
                    _log.Error("RuleSetName:  " + ruleSet.Name + "Unhandled exception during execution of", ex);
                    ruleSetExecutionResult.AddExecutionError(Guid.Empty.ToString(), ruleSet.Name, ex);

                }
                    
            }

            return ruleSetExecutionResult;
        }

        private void ExecuteRuleSet(System.Workflow.Activities.Rules.RuleSet ruleSet, 
            RuleValidation rv,
            ref MSRuleSetExecutionResult ruleSetExecutionResult)
        {
            RuleExecution re = new RuleExecution(rv, _instanceOfObject);
            ruleSet.Execute(re);
            ruleSetExecutionResult.AddExecutionResult(ruleSet.Name, re);
        }

        internal MSRuleSetExecutionResult Execute(System.Workflow.Activities.Rules.RuleSet ruleSet, T instanceOfObject)
        {
            _instanceOfObject = instanceOfObject;
            return ExecuteRuleSet(ruleSet, false);
        }
    }
}
