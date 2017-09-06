namespace Icon.Dashboard.DataFileAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// Class representing an ESB Environment, as laid out in an XML data file
    /// </summary>
    public class EsbEnvironmentDefinition : IEsbEnvironmentDefinition
    {
        public EsbEnvironmentDefinition() { }

        public string Name { get; set; }

        public string ServerUrl { get; set; }

        public string TargetHostName { get; set; }

        public string JmsUsername { get; set; }

        public string JmsPassword { get; set; }

        public string JndiUsername { get; set; }

        public string JndiPassword { get; set; }

        //public string ConnectionFactoryName { get; set; }

        public string SslPassword { get; set; }

        //public string QueueName { get; set; }

        public string SessionMode { get; set; }

        public string CertificateName { get; set; }

        public string CertificateStoreName { get; set; }

        public string CertificateStoreLocation { get; set; }

        public string ReconnectDelay { get; set; }

        public int NumberOfListenerThreads { get; set; }

        /// <summary>
        /// Hard-coded list of EsbEnvironment Properties. This can be used to distinguish App Settings related to 
        ///   an ESB environment from other App Settings in a .config file
        /// </summary>
        public static ReadOnlyCollection<string> EsbAppSettingsNames =>
            new ReadOnlyCollection<string>(esbEnvironmentPropertyNames);

        private static List<string> esbEnvironmentPropertyNames = new List<string>
        {
            nameof(ServerUrl),
            nameof(TargetHostName),
            nameof(JmsUsername),
            nameof(JmsPassword),
            nameof(JndiUsername),
            nameof(JndiPassword),
            //nameof(ConnectionFactoryName),
            nameof(SslPassword),
            //nameof(QueueName),
            nameof(SessionMode),
            nameof(CertificateName),
            nameof(CertificateStoreName),
            nameof(CertificateStoreLocation),
            nameof(ReconnectDelay),
            nameof(NumberOfListenerThreads)
        };
    }
}
