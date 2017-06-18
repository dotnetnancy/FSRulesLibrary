using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Activities.Rules;
using System.CodeDom;
using System.Workflow.ComponentModel.Compiler;

namespace DotNetNancy.Rules.RuleSet.MSRuleSetTranslation
{
    public class InsertRuleStatsAction : RuleAction
    {
        CodeExpression _applicationID = null;

        public CodeExpression ApplicationID
        {
            get { return _applicationID; }
            set { _applicationID = value; }
        }

        CodeExpression _referenceID = null;

        public CodeExpression ReferenceID
        {
            get { return _referenceID; }
            set { _referenceID = value; }
        }        

        CodeExpression _typeID = null;

        public CodeExpression TypeID
        {
            get { return _typeID; }
            set { _typeID = value; }
        }
        CodeExpression _ruleID = null;

        public CodeExpression RuleID
        {
            get { return _ruleID; }
            set { _ruleID = value; }
        }
        CodeExpression _ruleName = null;

        public CodeExpression RuleName
        {
            get { return _ruleName; }
            set { _ruleName = value; }
        }
        CodeExpression _result = null;

        public CodeExpression Result
        {
            get { return _result; }
            set { _result = value; }
        }

        CodeExpression _createDate = null;

        public CodeExpression CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }
      
        public InsertRuleStatsAction()
        {
            // constructor required for deserialization
        }

        public InsertRuleStatsAction(CodeExpression applicationID,CodeExpression typeID,
            CodeExpression ruleID, CodeExpression ruleName, CodeExpression result, CodeExpression createDate,
            CodeExpression referenceID)
        {
            // constructor required by parser
            _applicationID = applicationID;
            _typeID = typeID;
            _ruleID = ruleID;
            _ruleName = ruleName;
            _result = result;
            _createDate = createDate;
            _referenceID = referenceID;
        }

        public override bool Validate(RuleValidation validator)
        {
            ValidationError error;
            if (_applicationID == null || _typeID == null || _ruleID == null || _ruleName == null || _result == null || _createDate == null || _referenceID == null)
            {
                error = new ValidationError("Rule Stat Parameters cannot be null", 123);
                validator.Errors.Add(error);
                return false;
            }
            else
            {
                RuleExpressionInfo applicationIDResult = RuleExpressionWalker.Validate(validator, _applicationID, false);
                RuleExpressionInfo typeIDResult = RuleExpressionWalker.Validate(validator, _typeID, false);
                RuleExpressionInfo ruleIDResult = RuleExpressionWalker.Validate(validator, _ruleID, false);
                RuleExpressionInfo ruleNameResult = RuleExpressionWalker.Validate(validator, _ruleName, false);
                RuleExpressionInfo resultResult = RuleExpressionWalker.Validate(validator, _result, false);
                RuleExpressionInfo createDateResult = RuleExpressionWalker.Validate(validator, _createDate, false);
                RuleExpressionInfo referenceIDResult = RuleExpressionWalker.Validate(validator, _referenceID, false);

                if ((applicationIDResult == null) || (applicationIDResult.ExpressionType != typeof(Guid)))
                {
                    error = new ValidationError("application id must be Guid", 123);
                    validator.Errors.Add(error);
                    return false;
                }

                if ((typeIDResult == null) || (typeIDResult.ExpressionType != typeof(Guid)))
                {
                    error = new ValidationError("type id must be Guid", 123);
                    validator.Errors.Add(error);
                    return false;
                }


                if ((ruleIDResult == null) || (ruleIDResult.ExpressionType != typeof(Guid)))
                {
                    error = new ValidationError("rule id must be a guid", 123);
                    validator.Errors.Add(error);
                    return false;
                }

                if ((ruleNameResult == null) || (ruleNameResult.ExpressionType != typeof(string)))
                {
                    error = new ValidationError("rule name must be a string", 123);
                    validator.Errors.Add(error);
                    return false;
                }

                if ((resultResult == null) || (resultResult.ExpressionType != typeof(bool)))
                {
                    error = new ValidationError("result must be a true or false", 123);
                    validator.Errors.Add(error);
                    return false;
                }
              
                if ((createDateResult == null) || (createDateResult.ExpressionType != typeof(DateTime)))
                {
                    error = new ValidationError("create date must be a DateTime", 123);
                    return false;
                }

                if ((referenceIDResult == null) || (referenceIDResult.ExpressionType != typeof(Guid)))
                {
                    error = new ValidationError("reference id must be Guid", 123);
                    validator.Errors.Add(error);
                    return false;
                }
  
           
            }
            return (validator.Errors.Count == 0);
        }

        public override RuleAction Clone()
        {
            InsertRuleStatsAction result = new InsertRuleStatsAction();
            result.ApplicationID = RuleExpressionWalker.Clone(_applicationID);
            result.TypeID = RuleExpressionWalker.Clone(_typeID);
            result.Result = RuleExpressionWalker.Clone(_result);
            result.RuleID = RuleExpressionWalker.Clone(_ruleID);
            result.RuleName = RuleExpressionWalker.Clone(_ruleName);
            result.CreateDate = RuleExpressionWalker.Clone(_createDate);
            result.ReferenceID = RuleExpressionWalker.Clone(_referenceID);
            return result;
        }

        public override void Execute(RuleExecution context)
        {
            //we passed in at definition time what the code expressions for various properties are
            //then they are evaluated so this.PropertyName for example would now be "the value of the property" if we passed
            //in a CodePrimitiveExpression like "true" or "false" or "1" etc those values would now be here

            RuleExpressionResult applicationIDResult = RuleExpressionWalker.Evaluate(context, _applicationID);
            RuleExpressionResult typeIDResult = RuleExpressionWalker.Evaluate(context, _typeID);
            RuleExpressionResult ruleIDResult = RuleExpressionWalker.Evaluate(context, _ruleID);
            RuleExpressionResult ruleNameResult = RuleExpressionWalker.Evaluate(context, _ruleName);
            RuleExpressionResult resultResult = RuleExpressionWalker.Evaluate(context, _result);
            RuleExpressionResult createDateResult = RuleExpressionWalker.Evaluate(context, _createDate);
            RuleExpressionResult referenceIDResult = RuleExpressionWalker.Evaluate(context, _referenceID);

            
            //this is where the work is actually done 
            if (applicationIDResult != null && ruleIDResult != null && ruleNameResult != null && resultResult != null 
                && createDateResult != null)
            {
                RuleStats ruleStats = new RuleStats();

                ruleStats.Insert((Guid)applicationIDResult.Value, (Guid)typeIDResult.Value,(Guid)ruleIDResult.Value, (string)ruleNameResult.Value, (bool)resultResult.Value,(DateTime)createDateResult.Value,
                    (Guid)referenceIDResult.Value);
            }
                
        }

        public override ICollection<string> GetSideEffects(RuleValidation validation)
        {
            RuleAnalysis analysis = new RuleAnalysis(validation, true);

            if (_applicationID != null)
                RuleExpressionWalker.AnalyzeUsage(analysis, _applicationID, true, false, null);
            if (_typeID != null)
                RuleExpressionWalker.AnalyzeUsage(analysis, _typeID, true, false, null);

            if (_result != null)
                RuleExpressionWalker.AnalyzeUsage(analysis, _result, true, false, null);

            if (_ruleID != null)
                RuleExpressionWalker.AnalyzeUsage(analysis, _ruleID, true, false, null);

            if (_ruleName != null)
                RuleExpressionWalker.AnalyzeUsage(analysis, _ruleName, true, false, null);

            if (_createDate != null)
                RuleExpressionWalker.AnalyzeUsage(analysis, _createDate, true, false, null);

            if (_referenceID != null)
                RuleExpressionWalker.AnalyzeUsage(analysis, _referenceID, true, false, null);

            return analysis.GetSymbols();
        }

        public override string ToString()
        {
            // what should be displayed by the parser
            StringBuilder result = new StringBuilder("InsertRuleStat(");
            RuleExpressionWalker.Decompile(result, _applicationID, null);
            result.Append(", ");
            RuleExpressionWalker.Decompile(result, _typeID, null);
            result.Append(", ");
            RuleExpressionWalker.Decompile(result, _ruleID, null);
            result.Append(", ");
            RuleExpressionWalker.Decompile(result, _ruleName, null);
            result.Append(", ");
            RuleExpressionWalker.Decompile(result, _result, null);
            result.Append(", ");
            RuleExpressionWalker.Decompile(result, _createDate, null);
            result.Append(", ");
            RuleExpressionWalker.Decompile(result, _referenceID, null);
            result.Append(")");
            return result.ToString();
        }
    }
}



