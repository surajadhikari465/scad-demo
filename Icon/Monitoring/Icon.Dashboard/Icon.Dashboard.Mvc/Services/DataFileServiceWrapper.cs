using Icon.Dashboard.DataFileAccess.Models;
using Icon.Dashboard.DataFileAccess.Services;
using Icon.Dashboard.Mvc.Helpers;
using Icon.Dashboard.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Dashboard.Mvc.Services
{
    public class DataFileServiceWrapper : IDataFileServiceWrapper
    {
        public IIconDashboardDataService IconMonitoringService { get; private set; }

        public IIconDatabaseServiceWrapper LoggingService { get; private set; }

        public DataFileServiceWrapper(IIconDashboardDataService dataService = null,
            IIconDatabaseServiceWrapper loggingServiceWrapper = null)
        {
            IconMonitoringService = dataService ?? IconDashboardDataService.Instance;
            LoggingService = loggingServiceWrapper ?? new IconDatabaseServiceWrapper();
        }

        public IEnumerable<IconApplicationViewModel> GetApplications(string pathToXmlDataFile)
        {
            var apps = IconMonitoringService.GetApplications(pathToXmlDataFile);
            var allEsbEnvironments = GetEsbEnvironments(pathToXmlDataFile);

            var viewModels = new List<IconApplicationViewModel>(apps.Count());

            foreach (var app in apps)
            {
                viewModels.Add(FindEsbEnvironmentForApp(app, allEsbEnvironments));
            }
            return viewModels;
        }

        public IconApplicationViewModel GetApplication(string pathToXmlDataFile,
            string application, string server)
        {
            return GetApplication(pathToXmlDataFile, application, server, null);
        }

        public IconApplicationViewModel GetApplication(string pathToXmlDataFile,
            string application, string server, IEnumerable<EsbEnvironmentViewModel> esbEnvironments = null)
        {
            var app = IconMonitoringService.GetApplication(pathToXmlDataFile, application, server);
            var allEsbEnvironments = esbEnvironments ?? GetEsbEnvironments(pathToXmlDataFile);
            var viewModel = FindEsbEnvironmentForApp(app, allEsbEnvironments);
            return viewModel;
        }

        public void ExecuteServiceCommand(string pathToXmlDataFile,
            string application, string server, string command)
        {
            var app = IconMonitoringService.GetApplication(pathToXmlDataFile, application, server);
            if (app == null) throw new ArgumentNullException(
                String.Format("Unable to find process to monitor ({server}-{application})", server, application));
            if (app.TypeOfApplication == ApplicationTypeEnum.ScheduledTask) command = "Start";

            switch (command)
            {
                case "Start":
                case "start":
                    string[] startArgs = null;
                    IconMonitoringService.StartService(app, TimeSpan.FromMilliseconds(Utils.ServiceCommandTimeout), startArgs);
                    break;
                case "Stop":
                case "stop":
                default:
                    IconMonitoringService.StopService(app, TimeSpan.FromMilliseconds(Utils.ServiceCommandTimeout));
                    break;
            }
        }

        public void UpdateApplication(string pathToXmlDataFile, IconApplicationViewModel appViewModel)
        {
            var updatedApplication = appViewModel.ToDataModel();
            IconMonitoringService.UpdateApplication(updatedApplication, pathToXmlDataFile);
        }

        public void AddApplication(string pathToXmlDataFile, IconApplicationViewModel appViewModel)
        {
            var newApplication = appViewModel.ToDataModel();
            IconMonitoringService.AddApplication(newApplication, pathToXmlDataFile);
        }

        public void DeleteApplication(string pathToXmlDataFile, IconApplicationViewModel appViewModel)
        {
            var viewModelToDelete = appViewModel.ToDataModel();
            IconMonitoringService.DeleteApplication(viewModelToDelete, pathToXmlDataFile);
        }

        public void SaveAppSettings(IconApplicationViewModel appViewModel)
        {
            IconMonitoringService.SaveAppSettings(appViewModel.ToDataModel());
        }

        public IEnumerable<EsbEnvironmentViewModel> GetEsbEnvironments(string pathToXmlDataFile, bool includeUnassignedApps = false)
        {
            var environmentViewModels = new List<EsbEnvironmentViewModel>();            
            var esbEnvironments = IconMonitoringService.GetEsbEnvironments(pathToXmlDataFile);

            if (esbEnvironments != null && esbEnvironments.Any())
            {
                var allApplications = IconMonitoringService.GetApplications(pathToXmlDataFile);

                foreach (var esbEnvironment in esbEnvironments)
                {
                    var esbEnvironmentViewModel = AssignAppsForEsbEnvironment(esbEnvironment, allApplications);
                    environmentViewModels.Add(esbEnvironmentViewModel);
                }
                if (includeUnassignedApps)
                {
                    var unassignedEsbApps = allApplications.Where(a => a.HasEsbConfiguration
                        && a.EsbConnectionSettings != null
                        && a.EsbConnectionSettings.ContainsKey("Name")
                        && !esbEnvironments.Select(e => e.Name).Contains(a.EsbConnectionSettings["Name"]));
                    var unassignedEnvironment = new EsbEnvironmentViewModel()
                    {
                        Name = "UNASSIGNED",
                        ServerUrl = "N//A",
                        TargetHostName = "N//A",
                        //JmsUsername = JmsUsername,
                        //JmsPassword = JmsPassword,
                        //JndiUsername = JndiUsername,
                        //JndiPassword = JndiPassword,
                        //ConnectionFactoryName = ConnectionFactoryName,
                        //SslPassword = SslPassword,
                        //QueueName = QueueName,
                        //SessionMode = SessionMode,
                        //CertificateName = CertificateName,
                        //CertificateStoreName = CertificateStoreName,
                        //CertificateStoreLocation = CertificateStoreLocation,
                        //ReconnectDelay = ReconnectDelay,
                        //NumberOfListenerThreads = NumberOfListenerThreads,
                        //AppsInEnvironment = unassignedEsbApps
                        //    .Select(u => new IconAppSummaryViewModel(u)).OrderBy(a=>a.Name)
                        //    .ToList()
                    };
                    unassignedEnvironment.AppsInEnvironment = unassignedEsbApps?
                        .Select(u => new IconApplicationViewModel(u)).OrderBy(a => a.Name).ToList() 
                        ?? new List<IconApplicationViewModel>();
                    environmentViewModels.Add(unassignedEnvironment);
                }
            }

            return environmentViewModels.OrderBy(e=>e.Name);
        }

        public EsbEnvironmentViewModel AssignAppsForEsbEnvironment(
            IEsbEnvironmentDefinition esbEnvironment,
            IEnumerable<IconApplicationViewModel> allApplicationViewModels)
        {
            var serverUrlKey = nameof(EsbEnvironmentViewModel.ServerUrl);

            var esbEnvironmentViewModel = new EsbEnvironmentViewModel(esbEnvironment);
            var hostsForEnvironment = GetHostsFromServerUrl(esbEnvironmentViewModel.ServerUrl);
            if (hostsForEnvironment != null)
            {
                foreach (var app in allApplicationViewModels)
                {
                    if (app.EsbConnectionSettings.Any() && app.EsbConnectionSettings.ContainsKey(serverUrlKey))
                    {
                        var hostsForApp = GetHostsFromServerUrl(app.EsbConnectionSettings[serverUrlKey]);
                        bool match = false;
                        foreach (var appHost in hostsForApp)
                        {
                            match = hostsForEnvironment.Contains(appHost, StringComparer.InvariantCultureIgnoreCase);
                            if (!match) break;
                        }
                        if (match)
                        {
                            esbEnvironmentViewModel.AppsInEnvironment.Add(app);
                        }
                    }
                }
                esbEnvironmentViewModel.AppsInEnvironment = esbEnvironmentViewModel.AppsInEnvironment
                    .OrderBy(a => a.Server).ThenBy(a => a.Name)
                    .ToList();
            }
            return esbEnvironmentViewModel;
        }

        public static IEnumerable<string> GetHostsFromServerUrl(string serverUrlSettingValue)
        {
            if (String.IsNullOrWhiteSpace(serverUrlSettingValue)) return null;
            var hosts = new List<string>();
            // server url app setting could contain a single url or two urls separated by a comma
            var serverUrls = serverUrlSettingValue.Split(',');
            foreach (var serverUrl in serverUrls)
            {
                var systemUri = new Uri(serverUrl);
                if (systemUri.Host.Contains("."))
                {
                    //we only want the first element of the domain host (e.g. "myMachine" out of "myMachine.wfm.pvt")
                    hosts.Add(systemUri.Host.Split('.')[0]);
                }
                else
                {
                    hosts.Add(systemUri.Host);
                }
            }
            return hosts;
        }

        public EsbEnvironmentViewModel AssignAppsForEsbEnvironment(
            IEsbEnvironmentDefinition esbEnvironment,
            IEnumerable<IIconApplication> allIconApplications)
        {
            var allAppViewModels = allIconApplications.Select(a => new IconApplicationViewModel(a));
            return AssignAppsForEsbEnvironment(esbEnvironment, allAppViewModels);
        }

        public IconApplicationViewModel FindEsbEnvironmentForApp(
            IIconApplication iconApplication,
            IEnumerable<EsbEnvironmentViewModel> allEsbEnvironments)
        {
            var serverUrlKey = nameof(EsbEnvironmentViewModel.ServerUrl);
            if (iconApplication == null) throw new ArgumentNullException(nameof(iconApplication));
            if (allEsbEnvironments == null) throw new ArgumentNullException(nameof(allEsbEnvironments));

            var appViewModel = new IconApplicationViewModel(iconApplication);
            if (appViewModel.EsbConnectionSettings.Any() && appViewModel.EsbConnectionSettings.ContainsKey(serverUrlKey))
            {
                var hostsForApp = GetHostsFromServerUrl(appViewModel.EsbConnectionSettings[serverUrlKey]);
                if (hostsForApp != null)
                {
                    foreach (var env in allEsbEnvironments)
                    {
                        var hostsForEnvironment = GetHostsFromServerUrl(env.ServerUrl);
                        if (hostsForEnvironment != null)
                        {
                            bool match = false;
                            foreach( var appHost in hostsForApp)
                            {
                                match = hostsForEnvironment.Contains(appHost);
                                if (!match) break;
                            }
                            if (match) appViewModel.CurrentEsbEnvironment = env.Name;
                        }
                    }
                }
            }
            return appViewModel;
        }

        public Dictionary<string,string> ReconfigureEsbApps(string pathToXmlDataFile,
            IEnumerable<EsbEnvironmentViewModel> viewModelWithChanges)
        {
            var totalResults = new Dictionary<string, string>();
            foreach (var env in viewModelWithChanges)
            {
                var appsInEnv = env.AppsInEnvironment.Select(a => a.ToDataModel());
                var eachResults = IconMonitoringService.ReconfigureEsbSettingsAndRestartServices(
                    pathToXmlDataFile, appsInEnv, env.Name, Utils.ServiceCommandTimeout);
                //combine results
                eachResults.ToList().ForEach(x => totalResults.Add(x.Key, x.Value));
            }

            return totalResults;
        }

        public EsbEnvironmentViewModel GetEsbEnvironment(string pathToXmlDataFile, string name)
        {
            var environment = IconMonitoringService.GetEsbEnvironment(pathToXmlDataFile, name);
            var allApps = GetApplications(pathToXmlDataFile);
            var viewModel = AssignAppsForEsbEnvironment(environment, allApps);
            return viewModel;
        }

        public void AddEsbEnvironment(string pathToXmlDataFile, EsbEnvironmentViewModel esbEnvironment)
        {
            var newEnvironment = esbEnvironment.ToDataModel();
            IconMonitoringService.AddEsbEnvironment(newEnvironment, pathToXmlDataFile);
        }

        public void UpdateEsbEnvironment(string pathToXmlDataFile,
            EsbEnvironmentViewModel esbEnvironment)
        {
            var updated = esbEnvironment.ToDataModel();
            IconMonitoringService.UpdateEsbEnvironment(updated, pathToXmlDataFile);
        }

        public void DeleteEsbEnvironment(string pathToXmlDataFile,
            EsbEnvironmentViewModel esbEnvironment)
        {
            var toDelete = esbEnvironment.ToDataModel();
            IconMonitoringService.DeleteEsbEnvironment(toDelete, pathToXmlDataFile);
        }
    }
}