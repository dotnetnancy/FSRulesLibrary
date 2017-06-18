using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{    
    public class NameValuePair
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public NameValuePair() { }
        public NameValuePair(string id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
    }
}
