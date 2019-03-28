using Icon.Dashboard.DataFileAccess.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Icon.Dashboard.Mvc.Helpers
{

    public class EnvironmentSwitcher
    {
        public Dictionary<string, string> GetWebServersForEnvironments()
        {
            var webServerList = new Dictionary<string, string>();
            foreach (var environment in Enum.GetValues(typeof(EnvironmentEnum)).Cast<EnvironmentEnum>())
            {
                var server = ConfigurationManager.AppSettings["webServer_" + environment.ToString()];
                if (server != null)
                {
                    webServerList.Add(environment.ToString(), server);
                }
            }
            if (webServerList.Count < 1)
            {
                webServerList.Add("Test", "localhost");
            }
            return webServerList;
        }

        public KeyValuePair<string, string> GetWebServerForEnvironment(EnvironmentEnum environment)
        {
            var server = ConfigurationManager.AppSettings["webServer_" + environment.ToString()];
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
            string configEntryForEnv = ConfigurationManager.AppSettings["appServers_" + environment];

            if (!String.IsNullOrWhiteSpace(configEntryForEnv))
            {
                defaultServers.AddRange(configEntryForEnv.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            }

            return defaultServers;
        }

        public string GetTibcoAdminServerForEnivronment(string environment)
        {
            if (string.IsNullOrWhiteSpace(environment)) environment = "Dev";
            var serverUrl = ConfigurationManager.AppSettings["tibcoAdminServer_" + environment];
            return serverUrl;
        }

        public string GetMammothWebSupportServerForEnivronment(string environment)
        {
            if (string.IsNullOrWhiteSpace(environment)) environment = "Dev";
            var serverUrl = ConfigurationManager.AppSettings["mammothWebSupport_" + environment];
            return serverUrl;
        }

        public string GetIconWebServerForEnvironment(string environment)
        {
            if (string.IsNullOrWhiteSpace(environment)) environment = "Dev";
            var serverUrl = ConfigurationManager.AppSettings["iconWeb_" + environment];
            return serverUrl;
        }
    }
}