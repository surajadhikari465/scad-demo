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

        protected virtual void SetEsbEnvironmentProperties(IEsbEnvironment esbEnvironment, XElement esbEnvironmentElement)
        {
            if (esbEnvironment == null) throw new ArgumentNullException(nameof(esbEnvironment));
            if (esbEnvironmentElement == null) throw new ArgumentNullException(nameof(esbEnvironmentElement));

            var attributes = esbEnvironmentElement.Attributes().ToDictionary(
                a => a.Name.LocalName,
                a => a.Value);

            string configValue;

            esbEnvironment.Name = attributes.TryGetValue(nameof(esbEnvironment.Name), out configValue) ? configValue : string.Empty;
            esbEnvironment.ServerUrl = attributes.TryGetValue(nameof(esbEnvironment.ServerUrl), out configValue) ? configValue : string.Empty;
            esbEnvironment.TargetHostName = attributes.TryGetValue(nameof(esbEnvironment.TargetHostName), out configValue) ? configValue : string.Empty;
            esbEnvironment.JmsUsername = attributes.TryGetValue(nameof(esbEnvironment.JmsUsername), out configValue) ? configValue : string.Empty;
            esbEnvironment.JmsPassword = attributes.TryGetValue(nameof(esbEnvironment.JmsPassword), out configValue) ? configValue : string.Empty;
            esbEnvironment.JndiUsername = attributes.TryGetValue(nameof(esbEnvironment.JndiUsername), out configValue) ? configValue : string.Empty;
            esbEnvironment.JndiPassword = attributes.TryGetValue(nameof(esbEnvironment.JndiPassword), out configValue) ? configValue : string.Empty;

            var appsInEnvironmentElement = esbEnvironmentElement.Element(EsbEnvironmentSchema.EsbEnvironmentApplications);
            if (appsInEnvironmentElement != null)
            {
                string appIdentifierName = String.Empty;
                string appIdentifierServer = String.Empty;

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
