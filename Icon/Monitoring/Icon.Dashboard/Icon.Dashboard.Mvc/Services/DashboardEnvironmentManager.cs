using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.ViewModels;
using Icon.Dashboard.Mvc.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Icon.Dashboard.Mvc.Services
{

    public class DashboardEnvironmentManager : IDashboardEnvironmentManager
    {
        public string GetAppConfigValue(string appConfigKey)
        {
            return ConfigurationManager.AppSettings[appConfigKey];
        }

        public Dictionary<string, string> GetWebServersDictionary()
        {
            var webServerList = new Dictionary<string, string>();
            foreach (var environmentEnum in Enum.GetValues(typeof(EnvironmentEnum)).Cast<EnvironmentEnum>())
            {
                var kvp = GetWebServerKvpForEnvironment(environmentEnum);
                if (!string.IsNullOrWhiteSpace(kvp.Value))
                {
                    webServerList.Add(kvp.Key, kvp.Value);
                }
            }
            if (webServerList.Count < 1)
            {
                webServerList.Add("Dev", "localhost");
            }
            return webServerList;
        }

        public KeyValuePair<string, string> GetWebServerKvpForEnvironment(EnvironmentEnum environment)
        {
            var server = GetAppConfigValue("webServer_" + environment.ToString());
            return new KeyValuePair<string, string>(environment.ToString(), server);
        }

        public List<string> GetDefaultAppServersForEnvironment(EnvironmentEnum environment)
        {
            return GetDefaultAppServersForEnvironment(environment.ToString());
        }

        public List<string> GetDefaultAppServersForEnvironment(string environment)
        {
            if (string.IsNullOrWhiteSpace(environment)) environment = "Dev";
            var defaultServers = new List<string>();
            string configEntryForEnv = GetAppConfigValue("appServers_" + environment);

            if (!String.IsNullOrWhiteSpace(configEntryForEnv))
            {
                defaultServers.AddRange(configEntryForEnv.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            }

            return defaultServers;
        }


        public List<Tuple<string, string>> GetSupportServerLinks(EnvironmentEnum environment)
        {
            var serverLinks = new List<Tuple<string, string>>();

            var mammothWebSupportConfigValue = GetAppConfigValue($"mammothWebSupport_{environment}");

            if (!string.IsNullOrWhiteSpace(mammothWebSupportConfigValue))
            { 
                // is there more than one entry in a comma-separated list?
                if (mammothWebSupportConfigValue.Contains(","))
                {
                    int i = 1;
                    foreach (var separateServerUrl in mammothWebSupportConfigValue.Split(','))
                    {
                        serverLinks.Add(new Tuple<string, string>($"Mammoth Web Support {environment} {i}", separateServerUrl));
                        i++;
                    }
                }
                else
                {
                    serverLinks.Add(new Tuple<string, string>($"Mammoth Web Support {environment}", mammothWebSupportConfigValue));
                }
            }

            var iconWebConfigValue = GetAppConfigValue($"iconWeb_{environment}");
            if (!string.IsNullOrWhiteSpace(iconWebConfigValue))
            {
                // is there more than one entry in a comma-separated list?
                if (iconWebConfigValue.Contains(","))
                {
                    int i = 1;
                    foreach (var separateServerUrl in iconWebConfigValue.Split(','))
                    {
                        serverLinks.Add(new Tuple<string, string>($"Icon Web {environment} {i}", separateServerUrl));
                        i++;
                    }
                }
                else
                {
                    serverLinks.Add(new Tuple<string, string>($"Icon Web {environment}", iconWebConfigValue));
                }
            }
            
            var tibcoAdminConfigValue = GetAppConfigValue("tibcoAdminServer_" + environment);
            if (!string.IsNullOrWhiteSpace(tibcoAdminConfigValue))
            {
                // is there more than one entry in a comma-separated list?
                if (tibcoAdminConfigValue.Contains(","))
                {
                    int i = 1;
                    foreach (var separateServerUrl in tibcoAdminConfigValue.Split(','))
                    {
                        serverLinks.Add(new Tuple<string, string>($"TIBCO Admin {environment} {i}", separateServerUrl));
                        i++;
                    }
                }
                else
                {
                    serverLinks.Add(new Tuple<string, string>($"TIBCO Admin {environment}", tibcoAdminConfigValue));
                }
            }

            return serverLinks;
        }


        public EnvironmentEnum GetDefaultEnvironmentEnumFromWebhost(string webhost)
        {
            var defaultEnvironment = EnvironmentEnum.Undefined;
            string defaultEnvironmentName = GetDefaultEnvironmentNameFromWebhost(webhost);
            Enum.TryParse(defaultEnvironmentName, out defaultEnvironment);
            return defaultEnvironment;
        }

        public EnvironmentEnum GetEnvironmentFromAppserver(string appserver)
        {
            var environmentEnum = EnvironmentEnum.Undefined;

            // check the app servers listed in the config to find which environment this one is associated with by default
            foreach (var possibleEnvironment in Enum.GetValues(typeof(EnvironmentEnum)).Cast<EnvironmentEnum>())
            {
                var appServersForEnvironment = GetDefaultAppServersForEnvironment(possibleEnvironment);
                if (appServersForEnvironment.Contains(appserver))
                {
                    return possibleEnvironment;
                }
            }

            // in case the server wasn't found in the config, check if the server name contains the environment (vm-icon-test1 contains "test", mammoth-app01-qa contains "qa", etc.)
            foreach (var possibleEnvironment in Enum.GetValues(typeof(EnvironmentEnum)).Cast<EnvironmentEnum>())
            {
                if (appserver.IndexOf(possibleEnvironment.ToString(), Utils.StrcmpOption) > 0)
                {
                    return possibleEnvironment;
                }
            }

            return environmentEnum;
        }

        public string GetDefaultEnvironmentNameFromWebhost(string webhost)
        {
            var webServersDictionary = GetWebServersDictionary();
            foreach (var webServerKvp in webServersDictionary)
            {
                if (webServerKvp.Value.Equals(webhost, Utils.StrcmpOption))
                {
                    return webServerKvp.Key;
                }
            }

            return "Dev";
        }

        public DashboardEnvironmentViewModel BuildEnvironmentViewModel(EnvironmentEnum environment)
        {
            var defaultAppServersForEnvironment = GetDefaultAppServersForEnvironment(environment);

            var environmentViewModel = new DashboardEnvironmentViewModel()
            {
                Name = environment.ToString(),
                AppServers = defaultAppServersForEnvironment
                    .Select(s => new AppServerViewModel { ServerName = s })
                    .ToList()
            };
            return environmentViewModel;
        }

        public DashboardEnvironmentViewModel BuildEnvironmentViewModel(EnvironmentEnum environment, List<string> defaultAppServersForEnvironment)
        {
            var environmentViewModel = new DashboardEnvironmentViewModel()
            {
                Name = environment.ToString(),
                AppServers = defaultAppServersForEnvironment
                    .Select(s => new AppServerViewModel { ServerName = s })
                    .ToList()
            };
            return environmentViewModel;
        }

        public DashboardEnvironmentCollectionViewModel BuildEnvironmentCollection(EnvironmentEnum selectedEnvironment)
        {
            var environmentCollection = new DashboardEnvironmentCollectionViewModel();

            foreach (var environment in Enum.GetValues(typeof(EnvironmentEnum)).Cast<EnvironmentEnum>())
            {
                if (environment == EnvironmentEnum.Undefined) continue;

                var defaultAppServersForEnvironment = GetDefaultAppServersForEnvironment(environment);
                var environmentViewModel = new DashboardEnvironmentViewModel()
                {
                    Name = environment.ToString(),
                    AppServers = defaultAppServersForEnvironment
                       .Select(s => new AppServerViewModel { ServerName = s })
                       .ToList()
                };
                environmentCollection.Environments.Add(environmentViewModel);
                //}
            }
            var customEnvironment = new DashboardEnvironmentViewModel()
            {
                Name = "Custom",
                AppServers = new List<AppServerViewModel>()
            };
            environmentCollection.Environments.Add(customEnvironment);
            environmentCollection.SelectedEnvIndex = environmentCollection.Environments.IndexOf(customEnvironment);

            return environmentCollection;
        }

    }
}