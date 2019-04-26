using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.Models
{
    public class DashboardCustomConfigSectionGroup : ConfigurationSectionGroup
    {
        [ConfigurationProperty(nameof(EsbEnvironmentsSection), IsRequired = false)]  
        public EsbEnvironmentsSection EsbEnvironmentsSettings  
        {  
            get { return (EsbEnvironmentsSection)base.Sections[nameof(EsbEnvironmentsSection)]; }  
        }  
    }
}