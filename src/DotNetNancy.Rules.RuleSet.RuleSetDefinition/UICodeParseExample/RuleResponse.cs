using System;
using System.Collections.Generic;

namespace Centris.Common.Data
{
	[Serializable]
	public class RuleResponse
	{
		public string Initial { get; set; }
		public List<RuleElement> Clauses { get; set; }
		public List<RuleElement> Operators { get; set; }
		public List<RuleElement> Actions { get; set; }
		public List<RuleElement> Fields { get; set; }

		public RuleResponse()
		{
			this.Actions = new List<RuleElement>();
			this.Clauses = new List<RuleElement>();
			this.Fields = new List<RuleElement>();
			this.Operators = new List<RuleElement>();
			this.Initial = "If";
		}
	}
}