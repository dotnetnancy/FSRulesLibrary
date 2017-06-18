using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Provider;

namespace DotNetNancy.Rules.RuleSet.DataAccess
{
    public class ProviderList : ProviderCollection
    {
        public override void Add(ProviderBase provider)
        {
            if (provider == null)
                throw new ArgumentNullException(this.GetType().FullName + @":  " + @"The provider parameter cannot be null");
            base.Add(provider);
        }
    }   

}
