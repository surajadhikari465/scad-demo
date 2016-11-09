using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomAppConfig
{
    public class EsbConnectionsConfig : ConfigurationSection
    {
        public static EsbConnectionsConfig GetConfig()
        {
            return (EsbConnectionsConfig)ConfigurationManager.GetSection("esbConnections");
        }        

        [ConfigurationProperty("connections")]
        [ConfigurationCollection(typeof(EsbConnectionsCollection), AddItemName = "esbConnection")]
        public EsbConnectionsCollection Connections
        {
            get
            {
                return (EsbConnectionsCollection)base["connections"];
            }
        }
    }

    public class EsbConnectionsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new EsbConnectionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as EsbConnectionElement).Name;
        }

        public EsbConnectionElement this[int index]
        {
            get
            {
                return (EsbConnectionElement)base.BaseGet(index);
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

        public new EsbConnectionElement this[string name]
        {
            get { return (EsbConnectionElement)base.BaseGet(name); }
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

    public class EsbConnectionElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name { get { return (string)base["name"]; } }

        [ConfigurationProperty("serverUrl", IsRequired = true)]
        public string ServerUrl { get { return (string)base["serverUrl"]; } }

        [ConfigurationProperty("jmsUsername", IsRequired = true)]
        public string JmsUsername { get { return (string)base["jmsUsername"]; } }

        [ConfigurationProperty("jmsPassword", IsRequired = true)]
        public string JmsPassword { get { return (string)base["jmsPassword"]; } }

        [ConfigurationProperty("queueName", IsRequired = true)]
        public string QueueName { get { return (string)base["queueName"]; } }
    }
}
