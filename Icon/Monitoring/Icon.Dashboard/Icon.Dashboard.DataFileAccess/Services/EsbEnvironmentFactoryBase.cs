namespace Icon.Dashboard.DataFileAccess.Services
{
    using Constants;
    using Icon.Dashboard.DataFileAccess.Models;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Xml.Linq;

    public abstract class EsbEnvironmentFactoryBase
    {
        public abstract IEsbEnvironment GetEsbEnvironment(XElement esbEnvironmentElement);

        protected virtual void SetEsbEnvironmentProperties(
            IEsbEnvironment esbEnvironment, XElement esbEnvironmentElement)
        {
            if (esbEnvironment == null)
            {
                throw new ArgumentNullException(nameof(esbEnvironment));
            }
            if (esbEnvironmentElement == null)
            {
                throw new ArgumentNullException(nameof(esbEnvironmentElement));
            }

            var attributes = esbEnvironmentElement.Attributes().ToDictionary(
                a => a.Name.LocalName,
                a => a.Value);

            string configValue;
            int tempIntVal = 0;

            esbEnvironment.Name = attributes.TryGetValue(nameof(esbEnvironment.Name), out configValue)
                ? configValue : string.Empty;
            esbEnvironment.ServerUrl = attributes.TryGetValue(nameof(esbEnvironment.ServerUrl), out configValue)
                ? configValue : string.Empty;
            esbEnvironment.TargetHostName = attributes.TryGetValue(nameof(esbEnvironment.TargetHostName), out configValue)
                ? configValue : string.Empty;
            esbEnvironment.JmsUsername = attributes.TryGetValue(nameof(esbEnvironment.JmsUsername), out configValue)
                ? configValue : string.Empty;
            esbEnvironment.JmsPassword = attributes.TryGetValue(nameof(esbEnvironment.JmsPassword), out configValue)
                ? configValue : string.Empty;
            esbEnvironment.JndiUsername = attributes.TryGetValue(nameof(esbEnvironment.JndiUsername), out configValue)
                ? configValue : string.Empty;
            esbEnvironment.JndiPassword = attributes.TryGetValue(nameof(esbEnvironment.JndiPassword), out configValue)
                ? configValue : string.Empty;
            esbEnvironment.ConnectionFactoryName = attributes.TryGetValue(nameof(esbEnvironment.ConnectionFactoryName), out configValue)
                ? configValue : string.Empty;
            esbEnvironment.SslPassword = attributes.TryGetValue(nameof(esbEnvironment.SslPassword), out configValue)
                ? configValue : string.Empty;
            esbEnvironment.QueueName = attributes.TryGetValue(nameof(esbEnvironment.QueueName), out configValue)
                ? configValue : string.Empty;
            esbEnvironment.CertificateName = attributes.TryGetValue(nameof(esbEnvironment.CertificateName), out configValue)
                ? configValue : string.Empty;
            esbEnvironment.CertificateStoreName = attributes.TryGetValue(nameof(esbEnvironment.CertificateStoreName), out configValue)
                ? configValue : string.Empty;
            esbEnvironment.CertificateStoreLocation = attributes.TryGetValue(nameof(esbEnvironment.CertificateStoreLocation), out configValue)
                ? configValue : string.Empty;
            esbEnvironment.ReconnectDelay = attributes.TryGetValue(nameof(esbEnvironment.ReconnectDelay), out configValue)
                ? configValue : string.Empty;
            esbEnvironment.NumberOfListenerThreads = attributes.TryGetValue(nameof(esbEnvironment.NumberOfListenerThreads), out configValue)
                ? (int.TryParse(configValue, out tempIntVal) ? tempIntVal : 0) : 0;

            var appsInEnvironmentElement = esbEnvironmentElement.Element(EsbEnvironmentSchema.EsbEnvironmentApplications);
            if (appsInEnvironmentElement != null)
            {
                var appIdentifierName = String.Empty;
                var appIdentifierServer = String.Empty;

                foreach (var appIdentierElement in appsInEnvironmentElement.Elements())
                {
                    appIdentifierName = appIdentierElement.Attribute(nameof(IconApplicationIdentifier.Name))?.Value;
                    appIdentifierServer = appIdentierElement.Attribute(nameof(IconApplicationIdentifier.Server))?.Value;
                    if (!String.IsNullOrWhiteSpace(appIdentifierName) && !String.IsNullOrWhiteSpace(appIdentifierServer))
                    {
                        esbEnvironment.AddApplication(appIdentifierName, appIdentifierServer);
                    }
                }
            }
        }
    }
}
