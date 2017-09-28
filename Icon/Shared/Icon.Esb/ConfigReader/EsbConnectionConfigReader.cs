using System.Configuration;

namespace Icon.Esb.ConfigReader
{
    public class EsbConnectionConfigReader : ConfigurationSection
    {
        public static EsbConnectionConfigReader GetConfig()
        {
            return (EsbConnectionConfigReader)ConfigurationManager.GetSection("esbConnections");
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

        [ConfigurationProperty("serverUrl", IsRequired = false)]
        public string ServerUrl { get { return (string)base["serverUrl"]; } }

        [ConfigurationProperty("jmsUsername", IsRequired = false)]
        public string JmsUsername { get { return (string)base["jmsUsername"]; } }

        [ConfigurationProperty("jmsPassword", IsRequired = false)]
        public string JmsPassword { get { return (string)base["jmsPassword"]; } }

        [ConfigurationProperty("jndiUsername", IsRequired = false)]
        public string JndiUsername { get { return (string)base["jndiUsername"]; } }

        [ConfigurationProperty("jndiPassword", IsRequired = false)]
        public string JndiPassword { get { return (string)base["jndiPassword"]; } }

        [ConfigurationProperty("connectionFactoryName", IsRequired = false)]
        public string ConnectionFactoryName { get { return (string)base["connectionFactoryName"]; } }

        [ConfigurationProperty("sslPassword", IsRequired = false)]
        public string SslPassword { get { return (string)base["sslPassword"]; } }

        [ConfigurationProperty("queueName", IsRequired = false)]
        public string QueueName { get { return (string)base["queueName"]; } }

        [ConfigurationProperty("certificateName", IsRequired = false)]
        public string CertificateName { get { return (string)base["certificateName"]; } }

        [ConfigurationProperty("targetHostName", IsRequired = false)]
        public string TargetHostName { get { return (string)base["targetHostName"]; } }

        [ConfigurationProperty("certificateStoreName", IsRequired = false)]
        public string CertificateStoreName { get { return (string)base["certificateStoreName"]; } }

        [ConfigurationProperty("certificateStoreLocation", IsRequired = false)]
        public string CertificateStoreLocation { get { return (string)base["certificateStoreLocation"]; } }

        [ConfigurationProperty("reconnectDelay", IsRequired = false)]
        public string ReconnectDelay { get { return (string)base["reconnectDelay"]; } }

        [ConfigurationProperty("sessionMode", IsRequired = false)]
        public string SessionMode { get { return (string)base["sessionMode"]; } }
    }
}
