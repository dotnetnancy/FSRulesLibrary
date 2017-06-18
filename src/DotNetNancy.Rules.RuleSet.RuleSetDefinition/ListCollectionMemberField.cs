using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{
    public class EnumerableCollectionMemberField : Field
    {
        public string MemberOfCollection { get; set; }        
        public CollectionField CollectionField { get; set; }

        public EnumerableCollectionMemberField()
            : base()
        {
        }

        public EnumerableCollectionMemberField(string collectionMemberOf, string key)
        {
            MemberOfCollection = collectionMemberOf;           
        }
    }
}
