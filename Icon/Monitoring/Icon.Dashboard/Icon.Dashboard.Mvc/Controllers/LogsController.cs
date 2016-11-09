using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.Mvc.Helpers;
using Icon.Dashboard.Mvc.Services;
using Icon.Dashboard.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Icon.Dashboard.Mvc.Controllers
{
    public class LogsController : Controller
    {
        public IIconDatabaseServiceWrapper IconDatabaseDataAccess { get; private set; }

        private HttpServerUtilityBase _serverUtility;
        public HttpServerUtilityBase ServerUtility
        {
            get
            {
                return _serverUtility ?? Server;
            }
        }

        public LogsController() : this(null, null) { }

        public LogsController(IIconDatabaseServiceWrapper loggingServiceWrapper = null,
            HttpServerUtilityBase serverUtility = null)
        {
            IconDatabaseDataAccess = loggingServiceWrapper ?? new IconDatabaseServiceWrapper();
            _serverUtility = serverUtility;
        }
        
        [HttpGet]
        [SetViewBagMenuOptionsFilter]
        public ActionResult Index(string id = null, int page=1, int pageSize = PagingConstants.DefaultPageSize, string errorLevel = "Any")
        {
            //enable filter to use the data service
            HttpContext.Items["loggingDataService"] = IconDatabaseDataAccess;            
            var logs = GetAppLogsAndSetRelatedViewData(id, page, pageSize);
            SetViewBagLogEntryReportList();
            return View(logs);
        }

        [HttpPost]
        public ActionResult TableRefresh(string routeParameter = null, int page = 1, int pageSize = PagingConstants.DefaultPageSize, string errorLevel = "Any")
        {
            var logs = GetAppLogsAndSetRelatedViewData(routeParameter, page, pageSize);
            return PartialView("_AppLogTablePartial", logs);
        }

        private IEnumerable<IconAppLogViewModel> GetAppLogsAndSetRelatedViewData(string appName, int page, int pageSize)
        {
            if (String.IsNullOrWhiteSpace(appName))
            {
                return GetAppLogsAndSetRelatedViewData(page, pageSize);
            }
            List<IconAppLogViewModel> logs = IconDatabaseDataAccess.GetPagedAppLogsByApp(appName, page, pageSize);

            ViewBag.AppName = appName;
            ViewBag.Title = $"{appName} Log Viewer";
            ViewBag.PaginationPageSetViewModel = GetPaginationViewModel(appName, page, pageSize);

            return logs;
        }

        private IEnumerable<IconAppLogViewModel> GetAppLogsAndSetRelatedViewData(int page, int pageSize)
        {
            List<IconAppLogViewModel> logs = IconDatabaseDataAccess.GetPagedAppLogs(page, pageSize);

            //ViewBag.AppName = null;
            ViewBag.Title = "ICON Dashboard Log Viewer (All Apps)";
            ViewBag.PaginationPageSetViewModel = GetPaginationViewModel(page, pageSize);

            return logs;
        }

        protected PaginationPageSetViewModel GetPaginationViewModel(int page, int pageSize)
        {
            var pagingData = new PaginationPageSetViewModel("TableRefresh", "Logs", PagingConstants.NumberOfQuickLinks, page, pageSize);
            return pagingData;
        }

        protected PaginationPageSetViewModel GetPaginationViewModel(string appName, int page, int pageSize)
        {
            var pagingData = String.IsNullOrWhiteSpace(appName)
               ? GetPaginationViewModel(page, pageSize)
               : new PaginationPageSetViewModel("TableRefresh", "Logs", PagingConstants.NumberOfQuickLinks, page, pageSize, appName);
            return pagingData;
        }

        [HttpGet]
        public ActionResult RedrawPaging(string routeParameter = null, int page = 1, int pageSize = PagingConstants.DefaultPageSize)
        {
            var pagingData = GetPaginationViewModel(routeParameter, page, pageSize);
            return PartialView("_PaginationPartial", pagingData);
        }

        [HttpGet]
        public ActionResult RecentErrors(int appID, int hours = 24)
        {
            var recentLogEntryReportForApp = IconDatabaseDataAccess.GetRecentLogEntriesReportForApp(appID, new TimeSpan(hours, 0, 0), LoggingLevel.Error);
            return PartialView("_RecentLogEntriesReportPartial", recentLogEntryReportForApp);
        }

        private void SetViewBagLogEntryReportList()
        {
            if (null != IconDatabaseDataAccess && IconDatabaseDataAccess is IIconDatabaseServiceWrapper)
            {
                var allApps = (IconDatabaseDataAccess as IIconDatabaseServiceWrapper).GetApps();

                ViewBag.RecentLogEntriesReportList = IconDatabaseDataAccess.GetEmptyLogEntriesReportList(allApps);
                ViewBag.RecentLogEntriesHours = GetHoursForRecentErrors();
                ViewBag.MillisecondsForRecentErrorsPolling = GetMillisecondsForRecentErrorsPolling();
            }
        }

        private int GetHoursForRecentErrors()
        {
            int hoursForRecentErrors = 24;
            var valueFromAppConfig = ConfigurationManager.AppSettings["hoursForRecentErrors"];
            Int32.TryParse(valueFromAppConfig, out hoursForRecentErrors);
            return hoursForRecentErrors;
        }

        private int GetMillisecondsForRecentErrorsPolling()
        {
            int secondsForRecentErrorsPolling = 60;
            var valueFromAppConfig = ConfigurationManager.AppSettings["intervalForRecentErrorsPollingInSeconds"];
            Int32.TryParse(valueFromAppConfig, out secondsForRecentErrorsPolling);
            return secondsForRecentErrorsPolling * 1000;
        }
    }
}