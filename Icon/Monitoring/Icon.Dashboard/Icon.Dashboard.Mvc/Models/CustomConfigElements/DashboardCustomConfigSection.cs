using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Icon.Dashboard.Mvc.Models.CustomConfigElements
{
    public class DashboardCustomConfigSection : ConfigurationSectionGroup
    {
        [ConfigurationProperty(nameof(CustomConfigElements.EnvironmentsSection), IsRequired = false)]
        public EnvironmentsSection EnvironmentsSection
        {
            get { return (EnvironmentsSection)base.Sections[nameof(CustomConfigElements.EnvironmentsSection)]; }
        }

        [ConfigurationProperty(nameof(CustomConfigElements.EsbEnvironmentsSection), IsRequired = false)]
        public EsbEnvironmentsSection EsbEnvironmentsSection  
        {  
            get { return (EsbEnvironmentsSection)base.Sections[nameof(CustomConfigElements.EsbEnvironmentsSection)]; }  
        }  
    }
}