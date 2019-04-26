using Icon.Dashboard.Mvc.ViewModels;
using Icon.Dashboard.RemoteServicesAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Icon.Dashboard.Mvc.Services
{
    public interface IRemoteWmiServiceWrapper
    {
        IRemoteWmiAccessService WmiService { get; set; }
        IIconDatabaseServiceWrapper IconDbService { get; set; }
        IMammothDatabaseServiceWrapper MammothDbService { get; set; }
        IEsbEnvironmentManager EsbEnvironmentManager { get; set; }
        IconApplicationViewModel LoadRemoteService(string server, string application, bool commandsEnabled);
        List<IconApplicationViewModel> LoadRemoteServices(DashboardEnvironmentViewModel customEnvironment, bool commandsEnabled);
        string GetLoggingNameFromId(IEnumerable<IconLoggedAppViewModel> apps, int loggingId);
        int GetLoggingIdFromConfig(XDocument appConfig);
        string FindEsbEnvironmentForApp(IEnumerable<EsbEnvironmentViewModel> allEsbEnvironments, Dictionary<string, string> esbConnectionSettings);
        string GetConfigUncPath(string serverName, string pathName);
        void ExecuteServiceCommand(string server, string application, string command);
        void RestartServices(IEnumerable<EsbEnvironmentViewModel> esbEnvironments);
        void RestartServices(IEnumerable<IconApplicationViewModel> applications);
        void SaveRemoteServiceAppSettings(IconApplicationViewModel appViewModel);
    }
}