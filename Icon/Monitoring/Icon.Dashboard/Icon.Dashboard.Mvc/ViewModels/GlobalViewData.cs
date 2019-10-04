using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class GlobalViewData
    {
        public string ViewTitle { get; set; }
        public string ActiveEnvironmentName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool ServiceCommandsAreEnabled { get; set; }
        public EnvironmentViewModel HostingEnvironment { get; set; }
        public EnvironmentViewModel ActiveEnvironment { get; set; }
        public SubMenuViewModel SubMenuForIconLogs { get; set; }
        public SubMenuViewModel SubMenuForMammothLogs { get; set; }
        public SubMenuViewModel SubMenuForIconApiJobs { get; set; }
        public SubMenuForSupportAppsViewModel SubMenuForSupportApps { get; set; }
        public SubMenuViewModel SubMenuForEnvironments { get; set; }
        public int ServiceCommandTimeout { get; set; }
        public int HoursForRecentErrors { get; set; }
        public int MillisecondsForRecentErrorsPolling { get; set; }
    }
}