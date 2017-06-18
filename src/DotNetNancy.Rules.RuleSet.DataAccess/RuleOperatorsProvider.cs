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
    public static class InitRuleOperatorsProvider
    {
        public static bool isInitialized = false;

        static InitRuleOperatorsProvider()
        {
            Initialize();
        }

        public static void Initialize()
        {
            if (!isInitialized)
            {
                SectionConfig qc =
                (SectionConfig)ConfigurationManager.GetSection("GroupSection/RuleOperators");
                providerCollection = new ProviderList();
                ProvidersHelper.InstantiateProviders(qc.Providers,
                providerCollection, typeof(RuleOperatorsProvider));
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


    public abstract class RuleOperatorsProvider : ProviderBase
    {
        public abstract RuleOperators GetData(Guid applicationID, Guid typeID, 
            DotNetNancy.Rules.RuleSet.DataAccess.Enumerations.ConfigurationTypes configurationType);

        public abstract bool IsCurrentBusinessObjectProvider { get; }
        public abstract System.Type GetProviderType { get;}
    }


}


