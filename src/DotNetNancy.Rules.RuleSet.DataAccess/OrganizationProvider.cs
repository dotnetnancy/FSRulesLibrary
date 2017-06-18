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
    public static class InitOrganizationsProvider
    {
        public static bool isInitialized = false;

        static InitOrganizationsProvider()
        {
            Initialize();
        }

        public static void Initialize()
        {
            if (!isInitialized)
            {
                SectionConfig qc =
                (SectionConfig)ConfigurationManager.GetSection("GroupSection/Organizations");
                providerCollection = new ProviderList();
                ProvidersHelper.InstantiateProviders(qc.Providers,
                providerCollection, typeof(OrganizationsProvider));
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


    public abstract class OrganizationsProvider : ProviderBase
    {     
        public abstract OrganizationData GetOrganizationByDkNumber(string dkNumber);
        public abstract OrganizationData GetOrganizationByOrganizationID(Guid organizationID);

        public abstract bool IsCurrentBusinessObjectProvider { get; }
        public abstract Type GetProviderType { get; }
    }


}


