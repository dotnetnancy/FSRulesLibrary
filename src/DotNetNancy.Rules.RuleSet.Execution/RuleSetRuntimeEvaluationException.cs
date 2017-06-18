using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DotNetNancy.Rules.RuleSet.MSRuleSetExecution
{
    public class RuleSetRuntimeEvaluationException : Exception
    {
        public RuleSetRuntimeEvaluationException()
            : base()
        {
        }

        public RuleSetRuntimeEvaluationException(string message)
            : base(message)
        {
        }

        public RuleSetRuntimeEvaluationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public RuleSetRuntimeEvaluationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        

    }
}
