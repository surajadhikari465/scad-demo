using Icon.Esb;
using Icon.Esb.Producer;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TIBCO.EMS;

namespace Icon.Esb.Producer
{
    public class Sb1EsbProducer : IEsbProducer, IDisposable
    {
        private ConnectionFactory factory;
        private Connection connection;
        private Session session;
        private Destination destination;
        private MessageProducer producer;
        
        public string ClientId
        {
            get => connection.ClientID;
            set => connection.ClientID = value;
        }
    
        public EsbConnectionSettings Settings { get; set; }

        public bool IsConnected
        {
            get
            {
                if (connection == null || connection.IsClosed)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public event EventHandler<EMSException> ExceptionHandlers;

        public Sb1EsbProducer(EsbConnectionSettings settings)
        {
            this.Settings = settings;
        }


        public void Dispose()
        {
            if (session != null && !session.IsClosed)
            {
                session.Close();
            }

            if (connection != null && !connection.IsClosed)
            {
                connection.Close();
            }
        }

        public void OpenConnection()
        {
            EMSSSLFileStoreInfo storeInfo = new EMSSSLFileStoreInfo();
            storeInfo.SetSSLPassword(Settings.SslPassword.ToCharArray());
            storeInfo.SetSSLClientIdentity(GetEsbCert());

            factory = new ConnectionFactory(Settings.ServerUrl);
            factory.SetTargetHostName(Settings.TargetHostName);
            factory.SetCertificateStoreType(EMSSSLStoreType.EMSSSL_STORE_TYPE_FILE, storeInfo);

            connection = factory.CreateConnection(Settings.JmsUsername, Settings.JmsPassword);
            session = connection.CreateSession(false, Settings.SessionMode);
            destination = session.CreateQueue(Settings.QueueName);
            producer = session.CreateProducer(destination);
            producer.DeliveryMode = DeliveryMode.PERSISTENT;
        }

        public void Send(string message, Dictionary<string, string> messageProperties = null)
        {
            TextMessage textMessage = session.CreateTextMessage(message);

            Send(textMessage, messageProperties);
        }

        public void Send(string message, string messageId, Dictionary<string, string> messageProperties = null)
        {
            TextMessage textMessage = session.CreateTextMessage(message);
            textMessage.MessageID = messageId;

            Send(textMessage, messageProperties);
        }

        private void Send(TextMessage textMessage, Dictionary<string, string> messageProperties = null)
        {
            if (messageProperties != null)
            {
                foreach (var property in messageProperties)
                {
                    textMessage.SetStringProperty(property.Key, property.Value);
                }
            }

            producer.Send(textMessage);
        }


        private X509Certificate GetEsbCert()
        {
            try
            {
                var store = new X509Store(Settings.CertificateStoreName, Settings.CertificateStoreLocation);
                store.Open(OpenFlags.ReadOnly);
                var cert = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, Settings.CertificateName, true)[0];
                store.Close();
                return cert;
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to find certificate: {Settings.CertificateName}, ESB certificate is missing or invalid", ex);
            }
        }
    }
}
