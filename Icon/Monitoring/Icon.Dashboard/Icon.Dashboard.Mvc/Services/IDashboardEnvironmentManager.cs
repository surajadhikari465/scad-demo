using System;
using System.Collections.Generic;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.ViewModels;

namespace Icon.Dashboard.Mvc.Services
{
    public interface IDashboardEnvironmentManager
    {
        DashboardEnvironmentViewModel GetEnvironment(string webhost, string environment = null);
        DashboardEnvironmentCollectionViewModel BuildEnvironmentCollection(EnvironmentEnum selectedEnvironment);
        DashboardEnvironmentCollectionViewModel BuildEnvironmentCollection(DashboardEnvironmentViewModel selectedEnvironmen);
        DashboardEnvironmentViewModel BuildEnvironmentViewModel(EnvironmentEnum environment);
        DashboardEnvironmentViewModel BuildEnvironmentViewModel(EnvironmentEnum environment, List<string> defaultAppServersForEnvironment);
        DashboardEnvironmentViewModel BuildEnvironmentViewModelFromWebhost(string webhost);
        List<string> GetDefaultAppServersForEnvironment(EnvironmentEnum environment);
        List<string> GetDefaultAppServersForEnvironment(string environment);
        EnvironmentEnum GetDefaultEnvironmentEnumFromWebhost(string webhost);
        string GetDefaultEnvironmentNameFromWebhost(string webhost);
        EnvironmentEnum GetEnvironmentEnumFromAppserver(string appserver);
        List<Tuple<string, string>> GetSupportServerLinks(EnvironmentEnum environment);
        KeyValuePair<string, string> GetWebServerKvpForEnvironment(EnvironmentEnum environment);
        Dictionary<string, string> GetWebServersDictionary();
        string GetAppConfigValue(string appConfigKey);
        bool EnvironmentIsProduction(DashboardEnvironmentViewModel chosenEnvironment);
    }
}