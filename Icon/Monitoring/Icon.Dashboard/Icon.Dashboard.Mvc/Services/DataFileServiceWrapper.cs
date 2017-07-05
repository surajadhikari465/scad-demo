using Icon.Dashboard.DataFileAccess.Models;
using Icon.Dashboard.DataFileAccess.Services;
using Icon.Dashboard.Mvc.Helpers;
using Icon.Dashboard.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Icon.Dashboard.Mvc.Services
{
    public class DataFileServiceWrapper : IDataFileServiceWrapper
    {
        public IIconDashboardDataService IconMonitoringService { get; private set; }
        public IIconDatabaseServiceWrapper LoggingService { get; private set; }

        public DataFileServiceWrapper(IIconDashboardDataService dataService = null, IIconDatabaseServiceWrapper loggingServiceWrapper = null)
        {
            IconMonitoringService = dataService ?? IconDashboardDataService.Instance;
            LoggingService = loggingServiceWrapper ?? new IconDatabaseServiceWrapper();
        }

        public IEnumerable<IApplication> GetApplications(HttpServerUtilityBase serverUtility, string dataFileName)
        {
            var configFile = GetPathForDataFile(serverUtility, dataFileName);
            var apps = IconMonitoringService.GetApplications(configFile);
            return apps;
        }

        public IEnumerable<IconApplicationViewModel> GetApplicationListViewModels(HttpServerUtilityBase serverUtility, string dataFileName)
        {
            var apps = this.GetApplications(serverUtility, dataFileName);
            var viewModels = new List<IconApplicationViewModel>(apps.Count());

            foreach (var app in apps)
            {
                viewModels.Add(CreateViewModel(serverUtility, dataFileName, app));
            }
            return viewModels;
        }

        public IEnumerable<IconApplicationViewModel> ToViewModels(IEnumerable<IApplication> applications,
            HttpServerUtilityBase serverUtility, string dataFileName)
        {
            var viewModels = new List<IconApplicationViewModel>(applications.Count());

            foreach (var app in applications)
            {
                viewModels.Add(CreateViewModel(serverUtility, dataFileName, app));
            }
            return viewModels;
        }

        public IApplication GetApplication(HttpServerUtilityBase serverUtility, string dataFileName, string application, string server)
        {
            var configFile = this.GetPathForDataFile(serverUtility, dataFileName);
            var app = IconMonitoringService.GetApplication(configFile, application, server);
            return app;
        }

        public IconApplicationViewModel GetApplicationViewModel(HttpServerUtilityBase serverUtility, string dataFileName, string application, string server)
        {
            var configFile = this.GetPathForDataFile(serverUtility, dataFileName);
            var app = IconMonitoringService.GetApplication(configFile, application, server);
            var viewModel = CreateViewModel(serverUtility, dataFileName, app);
            return viewModel;
        }

        public ServiceViewModel GetServiceViewModel(HttpServerUtilityBase serverUtility, string dataFileName, string application, string server)
        {
            var configFile = this.GetPathForDataFile(serverUtility, dataFileName);
            var app = IconMonitoringService.GetApplication(configFile, application, server) as WindowsService;
            var serviceViewModel = CreateViewModel(serverUtility, dataFileName, app) as ServiceViewModel;
            return serviceViewModel;
        }

        public void ExecuteCommand(HttpServerUtilityBase serverUtility, string dataFileName, string application, string server, string command)
        {
            var configFile = this.GetPathForDataFile(serverUtility, dataFileName);
            var app = IconMonitoringService.GetApplication(configFile, application, server);
            if (app == null) throw new ArgumentNullException($"Unable to find process to monitor ({server}-{application})");
            if (app.TypeOfApplication == ApplicationTypeEnum.ScheduledTask) command = "Start";

            switch (command)
            {
                case "Start":
                case "start":
                    string[] startArgs = null;
                    IconMonitoringService.Start(app, startArgs);
                    break;
                case "Stop":
                case "stop":
                default:
                    IconMonitoringService.Stop(app);
                    break;
            }
        }

        public string GetPathForDataFile(HttpServerUtilityBase serverUtility, string dataFileName, bool validateXml = true)
        {
            if (serverUtility == null) throw new ArgumentNullException(nameof(serverUtility));
            if (String.IsNullOrWhiteSpace(dataFileName)) throw new ArgumentNullException(nameof(dataFileName));
            var dataFilePath = serverUtility.MapPath($"~/App_Data/{dataFileName}");

            if (!File.Exists(dataFilePath))
            {
                throw new FileNotFoundException($"Unable to find or read application data file for dashboard ('{dataFilePath}')", dataFilePath);
            }

            if (validateXml)
            {
                string pathToXsdSchema = "Applications.xsd";
                var schemaFilePath = serverUtility.MapPath($"~/App_Data/{pathToXsdSchema}");
                IconMonitoringService.VerifyDataFileSchema(dataFilePath, schemaFilePath);
            }

            return dataFilePath;
        }

        public void UpdateApplication(HttpServerUtilityBase serverUtility, IconApplicationViewModel appViewModel, string dataFileName)
        {
            var configFile = GetPathForDataFile(serverUtility, dataFileName);
            var updatedApplication = FromViewModel(appViewModel);
            IconMonitoringService.UpdateApplication(updatedApplication, configFile);
        }

        public void AddApplication(HttpServerUtilityBase serverUtility, IconApplicationViewModel appViewModel, string dataFileName)
        {
            var configFile = GetPathForDataFile(serverUtility, dataFileName);
            var newApplication = FromViewModel(appViewModel);
            IconMonitoringService.AddApplication(newApplication, configFile);
        }

        public void DeleteApplication(HttpServerUtilityBase serverUtility, IApplication application, string dataFileName)
        {
            var configFile = GetPathForDataFile(serverUtility, dataFileName);
            IconMonitoringService.DeleteApplication(application, configFile);
        }

        public void SaveAppSettings(IconApplicationViewModel viewModel)
        {
            IconMonitoringService.SaveAppSettings(FromViewModel(viewModel));
        }

        public IApplication FromViewModel(IconApplicationViewModel viewModel)
        {
            IApplication app = null;
            switch (viewModel.TypeOfApplication)
            {
                case ApplicationTypeEnum.WindowsService:
                    app = new WindowsService();
                    PopulatePropertiesFromViewModel(app, viewModel);
                    break;
                case ApplicationTypeEnum.ScheduledTask:
                //case ApplicationTypeEnum.SqlAgentJob:
                case ApplicationTypeEnum.Unknown:
                default:
                    break;
            }
            return app;
        }

        private void PopulatePropertiesFromViewModel(IApplication app, IconApplicationViewModel viewModel)
        {
            app.Server = viewModel.Server;
            app.Name = viewModel.Name;
            app.DisplayName = viewModel.DisplayName;
            app.DataFlowFrom = viewModel.DataFlowFrom;
            app.DataFlowTo = viewModel.DataFlowTo;
            app.ConfigFilePath = viewModel.ConfigFilePath;
            app.LoggingName = viewModel.LoggingName;

            // Updating basic settings does not need to call a save to the app.config's appsettings.
            viewModel.AppSettings?.ToList().ForEach(e =>
               app.AppSettings[e.Key] = e.Value ?? string.Empty);
        }

        private ServiceViewModel CreateViewModel(
           HttpServerUtilityBase serverUtility, string dataFileName, WindowsService app)
        {
            if (app == null) return null;
            var viewModel = new ServiceViewModel(app);
            SetCurrentEsbEnvironment(viewModel, serverUtility, dataFileName, app); ;
            return viewModel;
        }

        private void SetCurrentEsbEnvironment(IconApplicationViewModel viewModel,
            HttpServerUtilityBase serverUtility, string dataFileName, IApplication app)
        {
            viewModel.CurrentEsbEnvironment = GetCurrentEsbEnvironmentForApplication(serverUtility, dataFileName, app);
            viewModel.ConfigFilePathIsValid = File.Exists(viewModel.ConfigFilePath);
        }

        private IconApplicationViewModel CreateViewModel(
            HttpServerUtilityBase serverUtility, string dataFileName, IApplication app)
        {
            if (app == null) return null;
            IconApplicationViewModel viewModel = null;

            switch (app.TypeOfApplication)
            {
                case ApplicationTypeEnum.WindowsService:
                    viewModel = new ServiceViewModel(app as WindowsService);
                    break;
                case ApplicationTypeEnum.ScheduledTask:
                case ApplicationTypeEnum.Unknown:
                default:
                    viewModel = new IconApplicationViewModel(app);
                    break;
            }

            SetCurrentEsbEnvironment(viewModel, serverUtility, dataFileName, app);
            return viewModel;
        }

        public static IEsbEnvironment FromViewModel(EsbEnvironmentViewModel viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));

            IEsbEnvironment environment = new EsbEnvironment
            {
                Name = viewModel.Name,
                ServerUrl = viewModel.ServerUrl,
                TargetHostName = viewModel.TargetHostName,
                JmsUsername = viewModel.JmsUsername,
                JmsPassword = viewModel.JmsPassword,
                JndiUsername = viewModel.JndiUsername,
                JndiPassword = viewModel.JndiPassword
            };

            viewModel.Applications?.ToList().ForEach(a => environment.AddApplication(a.Name, a.Server));

            return environment;
        }

        public IEnumerable<EsbEnvironmentViewModel> GetEsbEnvironments(HttpServerUtilityBase serverUtility, string pathToXmlDataFile)
        {
            var configFile = GetPathForDataFile(serverUtility, pathToXmlDataFile);
            var esbEnvironments = IconMonitoringService.GetEsbEnvironments(configFile)
                .Select(each => new EsbEnvironmentViewModel(each));
            return esbEnvironments;
        }

        public IEnumerable<string> GetEsbEnvironmentNames(HttpServerUtilityBase serverUtility, string pathToXmlDataFile)
        {
            var esbEnvironments = GetEsbEnvironments(serverUtility, pathToXmlDataFile);
            if (esbEnvironments != null)
            {
                return esbEnvironments.Select(e => e.Name);
            }
            return null;
        }

        public EsbEnvironmentViewModel GetEsbEnvironment(HttpServerUtilityBase serverUtility, string pathToXmlDataFile, string name)
        {
            var configFile = GetPathForDataFile(serverUtility, pathToXmlDataFile);
            var esbEnvironment = new EsbEnvironmentViewModel(IconMonitoringService.GetEsbEnvironment(configFile, name));
            return esbEnvironment;
        }

        public void AddEsbEnvironment(HttpServerUtilityBase serverUtility, EsbEnvironmentViewModel esbEnvironment, string pathToXmlDataFile)
        {
            var configFile = GetPathForDataFile(serverUtility, pathToXmlDataFile);
            var newEnvironment = FromViewModel(esbEnvironment);
            IconMonitoringService.AddEsbEnvironment(newEnvironment, configFile);
        }

        public void UpdateEsbEnvironment(HttpServerUtilityBase serverUtility, EsbEnvironmentViewModel esbEnvironment, string pathToXmlDataFile)
        {
            var configFile = GetPathForDataFile(serverUtility, pathToXmlDataFile);
            var updated = FromViewModel(esbEnvironment);
            IconMonitoringService.UpdateEsbEnvironment(updated, configFile);
        }

        public void DeleteEsbEnvironment(HttpServerUtilityBase serverUtility, EsbEnvironmentViewModel esbEnvironment, string pathToXmlDataFile)
        {
            var configFile = GetPathForDataFile(serverUtility, pathToXmlDataFile);
            var toDelete = FromViewModel(esbEnvironment);
            IconMonitoringService.DeleteEsbEnvironment(toDelete, configFile);
        }

        public List<Tuple<bool, string>> SetEsbEnvironment(HttpServerUtilityBase serverUtility, EsbEnvironmentViewModel esbEnvironment, string pathToXmlDataFile)
        {
            var configFile = GetPathForDataFile(serverUtility, pathToXmlDataFile);
            var results = IconMonitoringService.UpdateApplicationsToEsbEnvironment(FromViewModel(esbEnvironment), configFile);
            return results;
        }

        public EsbEnvironmentViewModel GetCurrentEsbEnvironment(HttpServerUtilityBase serverUtility, string pathToXmlDataFile)
        {
            var configFile = GetPathForDataFile(serverUtility, pathToXmlDataFile);
            var esbEnvironment = IconMonitoringService.GetCurrentEsbEnvironment(configFile);
            if (esbEnvironment != null)
            {
                return new EsbEnvironmentViewModel(esbEnvironment);
            }
            return (EsbEnvironmentViewModel)null;
        }

        public string GetCurrentEsbEnvironmentForApplication(HttpServerUtilityBase serverUtility,
            string pathToXmlDataFile, IApplication application)
        {
            const string keyForServerUrl = "ServerUrl";
            const string keyForTargetHostNamel = "TargetHostName";
            string applicationName = $"{application.Name}-{application.Server}";
            var possibleEsbEnvironments = GetEsbEnvironments(serverUtility, pathToXmlDataFile);
            string currentEsbEnvironment = null;

            //check for an explict EsbConfigurationSettings section
            if (application.EsbConnectionSettings != null && application.EsbConnectionSettings.Count > 0)
            {
                currentEsbEnvironment = ReadEsbEnvironmentFromConfiguration(
                    configSection: application.EsbConnectionSettings,
                    knownEsbEnvironments: possibleEsbEnvironments,
                    keyForServerName: keyForServerUrl,
                    keyForTargetHost: keyForTargetHostNamel,
                    nameOfApplication: applicationName);
            }

            return currentEsbEnvironment;
        }

        /// <summary>
        /// Checks a dictionary (such as from an app.settings file section) for a key/value pair which indicates that the 
        ///   application using the configration data is configured for communication with a known ESB environment
        /// </summary>
        /// <param name="configSection">A Dictionary of strings represeentng the key/value pair settings</param>
        /// <param name="knownEsbEnvironments">An enumerable collection of EsbEnvironment models, to use when searching for a match</param>
        /// <param name="keyForServerName">Key for the key/value pair in the configuration dictionary representing the target ESB server</param>
        /// <param name="keyForTargetHost">Key for the key/value pair in the configuration dictionary representing the target ESB Host Name</param>
        /// <param name="nameOfApplication">Name of the application associated with the config data, used when potentially building an error message</param>
        /// <returns></returns>
        private string ReadEsbEnvironmentFromConfiguration(Dictionary<string,string> configSection,
            IEnumerable<EsbEnvironmentViewModel> knownEsbEnvironments,
            string keyForServerName,
            string keyForTargetHost,
            string nameOfApplication)
        {
            if (configSection != null)
            {
                if (configSection.ContainsKey(keyForServerName) && configSection.ContainsKey(keyForTargetHost))
                {
                    string esbServer = String.Empty;
                    string esbTargetHost = String.Empty;
                    if (configSection.TryGetValue(keyForServerName, out esbServer) && configSection.TryGetValue(keyForTargetHost, out esbTargetHost))
                    {
                        var matchingEsbEnvironments = knownEsbEnvironments.Where(e =>
                            String.Compare(e.ServerUrl, esbServer, StringComparison.CurrentCultureIgnoreCase) == 0 &&
                            String.Compare(e.TargetHostName, esbTargetHost, StringComparison.CurrentCultureIgnoreCase) == 0);

                        if (matchingEsbEnvironments != null && matchingEsbEnvironments.Count() == 1)
                        {
                            return matchingEsbEnvironments.First().Name;
                        }
                        else if (matchingEsbEnvironments != null && matchingEsbEnvironments.Count() > 1)
                        {
                            throw new ArgumentOutOfRangeException($"{nameOfApplication} is configured for more than 1 esb environment");
                        }
                    }
                }
            }
            return null;
        }
    }
}