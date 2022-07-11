using System;
using Services.Extract.Config;
using Icon.ActiveMQ;
using Apache.NMS;

namespace Services.Extract.Credentials
{
    public class ActiveMqCredential
    {
        public string ServerUrl { get; set; }
        public string JmsUsername { get; set; }
        public string JmsPassword { get; set; }
        public string QueueName { get; set; }
        public string SessionMode { get; set; }
        public string ReconnectDelay { get; set; }
        public string TransactionType { get; set; }
        public string TransactionId { get; set; }
        public string MessageType { get; set; }
        public string DestinationType { get; set; }

        public ActiveMqCredential(
            string serverUrl,
            string jmsUsername,
            string jmsPassword,
            string queueName,
            string sessionMode,
            string reconnectDelay,
            string transactionType,
            string transactionId,
            string messageType,
            string destinationType = null)
        {
            ServerUrl = serverUrl;
            JmsUsername = jmsUsername;
            JmsPassword = jmsPassword;
            QueueName = queueName;
            SessionMode = sessionMode;
            ReconnectDelay = reconnectDelay;
            TransactionType = transactionType;
            TransactionId = transactionId;
            MessageType = messageType;
            DestinationType = destinationType;
        }

        public static ActiveMqCredential getCredentialFromConfigurationItem(ActiveMqCredentialConfigItem configItem)
        {
            return new ActiveMqCredential(
                configItem.ServerUrl,
                configItem.JmsUsername,
                configItem.JmsPassword,
                configItem.QueueName,
                configItem.SessionMode,
                configItem.ReconnectDelay,
                configItem.TransactionType,
                configItem.TransactionId,
                configItem.MessageType,
                configItem.DestinationType
            );
        }

        public ActiveMQConnectionSettings getActiveMQConnectionSettings()
        {
            return new ActiveMQConnectionSettings()
            {
                ServerUrl = ServerUrl,
                JmsUsername = JmsUsername,
                JmsPassword = JmsPassword,
                QueueName = QueueName,
                SessionMode = SessionMode.ConvertToEnum<AcknowledgementMode>(),
                ReconnectDelay = String.IsNullOrEmpty(ReconnectDelay) ? 0 : int.Parse(ReconnectDelay),
                DestinationType = DestinationType
            };
        }
    }
}
