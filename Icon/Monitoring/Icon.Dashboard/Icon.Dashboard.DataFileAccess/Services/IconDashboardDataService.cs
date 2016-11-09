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
                    var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly().CodeBase);
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
            nameof(IApplication.AppSettings),
            nameof(IApplication.ValidCommands),
            nameof(IApplication.StatusIsGreen)
        };

        /// <summary>
        /// Adds an application definition to the configuration.
        /// </summary>
        /// <param name="application">Only need to supply the name of the application.</param>
        /// <param name="pathToXmlDataFile">Path to the configuraiton file</param>
        public void AddApplication(IApplication application, string pathToXmlDataFile)
        {
            var appName = application.Name;
            var appServer = application.Server;
            var targetConfig = LoadDataFile(pathToXmlDataFile);

            var duplicateAppNames = targetConfig.Root.Descendants(ApplicationSchema.Application).Where(e =>
                e.Attribute(nameof(application.Name)).Value.Equals(appName, StringComparison.InvariantCultureIgnoreCase)
                && e.Attribute(nameof(application.Server)).Value.Equals(appServer, StringComparison.InvariantCultureIgnoreCase));

            if (duplicateAppNames.Any())
            {
                throw new ArgumentException(
                    string.Format("Application Name:{0} Server:{1} already exists.", appName, appServer));
            }

            var properties = typeof(IApplication)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where (p => !applicationPropertiesToSkipWhenSaving.Contains(p.Name))
                .AsParallel()
                .Select(p => new XAttribute(p.Name, p.GetValue(application) ?? string.Empty));

            targetConfig.Root.Element(ApplicationSchema.Applications).Add(
                new XElement(
                    ApplicationSchema.Application,
                    properties));

            targetConfig.Save(pathToXmlDataFile);
        }

        /// <summary>
        /// Deletes and application definition from the configuration.
        /// </summary>
        /// <param name="application">Only need to supply the name of the application.</param>
        /// <param name="pathToXmlDataFile">Path to the configuraiton file</param>
        public void DeleteApplication(IApplication application, string pathToXmlDataFile)
        {
            var targetConfig = LoadDataFile(pathToXmlDataFile);
            var appConfig = targetConfig.Root.Element(ApplicationSchema.Applications).Elements(ApplicationSchema.Application)
                .FirstOrDefault(
                    e => e.Attribute(nameof(application.Name)).Value.Equals(application.Name, StringComparison.InvariantCultureIgnoreCase)
                    && e.Attribute(nameof(application.Server)).Value.Equals(application.Server, StringComparison.InvariantCultureIgnoreCase));

            appConfig?.Remove();

            targetConfig.Save(pathToXmlDataFile);
        }

        public IApplication GetApplication(string pathToXmlDataFile, string appName, string server)
        {
            if (pathToXmlDataFile == null) throw new ArgumentNullException(nameof(pathToXmlDataFile));
            if (appName == null) throw new ArgumentNullException(nameof(appName));
            if (server == null) throw new ArgumentNullException(nameof(server));

            var applications = new ConcurrentBag<IApplication>();
            var factories = this.ApplicationFactories.ToDictionary(af => af.GetType().Name.ToLowerInvariant());
            var dataFile = this.LoadDataFile(pathToXmlDataFile);

            var applicationElements = dataFile.Root.Element(ApplicationSchema.Applications).Elements(ApplicationSchema.Application);

            var matchingApplicationElement = applicationElements.FirstOrDefault(el =>
                String.Compare(el.Attribute("Name")?.Value, appName, StringComparison.InvariantCultureIgnoreCase) == 0 &&
                String.Compare(el.Attribute("Server")?.Value,server, StringComparison.InvariantCultureIgnoreCase)==0);

            if (matchingApplicationElement != default(XElement))
            {
                // Convention that the factory for an application must be named <ApplicationType>Factory.
                ApplicationFactory appFactory;
                var factoryName = matchingApplicationElement.Attribute(ApplicationSchema.TypeOfApplication).Value + "Factory";
                if (factories.TryGetValue(factoryName.ToLowerInvariant(), out appFactory))
                {
                    return appFactory.GetApplication(matchingApplicationElement);
                }
            }

            return (IApplication)null;
        }

        /// <summary>
        /// Retrieves all applications from configuration.
        /// </summary>
        /// <param name="targetConfig">The xml config where the applications are defined.</param>
        /// <param name="environment">Filter by environment (DEV/TEST/QA/PROD)</param>
        /// <returns>Applications from the configuration file that the service could create.</returns>
        public IEnumerable<IApplication> GetApplications(string pathToXmlDataFile, EnvironmentEnum environment)
        {
            var applications = GetApplications(pathToXmlDataFile);
            return applications.Where(a => a.Environment == environment).OrderBy(a => a.DisplayName);
        }

        /// <summary>
        /// Updates the application definition in the configuraiton
        /// </summary>
        /// <param name="application">Only need to supply the name of the application.</param>
        /// <param name="pathToXmlDataFile">Path to the configuraiton file</param>
        /// <returns>XDocument</returns>
        public void UpdateApplication(IApplication application, string pathToXmlDataFile)
        {
            var targetConfig = LoadDataFile(pathToXmlDataFile);

            var appConfig = targetConfig.Root.Element(ApplicationSchema.Applications).Elements(ApplicationSchema.Application)
                .FirstOrDefault(
                    e => e.Attribute(nameof(application.Name)).Value.Equals(application.Name, StringComparison.InvariantCultureIgnoreCase)
                    && e.Attribute(nameof(application.Server)).Value.Equals(application.Server, StringComparison.InvariantCultureIgnoreCase));

            var properties = typeof(IApplication)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => !applicationPropertiesToSkipWhenSaving.Contains(p.Name))
                .AsParallel()
                .Select(p => new XAttribute(p.Name, p.GetValue(application) ?? string.Empty));

            appConfig?.ReplaceWith(
                new XElement(
                    ApplicationSchema.Application,
                    properties));

            targetConfig.Save(pathToXmlDataFile);
        }

        public void SaveAppSettings(IApplication application)
        {
            try
            {
                var appConfig = XDocument.Load(application.ConfigFilePath);

                appConfig.Root.Element("appSettings").ReplaceNodes(
                    application.AppSettings.Select(i =>
                        new XElement("add",
                            new XAttribute("key", i.Key),
                            new XAttribute("value", i.Value))));

                appConfig.Save(application.ConfigFilePath);
            }
            catch(Exception)
            {
            }
        }

        public void Start(IApplication application, params string[] args)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));
            if (string.IsNullOrEmpty(application.Name)) throw new ArgumentNullException(nameof(application.Name));
            if (string.IsNullOrEmpty(application.Server)) throw new ArgumentNullException(nameof(application.Server));

            application.Start(args);
        }

        public void Stop(IApplication application)
        {
            if (application == null) throw new ArgumentNullException(nameof(application));
            if (string.IsNullOrEmpty(application.Name)) throw new ArgumentNullException(nameof(application.Name));
            if (string.IsNullOrEmpty(application.Server)) throw new ArgumentNullException(nameof(application.Server));

            application.Stop();
        }        

        /// <summary>
        /// Checks the structure of the data file and applies corrections if needed. (For example,
        ///  if the data file has a different root element than the current schema expects.)
        /// </summary>
        public void VerifyDataFileSchema(string pathToXmlDataFile, string pathToSchema)
        {
            var xmlDocument = LoadDataFile(pathToXmlDataFile);

            var currentRootName = xmlDocument.Root.Name.ToString();

            //is there an unexepected root element?
            if (String.Compare(currentRootName, ApplicationSchema.Root, StringComparison.CurrentCultureIgnoreCase) != 0)
            {
                // is this an old-style schema? (before Applications was moved below the new IconApplicationData root element)
                if (String.Compare(currentRootName, ApplicationSchema.Applications, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    var applicationsElement = xmlDocument.Root;
                    var newRoot = new XElement(ApplicationSchema.Root, applicationsElement);
                    xmlDocument.Root.ReplaceWith(newRoot);
                    xmlDocument.Save(pathToXmlDataFile);
                }
            }
            
            XmlSchemaSet schemaSet = new XmlSchemaSet();

            schemaSet.Add(null, pathToSchema);
            xmlDocument.Validate(schemaSet, new ValidationEventHandler(XmlSchemaValidationEventHandler));
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

        #region Private Methods

        /// <summary>
        /// Retrieves all applications from configuration.
        /// </summary>
        /// <param name="targetConfig">The xml config where the applications are defined.</param>
        /// <returns>Applications from the configuration file that the service could create.</returns>
        private IEnumerable<IApplication> GetApplications(string pathToXmlDataFile)
        {
            var applications = new ConcurrentBag<IApplication>();
            var factories = this.ApplicationFactories.ToDictionary(af => af.GetType().Name.ToLowerInvariant());
            var dataFile = this.LoadDataFile(pathToXmlDataFile);
            var applicationElements = dataFile.Root.Element(ApplicationSchema.Applications).Elements(ApplicationSchema.Application);

            Parallel.ForEach(applicationElements, ac =>
            {
                // Convention that the factory for an application must be named <ApplicationType>Factory.
                ApplicationFactory appFactory;
                var factoryName = ac.Attribute(ApplicationSchema.TypeOfApplication).Value + "Factory";

                if (factories.TryGetValue(factoryName.ToLowerInvariant(), out appFactory))
                {
                    applications.Add(appFactory.GetApplication(ac));
                }
            });

            return applications;
        }
 
        private XDocument LoadDataFile(string pathToXmlDataFile)
        {
            if (!System.IO.File.Exists(pathToXmlDataFile))
                throw new IOException($"Unable to load xml data file '{pathToXmlDataFile}'");

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
    }
}