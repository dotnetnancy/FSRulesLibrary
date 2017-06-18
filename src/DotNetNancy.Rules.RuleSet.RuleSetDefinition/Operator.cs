using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{
	[DataContract]
	public class Operator
	{
		public string Name { get; set; }
		public string Value { get; set; }
		public RuleElementTypes Type { get; set; }
		public string OperatorSet { get; set; }		

		public Operator()
		{
            Type = RuleElementTypes.Operator;
		}
	}
}