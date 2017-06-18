using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Provider;
using System.Configuration;
using System.Web.Configuration;
using DotNetNancy.Rules.RuleSet.DataAccess;
using DotNetNancy.Rules.RuleSet.DataAccess.DTO;


namespace DotNetNancy.Rules.RuleSet.DataAccess
{
    public static class InitRuleStatsProvider
    {
        public static bool isInitialized = false;

        static InitRuleStatsProvider()
        {
            Initialize();
        }

        public static void Initialize()
        {
            if (!isInitialized)
            {
                SectionConfig qc =
                (SectionConfig)ConfigurationManager.GetSection("GroupSection/RuleStats");
                providerCollection = new ProviderList();
                ProvidersHelper.InstantiateProviders(qc.Providers,
                providerCollection, typeof(RuleStatsProvider));
                providerCollection.SetReadOnly();
                isInitialized = true; //error-free initialization
            }
        }


        private static ProviderList providerCollection;

        public static ProviderList Providers
        {
            get
            {
                return providerCollection;
            }
        }
    }


    public abstract class RuleStatsProvider : ProviderBase
    {
        //get all
        public abstract List<DTO.RuleStatistic> GetRuleStats(Guid applicationID, Guid typeID);
       
        /// <summary>
        /// this will take in by sequence items to insert then insert them, we generate and pass the guid from our code,
        /// returns a dictionary with the rulestats object and whether or not the insert was successful
        /// 
        /// </summary>
        /// <param name="sequenceToRuleStatToInsert"></param>
        /// <returns></returns>
        public abstract bool InsertRuleStats(Guid applicationID, 
            Guid typeID,
            Guid ruleID, 
            string ruleName, 
            bool result, 
            DateTime createDate,
            Guid referenceID);

        public abstract bool IsCurrentBusinessObjectProvider { get; }
        public abstract System.Type GetProviderType { get; }
    }


}


