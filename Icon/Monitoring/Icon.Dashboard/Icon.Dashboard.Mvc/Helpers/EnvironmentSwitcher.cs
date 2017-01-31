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
        public Dictionary<string, string> GetServersForEnvironments()
        {
            var serverList = new Dictionary<string, string>();
            foreach (var environment in Enum.GetValues(typeof(EnvironmentEnum)).Cast<EnvironmentEnum>())
            {
                var server = ConfigurationManager.AppSettings[$"appServer_{Utils.Environment.ToLower()}"];
                if (server != null)
                {
                    serverList.Add(environment.ToString(), server);
                }
            }
            return serverList;
        }        
    }
}