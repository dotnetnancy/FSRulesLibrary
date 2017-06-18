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
    public static class InitRuleFieldsProvider
    {
        public static bool isInitialized = false;

        static InitRuleFieldsProvider()
        {
            Initialize();
        }

        public static void Initialize()
        {
            if (!isInitialized)
            {
                SectionConfig qc =
                (SectionConfig)ConfigurationManager.GetSection("GroupSection/RuleFields");
                providerCollection = new ProviderList();
                ProvidersHelper.InstantiateProviders(qc.Providers,
                providerCollection, typeof(RuleFieldsProvider));
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


    public abstract class RuleFieldsProvider : ProviderBase
    {
        public abstract RuleFields GetData(Guid applicationID, Guid typeID,
            DotNetNancy.Rules.RuleSet.DataAccess.Enumerations.ConfigurationTypes configurationType);

        public abstract bool IsCurrentBusinessObjectProvider { get; }
        public abstract System.Type GetProviderType { get;}
    }


}


