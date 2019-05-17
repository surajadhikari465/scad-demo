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

            serverLinks.AddRange(GetSupportAppLinksFromConfig(environment, "iconDashboard", "Icon Dashboard"));
            serverLinks.AddRange(GetSupportAppLinksFromConfig(environment, "mammothWebSupport", "Mammoth Web Support"));
            serverLinks.AddRange(GetSupportAppLinksFromConfig(environment, "iconWeb", "Icon Web"));
            serverLinks.AddRange(GetSupportAppLinksFromConfig(environment, "tibcoAdminServer", "TIBCO Admin"));

            return serverLinks;
        }

        /// <summary>
        /// Returns a list of string pairs representing a URL link and the text to display for that link in the client.
        ///   Reads from the application .config file to find an expected app setting key-value in the format
        ///   like the following:
        ///   <add key="iconWeb_Test" value="http://icon-test/" />
        ///   <add key="mammothWebSupport_QA" value="http://irmaqaapp1/MammothWebSupport" />
        ///   <add key="tibcoAdminServer_QA" value="https://cerd1673:28090/,https://cerd1674:28090/" />
        /// </summary>
        /// <param name="environment">The dashboard environment (Dev,Test,QA,Perf,Prd)</param>
        /// <param name="appSettingKeyPrefix">The app setting key root (without the _ and environment suffix),
        ///     for example "iconWeb" or "mammothWebSupport"</param>
        /// <param name="linkDisplayPrefix">The leading text to display to the user for the link,
        ///     for example "Icon Web" or "Mammoth Web Support"</param>
        /// <returns>A list of string tuples representing the link text and url, for example 
        ///     "Icon Web Test", "http://icon-test/"
        ///     or
        ///     "TIBCO Admin Web 1", "https://cerd1673:28090/"
        ///     "TIBCO Admin Web 2", "https://cerd1674:28090/"
        ///     </returns>
        private List<Tuple<string,string>> GetSupportAppLinksFromConfig(EnvironmentEnum environment, string appSettingKeyPrefix, string linkDisplayPrefix)
        {
            var serverLinks = new List<Tuple<string, string>>();
            var configValue = GetAppConfigValue($"{appSettingKeyPrefix}_{environment}");
            if (!string.IsNullOrWhiteSpace(configValue))
            {
                // is there more than one entry in a comma-separated list?
                if (configValue.Contains(","))
                {
                    int i = 1;
                    foreach (var separateServerUrl in configValue.Split(','))
                    {
                        serverLinks.Add(new Tuple<string, string>($"{linkDisplayPrefix} {environment} {i}", separateServerUrl));
                        i++;
                    }
                }
                else
                {
                    serverLinks.Add(new Tuple<string, string>($"{linkDisplayPrefix} {environment}", configValue));
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

        public EnvironmentEnum GetEnvironmentEnumFromAppserver(string appserver)
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

        public DashboardEnvironmentViewModel BuildEnvironmentViewModelFromWebhost(string webhost)
        {
            var environmentEnum = GetDefaultEnvironmentEnumFromWebhost(webhost);
            return BuildEnvironmentViewModel(environmentEnum);
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

        public DashboardEnvironmentCollectionViewModel BuildEnvironmentCollection(EnvironmentEnum selectedEnvironmentEnum)
        {
            var environmentCollection = BuildStandardEnvironmentCollection();

            var selectedEnvironmentElement = environmentCollection.Environments
                .FirstOrDefault(e=>e.Name.Equals(selectedEnvironmentEnum.ToString(), Utils.StrcmpOption));
            environmentCollection.SelectedEnvIndex = environmentCollection.Environments.IndexOf(selectedEnvironmentElement);

            return environmentCollection;
        }

        public DashboardEnvironmentCollectionViewModel BuildEnvironmentCollection(DashboardEnvironmentViewModel selectedEnvironment)
        {
            var environmentCollection = BuildStandardEnvironmentCollection();

            var selectedEnvironmentElement = environmentCollection.Environments
                .FirstOrDefault(e => e.Name.Equals(selectedEnvironment.Name, Utils.StrcmpOption));
            environmentCollection.SelectedEnvIndex = environmentCollection.Environments.IndexOf(selectedEnvironmentElement);

            return environmentCollection;
        }

        public DashboardEnvironmentCollectionViewModel BuildStandardEnvironmentCollection()
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
            }
            var customEnvironment = new DashboardEnvironmentViewModel()
            {
                Name = "Custom",
                AppServers = new List<AppServerViewModel>()
            };
            environmentCollection.Environments.Add(customEnvironment);

            return environmentCollection;
        }

        public DashboardEnvironmentViewModel GetEnvironment(string webhost, string environment = null)
        {
            var chosenEnvironmentEnum = EnvironmentEnum.Undefined;
            if (string.IsNullOrWhiteSpace(environment) || !Enum.TryParse(environment, out chosenEnvironmentEnum))
            {
                // determine the default environment based on the hosting web server
                chosenEnvironmentEnum = GetDefaultEnvironmentEnumFromWebhost(webhost);
            }
            var chosenEnvironmentViewModel = BuildEnvironmentViewModel(chosenEnvironmentEnum);
            return chosenEnvironmentViewModel;
        }

        public bool EnvironmentIsProduction(DashboardEnvironmentViewModel chosenEnvironment)
        {
            // check app config for app servers associated w/ prd
            var productionAppServers = GetDefaultAppServersForEnvironment(EnvironmentEnum.Prd);
            foreach(var chosenAppServer in chosenEnvironment.AppServers)
            {
                if (productionAppServers.Any(s=> s.Equals(chosenAppServer.ServerName, Utils.StrcmpOption)))
                {
                    return true;
                }
            }
            // just in case, check for app servers containing "prd" in them
            foreach(var chosenAppServer in chosenEnvironment.AppServers)
            {
                if (chosenAppServer.ServerName.IndexOf("prd", Utils.StrcmpOption)!=-1)
                {
                    return true;
                }
            }
            return false;
        }
    }
}