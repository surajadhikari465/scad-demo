using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using TIBCO.EMS;

namespace Icon.Esb
{
    public class EsbConnection : IEsbConnection
    {
        protected ConnectionFactory factory;
        protected Connection connection = null;
        protected Session session = null;
        protected Destination destination = null;

        public EsbConnectionSettings Settings { get; set; }

        public event EventHandler<EMSException> ExceptionHandlers;

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

        public EsbConnection(EsbConnectionSettings settings)
        {
            this.Settings = settings;
        }

        public virtual void OpenConnection()
        {
            LookupContext lookupContext = CreateLookupContext();

            factory = (ConnectionFactory)lookupContext.Lookup(Settings.ConnectionFactoryName);
            factory.SetTargetHostName(Settings.TargetHostName);

            connection = factory.CreateConnection(Settings.JmsUsername, Settings.JmsPassword);
            session = connection.CreateSession(false, Settings.SessionMode);
            CreateDestination();

            connection.ExceptionHandler += ConnectionExceptionHandler;
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

        private void ConnectionExceptionHandler(object sender, EMSExceptionEventArgs args)
        {
            EMSException exception = args.Exception;

            var handler = ExceptionHandlers;
            if (handler != null)
            {
                handler(sender, exception);
            }
        }

        public virtual void Dispose()
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
    }
}
