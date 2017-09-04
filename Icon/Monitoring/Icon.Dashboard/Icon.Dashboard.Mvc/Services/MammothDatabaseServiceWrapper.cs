using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.MammothDatabaseAccess;
using Icon.Dashboard.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Icon.Dashboard.Mvc.Services
{
    public class MammothDatabaseServiceWrapper : IMammothDatabaseServiceWrapper
    {
        public IMammothDatabaseService MammothLoggingService { get; private set; }

        public MammothDatabaseServiceWrapper(IMammothDatabaseService dataService = null)
        {
            MammothLoggingService = dataService ?? new MammothDataService();
        }

        public IApp GetApp(string appName)
        {
            return MammothLoggingService.GetApp(appName);
        }

        public IApp GetApp(int appID)
        {
            return MammothLoggingService.GetApp(appID);
        }

        public List<IconLoggedAppViewModel> GetApps()
        {
            var apps = MammothLoggingService.GetApps();
            var appViewModels = new List<IconLoggedAppViewModel>(apps.Count());
            foreach (var app in apps)
            {
                appViewModels.Add(new IconLoggedAppViewModel(app));
            }
            return appViewModels;
        }

        public List<IconLogEntryViewModel> GetPagedAppLogsByApp(string appName, int page = PagingConstants.DefaultPage, int pageSize = PagingConstants.DefaultPageSize)
        {
            var appLogs = MammothLoggingService.GetPagedAppLogsByApp(appName, page, pageSize);
            var logViewModels = AppLogViewModelsFromEntities(appLogs);
            return logViewModels;
        }

        public List<IconLogEntryViewModel> GetPagedAppLogs(int page = PagingConstants.DefaultPage, int pageSize = PagingConstants.DefaultPageSize)
        {
            var appLogs = MammothLoggingService.GetPagedAppLogs(page, pageSize);
            var logViewModels = AppLogViewModelsFromEntities(appLogs);
            return logViewModels;
        }

        public List<IconLogEntryViewModel> GetPagedFilteredAppLogs(
           Expression<Func<IAppLog, bool>> filter,
           int page = PagingConstants.DefaultPage,
           int pageSize = PagingConstants.DefaultPageSize,
           QuerySortOrder sortOrder = QuerySortOrder.Unspecified)
        {
            var appLogs = MammothLoggingService.GetPagedAppLogsWithFilter(filter, page, pageSize, sortOrder);
            var logViewModels = AppLogViewModelsFromEntities(appLogs);
            return logViewModels;
        }

        private List<IconLogEntryViewModel> AppLogViewModelsFromEntities(IEnumerable<IAppLog> appLogs)
        {
            var logViewModels = new List<IconLogEntryViewModel>(appLogs.Count());
            foreach (var appLog in appLogs)
            {
                logViewModels.Add(new IconLogEntryViewModel(appLog));
            }
            return logViewModels;
        }

        public IconLogEntryViewModel GetSingleAppLog(int appLogId)
        {
            var appLog = MammothLoggingService.GetSingleAppLog(appLogId);
            var logViewModel = new IconLogEntryViewModel(appLog);
            return logViewModel;
        }

        public RecentLogEntriesReportViewModel GetRecentLogEntriesReportForApp(string appName, TimeSpan timePeriod, LoggingLevel logLevel)
        {
            var logEntryReport = MammothLoggingService.GetRecentLogEntriesReport(appName, timePeriod, logLevel);
            return LogEntryReportViewModelFromEntity(logEntryReport);
        }

        public RecentLogEntriesReportViewModel GetRecentLogEntriesReportForApp(int appID, TimeSpan timePeriod, LoggingLevel logLevel)
        {
            var logEntryReport = MammothLoggingService.GetRecentLogEntriesReport(appID, timePeriod, logLevel);
            return LogEntryReportViewModelFromEntity(logEntryReport);
        }

        public RecentLogEntriesReportViewModel LogEntryReportViewModelFromEntity(IAppLogSummary logEntryReport)
        {
            var logEntryReportViewModel = new RecentLogEntriesReportViewModel(logEntryReport);
            return logEntryReportViewModel;
        }

        public IEnumerable<RecentLogEntriesReportViewModel> GetEmptyLogEntriesReportList(IEnumerable<IconLoggedAppViewModel> iconAppDefinitions)
        {
            if (iconAppDefinitions != null)
            {
                var emptyLogEntryReports = iconAppDefinitions
                    .Where(appDef => appDef.AppID > 0)
                    .GroupBy(appDef => appDef.AppID)
                    .Select(appDefWithId => new RecentLogEntriesReportViewModel(appDefWithId.First()))
                    .ToList();

                return emptyLogEntryReports;
            }
            return null;
        }
    }
}