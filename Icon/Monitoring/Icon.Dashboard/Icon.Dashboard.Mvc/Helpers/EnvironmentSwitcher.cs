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
            var serverList = new Dictionary<string, string>();
            foreach (var environment in Enum.GetValues(typeof(EnvironmentEnum)).Cast<EnvironmentEnum>())
            {
                var server = ConfigurationManager.AppSettings["webServer_" + environment.ToString()];
                if (server != null)
                {
                    serverList.Add(environment.ToString(), server);
                }
            }
            return serverList;
        }

        public KeyValuePair<string, string> GetWebServerForEnvironment(EnvironmentEnum environment)
        {
            var server = ConfigurationManager.AppSettings["webServer_" + environment.ToString()];
            return new KeyValuePair<string,string>(environment.ToString(), server);
        }

        public List<string> GetDefaultIconServersForEnvironment(EnvironmentEnum environment)
        {
            return GetDefaultIconServersForEnvironment(environment.ToString());
        }

        public List<string> GetDefaultIconServersForEnvironment(string environment)
        {
            if (string.IsNullOrWhiteSpace(environment)) environment = "Dev";
            var defaultServers = new List<string>();
            string configEntryForEnv = ConfigurationManager.AppSettings["serviceServers_" + environment];

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