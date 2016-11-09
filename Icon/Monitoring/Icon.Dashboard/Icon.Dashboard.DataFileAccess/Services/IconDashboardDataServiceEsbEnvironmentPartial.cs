namespace Icon.Dashboard.DataFileAccess.Services
{
    using Constants;
    using Models;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Linq;
    
    public sealed partial class IconDashboardDataService : IIconDashboardDataService
    {
        [ImportMany(typeof(EsbEnvironmentFactory))]
        public IEnumerable<EsbEnvironmentFactory> EsbEnvironmentFactories { get; private set; }

        List<string> esbEnvironmentPropertiesToSkipWhenSaving = new List<string>()
        {
            nameof(IEsbEnvironment.Applications)
        };        

        public IEnumerable<IEsbEnvironment> GetEsbEnvironments(string pathToXmlDataFile)
        {
            var applications = new ConcurrentBag<IEsbEnvironment>();
            var factories = this.EsbEnvironmentFactories.ToDictionary(af => af.GetType().Name.ToLowerInvariant());
            var dataFile = this.LoadDataFile(pathToXmlDataFile);
            if (dataFile.Root.Element(EsbEnvironmentSchema.EsbEnvironments) == null ||
                dataFile.Root.Element(EsbEnvironmentSchema.EsbEnvironments).Elements(EsbEnvironmentSchema.EsbEnvironment)==null)
            {
                return null;
            }
            var elements = dataFile.Root.Element(EsbEnvironmentSchema.EsbEnvironments).Elements(EsbEnvironmentSchema.EsbEnvironment);

            Parallel.ForEach(elements, e =>
            {
                // Convention that the factory for an application must be named <ApplicationType>Factory.
                EsbEnvironmentFactory esbEnvironmentFactory;
                var factoryName = $"{EsbEnvironmentSchema.EsbEnvironment}Factory";
                if (factories.TryGetValue(factoryName.ToLowerInvariant(), out esbEnvironmentFactory))
                {
                    applications.Add(esbEnvironmentFactory.GetEsbEnvironment(e));
                }
            });

            return applications;
        }

        public IEsbEnvironment GetEsbEnvironment(string pathToXmlDataFile, string name)
        {
            if (pathToXmlDataFile == null) throw new ArgumentNullException(nameof(pathToXmlDataFile));
            if (name == null) throw new ArgumentNullException(nameof(name));

            var esbEnvironments = GetEsbEnvironments(pathToXmlDataFile);

            return esbEnvironments.FirstOrDefault(
                a => a.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public void AddEsbEnvironment(IEsbEnvironment esbEnvironment, string pathToXmlDataFile)
        {
            var environmentName = esbEnvironment.Name;
            var dataFile = LoadDataFile(pathToXmlDataFile);

            var duplicateEnivronmentNames = dataFile.Root.Descendants(EsbEnvironmentSchema.EsbEnvironment)
                .Where(e =>
                    e.Attribute(nameof(esbEnvironment.Name)).Value.Equals(environmentName, StringComparison.InvariantCultureIgnoreCase));

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

        private XElement BuildEsbEnvironmentElement(IEsbEnvironment esbEnvironment)
        {
            var environmentAttributes = typeof(IEsbEnvironment)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => !esbEnvironmentPropertiesToSkipWhenSaving.Contains(p.Name))
                .AsParallel()
                .Select(p => new XAttribute(p.Name, p.GetValue(esbEnvironment) ?? string.Empty));
            var esbEnvironmentElement = new XElement(EsbEnvironmentSchema.EsbEnvironment, environmentAttributes);

            var applicationsElement = new XElement(EsbEnvironmentSchema.EsbEnvironmentApplications);
            if (esbEnvironment.Applications != null)
            {
                foreach (var app in esbEnvironment.Applications)
                {
                    var appIdentifierAttributes = typeof(IconApplicationIdentifier)
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                        .AsParallel()
                        .Select(p => new XAttribute(p.Name, p.GetValue(app) ?? string.Empty));
                    var appIdentifierElement = new XElement(EsbEnvironmentSchema.EsbEnvironmentAppIdentifier, appIdentifierAttributes);
                    applicationsElement.Add(appIdentifierElement);
                }
            }
            esbEnvironmentElement.Add(applicationsElement);
            return esbEnvironmentElement;
        }

        public void UpdateEsbEnvironment(IEsbEnvironment esbEnvironment, string pathToXmlDataFile)
        {
            var dataFile = LoadDataFile(pathToXmlDataFile);
            var currentData = GetEsbEnvironmentElement(dataFile, esbEnvironment);

            var esbEnvironmentElement = BuildEsbEnvironmentElement(esbEnvironment);
            currentData?.ReplaceWith(esbEnvironmentElement);

            dataFile.Save(pathToXmlDataFile);
        }

        public void DeleteEsbEnvironment(IEsbEnvironment esbEnvironment, string pathToXmlDataFile)
        {
            var dataFile = LoadDataFile(pathToXmlDataFile);
            var currentData = GetEsbEnvironmentElement(dataFile, esbEnvironment);
            currentData?.Remove();

            dataFile.Save(pathToXmlDataFile);
        }

        private XElement GetEsbEnvironmentElement(XDocument dataFile, IEsbEnvironment esbEnvironment)
        {
            return dataFile.Root.Element(EsbEnvironmentSchema.EsbEnvironments).Elements(EsbEnvironmentSchema.EsbEnvironment)
                .FirstOrDefault(e => e.Attribute(nameof(esbEnvironment.Name)).Value.Equals(esbEnvironment.Name, StringComparison.InvariantCultureIgnoreCase));
        }

        public Tuple<bool,string> UpdateApplicationEsbEnvironmentAndRestart(IconApplicationIdentifier applicationIdentifier, IEsbEnvironment esbEnvironment, string pathToXmlDataFile)
        {
            try
            {
                var application = GetApplication(pathToXmlDataFile, applicationIdentifier.Name, applicationIdentifier.Server);

                if (application != null)
                {
                    SetAppSettingsIfItExists(application, EsbAppSettings.ServerUrlKey, esbEnvironment.ServerUrl);
                    SetAppSettingsIfItExists(application, EsbAppSettings.TargetHostNameKey, esbEnvironment.TargetHostName);
                    SetAppSettingsIfItExists(application, EsbAppSettings.JmsUsernameKey, esbEnvironment.JmsUsername);
                    SetAppSettingsIfItExists(application, EsbAppSettings.JmsPasswordKey, esbEnvironment.JmsPassword);
                    SetAppSettingsIfItExists(application, EsbAppSettings.JndiUsernameKey, esbEnvironment.JndiUsername);
                    SetAppSettingsIfItExists(application, EsbAppSettings.JndiPasswordKey, esbEnvironment.JndiPassword);

                    SaveAppSettings(application);

                    if (application.TypeOfApplication == ApplicationTypeEnum.WindowsService)
                    {
                        application.Stop();
                        application.Start();
                    }
                    return new Tuple<bool, string>(true, String.Empty);
                }
                return new Tuple<bool, string>(false, $"Unknown application {applicationIdentifier.Name}-{applicationIdentifier.Server}");
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                return new Tuple<bool, string>(false, e.Message);
            }
        }

        public List<Tuple<bool,string>> UpdateApplicationsToEsbEnvironment(IEsbEnvironment esbEnvironment, string dataFile)
        {
            var resultCollection = new ConcurrentBag<Tuple<bool, string>>();

            Parallel.ForEach(esbEnvironment.Applications, (eachAppId) => 
                resultCollection.Add(UpdateApplicationEsbEnvironmentAndRestart(eachAppId, esbEnvironment, dataFile)));

            return resultCollection.ToList();
        }

        private void SetAppSettingsIfItExists(IApplication application, string key, string value)
        {
            if (application.AppSettings.ContainsKey(key))
            {
                application.AppSettings[key] = value;
            }
        }

        private string GetAppSettingsIfItExists(IApplication application, string key)
        {
            if (application.AppSettings.ContainsKey(key))
            {
                return application.AppSettings[key];
            }
            return null;
        }

        /// <summary>
        /// Searches the data file to find whether one of the esb envrionments defined in the data 
        ///   has every ESB-using application configured to use the server associated with the 
        ///   esb environment definition
        /// </summary>
        /// <param name="pathToXmlDataFile">ICON Dashboard data file</param>
        /// <returns>Returns an object implementing IEsbEnvrionment. Or null if none of the environments has all its applications
        /// configured for the same esb environment.</returns>
        public IEsbEnvironment GetCurrentEsbEnvironment(string pathToXmlDataFile)
        {
            var bestMatchingEnvironment = new Tuple<int, IEsbEnvironment>(0,null);
            var dataFile = LoadDataFile(pathToXmlDataFile);
            var esbEnvironmentDefinitions = GetEsbEnvironments(pathToXmlDataFile);

            if (esbEnvironmentDefinitions != null)
            {
                foreach (var currentEnvironment in esbEnvironmentDefinitions)
                {
                    var applicationsForEnvironment = currentEnvironment.Applications.Select(a => GetApplication(pathToXmlDataFile, a.Name, a.Server));

                    //how many of the apps associated with this environment have app settings keys for ESB communication?
                    int countOfAppsConfiguredForAnyEsbEnvironment = applicationsForEnvironment
                        .Where(a =>
                            a != null &&
                            a.AppSettings != null &&
                            a.AppSettings.ContainsKey(EsbAppSettings.ServerUrlKey) &&
                            a.AppSettings.ContainsKey(EsbAppSettings.TargetHostNameKey)
                            )
                        .Count();

                    // how many of the apps associated with this environment are configured to communicate with the current environment we are iterating? 
                    int countOfAppsConfiguredForThisEnvironment = applicationsForEnvironment
                        .Where(a =>
                            a != null &&
                            a.AppSettings != null &&
                            a.AppSettings.ContainsKey(EsbAppSettings.ServerUrlKey) &&
                            a.AppSettings.ContainsKey(EsbAppSettings.TargetHostNameKey) &&
                            String.Compare(a.AppSettings[EsbAppSettings.ServerUrlKey], currentEnvironment.ServerUrl, StringComparison.CurrentCultureIgnoreCase) == 0 &&
                            String.Compare(a.AppSettings[EsbAppSettings.TargetHostNameKey], currentEnvironment.TargetHostName, StringComparison.CurrentCultureIgnoreCase) == 0)
                        .Count();

                    // if this environment has some apps configured for ESB AND
                    // all apps that are configured for ESB are connected to the the current environment's ESB server AND
                    // either we have not yet found a current environment or this one has more apps talking to ESB than any previous match
                    if ((countOfAppsConfiguredForAnyEsbEnvironment > 0)
                        && (countOfAppsConfiguredForThisEnvironment - countOfAppsConfiguredForAnyEsbEnvironment == 0)
                        && countOfAppsConfiguredForThisEnvironment > bestMatchingEnvironment.Item1)
                    {
                        //this is the ESB environment with the most apps currently configured for it (and none configured for any other)
                        bestMatchingEnvironment = new Tuple<int, IEsbEnvironment>(countOfAppsConfiguredForThisEnvironment, currentEnvironment);
                    }
                }
            }

            //return match (or null)
            return bestMatchingEnvironment.Item2;
        }
    }
}