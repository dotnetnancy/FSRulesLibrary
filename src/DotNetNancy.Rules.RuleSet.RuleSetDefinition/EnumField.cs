using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{
    public class EnumField : Field
    {
        public List<Item> Items { get; set; }

        public EnumField()
            : base()
        {
            Items = new List<Item>();
        }

        public EnumField(List<Item> items)
            : this()
        {
            Items = items;
        }
    }
}
