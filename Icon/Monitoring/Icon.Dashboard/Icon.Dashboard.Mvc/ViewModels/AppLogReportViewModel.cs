using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.ViewModels
{
    public class AppLogReportViewModel
    {
        public AppLogReportViewModel() { }

        public AppLogReportViewModel(
            int pollingInterval,
            string controllerName,
            string actionForTableRefresh,
            string actionForRecentErrors,
            string actionForRedrawPaging
            ) : this()
        {
            PollingRefreshIntervalMilliseconds = pollingInterval;
            ControllerName = controllerName;
            ActionForTableRefresh = actionForTableRefresh;
            ActionForRecentErrors = actionForRecentErrors;
            ActionForRedrawPaging = actionForRedrawPaging;
        }

        public string AppName { get; set; }

        public string PartialViewTitle { get; set; }

        public LogErrorLevelEnum ErrorLevel { get; set; }

        public IEnumerable<AppLogEntryViewModel> LogEntries { get; set; }

        public PaginationPageSetViewModel PaginationModel { get; set; }

        public RecentErrorsReportCollectionViewModel RecentErrorsReportCollection { get; set; }

        public int HoursConsideredRecent { get; set; }
        public int PollingRefreshIntervalMilliseconds { get; set; }
        public string ControllerName { get; set; }
        public string ActionForTableRefresh { get; set; }
        public string ActionForRecentErrors { get; set; }
        public string ActionForRedrawPaging { get; set; }
    }
}