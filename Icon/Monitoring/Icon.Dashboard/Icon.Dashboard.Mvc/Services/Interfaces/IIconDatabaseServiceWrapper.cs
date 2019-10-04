using System.Collections.Generic;
using Icon.Dashboard.Mvc.ViewModels;
using System;
using System.Linq.Expressions;
using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.IconDatabaseAccess;
using Icon.Dashboard.Mvc.Enums;

namespace Icon.Dashboard.Mvc.Services
{
    public interface IIconDatabaseServiceWrapper
    {
        IIconDatabaseService DatabaseService { get; }
        IApp GetApp(string appName);
        IApp GetApp(int appID);
        int GetAppID(string appName);

        List<LoggedAppViewModel> GetApps();

        List<AppLogEntryViewModel> GetPagedAppLogs(
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize,
            LogErrorLevelEnum errorLevel = LogErrorLevelEnum.Error);

        List<AppLogEntryViewModel> GetPagedAppLogsByApp(
            string appName,
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize,
            LogErrorLevelEnum errorLevel = LogErrorLevelEnum.Error);

        List<AppLogEntryViewModel> GetPagedFilteredAppLogs(
           Expression<Func<IAppLog, bool>> filter,
           int page = PagingConstants.DefaultPage,
           int pageSize = PagingConstants.DefaultPageSize,
           QuerySortOrder sortOrder = QuerySortOrder.Unspecified);

        AppLogEntryViewModel GetSingleAppLog(int appLogId);

        AppRecentErrorsReportViewModel GetRecentLogEntriesReportForApp(string appName, TimeSpan timePeriod, LoggingLevel logLevel);

        AppRecentErrorsReportViewModel GetRecentLogEntriesReportForApp(int appID, TimeSpan timePeriod, LoggingLevel logLevel);

        List<AppRecentErrorsReportViewModel> BuildEmptyRecentLogReport();

        List<ApiMessageJobSummaryViewModel> GetPagedApiJobSummaries(
            int page = PagingConstants.DefaultPage, int pageSize = PagingConstants.DefaultPageSize);

        List<ApiMessageJobSummaryViewModel> GetPagedApiJobSummariesByMessageType(string messageType,
            int page = PagingConstants.DefaultPage, int pageSize = PagingConstants.DefaultPageSize);

        ApiMessageJobTimedReportViewModel GetApiJobSummaryReport(string messageType, DateTime startTime, DateTime endTime);

        PendingMessagesViewModel GetPendingMessages();
    }
}