using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.DataFileGenerator.Configuration
{
    public class EsbEnvironmentsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new EsbEnvironmentConfigElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return (element as EsbEnvironmentConfigElement).Name;
        }

        public EsbEnvironmentConfigElement this[int index]
        {
            get
            {
                return (EsbEnvironmentConfigElement)base.BaseGet(index);
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                base.BaseAdd(index, value);
            }
        }

        public new EsbEnvironmentConfigElement this[string name]
        {
            get
            {
                return (EsbEnvironmentConfigElement)base.BaseGet(name);
            }
            set
            {
                if (BaseGet(name) != null)
                {
                    BaseRemoveAt(BaseIndexOf(BaseGet(name)));
                }
                BaseAdd(value);
            }
        }
    }
}
