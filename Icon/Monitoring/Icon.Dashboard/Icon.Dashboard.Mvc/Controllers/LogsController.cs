using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.Mvc.Filters;
using Icon.Dashboard.Mvc.Helpers;
using Icon.Dashboard.Mvc.Models;
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
        private HttpServerUtilityBase _serverUtility;

        public LogsController() : this(null, null) { }

        public LogsController(IIconDatabaseServiceWrapper loggingServiceWrapper = null,
            HttpServerUtilityBase serverUtility = null)
        {
            IconDatabaseDataAccess = loggingServiceWrapper ?? new IconDatabaseServiceWrapper();
            _serverUtility = serverUtility;
        }
        public IIconDatabaseServiceWrapper IconDatabaseDataAccess { get; private set; }
        public HttpServerUtilityBase ServerUtility
        {
            get
            {
                return _serverUtility ?? Server;
            }
        }

        #region GET
        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.ReadOnly)]
        public ActionResult Index(string id = null, int page = 1, int pageSize = PagingConstants.DefaultPageSize,
            string errorLevel = "Any")
        {
            //enable filter to use the data service
            HttpContext.Items["loggingDataService"] = IconDatabaseDataAccess;
            var logs = GetAppLogsAndSetRelatedViewData(id, page, pageSize);
            SetViewBagLogEntryReportList();
            return View(logs);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.ReadOnly)]
        public ActionResult RecentErrors(int appID, int hours = 24)
        {
            var recentLogEntryReportForApp = IconDatabaseDataAccess.GetRecentLogEntriesReportForApp(
                appID, new TimeSpan(hours, 0, 0), LoggingLevel.Error);
            return PartialView("_RecentLogEntriesReportPartial", recentLogEntryReportForApp);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.ReadOnly)]
        public ActionResult RedrawPaging(string routeParameter = null, int page = 1,
            int pageSize = PagingConstants.DefaultPageSize)
        {
            var pagingData = GetPaginationViewModel(routeParameter, page, pageSize);
            return PartialView("_PaginationPartial", pagingData);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.ReadOnly)]
        public ActionResult TableRefresh(string routeParameter = null, int page = 1,
            int pageSize = PagingConstants.DefaultPageSize, string errorLevel = "Any")
        {
            var logs = GetAppLogsAndSetRelatedViewData(routeParameter, page, pageSize);
            return PartialView("_AppLogTablePartial", logs);
        }
        #endregion

        protected PaginationPageSetViewModel GetPaginationViewModel(int page, int pageSize)
        {
            var pagingData = new PaginationPageSetViewModel(
                "TableRefresh", "Logs", PagingConstants.NumberOfQuickLinks, page, pageSize);
            return pagingData;
        }

        protected PaginationPageSetViewModel GetPaginationViewModel(string appName, int page, int pageSize)
        {
            var pagingData = String.IsNullOrWhiteSpace(appName)
               ? GetPaginationViewModel(page, pageSize)
               : new PaginationPageSetViewModel(
                   "TableRefresh", "Logs", PagingConstants.NumberOfQuickLinks, page, pageSize, appName);
            return pagingData;
        }

        private IEnumerable<IconLogEntryViewModel> GetAppLogsAndSetRelatedViewData(string appName, int page, int pageSize)
        {
            if (String.IsNullOrWhiteSpace(appName))
            {
                return GetAppLogsAndSetRelatedViewData(page, pageSize);
            }
            var logs = IconDatabaseDataAccess.GetPagedAppLogsByApp(appName, page, pageSize);

            ViewBag.AppName = appName;
            ViewBag.Title = appName + " Log Viewer";
            ViewBag.PaginationPageSetViewModel = GetPaginationViewModel(appName, page, pageSize);

            return logs;
        }

        private IEnumerable<IconLogEntryViewModel> GetAppLogsAndSetRelatedViewData(int page, int pageSize)
        {
            var logs = IconDatabaseDataAccess.GetPagedAppLogs(page, pageSize);
            
            ViewBag.Title = "ICON Dashboard Log Viewer (All Apps)";
            ViewBag.PaginationPageSetViewModel = GetPaginationViewModel(page, pageSize);

            return logs;
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
    }
}