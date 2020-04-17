using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TIBCO.EMS;

namespace Services.Extract.Credentials
{
    public class EsbCredential
    {
        public EsbCredential(
            string serverUrl,
            string jndiUsername,
            string jndiPassword,
            string connectionFactoryName,
            string sslPassword,
            string jmsUsername,
            string jmsPassword,
            string destinationType,
            string queueName,
            string sessionMode,
            string targetHostName,
            string certificateName,
            string certificateStoreName,
            string certificateStoreLocation,
            string reconnectDelay,
            string messageType,
            string transactionType,
            string transactionId)
        {
            ServerUrl = serverUrl;
            JndiUsername = jndiUsername;
            JndiPassword = jndiPassword;
            ConnectionFactoryName = connectionFactoryName;
            SslPassword = sslPassword;
            JmsUsername = jmsUsername;
            JmsPassword = jmsPassword;
            DestinationType = destinationType;
            QueueName = queueName;
            SessionMode = sessionMode;
            TargetHostName = targetHostName;
            CertificateName = certificateName;
            CertificateStoreName = certificateStoreName;
            CertificateStoreLocation = certificateStoreLocation;
            ReconnectDelay = reconnectDelay;
            MessageType = messageType;
            TransactionType = transactionType;
            TransactionId = transactionId;
        }

        public string ServerUrl { get; set; }
        public string JndiUsername { get; set; }
        public string JndiPassword { get; set; }
        public string ConnectionFactoryName { get; set; }
        public string SslPassword { get; set; }
        public string JmsUsername { get; set; }
        public string JmsPassword { get; set; }
        public string DestinationType { get; set; }
        public string QueueName { get; set; }
        public string SessionMode { get; set; }
        public string TargetHostName { get; set; }
        public string CertificateName { get; set; }
        public string CertificateStoreName { get; set; }
        public string CertificateStoreLocation { get; set; }
        public string ReconnectDelay { get; set; }
        public string MessageType { get; set; }
        public string TransactionType { get; set; }
        public string TransactionId { get; set; }


    }
}
