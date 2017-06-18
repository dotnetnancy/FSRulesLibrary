using System;
using System.Collections.Generic;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{
	public class RuleOperatorSet
	{
		public string Set { get; set; }
		public List<Operator> Operators { get; set; }

		public RuleOperatorSet() { this.Operators = new List<Operator>(); }
	}
}