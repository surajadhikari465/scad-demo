using System.Collections.Generic;
using Icon.Dashboard.Mvc.ViewModels;
using System;
using System.Linq.Expressions;
using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.MammothDatabaseAccess;

namespace Icon.Dashboard.Mvc.Services
{
    public interface IMammothDatabaseServiceWrapper
    {
        IMammothDatabaseService MammothLoggingService { get; }

        IApp GetApp(string appName);
        IApp GetApp(int appID);
        List<IconAppViewModel> GetApps();
        List<IconAppLogViewModel> GetPagedAppLogs(int page = PagingConstants.DefaultPage, int pageSize = PagingConstants.DefaultPageSize);
        List<IconAppLogViewModel> GetPagedAppLogsByApp(string appName, int page = PagingConstants.DefaultPage, int pageSize = PagingConstants.DefaultPageSize);

        List<IconAppLogViewModel> GetPagedFilteredAppLogs(
           Expression<Func<IAppLog, bool>> filter,
           int page = PagingConstants.DefaultPage,
           int pageSize = PagingConstants.DefaultPageSize,
           QuerySortOrder sortOrder = QuerySortOrder.Unspecified);

        IconAppLogViewModel GetSingleAppLog(int appLogId);
        RecentLogEntriesReportViewModel GetRecentLogEntriesReportForApp(string appName, TimeSpan timePeriod, LoggingLevel logLevel);
        RecentLogEntriesReportViewModel GetRecentLogEntriesReportForApp(int appID, TimeSpan timePeriod, LoggingLevel logLevel);
        IEnumerable<RecentLogEntriesReportViewModel> GetEmptyLogEntriesReportList(IEnumerable<IconAppViewModel> iconAppDefinitions);


    }
}