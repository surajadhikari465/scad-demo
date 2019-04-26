using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.Models
{
    public class EsbEnvironmentsSection : ConfigurationSection
    {
        [ConfigurationProperty(nameof(EsbEnvironments), IsDefaultCollection = true)]
        public EsbEnvironmentCollection EsbEnvironments
        {
            get { return (EsbEnvironmentCollection)this[nameof(EsbEnvironments)]; }
            set { this[nameof(EsbEnvironments)] = value; }
        }
    }
}