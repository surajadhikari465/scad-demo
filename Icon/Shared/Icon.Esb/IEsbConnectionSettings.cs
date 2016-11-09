using System.Security.Cryptography.X509Certificates;
using TIBCO.EMS;

namespace Icon.Esb
{
    public interface IEsbConnectionSettings
    {
        string ServerUrl { get; set; }
        string JndiUsername { get; set; }
        string JndiPassword { get; set; }
        string ConnectionFactoryName { get; set; }
        string SslPassword { get; set; }
        string JmsUsername { get; set; }
        string JmsPassword { get; set; }
        string QueueName { get; set; }
        SessionMode SessionMode { get; set; }
        string TargetHostName { get; set; }
        string CertificateName { get; set; }
        StoreName CertificateStoreName { get; set; }
        StoreLocation CertificateStoreLocation { get; set; }
        int ReconnectDelay { get; set; }
    }
}
