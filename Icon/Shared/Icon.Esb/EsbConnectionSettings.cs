using Icon.Common;
using Icon.Esb.ConfigReader;
using System;
using System.Security.Cryptography.X509Certificates;
using TIBCO.EMS;

namespace Icon.Esb
{
    public class EsbConnectionSettings : IEsbConnectionSettings
    {
        public string ServerUrl { get; set; }
        public string JndiUsername { get; set; }
        public string JndiPassword { get; set; }
        public string ConnectionFactoryName { get; set; }
        public string SslPassword { get; set; }
        public string JmsUsername { get; set; }
        public string JmsPassword { get; set; }
        public string QueueName { get; set; }
        public SessionMode SessionMode { get; set; }
        public string TargetHostName { get; set; }
        public string CertificateName { get; set; }
        public StoreName CertificateStoreName { get; set; }
        public StoreLocation CertificateStoreLocation { get; set; }
        public int ReconnectDelay { get; set; }

        public virtual void LoadFromConfig(string queueConfigName = "QueueName")
        {
            ServerUrl = AppSettingsAccessor.GetStringSetting("ServerUrl", false);
            JndiUsername = AppSettingsAccessor.GetStringSetting("JndiUsername", false);
            JndiPassword = AppSettingsAccessor.GetStringSetting("JndiPassword", false);
            ConnectionFactoryName = AppSettingsAccessor.GetStringSetting("ConnectionFactoryName", false);
            SslPassword = AppSettingsAccessor.GetStringSetting("SslPassword", false);
            JmsUsername = AppSettingsAccessor.GetStringSetting("JmsUsername", false);
            JmsPassword = AppSettingsAccessor.GetStringSetting("JmsPassword", false);

            TargetHostName = AppSettingsAccessor.GetStringSetting("TargetHostName", false);
            CertificateName = AppSettingsAccessor.GetStringSetting("CertificateName", false);
            CertificateStoreName = AppSettingsAccessor.GetEnumSetting<StoreName>("CertificateStoreName", false);
            CertificateStoreLocation = AppSettingsAccessor.GetEnumSetting<StoreLocation>("CertificateStoreLocation", false);
            ReconnectDelay = AppSettingsAccessor.GetIntSetting("ReconnectDelay", false);

            QueueName = AppSettingsAccessor.GetStringSetting(queueConfigName, false);
            SessionMode = AppSettingsAccessor.GetEnumSetting<SessionMode>("SessionMode", false);
        }

        public virtual void LoadFromNamedConfig(string connectionName)
        {
            var esbConnectionConfiguration = EsbConnectionConfigReader.GetConfig();
            var connectionConfiguration = esbConnectionConfiguration?.Connections[connectionName];

            if(esbConnectionConfiguration == null)
            {
                throw new ArgumentException("Could not find esbConnections");
            }

            if(connectionConfiguration == null)
            {
                throw new ArgumentException($"'Could not find '{connectionName}' in esbConnections");
            }

            ServerUrl = connectionConfiguration.ServerUrl;
            JmsUsername = connectionConfiguration.JmsUsername;
            JmsPassword = connectionConfiguration.JmsPassword;
            JndiUsername = connectionConfiguration.JndiUsername;
            JndiPassword = connectionConfiguration.JndiPassword;
            ConnectionFactoryName = connectionConfiguration.ConnectionFactoryName;
            SslPassword = connectionConfiguration.SslPassword;
            QueueName = connectionConfiguration.QueueName;
            SessionMode = !string.IsNullOrWhiteSpace(connectionConfiguration.SessionMode) 
                ? (SessionMode)Enum.Parse(typeof(SessionMode), connectionConfiguration.SessionMode) 
                : default(SessionMode);
            CertificateName = connectionConfiguration.CertificateName;
            CertificateStoreName = !string.IsNullOrWhiteSpace(connectionConfiguration.CertificateStoreName) 
                ? (StoreName)Enum.Parse(typeof(StoreName), connectionConfiguration.CertificateStoreName) 
                : default(StoreName);
            CertificateStoreLocation = !string.IsNullOrWhiteSpace(connectionConfiguration.CertificateStoreLocation) 
                ? (StoreLocation)Enum.Parse(typeof(StoreLocation), connectionConfiguration.CertificateStoreLocation) 
                : default(StoreLocation);
            TargetHostName = connectionConfiguration.TargetHostName;
            ReconnectDelay = !string.IsNullOrWhiteSpace(connectionConfiguration.ReconnectDelay) 
                ? Int32.Parse(connectionConfiguration.ReconnectDelay) 
                : 0;
        }

        public static EsbConnectionSettings CreateSettingsFromConfig()
        {
            var settings = new EsbConnectionSettings();
            settings.LoadFromConfig();

            return settings;
        }

        public static EsbConnectionSettings CreateSettingsFromConfig(string queueConfigName)
        {
            var settings = new EsbConnectionSettings();
            settings.LoadFromConfig(queueConfigName);

            return settings;
        }

        public static EsbConnectionSettings CreateSettingsFromNamedConnectionConfig(string connectionName)
        {
            var settings = new EsbConnectionSettings();
            settings.LoadFromNamedConfig(connectionName);

            return settings;
        }
    }
}
