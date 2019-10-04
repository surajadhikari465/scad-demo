using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.IconDatabaseAccess;
using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Icon.Dashboard.Mvc.Services
{
    public class IconDatabaseServiceWrapper : IIconDatabaseServiceWrapper
    {
        public IIconDatabaseService DatabaseService { get; private set; }

        public IconDatabaseServiceWrapper(IIconDatabaseService dataService = null)
        {
            DatabaseService = dataService ?? new IconDataService();
        }

        public IApp GetApp(string appName)
        {
            if (string.IsNullOrWhiteSpace(appName))
            {
                return null;
            }
            return DatabaseService.GetApp(appName);
        }

        public IApp GetApp(int appID)
        {
            if (appID<=0)
            {
                return null;
            }
            return DatabaseService.GetApp(appID);
        }

        public int GetAppID(string appName)
        {
            if (!string.IsNullOrWhiteSpace(appName))
            {
                var app = GetApp(appName);
                if (app != null)
                {
                    return app.AppID;
                }
            }
            return -1;
        } 

        public List<LoggedAppViewModel> GetApps()
        {
            var apps = DatabaseService.GetApps();
            return (apps == null)
                ? new List<LoggedAppViewModel>()
                : apps.Select(a => new LoggedAppViewModel(a)).ToList();
        }

        public List<AppLogEntryViewModel> GetPagedAppLogsByApp(
            string appName,
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize,
            LogErrorLevelEnum errorLevelEnum = LogErrorLevelEnum.Error)
        {
            if (!string.IsNullOrWhiteSpace(appName))
            {
                switch (errorLevelEnum)
                {
                    case LogErrorLevelEnum.Any:
                        var appLogs = DatabaseService.GetPagedAppLogsByApp(appName, page, pageSize);
                        if (appLogs != null)
                        {
                            return BuildAppLogViewModels(appLogs);
                        }
                        break;
                    case LogErrorLevelEnum.Info:
                    case LogErrorLevelEnum.Warn:
                    case LogErrorLevelEnum.Error:
                    default:
                        var appID = GetAppID(appName);
                        if (appID > 0)
                        {
                            var filter = GenerateAppLogFilter(appID, errorLevelEnum);
                            return GetPagedFilteredAppLogs(filter, page, pageSize, QuerySortOrder.Descending);
                        }
                        break;
                }
            }
            return new List<AppLogEntryViewModel>();
        }

        public List<AppLogEntryViewModel> GetPagedAppLogs(
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize,
            LogErrorLevelEnum errorLevelEnum = LogErrorLevelEnum.Error)
        {
            switch (errorLevelEnum)
            {
                case LogErrorLevelEnum.Any:
                    var appLogs = DatabaseService.GetPagedAppLogs(page, pageSize);
                    if (appLogs != null)
                    {
                        return BuildAppLogViewModels(appLogs);
                    }
                    break;
                case LogErrorLevelEnum.Info:
                case LogErrorLevelEnum.Warn:
                case LogErrorLevelEnum.Error:
                    var filter = GenerateAppLogFilter(errorLevelEnum);
                    return GetPagedFilteredAppLogs(filter, page, pageSize, QuerySortOrder.Descending);
                default:
                    break;
            }
            return new List<AppLogEntryViewModel>();
        } 

        public List<AppLogEntryViewModel> GetPagedFilteredAppLogs(
           Expression<Func<IAppLog, bool>> filter,
           int page = PagingConstants.DefaultPage,
           int pageSize = PagingConstants.DefaultPageSize,
           QuerySortOrder sortOrder = QuerySortOrder.Unspecified)
        {
            var appLogs = DatabaseService.GetPagedAppLogsWithFilter(filter, page, pageSize, sortOrder);
            if (appLogs != null)
            {
                return BuildAppLogViewModels(appLogs);
            }
            return new List<AppLogEntryViewModel>();
        }

        private List<AppLogEntryViewModel> BuildAppLogViewModels(IEnumerable<IAppLog> appLogs)
        {
            var logViewModels = new List<AppLogEntryViewModel>(appLogs.Count());
            foreach (var appLog in appLogs)
            {
                logViewModels.Add(new AppLogEntryViewModel(appLog));
            }
            return logViewModels;
        }

        public AppLogEntryViewModel GetSingleAppLog(int appLogId)
        {
            var appLog = DatabaseService.GetSingleAppLog(appLogId);
            var logViewModel = new AppLogEntryViewModel(appLog);
            return logViewModel;
        }

        public AppRecentErrorsReportViewModel GetRecentLogEntriesReportForApp(string appName, TimeSpan timePeriod, LoggingLevel logLevel)
        {
            var logEntryReport = DatabaseService.GetRecentLogEntriesReport(appName, timePeriod, logLevel);
            return LogEntryReportViewModelFromEntity(logEntryReport);
        }

        public AppRecentErrorsReportViewModel GetRecentLogEntriesReportForApp(int appID, TimeSpan timePeriod, LoggingLevel logLevel)
        {
            var logEntryReport = DatabaseService.GetRecentLogEntriesReport(appID, timePeriod, logLevel);
            return LogEntryReportViewModelFromEntity(logEntryReport);
        }

        internal AppRecentErrorsReportViewModel LogEntryReportViewModelFromEntity(IAppLogSummary logEntryReport)
        {
            var logEntryReportViewModel = new AppRecentErrorsReportViewModel(IconOrMammothEnum.Icon, logEntryReport);
            return logEntryReportViewModel;
        }

        public List<AppRecentErrorsReportViewModel> BuildEmptyRecentLogReport()
        {
            var iconAppDefinitions = GetApps();
            if (iconAppDefinitions != null)
            {
                var emptyLogEntryReports = iconAppDefinitions
                    .Where(appDef => appDef.AppID > 0)
                    .GroupBy(appDef => appDef.AppID)
                    .Select(appDefWithId => new AppRecentErrorsReportViewModel(appDefWithId.First()))
                    .ToList();

                return emptyLogEntryReports;
            }
            return null;
        }

        public List<ApiMessageJobSummaryViewModel> GetPagedApiJobSummaries(
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize)
        {
            var jobSummaries = DatabaseService.GetPagedApiJobSummaries(page, pageSize);
            var jobSummaryViewModels = JobSummaryViewModelsFromEntities(jobSummaries);
            return jobSummaryViewModels;
        }

        public List<ApiMessageJobSummaryViewModel> GetPagedApiJobSummariesByMessageType(
            string messageType,
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize)
        {
            var jobSummaries = DatabaseService.GetPagedApiJobSummariesByMessageType(messageType, page, pageSize);
            var jobSummaryViewModels = JobSummaryViewModelsFromEntities(jobSummaries);
            return jobSummaryViewModels;
        }

        internal List<ApiMessageJobSummaryViewModel> JobSummaryViewModelsFromEntities(IEnumerable<IAPIMessageMonitorLog> jobSummaries)
        {
            var jobSummaryViewModels = new List<ApiMessageJobSummaryViewModel>(jobSummaries.Count());
            foreach (var entity in jobSummaries)
            {
                jobSummaryViewModels.Add(new ApiMessageJobSummaryViewModel(entity));
            }
            return jobSummaryViewModels;
        }

        public ApiMessageJobTimedReportViewModel GetApiJobSummaryReport(string messageType, DateTime startTime, DateTime endTime)
        {
            var jobSummaryReport = DatabaseService.GetApiJobSummaryReport(messageType, startTime, endTime);
            var jobSummaryReportViewModel = JobSummaryReportViewModelFromEntity(jobSummaryReport);
            return jobSummaryReportViewModel;
        }

        internal ApiMessageJobTimedReportViewModel JobSummaryReportViewModelFromEntity(IApiJobSummary jobSummaryReport)
        {
            var jobSummaryReportViewModel = new ApiMessageJobTimedReportViewModel(jobSummaryReport);
            return jobSummaryReportViewModel;
        }

        public PendingMessagesViewModel GetPendingMessages()
        {
            var pendingMessages = DatabaseService.GetPendingMessageCountByMessageType();
            return new PendingMessagesViewModel(pendingMessages);            
        }

        private Expression<Func<IAppLog, bool>> GenerateAppLogFilter(int appID, LogErrorLevelEnum errorLevelEnum)
        {
            return (appLog) =>
                appLog.AppID == appID && appLog.Level == errorLevelEnum.ToString();
        }

        private Expression<Func<IAppLog, bool>> GenerateAppLogFilter(LogErrorLevelEnum errorLevelEnum)
        {
            return (appLog) =>
                appLog.Level == errorLevelEnum.ToString();
        }
    }
}