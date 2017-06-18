using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetNancy.Rules.RuleSet.Translation;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{

    public class CollectionField : Field
    {
        public CollectionTypes CollectionType { get; set; }
        public List<Field> CollectionMembersList { get; set; }

        public CollectionField()
            : base()
        {
        }

        public CollectionField(CollectionTypes collectionType)
        {
            CollectionType = collectionType;
        }
    }
}
