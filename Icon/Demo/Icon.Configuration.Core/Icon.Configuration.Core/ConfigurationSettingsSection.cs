using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Configuration.Core
{
    public class ConfigurationSettingsSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public AppSettingCollection AppSettings
        {
            get { return (AppSettingCollection)this[""]; }
            set { this[""] = value; }
        }
    }

    public class AppSettingCollection : ConfigurationElementCollection
    { 
        protected override ConfigurationElement CreateNewElement()
        {
            return new AppSettingConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AppSettingConfigurationElement)element).Key;
        }

        public AppSettingConfigurationElement this[int index]
        {
            get { return (AppSettingConfigurationElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public AppSettingConfigurationElement this[string key]
        {
            get
            {
                return (AppSettingConfigurationElement)BaseGet(key);
            }
        }

        //public override ConfigurationElementCollectionType CollectionType
        //{
        //    get
        //    {
        //        return new ConfigurationElementCollectionType.AddRemoveClearMap;
        //    }
        //}
    }

    public class AppSettingConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }
    }
}
