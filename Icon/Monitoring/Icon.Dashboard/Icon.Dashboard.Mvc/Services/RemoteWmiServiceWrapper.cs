using Icon.Dashboard.DataFileAccess.Models;
using Icon.Dashboard.Mvc.ViewModels;
using Icon.Dashboard.RemoteServicesAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Xml.Linq;

namespace Icon.Dashboard.Mvc.Services
{
    public class RemoteWmiServiceWrapper : IRemoteWmiServiceWrapper
    {
        public IRemoteWmiAccessService WmiService { get; private set; }

        public IEnumerable<IconLoggedAppViewModel> IconApps { get; set; }
        public IEnumerable<IconLoggedAppViewModel> MammothApps { get; set; }
        public IEnumerable<EsbEnvironmentViewModel> EsbEnvironments { get; set; }

        public RemoteWmiServiceWrapper(IRemoteWmiAccessService dataService = null,
            IEnumerable<IconLoggedAppViewModel> iconApps = null,
            IEnumerable<IconLoggedAppViewModel> mammothApps = null,
            IEnumerable<EsbEnvironmentViewModel> esbEnvironments = null)
        {
            WmiService = dataService ?? new RemoteWmiAccessService();
            IconApps = iconApps ?? new List<IconLoggedAppViewModel>();
            MammothApps = mammothApps ?? new List<IconLoggedAppViewModel>();
            EsbEnvironments = esbEnvironments ?? new List<EsbEnvironmentViewModel>();
        }

        public IconApplicationViewModel LoadRemoteService(string server, string application, bool commandsEnabled)
        {
            return LoadRemoteService(server, application, commandsEnabled, IconApps, MammothApps, EsbEnvironments);
        }

        public IconApplicationViewModel LoadRemoteService(string server, string application, bool commandsEnabled,
            IEnumerable<IconLoggedAppViewModel> iconAppList,
            IEnumerable<IconLoggedAppViewModel> mammothAppList,
            IEnumerable<EsbEnvironmentViewModel> allEsbEnvironments)
        {
            var remoteServiceModel = WmiService.LoadRemoteService(server, application);

            return CreateViewModel(server, remoteServiceModel, commandsEnabled, iconAppList, mammothAppList, allEsbEnvironments);
        }

        public IconApplicationViewModel CreateViewModel(string serverName,
           RemoteServiceModel remoteService,
           bool commandsEnabled)
        {
            return CreateViewModel(serverName, remoteService, commandsEnabled, this.IconApps, this.MammothApps, this.EsbEnvironments);
        }

        public IconApplicationViewModel CreateViewModel(string serverName, 
            RemoteServiceModel remoteService,
            bool commandsEnabled,
            IEnumerable<IconLoggedAppViewModel> iconAppList,
            IEnumerable<IconLoggedAppViewModel> mammothAppList,
            IEnumerable<EsbEnvironmentViewModel> allEsbEnvironments)
        {
            var appViewModel = new IconApplicationViewModel()
            {
                Name = remoteService.FullName,
                DisplayName = remoteService.DisplayName,
                Server = serverName,
                Family = remoteService.FullName.Contains("Mammoth") ? "Mammoth" : "Icon",
                ConfigFilePath = remoteService.ConfigFilePath,
                CommandsEnabled = commandsEnabled,
                Status = remoteService.State,
                ValidCommands = SetValidCommands(remoteService.State),
                StatusIsGreen = IsStatusGreen(remoteService.State),
                LoggingID = 0,
                LoggingName = "",
                AppSettings = new Dictionary<string, string>(),
                EsbConnectionSettings = new Dictionary<string, string>(),
                HostName = remoteService.SystemName,
                AccountName = remoteService.RunningAs.Replace(@"wfm\", "").Replace(@"@wfm.pvt", "")
            };

            try
            {
                if (IsRunningAsUnitTest())
                {
                    // testing or loading service from local machine, just use filename without path
                    appViewModel.ConfigFilePath = Path.GetFileName(appViewModel.ConfigFilePath);
                }
                else
                {
                    string configPathOnHost = string.Empty;
                    if (appViewModel.ConfigFilePath.Contains("-"))
                    {
                        configPathOnHost = appViewModel.ConfigFilePath.Substring(0, appViewModel.ConfigFilePath.IndexOf('-'));
                    }
                    configPathOnHost = appViewModel.ConfigFilePath.Replace(":", "$").Replace("\"", "").Trim();

                    // use the unc path
                    appViewModel.ConfigFilePath = $"\\\\{appViewModel.Server}\\{configPathOnHost}";
                    //var config = ConfigurationManager.OpenExeConfiguration(uncPath);
                    //appViewModel.ConfigFilePath = config.FilePath;
                }

                if (System.IO.File.Exists(appViewModel.ConfigFilePath))
                {
                    appViewModel.ConfigFilePathIsValid = true;
                    // load the application's config file
                    var appConfig = XDocument.Load(appViewModel.ConfigFilePath);

                    //populate app and esb settings from config file
                    var allAppSettings = appConfig.Root.Element("appSettings").Elements()
                        .Select(e => new
                        {
                            Key = e.Attribute("key").Value,
                            Value = e.Attribute("value").Value
                        });

                    var nonEsbSettings = allAppSettings
                        .Where(s => !EsbEnvironmentDefinition.EsbAppSettingsNames.Contains(s.Key))
                        .ToList();
                    nonEsbSettings.ForEach(e => appViewModel.AppSettings.Add(e.Key, e.Value));

                    var esbEnvironmentSettings = allAppSettings
                        .Where(s => EsbEnvironmentDefinition.EsbAppSettingsNames.Contains(s.Key))
                        .ToList();
                    esbEnvironmentSettings.ForEach(e => appViewModel.EsbConnectionSettings.Add(e.Key, e.Value));

                    appViewModel.CurrentEsbEnvironment = FindEsbEnvironmentForApp(allEsbEnvironments, appViewModel.EsbConnectionSettings);

                    // populate logging id and name from config file and database App data 
                    if (appViewModel.Family.Contains("Mammoth"))
                    {
                        appViewModel.LoggingID = GetLoggingIdFromConfig(appConfig);
                        if (appViewModel.LoggingID.GetValueOrDefault(0) > 0)
                        {
                            appViewModel.LoggingName = GetLoggingNameFromId(mammothAppList, appViewModel.LoggingID.Value);
                        }
                    }
                    else
                    {
                        appViewModel.LoggingID = GetLoggingIdFromConfig(appConfig);
                        if (appViewModel.LoggingID.GetValueOrDefault(0) > 0)
                        {
                            appViewModel.LoggingName = GetLoggingNameFromId(iconAppList, appViewModel.LoggingID.Value);
                        }
                    }
                }
                else
                {
                    appViewModel.ConfigFilePathIsValid = false;
                }
            }
            catch (Exception ex)
            {
                // eat error for now
                string errMsg = ex.Message;
            }
            return appViewModel;
        }

        public bool IsRunningAsUnitTest()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Any(a => a.FullName.StartsWith("Microsoft.VisualStudio.QualityTools.UnitTestFramework"));
        }

        public List<IconApplicationViewModel> LoadRemoteServices(DashboardEnvironmentViewModel customEnvironment, bool commandsEnabled)
        {
            return LoadRemoteServices(customEnvironment, commandsEnabled, this.IconApps, this.MammothApps, this.EsbEnvironments);
        }

        public List<IconApplicationViewModel> LoadRemoteServices(DashboardEnvironmentViewModel customEnvironment,
            bool commandsEnabled,
            IEnumerable<IconLoggedAppViewModel> iconAppList,
            IEnumerable<IconLoggedAppViewModel> mammothAppList,
            IEnumerable<EsbEnvironmentViewModel> allEsbEnvironments)
        {
            var appViewModels = new List<IconApplicationViewModel>();

            foreach (var serverName in customEnvironment.AppServers.Select(s => s.ServerName))
            {
                var remoteServiceModels = WmiService.LoadRemoteServices(serverName);

                appViewModels.AddRange(remoteServiceModels.Select(s =>
                    CreateViewModel(serverName, s, commandsEnabled, iconAppList, mammothAppList, allEsbEnvironments)));
            }

            return appViewModels;
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

        public string GetLoggingNameFromId(IEnumerable<IconLoggedAppViewModel> apps, int loggingId)
        {
            var loggingName = string.Empty;
            var matchingApp = apps.FirstOrDefault(a => a.AppID == loggingId);
            if (matchingApp != null)
            {
                loggingName = matchingApp.AppName;
            }
            return loggingName;
        }

        public int GetLoggingIdFromConfig(XDocument appConfig)
        {
            int loggingId = 0;

            try
            {
                var nlogElement = appConfig.Root.Elements()
                        .FirstOrDefault(rootEl => rootEl.Name.ToString().Contains("nlog"));
                if (nlogElement != null)
                {
                    var appIdParameter = nlogElement.Descendants()
                        .FirstOrDefault(d => d.Name.ToString().Contains("parameter") && d.Attribute("name").Value == "@AppId");
                    if (appIdParameter != null)
                    {
                        int.TryParse(appIdParameter.Attribute("layout").Value, out loggingId);
                    }
                }
            }
            catch (Exception)
            {
                loggingId = -1;
            }
            return loggingId;
        }
        
        public string FindEsbEnvironmentForApp(IEnumerable<EsbEnvironmentViewModel> allEsbEnvironments,
            Dictionary<string,string> appSettingsForEsb)
        {
            var serverUrlKey = nameof(EsbEnvironmentViewModel.ServerUrl);
            if (allEsbEnvironments == null) throw new ArgumentNullException(nameof(allEsbEnvironments));

            if (appSettingsForEsb.Any() && appSettingsForEsb.ContainsKey(serverUrlKey))
            {
                var hostsForApp = GetHostsFromServerUrl(appSettingsForEsb[serverUrlKey]);
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
                            if (match) return env.Name;
                        }
                    }
                }
            }
            return string.Empty;
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
    }
}