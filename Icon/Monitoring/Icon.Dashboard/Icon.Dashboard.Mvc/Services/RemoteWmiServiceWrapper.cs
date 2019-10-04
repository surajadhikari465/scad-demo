using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Helpers;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.ViewModels;
using Icon.Dashboard.RemoteServicesAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Dashboard.Mvc.Services
{
    public class RemoteWmiServiceWrapper : IRemoteWmiServiceWrapper
    {
        public IRemoteWmiAccessService WmiService { get;  set; }

        public RemoteWmiServiceWrapper(
            IRemoteWmiAccessService dataService = null)
        {
            this.WmiService = dataService ?? new RemoteWmiAccessService();
        }

        public ServiceViewModel LoadRemoteService(
            string server,
            string application,
            bool commandsEnabled,
            List<LoggedAppViewModel> iconLoggedApps,
            List<LoggedAppViewModel> mammothLoggedApps,
            List<EnvironmentModel> environments,
            List<EsbEnvironmentModel> esbEnvironments,
            IExternalConfigXmlManager serviceConfigDataReader = null)
        {
            var remoteServiceModel = WmiService.LoadRemoteService(server, application);

            return CreateServiceViewModel(
                server,
                remoteServiceModel,
                commandsEnabled,
                iconLoggedApps,
                mammothLoggedApps,
                environments,
                esbEnvironments,
                serviceConfigDataReader);
        }

        public List<ServiceViewModel> LoadRemoteServices(
            List<string> appServerNames,
            bool commandsEnabled,
            List<LoggedAppViewModel> iconLoggedApps,
            List<LoggedAppViewModel> mammothLoggedApps,
            List<EnvironmentModel> environments,
            List<EsbEnvironmentModel> esbEnvironments)
        {
            var appViewModels = new List<ServiceViewModel>();

            foreach (var serverName in appServerNames)
            {
                var remoteServiceModels = WmiService.LoadRemoteServices(serverName);

                appViewModels.AddRange(remoteServiceModels.Select(svc =>
                    CreateServiceViewModel(
                        serverName,
                        svc,
                        commandsEnabled,
                        iconLoggedApps,
                        mammothLoggedApps,
                        environments,
                        esbEnvironments,
                        null)));
            }

            return appViewModels;
        }

        public ServiceViewModel CreateServiceViewModel(
            string serverName,
            RemoteServiceModel remoteService,
            bool commandsEnabled,
            List<LoggedAppViewModel> iconLoggedApps,
            List<LoggedAppViewModel> mammothLoggedApps,
            List<EnvironmentModel> environments,
            List<EsbEnvironmentModel> esbEnvironments,
            IExternalConfigXmlManager serviceConfigDataReader = null)
        {
            var appViewModel = new ServiceViewModel()
            {
                Name = remoteService.FullName,
                DisplayName = remoteService.DisplayName,
                Server = serverName,
                Description = remoteService.Description,
                Family = remoteService.FullName.Contains("Mammoth") ? "Mammoth" : "Icon",
                CommandsEnabled = commandsEnabled,
                Status = remoteService.State,
                ValidCommands = SetValidCommands(remoteService.State),
                StatusIsGreen = IsStatusGreen(remoteService.State),
                LoggingID = 0,
                LoggingName = "",
                AppSettings = new Dictionary<string, string>(),
                EsbConnections = new List<EsbConnectionViewModel>(),
                HostName = remoteService.SystemName,
                AccountName = remoteService.RunningAs.Replace(@"wfm\", "").Replace(@"@wfm.pvt", "")
            };

            // when testing or using service on local machine, use config filename as is
            //var serviceConfigFilePath = Path.GetFileName(remoteService.ConfigFilePath);
            var serviceConfigFilePath = remoteService.ConfigFilePath;
            if (!IsRunningAsUnitTest())
            {
                // pre-pend the UNC path/share "\\SERVER\E$" to the local config path returned from WMI
                serviceConfigFilePath = PrependConfigUncPath(appViewModel.Server, remoteService.ConfigFilePath);
            }
            // open and read the service's config file
            if (serviceConfigDataReader == null)
            {
                serviceConfigDataReader = new ExternalConfigXmlManager(serviceConfigFilePath);
            }
            var serviceConfigData = serviceConfigDataReader.ReadConfigData(serviceConfigFilePath, environments, esbEnvironments);

            // set service model properties from config data
            appViewModel.ConfigFilePath = serviceConfigData.ConfigFilePath;
            appViewModel.ConfigFilePathIsValid = serviceConfigData.IsConfigFilePathValid;
            if (appViewModel.ConfigFilePathIsValid)
            {
                // populate app settings dictionary for service
                foreach (var kvp in serviceConfigData.NonEsbAppSettings)
                {
                    appViewModel.AppSettings.Add(kvp.Key, kvp.Value);
                }
                // determine the service's ESB settings (if any)
                appViewModel.EsbConnections = serviceConfigData.EsbConnections.ToList();
                appViewModel.HasEsbSettingsInCustomConfigSection = serviceConfigData.HasEsbSettingsInCustomConfigSection;
                appViewModel.EsbEnvironmentEnum = DetermineEsbEnvironment(esbEnvironments, serviceConfigData.EsbConnections);
                // populate logging id and name from config file and database App data 
                appViewModel.LoggingID = serviceConfigData.LoggingID;
                var appFamily = remoteService.FullName.Contains("Mammoth")
                    ? IconOrMammothEnum.Mammoth
                    : IconOrMammothEnum.Icon;
                appViewModel.LoggingName = GetLoggingNameFromId(
                    serviceConfigData.LoggingID.GetValueOrDefault(0),
                    appFamily,
                    iconLoggedApps,
                    mammothLoggedApps);
                // load database configuration
                appViewModel.DatabaseConfiguration = new AppDatabaseConfigurationViewModel(serviceConfigData.DatabaseConfiguration);
            }
            return appViewModel;
        }

        /// <summary>
        /// Converts PathName from a WMI ManagementObject into a UNC path to a configuration file
        /// </summary>
        /// <param name="serverName">The remote server name for the start of the UNC path</param>
        /// <param name="pathName">PathName from a WMI Management Object representing a remote service, for example
        ///     "E:\Icon\API Controller Phase 2\Hierarchy\Icon.ApiController.Controller.exe"  -displayname "Icon API Controller - Hierarchy" -servicename "IconAPIController-Hierarchy"</param>
        /// <returns>UNC path to the config file, for example
        ///     "\\vm-icon-test1\E$\Icon\API Controller Phase 2\Hierarchy\Icon.ApiController.Controller.exe.config" </returns>
        public string PrependConfigUncPath(string serverName, string pathName)
        {
            // remove quotation marks
            string configPathOnHost = pathName.Replace("\"", "");
            if (configPathOnHost.Contains("-"))
            {
                // remove any parameters (we only want the path to the executable)
                configPathOnHost = configPathOnHost.Substring(0, configPathOnHost.IndexOf('-'));
            }
            // replace local drive letter from remote server with an administrative share path ($ instead of :)
            configPathOnHost = configPathOnHost.Replace(":", "$");
            // trim and add .config suffix
            configPathOnHost = configPathOnHost.Trim();
            configPathOnHost += ".config";

            // build the unc path using the server and converted path
            configPathOnHost = $"\\\\{serverName}\\{configPathOnHost}";

            return configPathOnHost;
        }

        public EsbEnvironmentEnum DetermineEsbEnvironment(
            List<EsbEnvironmentModel> possibleEsbEnvironments,
            List<EsbConnectionViewModel> serviceEsbSettings)
        {
            if (possibleEsbEnvironments != null 
                && possibleEsbEnvironments.Count > 0
                && serviceEsbSettings != null
                && serviceEsbSettings.Count> 0)
            {
                // assume for now that in the case of multiple ESB connections, they are all set for the same environment
                var settingForEsbServer = serviceEsbSettings[0].ServerUrl;
                return ExternalConfigXmlManager.DetermineEsbEnvironmentFromServerUrlSetting(
                    possibleEsbEnvironments, settingForEsbServer);
            }

            return EsbEnvironmentEnum.None;
        }

        public bool IsRunningAsUnitTest()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Any(a => a.FullName.StartsWith("Microsoft.VisualStudio.QualityTools.UnitTestFramework"));
        }

        public bool IsStatusGreen(string state)
        {
            switch (state)
            {
                case "Running":
                    return true;
                case "Stopped":
                case "StartPending":
                case "StopPending":
                case "ContinuePending":
                case "PausePending":
                case "Paused":
                default:
                    break;
            }
            return false;
        }

        public List<string> SetValidCommands(string state)
        {
            var validCommands = new List<string>();
            switch (state)
            {
                case "Running":
                    validCommands.Add("Stop");
                    break;
                case "Stopped":
                    validCommands.Add("Start");
                    break;
                case "StartPending":
                case "StopPending":
                case "ContinuePending":
                case "PausePending":
                case "Paused":
                    validCommands.Add("Start");
                    validCommands.Add("Stop");
                    break;
                case "Undefined":
                default:
                    break;
            }
            return validCommands;
        }

        public string GetLoggingNameFromId(IEnumerable<LoggedAppViewModel> apps, int loggingId)
        {
            var loggingName = string.Empty;
            var matchingApp = apps.FirstOrDefault(a => a.AppID == loggingId);
            if (matchingApp != null)
            {
                loggingName = matchingApp.AppName;
            }
            return loggingName;
        }

        public string GetLoggingNameFromId(int appLoggingId,
            IconOrMammothEnum appFamily,
            IEnumerable<LoggedAppViewModel> iconLoggedApps,
            IEnumerable<LoggedAppViewModel> mammothLoggedApps)
        {
            LoggedAppViewModel matchingApp = null;
            if (appLoggingId > 0)
            {
                switch (appFamily)
                {
                    case IconOrMammothEnum.Icon:
                        matchingApp = iconLoggedApps.FirstOrDefault(a => a.AppID == appLoggingId);
                        break;
                    case IconOrMammothEnum.Mammoth:
                        matchingApp = mammothLoggedApps.FirstOrDefault(a => a.AppID == appLoggingId);
                        break;
                    default:
                        break;
                }
            }
            return matchingApp?.AppName ?? string.Empty;
        }

        public void ExecuteServiceCommand(string server, string application, string command)
        {
            switch (command)
            {
                case "Start":
                case "start":
                    WmiService.StartRemoteService(server, application, new string[] { });
                    break;
                case "Stop":
                case "stop":
                    WmiService.StopRemoteService(server, application, new string[] { });
                    break;
                default:
                    throw new ArgumentException($"Unexpected command '{command}' for ExecuteServiceCommand");
            }
        }

        public void RestartServices(IEnumerable<EsbEnvironmentViewModel> esbEnvironments)
        {
            foreach (var esbEnvironment in esbEnvironments)
            {
                RestartServices(esbEnvironment.AppsInEnvironment);
            }
        }

        public void RestartServices(IEnumerable<ServiceViewModel> applications)
        {
            foreach (var appToRestart in applications)
            {
                WmiService.StopRemoteService(appToRestart.Server, appToRestart.Name, new string[] { });
                //TODO need pause/threading here?
                WmiService.StartRemoteService(appToRestart.Server, appToRestart.Name, new string[] { });
            }
        }

        /// <summary>
        /// Re-writes external config files for services so that their ESB settings match
        ///  the provided ESB environment/service groupings
        /// </summary>
        /// <param name="esbEnvironmentsWithServices">View Model from Esb re-configuration action
        ///   containing ESB environment definitions with lists of remote services: the list of
        ///   services for each esb environment will have their config files updated in line with 
        ///   the ESB environment they are grouped under</param>
        /// <param name="services">list of services</param>
        public void ReconfigureServiceEsbEnvironmentSettings(
            List<EsbEnvironmentViewModel> esbEnvironmentsWithServices,
            List<ServiceViewModel> services,
            IExternalConfigXmlManager serviceConfigDataWriter = null)
        {
            if (esbEnvironmentsWithServices == null || services == null)
            {
                return;
            }
            if (serviceConfigDataWriter == null)
            {
                serviceConfigDataWriter = new ExternalConfigXmlManager();
            }
            foreach (var esbEnvironment in esbEnvironmentsWithServices)
            {
                foreach (var serviceMiniModelFromEsbModel in esbEnvironment.AppsInEnvironment)
                {
                    var serviceToUpdate = services
                        .FirstOrDefault(svc =>
                            svc.Server.Equals(serviceMiniModelFromEsbModel.Server, Utils.StrcmpOption)
                            && svc.Name.Equals(serviceMiniModelFromEsbModel.Name, Utils.StrcmpOption)
                            && svc.HasEsbConnections
                            && svc.EsbEnvironmentEnum != EsbEnvironmentEnum.None
                            && svc.EsbConnections.Count > 0
                            && svc.ConfigFilePathIsValid);
                    if (serviceToUpdate != null)
                    {
                        foreach (var esbConnection in serviceToUpdate.EsbConnections)
                        {
                            var esbConnectionType = EsbConnectionViewModel.DetermineEsbConnectionTypeByJmsUsername(esbConnection.JmsUsername);
                            var updatedEsbSettingsDictionary = BuildEsbAppSettingsForUpdate(esbEnvironment, esbConnectionType);

                            // which type of esb settings are used (app config key/value pairs or esbEnvironments sectio?)
                            if (serviceToUpdate.HasEsbSettingsInCustomConfigSection)
                            {
                                serviceConfigDataWriter.ReconfigureEsbEnvironmentCustomConfigSection(
                                    serviceToUpdate.ConfigFilePath,
                                    esbConnection.ConnectionName,
                                    updatedEsbSettingsDictionary);
                            }
                            else
                            {
                                serviceConfigDataWriter.UpdateExternalAppSettings(
                                    serviceToUpdate.ConfigFilePath,
                                    updatedEsbSettingsDictionary);
                            }
                        } 
                        //update the application model with the ESB environment
                        serviceToUpdate.EsbEnvironmentEnum = esbEnvironment.EsbEnvironment;
                    }
                }
            }
        }

        public void UpdateRemoteServiceConfig(ServiceViewModel serviceViewModel,
            IExternalConfigXmlManager serviceConfigDataWriter = null)
        {
            if (serviceViewModel != null && serviceViewModel.ConfigFilePathIsValid)
            {
                if (serviceConfigDataWriter == null)
                {
                    serviceConfigDataWriter = new ExternalConfigXmlManager(serviceViewModel.ConfigFilePath);
                }
                try
                {
                    var configFilePath = serviceViewModel.ConfigFilePath;
                    var appSettings = serviceViewModel.AppSettings;

                    // does the service have ESB connection settings in its app settings?
                    if (serviceViewModel.HasEsbConnections &&
                        !serviceViewModel.HasEsbSettingsInCustomConfigSection)
                    {
                        // combine the AppSettings and the ESB-related subset of AppSettings into 1 collection
                        appSettings = Utils.CombineDictionariesIgnoreDuplicates(
                            serviceViewModel.AppSettings,
                            serviceViewModel.EsbConnections[0].SettingsAsDictionary());
                    }
                    serviceConfigDataWriter.UpdateExternalAppSettings(configFilePath, appSettings);
                }
                catch
                {

                    //for now just eat the exception...
                }
            }
        }

        internal Dictionary<string, string> BuildEsbAppSettingsForUpdate(
            EsbEnvironmentViewModel esbEnvironment,
            EsbConnectionTypeEnum esbConnectionType)
        {
            var updatedSettings = new Dictionary<string, string>();

            if (esbEnvironment != null
                && esbConnectionType != EsbConnectionTypeEnum.None)
            {
                updatedSettings.Add(Constants.EsbSettingKeys.ServerUrlKey, esbEnvironment.ServerUrl);
                updatedSettings.Add(Constants.EsbSettingKeys.TargetHostNameKey, esbEnvironment.TargetHostName);
                updatedSettings.Add(Constants.EsbSettingKeys.CertificateNameKey, esbEnvironment.CertificateName);
                //  settings that are the same across environments - no need to update:
                // CertificateStoreName CertificateStoreLocation SslPassword SessionMode /JmsUsernameIcon /JndiUsernameIcon

                switch (esbConnectionType)
                {
                    case EsbConnectionTypeEnum.Icon:
                        updatedSettings.Add(Constants.EsbSettingKeys.JmsPasswordKey, esbEnvironment.JmsPasswordIcon);
                        updatedSettings.Add(Constants.EsbSettingKeys.JndiPasswordKey, esbEnvironment.JndiPasswordIcon);
                        break;
                    case EsbConnectionTypeEnum.Mammoth:
                        updatedSettings.Add(Constants.EsbSettingKeys.JmsPasswordKey, esbEnvironment.JmsPasswordMammoth);
                        updatedSettings.Add(Constants.EsbSettingKeys.JndiPasswordKey, esbEnvironment.JndiPasswordMammoth);
                        break;
                    case EsbConnectionTypeEnum.Ewic:
                        updatedSettings.Add(Constants.EsbSettingKeys.JmsPasswordKey, esbEnvironment.JmsPasswordEwic);
                        updatedSettings.Add(Constants.EsbSettingKeys.JndiPasswordKey, esbEnvironment.JndiPasswordEwic);
                        break;
                    case EsbConnectionTypeEnum.None:
                    default:
                        break;
                }
            }
            return updatedSettings;
        }
    }
}