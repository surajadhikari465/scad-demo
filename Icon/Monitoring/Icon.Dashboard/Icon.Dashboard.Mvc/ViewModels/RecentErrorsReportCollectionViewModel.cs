using Icon.Dashboard.Mvc.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class RecentErrorsReportCollectionViewModel
    {
        public RecentErrorsReportCollectionViewModel() { }

        public RecentErrorsReportCollectionViewModel(
            //string viewTitle,
            IconOrMammothEnum system,
            EnvironmentEnum environment,
            int hoursForRecent,
            string controllerName,
            string actionForRecentErrors
            ) : this()
        {
            Reports = new List<AppRecentErrorsReportViewModel>();
            //ViewTitle = viewTitle;
            System = system;
            Environment = environment;
            HoursConsideredRecent = hoursForRecent;
            ControllerName = controllerName;
            ActionForRecentErrors = actionForRecentErrors;
        }

        public List<AppRecentErrorsReportViewModel> Reports { get; set; }
        public int HoursConsideredRecent { get; set; }
        public string PartialViewTitle
        {
            get
            {
                return $"{System} {Environment} DB Errors Logged during Last {HoursConsideredRecent} Hours";
            }
        }
        public IconOrMammothEnum System { get; set; }
        public EnvironmentEnum Environment { get; set; }
        public string ControllerName { get; set; }
        public string ActionForRecentErrors { get; set; }
        public List<int> ReportAppIDs
        {
            get
            {
                var appIDs = new List<int>();
                if (Reports!=null && Reports.Count > 0)
                {
                    appIDs = Reports.Where(r => r.AppID > 0).Select(r => r.AppID).ToList();
                }
                return appIDs;
            }
        }
    }
}