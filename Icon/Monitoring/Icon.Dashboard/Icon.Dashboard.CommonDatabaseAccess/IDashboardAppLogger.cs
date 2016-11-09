using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.CommonDatabaseAccess
{
    public interface IDashboardAppLogger
    {
        IAppLoggingRepository LoggingRepository { get; }

        IApp GetApp(string appName);

        IApp GetApp(int appID);

        IEnumerable<IApp> GetApps();

        IEnumerable<IAppLog> GetPagedAppLogs(
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize);

        IEnumerable<IAppLog> GetPagedAppLogsByApp(
            string appName,
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize);

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

        IAppLog GetSingleAppLog(int appLogId);

        IAppLogSummary GetRecentLogEntriesReport(int appID, TimeSpan timePeriod, LoggingLevel loggingLevel = LoggingLevel.Error);

        IAppLogSummary GetRecentLogEntriesReport(string appName, TimeSpan timePeriod, LoggingLevel loggingLevel = LoggingLevel.Error);
    }
}
