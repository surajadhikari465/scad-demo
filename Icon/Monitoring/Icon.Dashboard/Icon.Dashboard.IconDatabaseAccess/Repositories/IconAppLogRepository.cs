using Icon.Dashboard.CommonDatabaseAccess;
using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.IconDatabaseAccess.Repositories
{
    public class IconAppLogRepository : IAppLoggingRepository
    {
        private readonly IAmbientDbContextLocator _ambientDbContextLocator;

        private IconDatabaseContext DbContext
        {
            get
            {
                var dbContext = _ambientDbContextLocator.Get<IconDatabaseContext>();

                if (dbContext == null)
                    throw new InvalidOperationException("No ambient DbContext of type UserManagementDbContext found. This means that this repository method has been called outside of the scope of a DbContextScope. A repository must only be accessed within the scope of a DbContextScope, which takes care of creating the DbContext instances that the repositories need and making them available as ambient contexts. This is what ensures that, for any given DbContext-derived type, the same instance is used throughout the duration of a business transaction. To fix this issue, use IDbContextScopeFactory in your top-level business logic service method to create a DbContextScope that wraps the entire business transaction that your service method implements. Then access this repository within that scope. Refer to the comments in the IDbContextScope.cs file for more details.");

                return dbContext;
            }
        }

        public IconAppLogRepository(IAmbientDbContextLocator ambientDbContextLocator)
        {
            if (ambientDbContextLocator == null) throw new ArgumentNullException(nameof(ambientDbContextLocator));
            _ambientDbContextLocator = ambientDbContextLocator;
        }

        public IAppLog GetSingleAppLog(int appLogId)
        {
            return DbContext.AppLogs.Find(appLogId);
        }

        public IEnumerable<IAppLog> GetPagedAppLogs(
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize)
        {
            return GetPagedAppLogs(DefaultOrderBy, page, pageSize, QuerySortOrder.Unspecified);
        }

        public IEnumerable<IAppLog> GetPagedAppLogsByApp(string appName,
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize)
        {
            return GetPagedAppLogsByApp(appName, DefaultOrderBy, page, pageSize, QuerySortOrder.Unspecified);
        }

        public IEnumerable<IAppLog> GetPagedAppLogs(
            Expression<Func<IAppLog, int>> orderBy,
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize,
            QuerySortOrder sortOrder = QuerySortOrder.Unspecified)
        {
            var convertedOrderBy = ExpressionConvert<AppLog, IAppLog, int>(orderBy);

            return GetPagedAppLogsWithFilterForEntity(null, convertedOrderBy, page, pageSize, sortOrder);
        }

        public IEnumerable<IAppLog> GetPagedAppLogsByApp(string appName,
            Expression<Func<IAppLog, int>> orderBy,
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize,
            QuerySortOrder sortOrder = QuerySortOrder.Unspecified)
        {
            var app = GetApp(appName);
            if (app != null && !String.IsNullOrWhiteSpace(app.AppName))
            {
                return GetPagedAppLogsWithFilter(
                    l => l.AppID == app.AppID,
                    orderBy,
                    page,
                    pageSize,
                    sortOrder);
            }
            return null;
        }

        public IEnumerable<IAppLog> GetPagedAppLogsWithFilter(
            Expression<Func<IAppLog, bool>> filter,
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize,
            QuerySortOrder sortOrder = QuerySortOrder.Unspecified)
        {
            return GetPagedAppLogsWithFilter(filter, DefaultOrderBy, page, pageSize, sortOrder);
        }

        public IEnumerable<IAppLog> GetPagedAppLogsWithFilter(
            Expression<Func<IAppLog, bool>> filter,
            Expression<Func<IAppLog, int>> orderBy,
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize,
            QuerySortOrder sortOrder = QuerySortOrder.Unspecified)
        {
            var convertedFilter = ExpressionConvert<AppLog, IAppLog, bool>(filter);
            var convertedOrderBy = ExpressionConvert<AppLog, IAppLog, int>(orderBy);

            return GetPagedAppLogsWithFilterForEntity(convertedFilter, convertedOrderBy, page, pageSize, sortOrder);
        }

        public IQueryable<IAppLog> AllAppLogsQueryable()
        {
            return DbContext.AppLogs.AsQueryable();
        }

        public IQueryable<IAppLog> FreeQuery(Func<AppLog, bool> queryFunc)
        {
            return DbContext.AppLogs.Where(queryFunc).AsQueryable();
        }

        public int GetLogErrorsOverPeriod()
        {
            return GetLogErrorsOverPeriod(PagingConstants.DefaultRecentTimeSpan);
        }

        public int GetLogErrorsOverPeriod(TimeSpan timePeriod)
        {
            return DbContext.AppLogs
                .Count(l => String.Compare(l.Level, "Error", _strcmpOption) == 0
                            && (DateTime.Now - l.LogDate).CompareTo(timePeriod) <= 0);
        }

        public int GetLogErrorsOverPeriod(string appName)
        {
            return GetLogErrorsOverPeriod(appName, PagingConstants.DefaultRecentTimeSpan);
        }

        public int GetLogErrorsOverPeriod(string appName, TimeSpan timePeriod)
        {
            return DbContext.AppLogs
                .Join(DbContext.Apps, al => al.AppID, a => a.AppID, (al, a) => new { al.Level, al.LogDate })
                 .Count(anonObj => String.Compare(anonObj.Level, "Error", _strcmpOption) == 0
                            && (DateTime.Now - anonObj.LogDate).CompareTo(timePeriod) <= 0);
        }

        public IEnumerable<IApp> GetApps()
        {
            return DbContext.Apps.ToList();
        }

        public IApp GetApp(string appName)
        {
            return DbContext.Apps.FirstOrDefault(a => a.AppName == appName);
        }

        public IApp GetApp(int appID)
        {
            return DbContext.Apps.Find(appID);
        }

        public IAppLogSummary GetRecentLogEntriesForApp(int appID, TimeSpan timePeriod, LoggingLevel logLevel = LoggingLevel.Error)
        {
            var app = GetApp(appID);
            return GetRecentLogEntriesForApp(app, timePeriod, logLevel);
        }

        public IAppLogSummary GetRecentLogEntriesForApp(string appName, TimeSpan timePeriod, LoggingLevel logLevel = LoggingLevel.Error)
        {
            var app = GetApp(appName);
            return GetRecentLogEntriesForApp(app, timePeriod, logLevel);
        }

        public IAppLogSummary GetRecentLogEntriesForApp(IApp app, TimeSpan timePeriod, LoggingLevel logLevel = LoggingLevel.Error)
        {
            if (app != null)
            {
                var result = new AppLogSummaryQueryResult(app.AppName, app.AppID, timePeriod, logLevel);

                result.LogEntryCount = DbContext.AppLogs
                     .Count(a => a.AppID == app.AppID
                                && a.Level == logLevel.ToString()
                                && SqlFunctions.DateDiff("minute", a.LogDate, DateTime.Now) <= timePeriod.TotalMinutes);

                return result;
            }

            return null;
        }

        #region private
        public Expression<Func<AppLog, int>> DefaultOrderByForEntity
        {
            get
            {
                return ExpressionConvert<AppLog, IAppLog, int>(DefaultOrderBy);
            }
        }

        public Expression<Func<IAppLog, int>> DefaultOrderBy
        {
            get
            {
                return (x => x.AppLogID);
            }
        }

        private static Expression<Func<AppLog, T>> ExpressionConvert<AppLog, IAppLog, T>(Expression<Func<IAppLog, T>> expression)
        {
            // Add the boxing operation, but get a weakly typed expression
            Expression converted = Expression.Convert(expression.Body, typeof(T));
            // Use Expression.Lambda to get back to strong typing
            return Expression.Lambda<Func<AppLog, T>>(converted, expression.Parameters);
        }

        private IEnumerable<AppLog> GetPagedAppLogsWithFilterForEntity(
            Expression<Func<AppLog, bool>> filter,
            Expression<Func<AppLog, int>> orderBy,
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize,
            QuerySortOrder sortOrder = QuerySortOrder.Unspecified)
        {
            if (filter == null)
            {
                switch (sortOrder)
                {
                    case QuerySortOrder.Ascending:
                        return DbContext.AppLogs
                            .OrderBy(orderBy)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Include(l => l.App)
                            .ToList();
                    case QuerySortOrder.Unspecified:
                    case QuerySortOrder.Descending:
                    default:
                        return DbContext.AppLogs
                            .OrderByDescending(orderBy)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Include(l => l.App)
                            .ToList();
                }
            }
            else
            {
                switch (sortOrder)
                {
                    case QuerySortOrder.Ascending:
                        return DbContext.AppLogs
                            .Where(filter)
                            .OrderBy(orderBy)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Include(l => l.App)
                            .ToList();
                    case QuerySortOrder.Unspecified:
                    case QuerySortOrder.Descending:
                    default:
                        return DbContext.AppLogs
                            .Where(filter)
                            .OrderByDescending(orderBy)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .Include(l => l.App)
                            .ToList();
                }
            }
        }

        private const StringComparison _strcmpOption = StringComparison.CurrentCultureIgnoreCase;
        #endregion
    }
}
