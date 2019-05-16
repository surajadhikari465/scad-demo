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
    public class IconLogsController : BaseDashboardController
    {
        public IconLogsController() : this(null, null, null) { }

        public IconLogsController(
            IDashboardEnvironmentManager environmentManager = null,
            IIconDatabaseServiceWrapper iconDbService = null,
            IMammothDatabaseServiceWrapper mammothDbService = null)
            : base(environmentManager, iconDbService, mammothDbService) { }

        #region GET
        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Index(string appName = null, int page = 1, int pageSize = PagingConstants.DefaultPageSize, string errorLevel = "Error")
        {
            //enable filter to use the data service
            HttpContext.Items["iconLoggingDataService"] = IconDatabaseService;
            HttpContext.Items["mammothLoggingDataService"] = MammothDatabaseService;

            var currentEnvironment = EnvironmentManager.GetEnvironment(Request.Url.Host);
            ViewBag.Environment = currentEnvironment.Name;

            var viewModel = new IconLogEntryCollectionViewModel();
            Enum.TryParse(errorLevel, out LogErrorLevelEnum errorLevelEnum);
            viewModel.ErrorLevel = errorLevelEnum;
            if (string.IsNullOrWhiteSpace(appName))
            {
                viewModel.LogEntries = IconDatabaseService.GetPagedAppLogs(page, pageSize, errorLevelEnum);
                viewModel.Title = currentEnvironment.Name + " DB ICON Dashboard Log Viewer (All Icon Apps)";
                viewModel.PaginationModel = new PaginationPageSetViewModel( "TableRefresh", "IconLogs", page, pageSize, errorLevelEnum);

            }
            else
            {
                viewModel.LogEntries = IconDatabaseService.GetPagedAppLogsByApp(appName, page, pageSize, errorLevelEnum);
                viewModel.AppName = appName;
                viewModel.Title = currentEnvironment.Name + " DB " + appName + " Log Viewer";
                viewModel.PaginationModel = new PaginationPageSetViewModel( "TableRefresh", "IconLogs", page, pageSize, appName, errorLevelEnum);
            }

            var allApps = IconDatabaseService.GetApps();
            var recentLogEntriesReport = new RecentLogEntriesReportCollectionViewModel();
            recentLogEntriesReport.Reports = IconDatabaseService.GetEmptyLogEntriesReportList(allApps);
            recentLogEntriesReport.HoursConsideredRecent = GetHoursForRecentErrors();
            recentLogEntriesReport.PollingRefreshIntervalMilliseconds = GetMillisecondsForRecentErrorsPolling();
            viewModel.RecentLogEntriesReport = recentLogEntriesReport;

            return View(viewModel);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult RecentErrors(int appID, int hours = 24)
        {
            HttpContext.Items["iconLoggingDataService"] = IconDatabaseService;
            HttpContext.Items["mammothLoggingDataService"] = MammothDatabaseService;
            var currentEnvironment = EnvironmentManager.GetEnvironment(Request.Url.Host);
            ViewBag.Environment = currentEnvironment.Name;
            var recentLogEntryReportForApp = IconDatabaseService.GetRecentLogEntriesReportForApp(
                appID, new TimeSpan(hours, 0, 0), LoggingLevel.Error);
            return PartialView("_RecentLogEntriesReportPartial", recentLogEntryReportForApp);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult RedrawPaging(string appName = null, int page = 1,
            int pageSize = PagingConstants.DefaultPageSize, string errorLevel = "Error")
        {
            HttpContext.Items["iconLoggingDataService"] = IconDatabaseService;
            HttpContext.Items["mammothLoggingDataService"] = MammothDatabaseService;
            var currentEnvironment = EnvironmentManager.GetEnvironment(Request.Url.Host);
            ViewBag.Environment = currentEnvironment.Name;
            Enum.TryParse(errorLevel, out LogErrorLevelEnum errorLevelEnum);
            var pagingData = (string.IsNullOrWhiteSpace(appName))
                ? new PaginationPageSetViewModel("TableRefresh", "IconLogs", page, pageSize, errorLevelEnum)
                : new PaginationPageSetViewModel("TableRefresh", "IconLogs", page, pageSize, appName, errorLevelEnum);
            return PartialView("_PaginationPartial", pagingData);
        }

        [HttpGet]
        [DashboardAuthorization(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult TableRefresh(string appName = null, int page = 1,
            int pageSize = PagingConstants.DefaultPageSize, string errorLevel = "Error")
        {
            HttpContext.Items["iconLoggingDataService"] = IconDatabaseService;
            HttpContext.Items["mammothLoggingDataService"] = MammothDatabaseService;
            var currentEnvironment = EnvironmentManager.GetEnvironment(Request.Url.Host);
            ViewBag.Environment = currentEnvironment.Name;
            Enum.TryParse(errorLevel, out LogErrorLevelEnum errorLevelEnum);

            var viewModel = new IconLogEntryCollectionViewModel();
            if (string.IsNullOrWhiteSpace(appName))
            {
                viewModel.LogEntries = IconDatabaseService.GetPagedAppLogs(page, pageSize, errorLevelEnum);
                viewModel.Title = currentEnvironment.Name + " DB ICON Dashboard Log Viewer (All Icon Apps)";
                viewModel.PaginationModel = new PaginationPageSetViewModel( "TableRefresh", "IconLogs", page, pageSize, errorLevelEnum);
            }
            else
            {
                viewModel.LogEntries = IconDatabaseService.GetPagedAppLogsByApp(appName, page, pageSize, errorLevelEnum);
                viewModel.AppName = appName;
                viewModel.Title = currentEnvironment.Name + " DB " + appName + " Log Viewer";
                viewModel.PaginationModel = new PaginationPageSetViewModel( "TableRefresh", "IconLogs", page, pageSize, appName, errorLevelEnum);
            }
            return PartialView("_AppLogTablePartial", viewModel);
        }
        #endregion

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