using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.DataFileGenerator.Configuration
{
    public class EsbSection : ConfigurationSection
    {
        public static EsbSection GetConfig()
        {
            return (EsbSection)ConfigurationManager.GetSection("esbSection");
        }
        [ConfigurationProperty("environments")]
        [ConfigurationCollection(typeof(EsbEnvironmentsCollection), AddItemName = "esbEnvironment")]
        public EsbEnvironmentsCollection EsbEnvironments
        {
            get
            {
                return (EsbEnvironmentsCollection)base["environments"];
            }
        }
    }
}
