namespace Icon.Dashboard.DataFileAccess.Services
{
    using Icon.Dashboard.DataFileAccess.Models;
    using System;
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

            string configValue;

            application.Name = config.TryGetValue(nameof(application.Name), out configValue) ? configValue : string.Empty;
            application.ConfigFilePath = config.TryGetValue(nameof(application.ConfigFilePath), out configValue) ? configValue : string.Empty;
            application.DisplayName = config.TryGetValue(nameof(application.DisplayName), out configValue) ? configValue : string.Empty;
            application.Server = config.TryGetValue(nameof(application.Server), out configValue) ? configValue : string.Empty;
            application.Environment = config.TryGetValue(nameof(application.Environment), out configValue)
                ? (EnvironmentEnum)Enum.Parse(typeof(EnvironmentEnum), configValue) : EnvironmentEnum.Undefined;
            application.DataFlowFrom = config.TryGetValue(nameof(application.DataFlowFrom), out configValue)
                ? (DataFlowSystemEnum)Enum.Parse(typeof(DataFlowSystemEnum), configValue) : DataFlowSystemEnum.None;
            application.DataFlowTo = config.TryGetValue(nameof(application.DataFlowTo), out configValue)
                ? (DataFlowSystemEnum)Enum.Parse(typeof(DataFlowSystemEnum), configValue) : DataFlowSystemEnum.None;
            application.LoggingName = config.TryGetValue(nameof(application.LoggingName), out configValue) ? configValue : string.Empty;
        }

        // This method tries to grab the app.config from the configFilePath and load the appsettings
        protected virtual void LoadAppSettings(IApplication application)
        {
            try
            {
                var appConfig = XDocument.Load(application.ConfigFilePath);
                var settings = appConfig.Root.Element("appSettings").Elements().Select(e => new
                {
                    Key = e.Attribute("key").Value,
                    Value = e.Attribute("value").Value
                });

                settings.ToList().ForEach(e => application.AppSettings.Add(e.Key, e.Value));
            }
            catch(Exception)
            {
            }
        }
    }
}
