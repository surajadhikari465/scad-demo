using System;
using System.Collections.Generic;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.ViewModels;

namespace Icon.Dashboard.Mvc.Services
{
    public interface IDashboardEnvironmentManager
    {
        DashboardEnvironmentCollectionViewModel BuildEnvironmentCollection(EnvironmentEnum selectedEnvironment);
        DashboardEnvironmentViewModel BuildEnvironmentViewModel(EnvironmentEnum environment);
        DashboardEnvironmentViewModel BuildEnvironmentViewModel(EnvironmentEnum environment, List<string> defaultAppServersForEnvironment);
        List<string> GetDefaultAppServersForEnvironment(EnvironmentEnum environment);
        List<string> GetDefaultAppServersForEnvironment(string environment);
        EnvironmentEnum GetDefaultEnvironmentEnumFromWebhost(string webhost);
        string GetDefaultEnvironmentNameFromWebhost(string webhost);
        EnvironmentEnum GetEnvironmentFromAppserver(string appserver);
        List<Tuple<string, string>> GetSupportServerLinks(EnvironmentEnum environment);
        KeyValuePair<string, string> GetWebServerKvpForEnvironment(EnvironmentEnum environment);
        Dictionary<string, string> GetWebServersDictionary();
        string GetAppConfigValue(string appConfigKey);
    }
}