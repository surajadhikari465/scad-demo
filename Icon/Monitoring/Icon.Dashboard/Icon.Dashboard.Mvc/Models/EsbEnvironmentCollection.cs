using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.Models
{
    [ConfigurationCollection(typeof(EsbEnvironmentElement), AddItemName = "EsbEnvironment")]
    public class EsbEnvironmentCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new EsbEnvironmentElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EsbEnvironmentElement)element).Name;
        }

        public EsbEnvironmentElement this[int index]
        {
            get
            {
                return (EsbEnvironmentElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public EsbEnvironmentElement this[string esbEnvironmentName]
        {
            get
            {
                return (EsbEnvironmentElement)BaseGet(esbEnvironmentName);
            }
            set
            {
                if (BaseGet(esbEnvironmentName) != null)
                {
                    BaseRemove(esbEnvironmentName);
                }
                BaseAdd(value);
            }
        }

        public void Add(EsbEnvironmentElement esbEnvironmentElement)
        {
            BaseAdd(esbEnvironmentElement);
        }

        public void Remove(string esbEnvironmentName)
        {
            BaseRemove(esbEnvironmentName);
        }

        public void Remove(EsbEnvironmentElement esbEnvironmentElement)
        {
            if (BaseIndexOf(esbEnvironmentElement) >=0)
            {
                BaseRemove(esbEnvironmentElement.Name);
            }
        }
    }
}