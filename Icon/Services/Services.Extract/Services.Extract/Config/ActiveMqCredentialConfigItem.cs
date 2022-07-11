using System.Configuration;

namespace Services.Extract.Config
{
    public class ActiveMqCredentialConfigItem : ConfigurationElement
    {
        [ConfigurationProperty("profileName", IsKey = true, IsRequired = true)]
        public string ProfileName
        {
            get => (string)base["profileName"];
            set => base["profileName"] = value;
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
        [ConfigurationProperty("serverUrl", IsRequired = true)]
        public string ServerUrl
        {
            get => (string)base["serverUrl"];
            set => base["serverUrl"] = value;
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
        [ConfigurationProperty("messageType", IsRequired = true)]
        public string MessageType
        {
            get => (string)base["messageType"];
            set => base["messageType"] = value;
        }
    }
}
