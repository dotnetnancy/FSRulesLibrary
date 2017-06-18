using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetNancy.Rules.RuleSet.Translation
{
    public enum StringComparisonType
    {
        Contains = 1,
        NotContains = 2,
        EndsWith = 3,
        NotEndsWith = 4,
        StartsWith = 5,
        NotStartsWith = 6,
        NotSupported = 7
    }
}
