using System;
using System.Security.Cryptography.X509Certificates;
using Icon.Esb;
using Icon.Esb.Subscriber;
using TIBCO.EMS;

namespace MammothR10Price.Esb.Subscriber
{
    // taken as is from Icon/Services/Services.Extract/Services.Extract/Infrastructure.Esb/Sb1EsbConsumer.cs.
    internal class Sb1EsbConsumer : IEsbSubscriber
    {
        private ConnectionFactory Factory;
        private Connection Connection;
        private Session Session;
        private Destination Destination;
        private MessageConsumer Consumer;

        public EsbConnectionSettings Settings { get; set; }
        public event EventHandler<EsbMessageEventArgs> MessageReceived;
        public event EventHandler<EMSException> ExceptionHandlers;

        public bool IsConnected
        {
            get
            {
                if (Connection == null || Connection.IsClosed)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public Sb1EsbConsumer(EsbConnectionSettings settings)
        {
            this.Settings = settings;
        }

        public void OpenConnection(string clientId)
        {
            EMSSSLFileStoreInfo storeInfo = new EMSSSLFileStoreInfo();
            storeInfo.SetSSLPassword(Settings.SslPassword.ToCharArray());
            storeInfo.SetSSLClientIdentity(GetEsbCert());

            Factory = new ConnectionFactory(Settings.ServerUrl);
            Factory.SetTargetHostName(Settings.TargetHostName);
            Factory.SetCertificateStoreType(EMSSSLStoreType.EMSSSL_STORE_TYPE_FILE, storeInfo);

            Connection = Factory.CreateConnection(Settings.JmsUsername, Settings.JmsPassword);
            Connection.ClientID = clientId;
            Session = Connection.CreateSession(false, Settings.SessionMode);
            Destination = Session.CreateQueue(Settings.QueueName);
            Consumer = Session.CreateConsumer(Destination);

        }

        public string ClientId { get; set; }

        public void BeginListening()
        {
            Consumer.MessageHandler += HandleMessage;
            Connection.Start();
        }


        internal void HandleMessage(object sender, EMSMessageEventArgs args)
        {
            EsbMessageEventArgs esbMessage = new EsbMessageEventArgs
            {
                Message = new EsbMessage(args.Message as TextMessage)
            };

            var handler = MessageReceived;
            handler?.Invoke(this, esbMessage);
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

        internal virtual void OnExceptionHandlers(EMSException e)
        {
            ExceptionHandlers?.Invoke(this, e);
        }
        public void Dispose()
        {

            if (Consumer != null)
            {
                Consumer.MessageHandler -= HandleMessage;
                Consumer.Close();
            }

            if (Session != null && !Session.IsClosed)
            {
                Session.Close();
            }

            if (Connection != null && !Connection.IsClosed)
            {
                Connection.Close();
            }
            MessageReceived = null;

        }
    }
}
