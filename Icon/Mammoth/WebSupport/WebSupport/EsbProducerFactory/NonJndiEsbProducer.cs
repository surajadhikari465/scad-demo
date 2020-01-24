using Icon.Esb.Producer;
using System;
using System.Collections.Generic;
using Icon.Esb;
using TIBCO.EMS;
using System.Security.Cryptography.X509Certificates;
using System.Collections;

namespace WebSupport.EsbProducerFactory
{
    public class NonJndiEsbProducer : IEsbProducer
    {
        protected ConnectionFactory factory;
        protected Connection connection = null;
        protected Session session = null;
        protected Destination destination = null;
        private MessageProducer producer;
        
        string IEsbConnection.ClientId { 
            get => connection.ClientID;
            set => connection.ClientID = value;
        }

        public NonJndiEsbProducer(EsbConnectionSettings settings)
        {
            Settings = settings;
        }

        public EsbConnectionSettings Settings { get; private set; }

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

            ExceptionHandlers = null;
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
            CreateDestination();
            producer = session.CreateProducer(destination);
            producer.DeliveryMode = DeliveryMode.PERSISTENT;
        }

        

        protected virtual void CreateDestination()
        {
            destination = session.CreateQueue(Settings.QueueName);
        }

        private LookupContext CreateLookupContext()
        {
            EMSSSLFileStoreInfo storeInfo = new EMSSSLFileStoreInfo();
            storeInfo.SetSSLPassword(Settings.SslPassword.ToCharArray());
            storeInfo.SetSSLClientIdentity(GetEsbCert());

            LookupContext lc = new LookupContext(new Hashtable
                {
                    { LookupContext.PROVIDER_URL, Settings.ServerUrl },
                    { LookupContext.SECURITY_PRINCIPAL, Settings.JndiUsername },
                    { LookupContext.SECURITY_CREDENTIALS, Settings.JndiPassword },
                    { LookupContext.SSL_STORE_INFO, storeInfo },
                    { LookupContext.SSL_STORE_TYPE, EMSSSLStoreType.EMSSSL_STORE_TYPE_FILE },
                    { LookupContext.SSL_TARGET_HOST_NAME, Settings.TargetHostName },
                    { LookupContext.SECURITY_PROTOCOL, "ssl" },
                    { LookupContext.SSL_TRACE, false }
                });

            return lc;
        }

        private X509Certificate GetEsbCert()
        {
            var store = new X509Store(Settings.CertificateStoreName, Settings.CertificateStoreLocation);
            store.Open(OpenFlags.ReadOnly);
            var cert = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, Settings.CertificateName, true)[0];
            store.Close();
            return cert;
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
    }
}