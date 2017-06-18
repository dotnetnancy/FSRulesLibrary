using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetNancy.Rules.RuleSet.Translation
{
    public enum ClauseTypes
    {
        And = 1,
        Or = 2,
        //not sure what to do with this one as a "clause" however that is the way it is defined in xml
        //TODO: revist this if necessary
        Then = 3,
        NotSupported = 4
    }
}
