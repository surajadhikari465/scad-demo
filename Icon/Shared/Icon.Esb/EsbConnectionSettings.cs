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
            ServerUrl = AppSettingsAccessor.GetStringSetting("ServerUrl");
            JndiUsername = AppSettingsAccessor.GetStringSetting("JndiUsername");
            JndiPassword = AppSettingsAccessor.GetStringSetting("JndiPassword");
            ConnectionFactoryName = AppSettingsAccessor.GetStringSetting("ConnectionFactoryName");
            SslPassword = AppSettingsAccessor.GetStringSetting("SslPassword");
            JmsUsername = AppSettingsAccessor.GetStringSetting("JmsUsername");
            JmsPassword = AppSettingsAccessor.GetStringSetting("JmsPassword");

            TargetHostName = AppSettingsAccessor.GetStringSetting("TargetHostName");
            CertificateName = AppSettingsAccessor.GetStringSetting("CertificateName");
            CertificateStoreName = AppSettingsAccessor.GetEnumSetting<StoreName>("CertificateStoreName");
            CertificateStoreLocation = AppSettingsAccessor.GetEnumSetting<StoreLocation>("CertificateStoreLocation");
            ReconnectDelay = AppSettingsAccessor.GetIntSetting("ReconnectDelay", false);

            QueueName = AppSettingsAccessor.GetStringSetting(queueConfigName);
            SessionMode = AppSettingsAccessor.GetEnumSetting<SessionMode>("SessionMode");
        }

        public virtual void LoadFromNamedConfig(string connectionName)
        {
            var connectionConfiguration = EsbConnectionConfigReader.GetConfig().Connections[connectionName];

            ServerUrl = connectionConfiguration.ServerUrl;
            JmsUsername = connectionConfiguration.JmsUsername;
            JmsPassword = connectionConfiguration.JmsPassword;
            JndiUsername = connectionConfiguration.JndiUsername;
            JndiPassword = connectionConfiguration.JndiPassword;
            ConnectionFactoryName = connectionConfiguration.ConnectionFactoryName;
            SslPassword = connectionConfiguration.SslPassword;
            QueueName = connectionConfiguration.QueueName;
            SessionMode = (SessionMode)Enum.Parse(typeof(SessionMode), connectionConfiguration.SessionMode);
            CertificateName = connectionConfiguration.CertificateName;
            CertificateStoreName = (StoreName)Enum.Parse(typeof(StoreName), connectionConfiguration.CertificateStoreName);
            CertificateStoreLocation = (StoreLocation)Enum.Parse(typeof(StoreLocation), connectionConfiguration.CertificateStoreLocation);
            TargetHostName = connectionConfiguration.TargetHostName;
            ReconnectDelay = Int32.Parse(connectionConfiguration.ReconnectDelay);
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
