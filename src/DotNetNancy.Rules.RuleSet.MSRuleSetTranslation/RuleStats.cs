using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetNancy.Rules.RuleSet.DataAccess;

namespace DotNetNancy.Rules.RuleSet.MSRuleSetTranslation
{
    public class RuleStats : List<RuleStat>
    {
        List< DotNetNancy.Rules.RuleSet.DataAccess.DTO.RuleStatistic > _ruleStatsDtos = null;

        public RuleStats()
        {

        }

        public void LoadAll(Guid applicationID, Guid typeID)
        {
            foreach (RuleStatsProvider provider in InitRuleStatsProvider.Providers)
            {
                if (provider.IsCurrentBusinessObjectProvider)
                {
                    Load(provider.GetRuleStats(applicationID,typeID));
                    break;
                }
            }
        }



        public bool Insert(Guid applicationID,
            Guid typeID,
            Guid ruleID,
            string ruleName,
            bool result,
            DateTime createDate,
            Guid referenceID)
        {
            bool success = false;

            foreach (RuleStatsProvider provider in InitRuleStatsProvider.Providers)
            {
                if (provider.IsCurrentBusinessObjectProvider)
                {
                    return provider.InsertRuleStats(applicationID,typeID,ruleID,ruleName,result,createDate,referenceID);                    
                }
            }

            return success;
        }

        private void Load( DotNetNancy.Rules.RuleSet.DataAccess.DTO.RuleStatistic  ruleStatsDto)
        {
            _ruleStatsDtos = new List<DotNetNancy.Rules.RuleSet.DataAccess.DTO.RuleStatistic >();
            _ruleStatsDtos.Add(ruleStatsDto);
            this.Clear();
            this.Add(new RuleStat(ruleStatsDto));
        }

        private void Load(List<DotNetNancy.Rules.RuleSet.DataAccess.DTO.RuleStatistic> ruleStatsDtos)
        {
            _ruleStatsDtos = ruleStatsDtos;
            this.Clear();
            foreach ( DotNetNancy.Rules.RuleSet.DataAccess.DTO.RuleStatistic  ruleStatsDto in ruleStatsDtos)
            {
                this.Add(new RuleStat(ruleStatsDto));
            }
        }

       

        
    }
}
