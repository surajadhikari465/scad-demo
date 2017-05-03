namespace Icon.Dashboard.DataFileAccess.Services
{
    using Icon.Dashboard.DataFileAccess.Models;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Xml.Linq;

    public abstract class ApplicationFactory
    {
        /// <summary>
        /// Creates an application from a configuration element.
        /// </summary>
        /// <param name="applicationElement">Application Configuration Element.</param>
        /// <returns>A concrete Application implementation.</returns>
        public abstract IApplication GetApplication(XElement applicationElement);

        protected virtual void SetApplicationProperties(IApplication application, XElement applicationElement)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));
            if (applicationElement == null) throw new ArgumentNullException(nameof(applicationElement));

            var config = applicationElement.Attributes().ToDictionary(
                a => a.Name.LocalName,
                a => a.Value);

            string configValue = String.Empty;
            int configValueInt = 0;

            try
            {
                application.Name = config.TryGetValue(nameof(application.Name), out configValue) ? configValue : string.Empty;
                application.ConfigFilePath = config.TryGetValue(nameof(application.ConfigFilePath), out configValue) ? configValue : string.Empty;
                application.DisplayName = config.TryGetValue(nameof(application.DisplayName), out configValue) ? configValue : string.Empty;
                application.Server = config.TryGetValue(nameof(application.Server), out configValue) ? configValue : string.Empty;
                application.DataFlowFrom = config.TryGetValue(nameof(application.DataFlowFrom), out configValue) ? configValue : string.Empty;
                application.DataFlowTo = config.TryGetValue(nameof(application.DataFlowTo), out configValue) ? configValue : string.Empty;
                application.LoggingName = config.TryGetValue(nameof(application.LoggingName), out configValue) ? configValue : string.Empty;
                application.LoggingName = config.TryGetValue(nameof(application.LoggingName), out configValue) ? configValue : string.Empty;
                if (config.TryGetValue(nameof(application.LoggingID), out configValue))
                {
                    if (int.TryParse(configValue, out configValueInt))
                    {
                        application.LoggingID = configValueInt;
                    }
                }
            }
            catch (Exception ex)
            {
                //debug
                string msg = ex.Message;
            }
        }

        /// <summary>
        /// This method tries to grab the app.config from the configFilePath and load the appsettings
        /// </summary>
        /// <param name="application">IApplication definition which will have its AppSettings (and
        ///     EsbConnectionSettings) property populated</param>
        protected virtual void LoadAppSettings(IApplication application)
        {
            try
            {
                if (application.ConfigFilePath != "Unknown")
                {
                    var appConfig = XDocument.Load(application.ConfigFilePath);
                    var allAppSettings = appConfig.Root.Element("appSettings").Elements()
                        .Select(e => new
                        {
                            Key = e.Attribute("key").Value,
                            Value = e.Attribute("value").Value
                        });

                    var esbEnvironmentSettings = allAppSettings.Where(s => EsbEnvironment.EsbAppSettingsNames.Contains(s.Key));
                    var nonEsbSettings = allAppSettings.Where(s => !EsbEnvironment.EsbAppSettingsNames.Contains(s.Key));

                    nonEsbSettings.ToList().ForEach(e => application.AppSettings.Add(e.Key, e.Value));
                    esbEnvironmentSettings.ToList().ForEach(e => application.EsbConnectionSettings.Add(e.Key, e.Value));
                }
            }
            catch (Exception ex)
            {
                //debug
                string msg = ex.Message;
            }
        }
    }
}
