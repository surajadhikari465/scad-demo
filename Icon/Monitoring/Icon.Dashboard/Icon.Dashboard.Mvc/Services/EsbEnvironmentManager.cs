using Icon.Dashboard.Mvc.Helpers;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Xml.Linq;

namespace Icon.Dashboard.Mvc.Services
{
    public class EsbEnvironmentManager : IEsbEnvironmentManager
    {
        public EsbEnvironmentViewModel GetEsbEnvironment(string name)
        {
            return GetEsbEnvironmentsFromWebConfig()
                .FirstOrDefault(e => e.Name.Equals(name, Utils.StrcmpOption));
        }

        public IEnumerable<EsbEnvironmentViewModel> GetEsbEnvironmentDefinitions()
        {
            return GetEsbEnvironmentsFromWebConfig();
        }

        public IEnumerable<EsbEnvironmentViewModel> GetEsbEnvironmentDefinitionsWithAppsPopulated(IEnumerable<IconApplicationViewModel> dashboardApps)
        {
            return GetEsbEnvironmentsFromWebConfig(dashboardApps);
        }

        internal IEnumerable<EsbEnvironmentViewModel> GetEsbEnvironmentsFromWebConfig(IEnumerable<IconApplicationViewModel> services = null)
        {
            var esbEnvironmentViewModels = new List<EsbEnvironmentViewModel>();

            // read the custom section from the config file
            Configuration webConfigFile = null;
            if (HttpContext.Current == null)
            {
                System.Configuration.ExeConfigurationFileMap map = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = $"{System.AppDomain.CurrentDomain.BaseDirectory}Web.Config"
                };
                webConfigFile = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            }
            else
            {
                webConfigFile = WebConfigurationManager.OpenWebConfiguration("~");
            }
            var esbConfigSectionGroup = webConfigFile.SectionGroups["DashboardCustomConfigSection"] as DashboardCustomConfigSectionGroup;

            foreach (var esbConfigSection in esbConfigSectionGroup.Sections)
            {
                if (esbConfigSection.GetType() == typeof(EsbEnvironmentsSection))
                {
                    var esbEnvironmentsSection = (EsbEnvironmentsSection)esbConfigSection;
                    var esbEnvironmentsCollection = esbEnvironmentsSection.EsbEnvironments;
                    foreach(EsbEnvironmentElement esbEnvironmentConfigElement in esbEnvironmentsCollection)
                    {
                        var esbEnvironmentViewModel = new EsbEnvironmentViewModel(esbEnvironmentConfigElement);
                        // were we provided a list of dashboard service models to populate the apps using the esb configuration?
                        if (services != null)
                        {
                            esbEnvironmentViewModel.AppsInEnvironment = services
                                .Where(s =>
                                    s.HasEsbConfiguration &&
                                    s.EsbConnectionSettings.ContainsKey("ServerUrl") &&
                                    esbEnvironmentViewModel.ServerUrl.Equals(s.EsbConnectionSettings["ServerUrl"], Utils.StrcmpOption))
                                .ToList();
                        }
                        esbEnvironmentViewModels.Add(esbEnvironmentViewModel);
                    }
                }
            }

            return esbEnvironmentViewModels.OrderBy(e => e.Name);
        }

        public string FindEsbEnvironmentBasedOnAppSettings(IEnumerable<EsbEnvironmentViewModel> possibleEsbEnvironments, Dictionary<string, string> appSettings)
        {
            var serverUrlKey = nameof(EsbEnvironmentViewModel.ServerUrl);
            if (possibleEsbEnvironments == null) throw new ArgumentNullException(nameof(possibleEsbEnvironments));

            if (appSettings.Any() && appSettings.ContainsKey(serverUrlKey))
            {
                var hostsForApp = GetHostsFromServerUrl(appSettings[serverUrlKey]);
                if (hostsForApp != null)
                {
                    foreach (var env in possibleEsbEnvironments)
                    {
                        var hostsForEnvironment = GetHostsFromServerUrl(env.ServerUrl);
                        if (hostsForEnvironment != null)
                        {
                            bool match = false;
                            foreach (var appHost in hostsForApp)
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

        public static IEnumerable<string> GetHostsFromServerUrl(string commaSeparatedServers)
        {
            if (String.IsNullOrWhiteSpace(commaSeparatedServers)) return null;
            var hosts = new List<string>();
            // server url app setting could contain a single url or two urls separated by a comma
            var serverUrls = commaSeparatedServers.Split(',');
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

        public void ReconfigureEsbApps(IEnumerable<EsbEnvironmentViewModel> viewModelWithChanges, IEnumerable<IconApplicationViewModel> applications)
        {
            foreach (var esbEnvironment in viewModelWithChanges)
            {
                foreach (var appName in esbEnvironment.AppsInEnvironment)
                {
                    var app = applications.FirstOrDefault(a => a.Server.Equals(appName.Server, Utils.StrcmpOption) && a.Name.Equals(appName.Name, Utils.StrcmpOption));
                    if (app != null
                        && app.ConfigFilePathIsValid 
                        && app.AppSettings!=null
                        && app.AppSettings.Count>0
                        && app.EsbConnectionType != EsbConnectionTypeEnum.None)
                    {
                        //update the application model with the settings for the ESB environment
                        app.CurrentEsbEnvironment = esbEnvironment.Name;
                        app.EsbConnectionSettings = ReconfigureEsbSettings(esbEnvironment, app);
                        
                        // combine the AppSettings and the ESB-related subset of AppSettings into 1 collection
                        var combinedAppSettings = AppConfigAssistant.CombineDictionariesIgnoreDuplicates(app.AppSettings, app.EsbConnectionSettings);

                        // write the updated settings to the app's configuration file
                        AppConfigAssistant.RewriteAllAppSettings(app.ConfigFilePath, combinedAppSettings);
                    }
                }
            }
        }

        internal Dictionary<string,string> ReconfigureEsbSettings(EsbEnvironmentViewModel esbEnvironment, IconApplicationViewModel appAssignedToEsbEnvironment)
        {
            if (esbEnvironment==null || appAssignedToEsbEnvironment==null || appAssignedToEsbEnvironment.EsbConnectionType==EsbConnectionTypeEnum.None)
            {
                return (Dictionary<string,string>)null;
            }

            var esbConnectionSettings = new Dictionary<string, string>();

            esbConnectionSettings.Add(EsbAppSettings.ServerUrlKey, esbEnvironment.ServerUrl);
            esbConnectionSettings.Add(EsbAppSettings.TargetHostNameKey, esbEnvironment.TargetHostName);

            switch (appAssignedToEsbEnvironment.EsbConnectionType)
            {
                case EsbConnectionTypeEnum.Icon:
                    esbConnectionSettings.Add(EsbAppSettings.JmsUsernameKey, esbEnvironment.JmsUsernameIcon);
                    esbConnectionSettings.Add(EsbAppSettings.JmsPasswordKey, esbEnvironment.JmsPasswordIcon);
                    esbConnectionSettings.Add(EsbAppSettings.JndiPasswordKey, esbEnvironment.JndiUsernameIcon);
                    esbConnectionSettings.Add(EsbAppSettings.JndiUsernameKey, esbEnvironment.JndiPasswordIcon);
                    break;
                case EsbConnectionTypeEnum.Mammoth:
                    esbConnectionSettings.Add(EsbAppSettings.JmsUsernameKey, esbEnvironment.JmsUsernameMammoth);
                    esbConnectionSettings.Add(EsbAppSettings.JmsPasswordKey, esbEnvironment.JmsPasswordMammoth);
                    esbConnectionSettings.Add(EsbAppSettings.JndiPasswordKey, esbEnvironment.JndiUsernameMammoth);
                    esbConnectionSettings.Add(EsbAppSettings.JndiUsernameKey, esbEnvironment.JndiPasswordMammoth);
                    break;
                case EsbConnectionTypeEnum.Ewic:
                    esbConnectionSettings.Add(EsbAppSettings.JmsUsernameKey, esbEnvironment.JmsUsernameEwic);
                    esbConnectionSettings.Add(EsbAppSettings.JmsPasswordKey, esbEnvironment.JmsPasswordEwic);
                    esbConnectionSettings.Add(EsbAppSettings.JndiPasswordKey, esbEnvironment.JndiUsernameEwic);
                    esbConnectionSettings.Add(EsbAppSettings.JndiUsernameKey, esbEnvironment.JndiPasswordEwic);
                    break;
                case EsbConnectionTypeEnum.None:
                default:
                    break;
            }

            return esbConnectionSettings;
        }

        public EsbEnvironmentElement PopulateEsbEnvironmentElement(EsbEnvironmentViewModel esbEnvironmentViewModel)
        {
            EsbEnvironmentElement elementToAdd = new EsbEnvironmentElement();
            elementToAdd.Name = esbEnvironmentViewModel.Name;
            elementToAdd.ServerUrl = esbEnvironmentViewModel.ServerUrl;
            elementToAdd.TargetHostName = esbEnvironmentViewModel.TargetHostName;
            elementToAdd.JmsUsernameIcon = esbEnvironmentViewModel.JmsUsernameIcon;
            elementToAdd.JmsPasswordIcon = esbEnvironmentViewModel.JmsPasswordIcon;
            elementToAdd.JndiUsernameIcon = esbEnvironmentViewModel.JndiUsernameIcon;
            elementToAdd.JndiPasswordIcon = esbEnvironmentViewModel.JndiPasswordIcon;
            elementToAdd.JmsUsernameMammoth = esbEnvironmentViewModel.JmsUsernameMammoth;
            elementToAdd.JmsPasswordMammoth = esbEnvironmentViewModel.JmsPasswordMammoth;
            elementToAdd.JndiUsernameMammoth = esbEnvironmentViewModel.JndiUsernameMammoth;
            elementToAdd.JndiPasswordMammoth = esbEnvironmentViewModel.JndiPasswordMammoth;
            elementToAdd.JmsUsernameEwic = esbEnvironmentViewModel.JmsUsernameEwic;
            elementToAdd.JmsPasswordEwic = esbEnvironmentViewModel.JmsPasswordEwic;
            elementToAdd.JndiUsernameEwic = esbEnvironmentViewModel.JndiUsernameEwic;
            elementToAdd.JndiPasswordEwic = esbEnvironmentViewModel.JndiPasswordEwic;
            return elementToAdd;
        }

        public void AddEsbEnvironmentDefinition(EsbEnvironmentViewModel esbEnvironmentViewModel)
        {
            // open the config file
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
             // read the custom section from the config file
            var esbConfigSectionGroup = configFile.SectionGroups["DashboardCustomConfigSection"] as DashboardCustomConfigSectionGroup;

            foreach (var esbConfigSection in esbConfigSectionGroup.Sections)
            {
                if (esbConfigSection.GetType() == typeof(EsbEnvironmentsSection))
                {
                    var esbEnvironmentsSection = (EsbEnvironmentsSection)esbConfigSection;
                    var esbEnvironmentsCollection = esbEnvironmentsSection.EsbEnvironments;
                    var esbEnvironmentElement = PopulateEsbEnvironmentElement(esbEnvironmentViewModel);
                    esbEnvironmentsCollection.Add(esbEnvironmentElement);
                    break;
                }
            }

            // save the config file
            configFile.Save(ConfigurationSaveMode.Modified);
        }

        public void UpdateEsbEnvironmenDefinition(EsbEnvironmentViewModel esbEnvironmentViewModel)
        {
            // open the config file
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
             // read the custom section from the config file
            var esbConfigSectionGroup = configFile.SectionGroups["DashboardCustomConfigSection"] as DashboardCustomConfigSectionGroup;

            foreach (var esbConfigSection in esbConfigSectionGroup.Sections)
            {
                if (esbConfigSection.GetType() == typeof(EsbEnvironmentsSection))
                {
                    var esbEnvironmentsSection = (EsbEnvironmentsSection)esbConfigSection;
                    var esbEnvironmentsCollection = esbEnvironmentsSection.EsbEnvironments;
                    var esbEnvironmentElement = PopulateEsbEnvironmentElement(esbEnvironmentViewModel);
                    esbEnvironmentsCollection[esbEnvironmentViewModel.Name] = esbEnvironmentElement;
                    break;
                }
            }

            // save the config file
            configFile.Save(ConfigurationSaveMode.Modified);;
        }

        public void DeleteEsbEnvironmentDefinition(EsbEnvironmentViewModel esbEnvironmentViewModel)
        {
            // open the config file
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // read the custom section from the config file
            var esbConfigSectionGroup = configFile.SectionGroups["DashboardCustomConfigSection"] as DashboardCustomConfigSectionGroup;

            foreach (var esbConfigSection in esbConfigSectionGroup.Sections)
            {
                if (esbConfigSection.GetType() == typeof(EsbEnvironmentsSection))
                {
                    var esbEnvironmentsSection = (EsbEnvironmentsSection)esbConfigSection;
                    var esbEnvironmentsCollection = esbEnvironmentsSection.EsbEnvironments;
                    esbEnvironmentsCollection.Remove(esbEnvironmentViewModel.Name);
                    break;
                }
            }

            // save the config file
            configFile.Save(ConfigurationSaveMode.Modified);
        }

         
    }
}