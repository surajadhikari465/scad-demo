using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.Models.CustomConfigElements
{
    public class EnvironmentsSection : ConfigurationSection
    {
        [ConfigurationProperty(nameof(Environments), IsDefaultCollection = true)]
        public EnvironmentCollection Environments
        {
            get { return (EnvironmentCollection)this[nameof(Environments)]; }
            set { this[nameof(Environments)] = value; }
        }
    }
}