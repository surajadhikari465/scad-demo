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
    using System.ServiceProcess;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;

    [Export]
    public sealed partial class IconDashboardDataService : IIconDashboardDataService
    {
        [ImportMany(typeof(ApplicationFactory))]
        public IEnumerable<ApplicationFactory> ApplicationFactories { get; private set; }

        #region Singleton

        private static readonly Lazy<IconDashboardDataService> lazyInstance =
            new Lazy<IconDashboardDataService>(() =>
                {
                    AggregateCatalog catalog = new AggregateCatalog();
                    catalog.Catalogs.Add(new AssemblyCatalog(typeof(IconDashboardDataService).Assembly));
                    return new CompositionContainer(catalog).GetExportedValue<IconDashboardDataService>();
                });

        public static IconDashboardDataService Instance
        {
            get { return lazyInstance.Value; }
        }

        private IconDashboardDataService()
        {
        }

        #endregion

        List<string> applicationPropertiesToSkipWhenSaving = new List<string>()
        {
            nameof(IIconApplication.AppSettings),
            nameof(IIconApplication.ValidCommands),
            nameof(IIconApplication.StatusIsGreen),
            nameof(IIconApplication.EsbConnectionSettings)
        };

        /// <summary>
        /// Adds an application definition to the configuration.
        /// </summary>
        /// <param name="application">Only need to supply the name of the application.</param>
        /// <param name="pathToXmlDataFile">Path to the configuraiton file</param>
        public void AddApplication(IIconApplication application, string pathToXmlDataFile)
        {
            var appName = application.Name;
            var appServer = application.Server;
            var dataFile = LoadDataFile(pathToXmlDataFile);

            var duplicateAppNames = dataFile.Root.Descendants(ApplicationSchema.Application).Where(e =>
                e.Attribute(nameof(application.Name)).Value.Equals(appName, _strcmpOption)
                && e.Attribute(nameof(application.Server)).Value.Equals(appServer, _strcmpOption));

            if (duplicateAppNames.Any())
            {
                throw new ArgumentException(
                    string.Format("Application Name:{0} Server:{1} already exists.", appName, appServer));
            }

            var properties = typeof(IIconApplication)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where (p => !applicationPropertiesToSkipWhenSaving.Contains(p.Name))
                .AsParallel()
                .Select(p => new XAttribute(p.Name, p.GetValue(application) ?? string.Empty));

            dataFile.Root.Element(ApplicationSchema.Applications).Add(
                new XElement(
                    ApplicationSchema.Application,
                    properties));

            dataFile.Save(pathToXmlDataFile);
        }

        /// <summary>
        /// Deletes and application definition from the configuration.
        /// </summary>
        /// <param name="application">Only need to supply the name of the application.</param>
        /// <param name="pathToXmlDataFile">Path to the configuraiton file</param>
        public void DeleteApplication(IIconApplication application, string pathToXmlDataFile)
        {
            var dataFile = LoadDataFile(pathToXmlDataFile);
            var appConfig = dataFile.Root.Element(ApplicationSchema.Applications).Elements(ApplicationSchema.Application)
                .FirstOrDefault(
                    e => e.Attribute(nameof(application.Name)).Value.Equals(application.Name, _strcmpOption)
                    && e.Attribute(nameof(application.Server)).Value.Equals(application.Server, _strcmpOption));

            if (appConfig != null)
            {
                appConfig.Remove();
            }

            dataFile.Save(pathToXmlDataFile);
        }

        private IIconApplication ManufactureApplication(XElement dataFileApplicationElement)
        {
            try
            {
                if (dataFileApplicationElement != default(XElement))
                {
                    ApplicationFactory appFactory;
                    var factories = this.ApplicationFactories.ToDictionary(af => af.GetType().Name.ToLowerInvariant());
                    // Convention that the factory for an application must be named <ApplicationType>Factory.
                    var factoryName = dataFileApplicationElement.Attribute(ApplicationSchema.TypeOfApplication).Value + "Factory";
                    if (factories.TryGetValue(factoryName.ToLowerInvariant(), out appFactory))
                    {
                        return appFactory.GetApplication(dataFileApplicationElement);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Unable to generate app factory named {factoryName}");
                    }
                };
            }
            catch (Exception ex)
            {
                //for debugging
                string msg = ex.Message;
                //return (IApplication)null;
            }

            return (IIconApplication)null;
        }       

        public IIconApplication GetApplication(string pathToXmlDataFile, string appName, string server)
        {
            if (pathToXmlDataFile == null) throw new ArgumentNullException(nameof(pathToXmlDataFile));
            if (appName == null) throw new ArgumentNullException(nameof(appName));
            if (server == null) throw new ArgumentNullException(nameof(server));
            
            var dataFile = LoadDataFile(pathToXmlDataFile);
            var applicationElements = dataFile.Root.Element(ApplicationSchema.Applications).Elements(ApplicationSchema.Application);
            var matchingApplicationElement = applicationElements.FirstOrDefault(el =>
                el.Attribute("Name") != null && el.Attribute("Server") != null &&
                el.Attribute("Name").Value.Equals(appName, _strcmpOption) &&
                el.Attribute("Server").Value.Equals(server, _strcmpOption));

            return ManufactureApplication(matchingApplicationElement);
        }

        /// <summary>
        /// Updates the application definition in the configuraiton
        /// </summary>
        /// <param name="application">Only need to supply the name of the application.</param>
        /// <param name="pathToXmlDataFile">Path to the configuraiton file</param>
        /// <returns>XDocument</returns>
        public void UpdateApplication(IIconApplication application, string pathToXmlDataFile)
        {
            var dataFile = LoadDataFile(pathToXmlDataFile);

            var appConfig = dataFile.Root.Element(ApplicationSchema.Applications).Elements(ApplicationSchema.Application)
                .FirstOrDefault(
                    e => e.Attribute(nameof(application.Name)).Value.Equals(application.Name, _strcmpOption)
                    && e.Attribute(nameof(application.Server)).Value.Equals(application.Server, _strcmpOption));

            var properties = typeof(IIconApplication)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => !applicationPropertiesToSkipWhenSaving.Contains(p.Name))
                .AsParallel()
                .Select(p => new XAttribute(p.Name, p.GetValue(application) ?? string.Empty));

            if (appConfig != null)
            {
                appConfig.ReplaceWith(
                    new XElement(
                        ApplicationSchema.Application,
                        properties));
            }

            dataFile.Save(pathToXmlDataFile);
        }

        public void SaveAppSettings(IIconApplication application)
        {
            try
            {
                var appConfig = XDocument.Load(application.ConfigFilePath);

                // combine the AppSettings and the ESB-related subset of AppSettings into 1 collection
                var combinedDictionary = CombineDictionariesIgnoreDuplicates(
                    application.AppSettings, application.EsbConnectionSettings);

                // create XML elements for each setting
                var updatedElements = combinedDictionary.Select(i =>
                        new XElement("add",
                            new XAttribute("key", i.Key),
                            new XAttribute("value", i.Value ?? String.Empty)));

                //replace the existing appSettings node in the XML file with the new element collection
                var configAppSettingsElement = appConfig.Root.Element("appSettings");
                configAppSettingsElement.ReplaceNodes(updatedElements);
                appConfig.Save(application.ConfigFilePath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static Dictionary<T,U> CombineDictionariesIgnoreDuplicates<T,U>(params Dictionary<T,U>[] dictionaries)
        {
            // In case there are any duplicates between the two dictionaries, convert the 
            // combined dictionary to a lookup (which handles multiple values per key) and 
            // then re -convert back to a dictionary using only the first value per key
            var combinedDictionaryWithDuplicatesIgnored = dictionaries.SelectMany(dict => dict)
                       .ToLookup(pair => pair.Key, pair => pair.Value)
                       .ToDictionary(group => group.Key, group => group.First());
            return combinedDictionaryWithDuplicatesIgnored;
        }

        public string StartService(IIconApplication application, TimeSpan waitTime, params string[] args)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));
            if (string.IsNullOrEmpty(application.Name)) throw new ArgumentNullException(nameof(application.Name));
            if (string.IsNullOrEmpty(application.Server)) throw new ArgumentNullException(nameof(application.Server));

            try
            {
                application.Start(waitTime, args);
            }
            catch (Exception ex)
            {
                return String.Format("Could not start {0} service on {1}.{2} Error: '{3}'",
                        application.Name, application.Server, Environment.NewLine, ex.Message);
            }
            return String.Empty;
        }

        public string StopService(IIconApplication application, TimeSpan waitTime)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));
            if (string.IsNullOrEmpty(application.Name)) throw new ArgumentNullException(nameof(application.Name));
            if (string.IsNullOrEmpty(application.Server)) throw new ArgumentNullException(nameof(application.Server));

            try
            {
                application.Stop(waitTime);
            }
            catch (Exception ex)
            {
                return String.Format("Could not stop {0} service on {1}.{2} Error: '{3}'",
                    application.Name, application.Server, Environment.NewLine, ex.Message);
            }
            return String.Empty;
        }

        public string RestartService(IIconApplication application, int waitTimeoutMilliseconds = ServiceConstants.DefaultServiceTimeoutMilliseconds)
        {
            if (application.TypeOfApplication != ApplicationTypeEnum.WindowsService)
            {
                return String.Format("Unable to restart application {0} on {1} since it is not identified as a Windows Service (type: {3})",
                    application.Name, application.Server, application.TypeOfApplication);
            }
            try
            {
                StopService(application, TimeSpan.FromMilliseconds(waitTimeoutMilliseconds));
                StartService(application, TimeSpan.FromMilliseconds(waitTimeoutMilliseconds));
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null) ex = ex.InnerException;
                return ex.Message;
            }
            return String.Empty;
        }      

        /// <summary>
        /// Checks the structure of the data file and applies corrections if needed. (For example,
        ///  if the data file has a different root element than the current schema expects.)
        /// </summary>
        public void VerifyDataFileSchema(string pathToXmlDataFile, string pathToSchema)
        {
            var dataFile = LoadDataFile(pathToXmlDataFile);

            var currentRootName = dataFile.Root.Name.ToString();

            //is there an unexepected root element?
            if (currentRootName.Equals(ApplicationSchema.Root, _strcmpOption))
            {
                // is this an old-style schema? (before Applications was moved below the new IconApplicationData root element)
                if (currentRootName.Equals(ApplicationSchema.Applications, _strcmpOption))
                {
                    var applicationsElement = dataFile.Root;
                    var newRoot = new XElement(ApplicationSchema.Root, applicationsElement);
                    dataFile.Root.ReplaceWith(newRoot);
                    dataFile.Save(pathToXmlDataFile);
                }
            }
            
            XmlSchemaSet schemaSet = new XmlSchemaSet();

            schemaSet.Add(null, pathToSchema);
            dataFile.Validate(schemaSet, new ValidationEventHandler(XmlSchemaValidationEventHandler));
        }

        static void XmlSchemaValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    throw new XmlSchemaException($"Invalid XML Schema: {e.Message}");
                case XmlSeverityType.Warning:
                default:
                    break;
            }
        }

        /// <summary>
        /// Retrieves all applications from configuration.
        /// </summary>
        /// <param name="serverDataFilePath">The xml config where the applications are defined.</param>
        /// <returns>Applications from the configuration file that the service could create.</returns>
        public IEnumerable<IIconApplication> GetApplications(string serverDataFilePath)
        {
            var applications = new ConcurrentBag<IIconApplication>();

            var dataFile = this.LoadDataFile(serverDataFilePath);
            var applicationElements = dataFile.Root
                .Element(ApplicationSchema.Applications)
                .Elements(ApplicationSchema.Application);

            Parallel.ForEach(applicationElements, ac =>
            {
                var manufactured = ManufactureApplication(ac);
                if (manufactured != null) applications.Add(manufactured);
            });
            return applications;
        }

        #region Private Methods

        private XDocument LoadDataFile(string pathToXmlDataFile)
        {
            if (!System.IO.File.Exists(pathToXmlDataFile))
            {
                throw new IOException($"Unable to load xml data file '{pathToXmlDataFile}'");
            }

            var xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.Load(pathToXmlDataFile);

            using (var nodeReader = new XmlNodeReader(xmlDoc))
            {
                nodeReader.MoveToContent();
                var xDoc = XDocument.Load(nodeReader);

                return xDoc;
            }
        }

        #endregion

        private const StringComparison _strcmpOption = StringComparison.InvariantCultureIgnoreCase;
    }
}