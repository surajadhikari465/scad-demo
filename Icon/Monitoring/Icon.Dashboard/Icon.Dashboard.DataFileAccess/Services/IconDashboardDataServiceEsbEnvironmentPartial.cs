namespace Icon.Dashboard.DataFileAccess.Services
{
    using Constants;
    using Models;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public sealed partial class IconDashboardDataService : IIconDashboardDataService
    {
        [ImportMany(typeof(EsbEnvironmentFactory))]
        public IEnumerable<EsbEnvironmentFactory> EsbEnvironmentFactories { get; private set; }

        /// <summary>
        /// Adds an ESB environment definition to the data file
        /// </summary>
        /// <param name="esbEnvironment">IEsbEnvironment implementation holding the data to be stored</param>
        /// <param name="pathToXmlDataFile">Path to the data file where the new definition will be written</param>
        public void AddEsbEnvironment(IEsbEnvironmentDefinition esbEnvironment, string pathToXmlDataFile)
        {
            var environmentName = esbEnvironment.Name;
            var dataFile = LoadDataFile(pathToXmlDataFile);

            var duplicateEnivronmentNames = dataFile.Root.Descendants(EsbEnvironmentSchema.EsbEnvironment)
                .Where(e =>
                    e.Attribute(nameof(esbEnvironment.Name)).Value.Equals(environmentName, _strcmpOption));

            if (duplicateEnivronmentNames.Any())
            {
                throw new ArgumentException($"ESB Environment Definition '{environmentName}' already exists.");
            }

            var esbEnvironmentElement = BuildEsbEnvironmentElement(esbEnvironment);

            if (dataFile.Root.Element(EsbEnvironmentSchema.EsbEnvironments) == null)
            {
                dataFile.Root.Add(new XElement(EsbEnvironmentSchema.EsbEnvironments));
            }
            dataFile.Root.Element(EsbEnvironmentSchema.EsbEnvironments).Add(esbEnvironmentElement);
            dataFile.Save(pathToXmlDataFile);
        }

        /// <summary>
        /// Removes an ESB environment definition from the data store
        /// </summary>
        /// <param name="esbEnvironment">an instance of IEsbEnvironment whose properties will identify
        ///     the data to be removed</param>
        /// <param name="pathToXmlDataFile">path to the data file containing the definitions</param>
        public void DeleteEsbEnvironment(IEsbEnvironmentDefinition esbEnvironment, string pathToXmlDataFile)
        {
            var dataFile = LoadDataFile(pathToXmlDataFile);
            var currentData = GetEsbEnvironmentElement(dataFile, esbEnvironment);
            if (currentData != null) currentData.Remove();

            dataFile.Save(pathToXmlDataFile);
        }

        public IEsbEnvironmentDefinition GetEsbEnvironment(string pathToXmlDataFile, string name)
        {
            if (pathToXmlDataFile == null) throw new ArgumentNullException(nameof(pathToXmlDataFile));
            if (name == null) throw new ArgumentNullException(nameof(name));

            var esbEnvironments = GetEsbEnvironments(pathToXmlDataFile);

            return esbEnvironments.FirstOrDefault(
                a => a.Name.Equals(name, _strcmpOption));
        }

        public IEnumerable<IEsbEnvironmentDefinition> GetEsbEnvironments(string pathToXmlDataFile)
        {
            var environments = new ConcurrentBag<IEsbEnvironmentDefinition>();

            var dataFile = this.LoadDataFile(pathToXmlDataFile);
            if (dataFile.Root.Element(EsbEnvironmentSchema.EsbEnvironments) == null ||
                dataFile.Root.Element(EsbEnvironmentSchema.EsbEnvironments).Elements(EsbEnvironmentSchema.EsbEnvironment) == null)
            {
                return null;
            }
            var elements = dataFile.Root.Element(EsbEnvironmentSchema.EsbEnvironments).Elements(EsbEnvironmentSchema.EsbEnvironment);

            Parallel.ForEach(elements, element =>
            {
                var environment = ManufactureEsbEnvironment(element);
                if (environment != null) environments.Add(environment);
            });

            return environments;
        }

        public KeyValuePair<string, string> ReconfigureEsbSettingsForSingleApp(string pathToXmlDataFile,
            IIconApplication application, IEsbEnvironmentDefinition chosenEsbEnvironment)
        {
            string appName = application.NameAndServer;
            string reconfigureResult = String.Empty;
            try
            {
                SetAppSettingsIfItExists(application, EsbAppSettings.ServerUrlKey, chosenEsbEnvironment.ServerUrl);
                SetAppSettingsIfItExists(application, EsbAppSettings.TargetHostNameKey, chosenEsbEnvironment.TargetHostName);
                SetAppSettingsIfItExists(application, EsbAppSettings.JmsUsernameKey, chosenEsbEnvironment.JmsUsername);
                SetAppSettingsIfItExists(application, EsbAppSettings.JmsPasswordKey, chosenEsbEnvironment.JmsPassword);
                SetAppSettingsIfItExists(application, EsbAppSettings.JndiUsernameKey, chosenEsbEnvironment.JndiUsername);
                SetAppSettingsIfItExists(application, EsbAppSettings.JndiPasswordKey, chosenEsbEnvironment.JndiPassword);
                //SetAppSettingsIfItExists(application, EsbAppSettings.ConnectionFactoryNameKey, chosenEsbEnvironment.ConnectionFactoryName);
                SetAppSettingsIfItExists(application, EsbAppSettings.SslPasswordKey, chosenEsbEnvironment.SslPassword);
                // SetAppSettingsIfItExists(application, EsbAppSettings.QueueNameKey, chosenEsbEnvironment.QueueName);
                SetAppSettingsIfItExists(application, EsbAppSettings.SessionModeKey, chosenEsbEnvironment.SessionMode);
                SetAppSettingsIfItExists(application, EsbAppSettings.CertificateNameKey, chosenEsbEnvironment.CertificateName);
                SetAppSettingsIfItExists(application, EsbAppSettings.CertificateStoreNameKey, chosenEsbEnvironment.CertificateStoreName);
                SetAppSettingsIfItExists(application, EsbAppSettings.CertificateStoreLocationKey, chosenEsbEnvironment.CertificateStoreLocation);
                SetAppSettingsIfItExists(application, EsbAppSettings.ReconnectDelayKey, chosenEsbEnvironment.ReconnectDelay);
                SetAppSettingsIfItExists(application, EsbAppSettings.NumberOfListenerThreadsKey, chosenEsbEnvironment.NumberOfListenerThreads.ToString());

                SaveAppSettings(application);
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                reconfigureResult = e.Message;
            }
            return new KeyValuePair<string, string>(appName, reconfigureResult);
        }

        public Dictionary<string, string> ReconfigureEsbSettings(string pathToXmlDataFile,
            IEnumerable<IIconApplication> apps, string esbEnvironmentName)
        {
            var esbEnvironmentDefinitions = GetEsbEnvironments(pathToXmlDataFile);
            var matchingEnvironment = esbEnvironmentDefinitions
                .FirstOrDefault(e => e.Name.Equals(esbEnvironmentName, _strcmpOption));
            if (matchingEnvironment == default(IEsbEnvironmentDefinition))
            {
                if (!esbEnvironmentName.Equals("UNASSIGNED", _strcmpOption))
                {
                    throw new ArgumentException(String.Format("Unknown ESB Environment \"{0}\"", esbEnvironmentName));
                }
            }

            var reconfigResultDictionary = new Dictionary<string, string>(apps.Count());
            foreach (var app in apps)
            {
                var appBeforeAnyChanges = GetApplication(pathToXmlDataFile, app.Name, app.Server);
                if (appBeforeAnyChanges == default(IIconApplication))
                    throw new ArgumentException(String.Format("Unknown application {0} {1}", app.Name, app.Server));
                if (HasEsbEnvrionmentChangedForApp(appBeforeAnyChanges, matchingEnvironment.TargetHostName))
                {
                    var appAndResult = ReconfigureEsbSettingsForSingleApp(pathToXmlDataFile, appBeforeAnyChanges, matchingEnvironment);
                    reconfigResultDictionary.Add(appAndResult.Key, appAndResult.Value);
                }
            }
            return reconfigResultDictionary;
        }

        public bool HasEsbEnvrionmentChangedForApp(IIconApplication appBeforeChanges, string intendedEsbHost)
        {
            //check whether the existing, passed-in app definition has the same esb environment setting
            // as the provided name of the (possibly) new environment

            if (appBeforeChanges != null
                && appBeforeChanges.HasEsbConfiguration
                && appBeforeChanges.EsbConnectionSettings != null
                && appBeforeChanges.EsbConnectionSettings.ContainsKey("TargetHostName"))
            {
                var hasChanged = !appBeforeChanges.EsbConnectionSettings["TargetHostName"].Equals(intendedEsbHost, _strcmpOption);
                return hasChanged;
            }

            return false;
        }

        public Dictionary<string, string> ReconfigureEsbSettingsAndRestartServices(string pathToXmlDataFile,
            IEnumerable<IIconApplication> apps,
            string esbEnvironmentName,
            int waitTimeoutMilliseconds = ServiceConstants.DefaultServiceTimeoutMilliseconds)
        {
            var reconfigResults = ReconfigureEsbSettings(pathToXmlDataFile, apps, esbEnvironmentName);
            return reconfigResults;
            //TODO while debugging, disable service restarts
            //var restartResults = RestartServices(pathToXmlDataFile, apps, reconfigResults, waitTimeoutMilliseconds);
            //return restartResults;
        }

        public Dictionary<string, string> RestartServices(string pathToXmlDataFile,
            IEnumerable<IIconApplication> apps,
            Dictionary<string, string> reconfigResults,
            int waitTimeoutMilliseconds = ServiceConstants.DefaultServiceTimeoutMilliseconds)
        {
            var restartResults = new Dictionary<string, string>(apps.Count());

            foreach (var reconfigResult in reconfigResults)
            {
                string restartResult = reconfigResult.Value;
                //lack of the 2nd string means no error occurred, so we can assume the reconfig succeeded
                if (String.IsNullOrWhiteSpace(restartResult))
                {
                    var appToRestart = apps.FirstOrDefault(a => a.NameAndServer.Equals(reconfigResult.Key, _strcmpOption));
                    if (appToRestart == default(IIconApplication))
                    {
                        restartResult = String.Format("Unknown app {0} on {1} for attempted restart", reconfigResult.Key, reconfigResult.Value);
                    }
                    else
                    {
                        restartResult = RestartService(appToRestart, waitTimeoutMilliseconds);
                    }
                }
                restartResults.Add(reconfigResult.Key, restartResult);
            }
            return restartResults;
        }

        public void UpdateEsbEnvironment(IEsbEnvironmentDefinition esbEnvironment, string pathToXmlDataFile)
        {
            var dataFile = LoadDataFile(pathToXmlDataFile);
            var currentData = GetEsbEnvironmentElement(dataFile, esbEnvironment);

            var esbEnvironmentElement = BuildEsbEnvironmentElement(esbEnvironment);
            if (currentData != null) currentData.ReplaceWith(esbEnvironmentElement);

            dataFile.Save(pathToXmlDataFile);
        }

        private static string GetAppSettingsIfItExists(IIconApplication application, string key)
        {
            if (application.AppSettings.ContainsKey(key))
            {
                return application.AppSettings[key];
            }
            else if (application.EsbConnectionSettings.ContainsKey(key))
            {
                return application.EsbConnectionSettings[key];
            }
            return null;
        }

        private static XElement GetEsbEnvironmentElement(XDocument dataFile, IEsbEnvironmentDefinition esbEnvironment)
        {
            return dataFile.Root.Element(EsbEnvironmentSchema.EsbEnvironments).Elements(EsbEnvironmentSchema.EsbEnvironment)
                .FirstOrDefault(e => e.Attribute(nameof(esbEnvironment.Name)).Value.Equals(esbEnvironment.Name, _strcmpOption));
        }

        private static void SetAppSettingsIfItExists(IIconApplication application, string key, string value)
        {
            if (application.AppSettings.ContainsKey(key))
            {
                application.AppSettings[key] = value;
            }
            else if (application.EsbConnectionSettings.ContainsKey(key))
            {
                application.EsbConnectionSettings[key] = value;
            }
        }

        private XElement BuildEsbEnvironmentElement(IEsbEnvironmentDefinition esbEnvironment)
        {
            var environmentAttributes = typeof(IEsbEnvironmentDefinition)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .AsParallel()
                .Select(p => new XAttribute(p.Name, p.GetValue(esbEnvironment) ?? string.Empty));
            var esbEnvironmentElement = new XElement(EsbEnvironmentSchema.EsbEnvironment, environmentAttributes);
            return esbEnvironmentElement;
        }

        private IEsbEnvironmentDefinition ManufactureEsbEnvironment(XElement dataFileEsbEnvironmentElement)
        {
            try
            {
                if (dataFileEsbEnvironmentElement != default(XElement))
                {
                    var factories = this.EsbEnvironmentFactories.ToDictionary(af => af.GetType().Name.ToLowerInvariant());
                    var factoryName = $"{EsbEnvironmentSchema.EsbEnvironment}Factory";
                    EsbEnvironmentFactory esbEnvironmentFactory = new EsbEnvironmentFactory();
                    if (factories.TryGetValue(factoryName.ToLowerInvariant(), out esbEnvironmentFactory))
                    {
                        return esbEnvironmentFactory.GetEsbEnvironment(dataFileEsbEnvironmentElement);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Unable to generate esb environment factory named {factoryName}");
                    }
                }
                return (IEsbEnvironmentDefinition)null;
            }
            catch (Exception ex)
            {
                //for debugging
                string msg = ex.Message;
                return (IEsbEnvironmentDefinition)null;
            }
        }
    }
}