using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetNancy.Rules.RuleSet.DataAccess;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{

    public static class RuleSetDefinitionHelper
    {
        public static DateTime GetMostRecentUpdatedDate(string dkNumber, out Guid hierarchyID, out int branchID)
        {
            DateTime result = DateTime.MinValue;
            hierarchyID = Guid.Empty;
            branchID = -1;

            foreach (SqlGeneralProvider provider in DataAccess.InitGeneralProvider.Providers)
            {
                if (provider.IsCurrentBusinessObjectProvider)
                {
                    result = (provider.GetMaxDateTimeByDkNumber(dkNumber, out hierarchyID,out branchID));
                    break;
                }
            }
            return result;
        }
    }
}

