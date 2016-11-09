using ApplicationMonitor.Core.Models;
using ApplicationMonitor.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationMonitor.Web.Extensions
{
    public static class ViewModelMappingExtensions
    {
        public static List<ApiControllerViewModel> ToApiControllerViewModels(
            this Application application,
            Dictionary<string, string> iconEnvironments,
            Dictionary<string, string> esbEnvironments)
        {
            return application.Instances.Select(i => new ApiControllerViewModel
            {
                IconEnvironment = iconEnvironments.ContainsKey(i.Server) ? iconEnvironments[i.Server] : "SERVER NOT REGISTERED TO ENVIRONMENT",
                Server = i.Server,
                Settings = new ApiControllerSettingsViewModel
                {
                    ServerUrl = esbEnvironments.ContainsKey(i.AppSettings["ServerUrl"]) ? esbEnvironments[i.AppSettings["ServerUrl"]] : i.AppSettings["ServerUrl"]
                },
                ApiControllerJobs = i.SubInstances.Select(si => new ApiControllerJobViewModel { Name = si.Name, Status = si.Status }).ToList()
            })
            .ToList();
        }

        public static List<GlobalControllerViewModel> ToGlobalControllerViewModels(
            this Application application,
            Dictionary<string, string> iconEnvironments)
        {
            return application.Instances.Select(i => new GlobalControllerViewModel
            {
                IconEnvironment = iconEnvironments.ContainsKey(i.Server) ? iconEnvironments[i.Server] : "SERVER NOT REGISTERED TO ENVIRONMENT",
                Server = i.Server,                
                Jobs = i.SubInstances.Select(si => new GlobalControllerJobViewModel { Name = si.Name, Status = si.Status }).ToList()
            })
            .ToList();
        }
    }
}
