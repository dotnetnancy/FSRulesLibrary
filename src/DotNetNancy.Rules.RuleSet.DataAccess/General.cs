using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetNancy.Rules.RuleSet.DataAccess;
using System.Configuration.Provider;
using System.Web.Configuration;
using System.Configuration;

namespace DotNetNancy.Rules.RuleSet.DataAccess
{
    public class InitGeneralProvider
    {
       protected static bool isInitialized = false;

        static InitGeneralProvider()
        {
            Initialize();
        }

        private static void Initialize()
        {
            SectionConfig qc =
            (SectionConfig)ConfigurationManager.GetSection("GroupSection/General");
            providerCollection = new ProviderList();
            ProvidersHelper.InstantiateProviders(qc.Providers,
            providerCollection, typeof(GeneralProvider));
            providerCollection.SetReadOnly();
            isInitialized = true; //error-free initialization
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


    public abstract class GeneralProvider : ProviderBase
    {
        public abstract DTO.UserStore GetUser(string email, string password, Guid applicationID, Guid typeID);
        public abstract bool IsCurrentBusinessObjectProvider { get; }
        public abstract Type GetProviderType { get; }
    }
}

