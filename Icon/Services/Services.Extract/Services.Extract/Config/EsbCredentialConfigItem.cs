using Icon.Common;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using TIBCO.EMS;

namespace Services.Extract.Config
{
    public class EsbCredentialConfigItem : ConfigurationElement
    {
        [ConfigurationProperty("profileName", IsKey = true, IsRequired = true)]
        public string ProfileName
        {
            get => (string)base["profileName"];
            set => base["profileName"] = value;
        }
        [ConfigurationProperty("connectionFactoryName", IsRequired = true)]
        public string ConnectionFactoryName
        {
            get => (string)base["connectionFactoryName"];
            set => base["connectionFactoryName"] = value;
        }
        [ConfigurationProperty("certificateName", IsRequired = true)]
        public string CertificateName
        {
            get => (string)base["certificateName"];
            set => base["certificateName"] = value;
        }
        [ConfigurationProperty("certificateStoreName", IsRequired = true)]
        public string CertificateStoreName
        {
            get => (string)base["certificateStoreName"];
            set => base["certificateStoreName"] = value;
        }
        [ConfigurationProperty("certificateStoreLocation", IsRequired = true)]
        public string CertificateStoreLocation
        {
            get => (string)base["certificateStoreLocation"];
            set => base["certificateStoreLocation"] = value;
        }
        [ConfigurationProperty("jmsPassword", IsRequired = true)]
        public string JmsPassword
        {
            get => (string)base["jmsPassword"];
            set => base["jmsPassword"] = value;
        }
        [ConfigurationProperty("jmsUsername", IsRequired = true)]
        public string JmsUsername
        {
            get => (string)base["jmsUsername"];
            set => base["jmsUsername"] = value;
        }
        [ConfigurationProperty("jndiPassword", IsRequired = true)]
        public string JndiPassword
        {
            get => (string)base["jndiPassword"];
            set => base["jndiPassword"] = value;
        }
        [ConfigurationProperty("jndiUsername", IsRequired = true)]
        public string JndiUsername
        {
            get => (string)base["jndiUsername"];
            set => base["jndiUsername"] = value;
        }
        [ConfigurationProperty("messageType", IsRequired = true)]
        public string MessageType
        {
            get => (string)base["messageType"];
            set => base["messageType"] = value;
        }
        [ConfigurationProperty("serverUrl", IsRequired = true)]
        public string ServerUrl
        {
            get => (string)base["serverUrl"];
            set => base["serverUrl"] = value;
        }
        [ConfigurationProperty("sslPassword", IsRequired = true)]
        public string SslPassword
        {
            get => (string)base["sslPassword"];
            set => base["sslPassword"] = value;
        }
        [ConfigurationProperty("targetHostName", IsRequired = true)]
        public string TargetHostName
        {
            get => (string)base["targetHostName"];
            set => base["targetHostName"] = value;
        }
        [ConfigurationProperty("queueName", IsRequired = true)]
        public string QueueName
        {
            get => (string)base["queueName"];
            set => base["queueName"] = value;
        }
        [ConfigurationProperty("destinationType", IsRequired = false)]
        public string DestinationType
        {
            get => (string)base["destinationType"];
            set => base["destinationType"] = value;
        }
        [ConfigurationProperty("reconnectDelay", IsRequired = false)]
        public string ReconnectDelay
        {
            get => (string)base["reconnectDelay"];
            set => base["reconnectDelay"] = value;
        }
        [ConfigurationProperty("sessionMode", IsRequired = true)]
        public string SessionMode
        {
            get => (string)base["sessionMode"];
            set => base["sessionMode"] = value;
        }
        [ConfigurationProperty("transactionType", IsRequired = false)]
        public string TransactionType
        {
            get => (string)base["transactionType"];
            set => base["transactionType"] = value;
        }
        [ConfigurationProperty("transactionId", IsRequired = false)]
        public string TransactionId
        {
            get => (string)base["transactionId"];
            set => base["transactionId"] = value;
        }
    }
}
