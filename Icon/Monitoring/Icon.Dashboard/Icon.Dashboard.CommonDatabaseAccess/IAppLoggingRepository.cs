using Icon.Dashboard.CommonDatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.CommonDatabaseAccess
{
    public interface IAppLoggingRepository
    {
        IApp GetApp(int appID);

        IApp GetApp(string appName);

        IEnumerable<IApp> GetApps();

        IAppLog GetSingleAppLog(int appLogId);

        IEnumerable<IAppLog> GetPagedAppLogs(int page = PagingConstants.DefaultPage, int pageSize = PagingConstants.DefaultPageSize);

        IEnumerable<IAppLog> GetPagedAppLogsByApp(string appName, int page = PagingConstants.DefaultPage, int pageSize = PagingConstants.DefaultPageSize);

        IEnumerable<IAppLog> GetPagedAppLogs(
         Expression<Func<IAppLog, int>> orderBy,
         int page = PagingConstants.DefaultPage,
         int pageSize = PagingConstants.DefaultPageSize,
         QuerySortOrder sortOrder = QuerySortOrder.Unspecified);

        IEnumerable<IAppLog> GetPagedAppLogsByApp(
            string appName,
            Expression<Func<IAppLog, int>> orderBy,
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize,
            QuerySortOrder sortOrder = QuerySortOrder.Unspecified);


        IEnumerable<IAppLog> GetPagedAppLogsWithFilter(
            Expression<Func<IAppLog, bool>> filter,
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize,
            QuerySortOrder sortOrder = QuerySortOrder.Unspecified);

        IAppLogSummary GetRecentLogEntriesForApp(int appID, TimeSpan timePeriod, LoggingLevel logLevel = LoggingLevel.Error);

        IAppLogSummary GetRecentLogEntriesForApp(string appName, TimeSpan timePeriod, LoggingLevel logLevel = LoggingLevel.Error);

        IAppLogSummary GetRecentLogEntriesForApp(IApp app, TimeSpan timePeriod, LoggingLevel logLevel = LoggingLevel.Error);
    }
}
