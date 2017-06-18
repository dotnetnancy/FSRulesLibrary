using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{

    public class DictionaryCollectionMemberField : Field
    {
        public string MemberOfCollection { get; set; }
        public string Key { get; set; }
        public CollectionField CollectionField { get; set; }

        public DictionaryCollectionMemberField()
            : base()
        {
        }

        public DictionaryCollectionMemberField(string collectionMemberOf, string key)
        {
            MemberOfCollection = collectionMemberOf;
            Key = key;
        }
    }
}
