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
    public static class InitRuleSetDefinitionsProvider
    {
        public static bool isInitialized = false;

        static InitRuleSetDefinitionsProvider()
        {
            Initialize();
        }

        public static void Initialize()
        {
            if (!isInitialized)
            {
                SectionConfig qc =
                (SectionConfig)ConfigurationManager.GetSection("GroupSection/RuleSetDefinitions");
                providerCollection = new ProviderList();
                ProvidersHelper.InstantiateProviders(qc.Providers,
                providerCollection, typeof(RuleSetDefinitionsProvider));
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


    public abstract class RuleSetDefinitionsProvider : ProviderBase
    {
        //get all
        public abstract List<DTO.RuleDefinition> GetRuleDefinitions(Guid applicationID, Guid typeID);

        //this returns the ruleid
        public abstract Guid InsertRuleDefinition(DTO.RuleDefinition ruleDefinition);
        public abstract void UpdateRuleDefinition(Guid ruleID, Guid applicationID, Guid typeID);
        public abstract void DeleteRuleDefinition(Guid ruleID, Guid applicationID, Guid typeID);

        public abstract bool IsCurrentBusinessObjectProvider { get; }
        public abstract System.Type GetProviderType { get; }
    }


}


