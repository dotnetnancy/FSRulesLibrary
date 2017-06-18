using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Activities.Rules;
using System.CodeDom;
using System.Workflow.ComponentModel.Compiler;

namespace DotNetNancy.Rules.RuleSet.MSRuleSetTranslation
{
    public class LogRuleAction : RuleAction
    {
        
        CodeExpression _message;

        public CodeExpression Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public LogRuleAction()
        {
            // constructor required for deserialization
        }

        public LogRuleAction(CodeExpression expression)
        {
            // constructor required by parser
            _message = expression;
        }

        public override bool Validate(RuleValidation validator)
        {
            ValidationError error;
            if (_message == null)
            {
                error = new ValidationError("Message cannot be null", 123);
                validator.Errors.Add(error);
                return false;
            }
            else
            {
                RuleExpressionInfo result = RuleExpressionWalker.Validate(validator, _message, false);
                if ((result == null) || (result.ExpressionType != typeof(string)))
                {
                    error = new ValidationError("Message must return string result", 123);
                    validator.Errors.Add(error);
                    return false;
                }
            }
            return (validator.Errors.Count == 0);
        }

        public override RuleAction Clone()
        {
            LogRuleAction result = new LogRuleAction();
            result.Message = RuleExpressionWalker.Clone(_message);
            return result;
        }

        public override void Execute(RuleExecution context)
        {
            RuleExpressionResult result = RuleExpressionWalker.Evaluate(context, _message);
            if (result != null)
                Console.WriteLine(result.Value);
        }

        public override ICollection<string> GetSideEffects(RuleValidation validation)
        {
            RuleAnalysis analysis = new RuleAnalysis(validation, true);
            if (_message != null)
                RuleExpressionWalker.AnalyzeUsage(analysis, _message, true, false, null);
            return analysis.GetSymbols();
        }

        public override string ToString()
        {
            // what should be displayed by the parser
            StringBuilder result = new StringBuilder("Log(");
            RuleExpressionWalker.Decompile(result, _message, null);
            result.Append(")");
            return result.ToString();
        }
    }
}

    

