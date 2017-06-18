using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetNancy.Rules.RuleSet.Translation
{
    public enum OperatorTypes
    {
        Is = 1,
        Not = 2,
        Contains = 3,
        NoContains = 4,
        Starts = 5,
        NoStarts = 6,
        Ends = 7,
        NoEnds = 8,
        Less = 9,
        LessIs = 10,
        More = 11,
        MoreIs = 12,
        In=13,
        NotIn,
        NotSupported = 14

    }
}
