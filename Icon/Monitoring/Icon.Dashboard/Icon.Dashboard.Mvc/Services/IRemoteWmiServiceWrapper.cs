using Icon.Dashboard.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Icon.Dashboard.Mvc.Services
{
    public interface IRemoteWmiServiceWrapper
    {
        IEnumerable<IconLoggedAppViewModel> IconApps { get; set; }

        IEnumerable<IconLoggedAppViewModel> MammothApps { get; set; }

        IEnumerable<EsbEnvironmentViewModel> EsbEnvironments { get; set; }

        IconApplicationViewModel LoadRemoteService(string server, string application, bool commandsEnabled);

        IconApplicationViewModel LoadRemoteService(string server,
            string application,
            bool commandsEnabled,
            IEnumerable<IconLoggedAppViewModel> iconAppList,
            IEnumerable<IconLoggedAppViewModel> mammothAppList,
            IEnumerable<EsbEnvironmentViewModel> allEsbEnvironments);

        List<IconApplicationViewModel> LoadRemoteServices(DashboardEnvironmentViewModel customEnvironment, bool commandsEnabled);

        List<IconApplicationViewModel> LoadRemoteServices(DashboardEnvironmentViewModel customEnvironment,
            bool commandsEnabled,
            IEnumerable<IconLoggedAppViewModel> iconAppList,
            IEnumerable<IconLoggedAppViewModel> mammothAppList,
            IEnumerable<EsbEnvironmentViewModel> allEsbEnvironments);

        string GetLoggingNameFromId(IEnumerable<IconLoggedAppViewModel> apps, int loggingId);

        int GetLoggingIdFromConfig(XDocument appConfig);

        string FindEsbEnvironmentForApp(IEnumerable<EsbEnvironmentViewModel> allEsbEnvironments,
            Dictionary<string, string> appSettingsForEsb);


    }
}