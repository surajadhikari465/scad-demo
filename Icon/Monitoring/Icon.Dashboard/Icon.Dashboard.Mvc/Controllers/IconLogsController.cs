using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Filters;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.Services;
using Icon.Dashboard.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Icon.Dashboard.Mvc.Controllers
{
    public class IconLogsController : BaseDashboardController
    {
        public IconLogsController() : this(null, null, null, null) { }

        public IconLogsController(
            IDashboardAuthorizer dashboardAuthorizer = null,
            IDashboardDataManager dashboardConfigManager = null,
            IIconDatabaseServiceWrapper iconDbService = null,
            IMammothDatabaseServiceWrapper mammothDbService = null)
            : base(dashboardAuthorizer, dashboardConfigManager, iconDbService, mammothDbService) { }

        private IconOrMammothEnum system = IconOrMammothEnum.Icon;

        [HttpGet]
        [DashboardAuthorizer(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult Index(string appName = null,
            int page = 1,
            int pageSize = PagingConstants.DefaultPageSize,
            string errorLevel = "Error")
        {
            Enum.TryParse(errorLevel, out LogErrorLevelEnum errorLevelEnum);
            
            var appLogReport = new AppLogReportViewModel(
                DashboardDataService.ConfigData.MillisecondsForRecentErrorsPolling,
                base.ControllerName,
                nameof(TableRefresh),
                nameof(RecentErrors),
                nameof(RedrawPaging));
            appLogReport.ErrorLevel = errorLevelEnum;

            if (string.IsNullOrWhiteSpace(appName))
            {
                // no app sepecified, so load logs for all apps
                appLogReport.LogEntries = IconDatabaseService.GetPagedAppLogs(page, pageSize, errorLevelEnum);
                appLogReport.PartialViewTitle = $"Icon {DashboardDataService.ActiveEnvironment.Name} DB Log Viewer (All {system} Apps)";
                appLogReport.PaginationModel = new PaginationPageSetViewModel(nameof(TableRefresh), base.ControllerName, page, pageSize, errorLevelEnum);

            }
            else
            {
                // single app specified, so load logs only for that app
                appLogReport.LogEntries = IconDatabaseService.GetPagedAppLogsByApp(appName, page, pageSize, errorLevelEnum);
                appLogReport.AppName = appName;
                appLogReport.PartialViewTitle = $"{system} {DashboardDataService.ActiveEnvironment.Name} DB \"{appName}\" Log Viewer";
                appLogReport.PaginationModel = new PaginationPageSetViewModel(nameof(TableRefresh), base.ControllerName, page, pageSize, appName, errorLevelEnum);
            }

            appLogReport.RecentErrorsReportCollection = new RecentErrorsReportCollectionViewModel(
                system,
                DashboardDataService.ActiveEnvironment.EnvironmentEnum,
                DashboardDataService.ConfigData.HoursForRecentErrors,
                //DashboardDataService.ConfigData.MillisecondsForRecentErrorsPolling,
                base.ControllerName,
                //nameof(TableRefresh),
                nameof(RecentErrors)
                //nameof(RedrawPaging)
                );
            // create an empty template for the recent errors reports - data will be filled in by client calls 
            appLogReport.RecentErrorsReportCollection.Reports = IconDatabaseService.BuildEmptyRecentLogReport();

            ViewBag.GlobalViewData = base.BuildGlobalViewModel(appName);
            //TODO change in views to use GlobalViewData
            //ViewBag.Environment = ConfigManager.ActiveEnvironmentName;

            return View(appLogReport);
        }

        [HttpGet]
        [DashboardAuthorizer(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult RecentErrors(int appID, int hours = 24)
        {
            var appRecentErrorsReport = IconDatabaseService.GetRecentLogEntriesReportForApp(
                appID, new TimeSpan(hours, 0, 0), LoggingLevel.Error);
            appRecentErrorsReport.LinkForIndividualAppLogger = Url
                .Action(nameof(Index), base.ControllerName, new { appName = appRecentErrorsReport.AppName });

            //ViewBag.GlobalViewData = base.BuildGlobalViewModel(HttpContext.User, appName);
            return PartialView("_RecentErrorsReportPartial", appRecentErrorsReport);
        }

        [HttpGet]
        [DashboardAuthorizer(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult RedrawPaging(string appName = null, int page = 1,
            int pageSize = PagingConstants.DefaultPageSize, string errorLevel = "Error")
        {
            Enum.TryParse(errorLevel, out LogErrorLevelEnum errorLevelEnum);

            var pagingData = (string.IsNullOrWhiteSpace(appName))
                ? new PaginationPageSetViewModel(nameof(TableRefresh), base.ControllerName, page, pageSize, errorLevelEnum)
                : new PaginationPageSetViewModel(nameof(TableRefresh), base.ControllerName, page, pageSize, appName, errorLevelEnum);

            //ViewBag.GlobalViewData = base.BuildGlobalViewModel(HttpContext.User, appName);
            return PartialView("_PaginationPartial", pagingData);
        }

        [HttpGet]
        [DashboardAuthorizer(RequiredRole = UserAuthorizationLevelEnum.ReadOnly)]
        public ActionResult TableRefresh(string appName = null, int page = 1,
            int pageSize = PagingConstants.DefaultPageSize, string errorLevel = "Error")
        {
            Enum.TryParse(errorLevel, out LogErrorLevelEnum errorLevelEnum);

            var appLogReport = new AppLogReportViewModel(
                DashboardDataService.ConfigData.MillisecondsForRecentErrorsPolling,
                base.ControllerName,
                nameof(TableRefresh),
                nameof(RecentErrors),
                nameof(RedrawPaging));
            appLogReport.ErrorLevel = errorLevelEnum;

            if (string.IsNullOrWhiteSpace(appName))
            {
                appLogReport.LogEntries = IconDatabaseService.GetPagedAppLogs(page, pageSize, errorLevelEnum);
                appLogReport.PartialViewTitle = $"{system} {DashboardDataService.ActiveEnvironment.Name} DB Log Viewer (All {system} Apps)";
                appLogReport.PaginationModel = new PaginationPageSetViewModel(nameof(TableRefresh), base.ControllerName, page, pageSize, errorLevelEnum);
            }
            else
            {
                appLogReport.LogEntries = IconDatabaseService.GetPagedAppLogsByApp(appName, page, pageSize, errorLevelEnum);
                appLogReport.AppName = appName;
                appLogReport.PartialViewTitle = $"{system} {DashboardDataService.ActiveEnvironment.Name} DB \"{appName}\" Log Viewer";
                appLogReport.PaginationModel = new PaginationPageSetViewModel(nameof(TableRefresh), base.ControllerName, page, pageSize, appName, errorLevelEnum);
            }
            //ViewBag.GlobalViewData = base.BuildGlobalViewModel(HttpContext.User, appName);
            return PartialView("_AppLogReportTablePartial", appLogReport);
        }
    }
}