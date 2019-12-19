using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Icon.Esb;
using Icon.Esb.Subscriber;
using TIBCO.EMS;

namespace Mammoth.Esb.AttributeListener.Infrastructure.Esb
{
    public class Sb1EsbConsumer : EsbConnection, IEsbSubscriber
    {
        private MessageConsumer consumer;

        public event EventHandler<EsbMessageEventArgs> MessageReceived;

        public Sb1EsbConsumer(EsbConnectionSettings settings) : base(settings) {  }

        public void BeginListening()
        {
            consumer.MessageHandler += Consumer_MessageHandler;
            connection.Start();
        }

        private void Consumer_MessageHandler(object sender, EMSMessageEventArgs args)
        {
            EsbMessageEventArgs message = new EsbMessageEventArgs
            {
                Message = new EsbMessage(args.Message as TextMessage)
            };

            MessageReceived?.Invoke(sender, message);
        }

        public override void Dispose()
        {
            consumer?.Close();
            MessageReceived = null;
            base.Dispose();
        }

        public new void OpenConnection()
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
            consumer = session.CreateConsumer(destination);
        }

        private X509Certificate GetEsbCert()
        {
            try
            {
                using (var store = new X509Store(Settings.CertificateStoreName, Settings.CertificateStoreLocation))
                {
                    store.Open(OpenFlags.ReadOnly);
                    var cert = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, Settings.CertificateName, true)[0];
                    store.Close();
                    return cert;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to find certificate: {Settings.CertificateName}, ESB certificate is missing or invalid", ex);
            }
        }
    }
}
