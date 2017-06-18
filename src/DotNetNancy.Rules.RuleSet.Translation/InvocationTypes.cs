using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetNancy.Rules.RuleSet.Translation
{
    public enum InvocationTypes
    {
        AsProperty = 1,
        AsMethodWithDefinedValueAsParameter = 2,
        AsAnyOneDynamic = 3,
        AsAllDynamic = 4,
        NotSupported = 5
    }
}
