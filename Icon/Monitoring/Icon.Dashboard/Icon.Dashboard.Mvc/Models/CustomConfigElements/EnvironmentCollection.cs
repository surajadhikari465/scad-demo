using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.Models.CustomConfigElements
{
    [ConfigurationCollection(typeof(EnvironmentElement), AddItemName = "Environment")]
    public class EnvironmentCollection : ConfigurationElementCollection, IEnumerable<EnvironmentElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new EnvironmentElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EnvironmentElement)element).Name;
        }

        public EnvironmentElement this[int index]
        {
            get
            {
                return (EnvironmentElement)BaseGet(index);
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

        new public EnvironmentElement this[string environmentName]
        {
            get
            {
                return (EnvironmentElement)BaseGet(environmentName);
            }
            set
            {
                if (BaseGet(environmentName) != null)
                {
                    BaseRemove(environmentName);
                }
                BaseAdd(value);
            }
        }

        public void Add(EnvironmentElement environmentElement)
        {
            BaseAdd(environmentElement);
        }

        public void Remove(string environmentName)
        {
            BaseRemove(environmentName);
        }

        public void Remove(EnvironmentElement environmentElement)
        {
            if (BaseIndexOf(environmentElement) >=0)
            {
                BaseRemove(environmentElement.Name);
            }
        }

        IEnumerator<EnvironmentElement> IEnumerable<EnvironmentElement>.GetEnumerator()
        {
            for (int i= 0; i< base.Count; i++)
            {
                yield return base.BaseGet(i) as EnvironmentElement;
            }
        }
    }
}