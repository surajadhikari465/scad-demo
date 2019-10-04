using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.ViewModels;
using Icon.Dashboard.RemoteServicesAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Dashboard.Mvc.Services
{
    public interface IRemoteWmiServiceWrapper
    {
        IRemoteWmiAccessService WmiService { get; set; }
        ServiceViewModel LoadRemoteService(
            string server,
            string application,
            bool commandsEnabled,
            List<LoggedAppViewModel> iconLoggedApps,
            List<LoggedAppViewModel> mammothLoggedApps,
            List<EnvironmentModel> environments,
            List<EsbEnvironmentModel> esbEnvironments,
            IExternalConfigXmlManager serviceConfigDataReader = null);
        List<ServiceViewModel> LoadRemoteServices(
            List<string> appServerNames,
            bool commandsEnabled,
            List<LoggedAppViewModel> iconLoggedApps,
            List<LoggedAppViewModel> mammothLoggedApps,
            List<EnvironmentModel> environments,
            List<EsbEnvironmentModel> esbEnvironments);
        string PrependConfigUncPath(string serverName, string pathName);
        string GetLoggingNameFromId(int appLoggingId,
          IconOrMammothEnum appFamily,
          IEnumerable<LoggedAppViewModel> iconLoggedApps,
          IEnumerable<LoggedAppViewModel> mammothLoggedApps);        
        void ExecuteServiceCommand(string server, string application, string command);
        void RestartServices(IEnumerable<EsbEnvironmentViewModel> esbEnvironments);
        void RestartServices(IEnumerable<ServiceViewModel> applications);
        void UpdateRemoteServiceConfig(ServiceViewModel serviceViewModel,
            IExternalConfigXmlManager serviceConfigDataWriter = null);
        void ReconfigureServiceEsbEnvironmentSettings(
            List<EsbEnvironmentViewModel> esbEnvironmentsWithServices,
            List<ServiceViewModel> services,
            IExternalConfigXmlManager serviceConfigDataWriter = null);
    }
}