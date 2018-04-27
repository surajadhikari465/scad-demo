using Icon.Esb;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using TIBCO.EMS;

namespace Mammoth.PrimeAffinity.Library.Esb
{
    public class EsbConnectionCache
    {
        private static ConnectionFactory connectionFactory;

        private static Connection connection;
        public static Connection Connection
        {
            get { return connection; }
        }

        private static EsbConnectionSettings esbConnectionSettings;
        public static EsbConnectionSettings EsbConnectionSettings
        {
            get { return esbConnectionSettings; }
            set { esbConnectionSettings = value; }
        }

        public static void InitializeConnectionFactoryAndConnection()
        {
            LookupContext lookupContext = CreateLookupContext();

            connectionFactory = (ConnectionFactory)lookupContext.Lookup(esbConnectionSettings.ConnectionFactoryName);
            connectionFactory.SetTargetHostName(esbConnectionSettings.TargetHostName);
            connection = connectionFactory.CreateConnection(esbConnectionSettings.JmsUsername, esbConnectionSettings.JmsPassword);
            connection.Start();
        }

        public static ICacheEsbProducer CreateProducer()
        {
            var session = connection.CreateSession(false, esbConnectionSettings.SessionMode);
            return new CacheEsbProducer(
                session.CreateProducer(session.CreateQueue(esbConnectionSettings.QueueName)),
                session);
        }

        private static LookupContext CreateLookupContext()
        {
            EMSSSLFileStoreInfo storeInfo = new EMSSSLFileStoreInfo();
            storeInfo.SetSSLPassword(esbConnectionSettings.SslPassword.ToCharArray());
            storeInfo.SetSSLClientIdentity(GetEsbCert());

            LookupContext lc = new LookupContext(new Hashtable
                {
                    { LookupContext.PROVIDER_URL, esbConnectionSettings.ServerUrl },
                    { LookupContext.SECURITY_PRINCIPAL, esbConnectionSettings.JndiUsername },
                    { LookupContext.SECURITY_CREDENTIALS, esbConnectionSettings.JndiPassword },
                    { LookupContext.SSL_STORE_INFO, storeInfo },
                    { LookupContext.SSL_STORE_TYPE, EMSSSLStoreType.EMSSSL_STORE_TYPE_FILE },
                    { LookupContext.SSL_TARGET_HOST_NAME, esbConnectionSettings.TargetHostName },
                    { LookupContext.SECURITY_PROTOCOL, "ssl" },
                    { LookupContext.SSL_TRACE, false }
                });

            return lc;
        }

        private static X509Certificate GetEsbCert()
        {
            var store = new X509Store(esbConnectionSettings.CertificateStoreName, esbConnectionSettings.CertificateStoreLocation);
            store.Open(OpenFlags.ReadOnly);
            var cert = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, esbConnectionSettings.CertificateName, true)[0];
            store.Close();
            return cert;
        }
    }
}
