using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetNancy.Rules.RuleSet.Translation
{
    /// <summary>
    /// when calling compare you are in this instance going to be looking for GreaterThan, LessThan or Equal
    /// This custom enumeration is used internally to this class to determine a boolean result 
    /// </summary>
    /// 
    public enum ComparisonType
    {
        Equal = 0,
        LessThan = -1,
        GreaterThan = 1,
        GreaterThanOrEqual = 2,
        LessThanOrEqual = 3,
        NotEqual = 4,
        In = 5,
        NotIn = 6,
        NotSupported = 7
    }
}
