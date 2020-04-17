using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Icon.Common;
using Services.Extract.Config;
using TIBCO.EMS;

namespace Services.Extract.Credentials
{
    public class EsbCredentialsCache : IEsbCredentialsCache
    {
        public Dictionary<string, EsbCredential> Credentials { get; set; }
        public void Refresh()
        {
            Credentials = EsbCredentialConfigSection.Config.SettingsList.ToDictionary(d => d.ProfileName, d => new EsbCredential(
                d.ServerUrl, d.JndiUsername, d.JndiPassword, d.ConnectionFactoryName, d.SslPassword, d.JmsUsername, d.JmsPassword, d.DestinationType, d.QueueName, d.SessionMode, d.TargetHostName, d.CertificateName,
                d.CertificateStoreName, d.CertificateStoreLocation, d.ReconnectDelay, d.MessageType, d.TransactionType, d.TransactionId));
        }
    }
}
