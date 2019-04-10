using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.Mvc.Filters;
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
    public class LogsController : BaseDashboardController
    {
        public LogsController() : this(null, null, null) { }

        public LogsController(
            HttpServerUtilityBase serverUtility = null,
            IIconDatabaseServiceWrapper iconDbService = null,
            IMammothDatabaseServiceWrapper mammothDbService = null)
            : base (serverUtility, iconDbService, mammothDbService) { }


        #region GET
        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.ReadOnly)]
        public ActionResult Index(string id = null, int page = 1, int pageSize = PagingConstants.DefaultPageSize,
            string errorLevel = "Any")
        {
            //enable filter to use the data service
            HttpContext.Items["loggingDataService"] = IconDatabaseService;
            var logs = GetAppLogsAndSetRelatedViewData(id, page, pageSize);
            SetViewBagLogEntryReportList();
            return View(logs);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.ReadOnly)]
        public ActionResult RecentErrors(int appID, int hours = 24)
        {
            HttpContext.Items["loggingDataService"] = IconDatabaseService;
            var recentLogEntryReportForApp = IconDatabaseService.GetRecentLogEntriesReportForApp(
                appID, new TimeSpan(hours, 0, 0), LoggingLevel.Error);
            return PartialView("_RecentLogEntriesReportPartial", recentLogEntryReportForApp);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.ReadOnly)]
        public ActionResult RedrawPaging(string routeParameter = null, int page = 1,
            int pageSize = PagingConstants.DefaultPageSize)
        {
            HttpContext.Items["loggingDataService"] = IconDatabaseService;
            var pagingData = GetPaginationViewModel(routeParameter, page, pageSize);
            return PartialView("_PaginationPartial", pagingData);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserRoleEnum.ReadOnly)]
        public ActionResult TableRefresh(string routeParameter = null, int page = 1,
            int pageSize = PagingConstants.DefaultPageSize, string errorLevel = "Any")
        {
            HttpContext.Items["loggingDataService"] = IconDatabaseService;
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
            var logs = IconDatabaseService.GetPagedAppLogsByApp(appName, page, pageSize);

            ViewBag.AppName = appName;
            ViewBag.Title = appName + " Log Viewer";
            ViewBag.PaginationPageSetViewModel = GetPaginationViewModel(appName, page, pageSize);

            return logs;
        }

        private IEnumerable<IconLogEntryViewModel> GetAppLogsAndSetRelatedViewData(int page, int pageSize)
        {
            var logs = IconDatabaseService.GetPagedAppLogs(page, pageSize);
            
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
            if (null != IconDatabaseService && IconDatabaseService is IIconDatabaseServiceWrapper)
            {
                var allApps = (IconDatabaseService as IIconDatabaseServiceWrapper).GetApps();

                ViewBag.RecentLogEntriesReportList = IconDatabaseService.GetEmptyLogEntriesReportList(allApps);
                ViewBag.RecentLogEntriesHours = GetHoursForRecentErrors();
                ViewBag.MillisecondsForRecentErrorsPolling = GetMillisecondsForRecentErrorsPolling();
            }
        }
    }
}