using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace DotNetNancy.Rules.RuleSet.DataAccess
{   
        public class SectionConfig : ConfigurationSection
        {
            /// <summary>
            /// this will read all providers defined in the web.config
            /// </summary>
            [ConfigurationProperty("providers")]
            public ProviderSettingsCollection Providers
            {
                get
                {
                    return (ProviderSettingsCollection)base["providers"];
                }
            }
        }
    }

