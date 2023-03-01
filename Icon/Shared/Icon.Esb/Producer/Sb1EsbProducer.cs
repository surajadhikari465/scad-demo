using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
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
        private string lastClientId = null;

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
            lastClientId = "Undefined-" + Guid.NewGuid().ToString("N");
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

        public void OpenConnection(string clientId)
        {
            this.lastClientId = clientId;
            if (!IsConnected)
            {
                EMSSSLFileStoreInfo storeInfo = new EMSSSLFileStoreInfo();
                storeInfo.SetSSLPassword(Settings.SslPassword.ToCharArray());
                storeInfo.SetSSLClientIdentity(GetEsbCert());

                factory = new ConnectionFactory(Settings.ServerUrl);
                factory.SetTargetHostName(Settings.TargetHostName);
                factory.SetCertificateStoreType(EMSSSLStoreType.EMSSSL_STORE_TYPE_FILE, storeInfo);

                connection = factory.CreateConnection(Settings.JmsUsername, Settings.JmsPassword);
                connection.ClientID = clientId;
            }

            session = connection.CreateSession(false, Settings.SessionMode);
            CreateDestination();
            producer = session.CreateProducer(destination);
            producer.DeliveryMode = DeliveryMode.PERSISTENT;
        }

        public void Send(string message, Dictionary<string, string> messageProperties = null)
        {
            Retry<Exception>(() =>
            {
                // Verify Connection
                VerifyConnectionAndGracefullyReconnect();

                TextMessage textMessage = session.CreateTextMessage(message);

                Send(textMessage, messageProperties);
            });
        }

        public void Send(string message, string messageId, Dictionary<string, string> messageProperties = null)
        {
            Retry<Exception>(() =>
            {
                // Verify Connection
                VerifyConnectionAndGracefullyReconnect();

                TextMessage textMessage = session.CreateTextMessage(message);
                textMessage.MessageID = messageId;

                Send(textMessage, messageProperties);
            });
        }

        private void Send(TextMessage textMessage, Dictionary<string, string> messageProperties = null)
        {
            // Set Properties
            if (messageProperties != null)
            {
                foreach (var property in messageProperties)
                {
                    textMessage.SetStringProperty(property.Key, property.Value);
                }
            }

            // Verify Connection
            VerifyConnectionAndGracefullyReconnect();

            // Send Message, Retry on TIBCO.EMS.IllegalStateException 10 times with 30s between tries
            Retry<IllegalStateException>(() => { producer.Send(textMessage); });
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

        protected void CreateDestination()
        {
            if (Settings.DestinationType.ToLower().Contains("topic"))
            {
                destination = session.CreateTopic(Settings.QueueName);
            }
            else
            {
                destination = session.CreateQueue(Settings.QueueName);
            }
        }

        private void VerifyConnectionAndGracefullyReconnect()
        {
            Retry<Exception>(() => {
                if (!this.IsConnected || session.IsClosed)
                {
                    this.OpenConnection(this.lastClientId);
                }
            });
        }

        /// <summary>
        /// Retry an action maxRetries times, and waits timeBetweenRetries milliseconds between retries.
        /// </summary>
        /// <typeparam name="TException">Type of exception to retry.</typeparam>
        /// <param name="action">Action to execute and retry if needed.</param>
        /// <param name="maxRetries">Max times to retry: Default 10 times</param>
        /// <param name="timeBetweenRetries">Time in milliseconds to wait between retries.: Default 30 seconds.</param>
        private void Retry<TException>(Action action, int maxRetries = 10, int timeBetweenRetries = 30000) where TException : Exception
        {
            int retryCount = 0;
            bool retry = true;
            while (retry == true)
            {
                try
                {
                    action();
                    retry = false;
                }
                catch (TException)
                {
                    retryCount++;
                    if (retryCount >= maxRetries)
                    {
                        throw;
                    }
                    Thread.Sleep(timeBetweenRetries);
                }
            }
        }
    }
}
