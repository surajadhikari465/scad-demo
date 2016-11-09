using Icon.Dashboard.CommonDatabaseAccess;
using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.IconDatabaseAccess.Repositories
{
    public class IconApiJobMonitoringRepository : IApiJobMonitoringRepository
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

        public IconApiJobMonitoringRepository(IAmbientDbContextLocator ambientDbContextLocator)
        {
            if (ambientDbContextLocator == null) throw new ArgumentNullException(nameof(ambientDbContextLocator));
            _ambientDbContextLocator = ambientDbContextLocator;
        }

        public IEnumerable<IAPIMessageMonitorLog> GetPagedApiJobSummaries(
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize)
        {
            return GetPagedApiJobSummariesWithFilter(js => js != null, page, pageSize);
        }

        public IEnumerable<IAPIMessageMonitorLog> GetPagedApiJobSummariesByMessageType(
            string messageType,
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize)
        {
            return GetPagedApiJobSummariesWithFilterForEntity(js => js.MessageType.MessageTypeName == messageType, DefaultOrderByForEntity, page, pageSize);
        }

        public IEnumerable<IAPIMessageMonitorLog> GetPagedApiJobSummariesWithFilter(
            Expression<Func<IAPIMessageMonitorLog, bool>> filter,
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize,
            QuerySortOrder sortOrder = QuerySortOrder.Unspecified)
        {
            return GetPagedApiJobSummariesWithFilter(filter, DefaultOrderBy, page, pageSize, sortOrder);
        }

        public IEnumerable<IAPIMessageMonitorLog> GetPagedApiJobSummariesWithFilter(
            Expression<Func<IAPIMessageMonitorLog, bool>> filter,
            Expression<Func<IAPIMessageMonitorLog, int>> orderBy,
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize,
            QuerySortOrder sortOrder = QuerySortOrder.Unspecified)
        {
            var convertedFilter = ExpressionConvertAPIMessageMonitorLog<APIMessageMonitorLog, IAPIMessageMonitorLog, bool>(filter);
            var convertedOrderBy = ExpressionConvertAPIMessageMonitorLog<APIMessageMonitorLog, IAPIMessageMonitorLog, int>(orderBy);
            return GetPagedApiJobSummariesWithFilterForEntity(convertedFilter, convertedOrderBy, page, pageSize, sortOrder);
        }

        public IApiJobSummary GetApiJobSummaryReport(string messageType, DateTime startTime, DateTime endTime)
        {
            switch (messageType)
            {
                case IconApiControllerMessageType.Locale:
                case IconApiControllerMessageType.Hierarchy:
                case IconApiControllerMessageType.ItemLocale:
                case IconApiControllerMessageType.Price:
                case IconApiControllerMessageType.DepartmentSale:
                case IconApiControllerMessageType.Product:
                case IconApiControllerMessageType.CCHTaxUpdate:
                case IconApiControllerMessageType.ProductSelectionGroup:
                case IconApiControllerMessageType.eWIC:
                case IconApiControllerMessageType.InforNewItem:
                    break;
                case IconApiControllerMessageType.Undefined:
                case IconApiControllerMessageType.NotUsed:
                default:
                    return null;
            }

            var query = DbContext.APIMessageMonitorLogs
                        .Where(l => l.MessageType.MessageTypeName == messageType && l.EndTime >= startTime && l.EndTime <= endTime)
                        .Select(l => new
                            {
                                Succeeded = l.CountProcessedMessages ?? 0,
                                Failed = l.CountFailedMessages ?? 0
                            });

            var succeededCount = query.Sum(x => (long?) x.Succeeded);
            var failedCount = query.Sum(x => (long?) x.Failed);

            ApiJobSummaryReport report = new ApiJobSummaryReport(messageType, startTime, endTime);
            report.CountProcessedMessages = succeededCount.GetValueOrDefault();
            report.CountFailedMessages = failedCount.GetValueOrDefault();

            return report;
        }

        public Dictionary<string, int> GetPendingMessageCountByMessageType()
        {
            var pendingMessages = new Dictionary<string, int>
            {
                { IconApiControllerMessageType.Locale, 0 },
                { IconApiControllerMessageType.Hierarchy, 0 },
                { IconApiControllerMessageType.ItemLocale, 0 },
                { IconApiControllerMessageType.Price, 0 },
                { IconApiControllerMessageType.Product, 0 },
                { IconApiControllerMessageType.ProductSelectionGroup, 0 },
            };

            pendingMessages[IconApiControllerMessageType.Locale] = this.DbContext.MessageQueueLocales.Where(q => q.ProcessedDate == null).Count();
            pendingMessages[IconApiControllerMessageType.Hierarchy] = this.DbContext.MessageQueueHierarchies.Where(q => q.ProcessedDate == null).Count();
            pendingMessages[IconApiControllerMessageType.ItemLocale] = this.DbContext.MessageQueueItemLocales.Where(q => q.ProcessedDate == null).Count();
            pendingMessages[IconApiControllerMessageType.Price] = this.DbContext.MessageQueuePrices.Where(q => q.ProcessedDate == null).Count();
            pendingMessages[IconApiControllerMessageType.Product] = this.DbContext.MessageQueueProducts.Where(q => q.ProcessedDate == null).Count();
            pendingMessages[IconApiControllerMessageType.ProductSelectionGroup] = this.DbContext.MessageQueueProductSelectionGroups.Where(q => q.ProcessedDate == null).Count();

            return pendingMessages;
        }

        #region private 

        private Expression<Func<APIMessageMonitorLog, int>> DefaultOrderByForEntity
        {
            get
            {
                return ExpressionConvertAPIMessageMonitorLog<APIMessageMonitorLog, IAPIMessageMonitorLog, int>(DefaultOrderBy);
            }
        }

        private Expression<Func<IAPIMessageMonitorLog, int>> DefaultOrderBy
        {
            get
            {
                return (x => x.APIMessageMonitorLogID);
            }
        }

        private static Expression<Func<AppLog, T>> ExpressionConvertAppLog<AppLog, IAppLog, T>(Expression<Func<IAppLog, T>> expression)
        {
            // Add the boxing operation, but get a weakly typed expression
            Expression converted = Expression.Convert(expression.Body, typeof(AppLog));
            // Use Expression.Lambda to get back to strong typing
            return Expression.Lambda<Func<AppLog, T>>(converted, expression.Parameters);
        }

        private static Expression<Func<APIMessageMonitorLog, T>> ExpressionConvertAPIMessageMonitorLog<APIMessageMonitorLog, IAPIMessageMonitorLog, T>(Expression<Func<IAPIMessageMonitorLog, T>> expression)
        {
            // Add the boxing operation, but get a weakly typed expression
            Expression converted = Expression.Convert(expression.Body, typeof(T));
            // Use Expression.Lambda to get back to strong typing
            return Expression.Lambda<Func<APIMessageMonitorLog, T>>(converted, expression.Parameters);
        }

        private IEnumerable<APIMessageMonitorLog> GetPagedApiJobSummariesWithFilterForEntity(
           Expression<Func<APIMessageMonitorLog, bool>> filter,
           Expression<Func<APIMessageMonitorLog, int>> orderBy,
           int page = PagingConstants.DefaultPage,
           int pageSize = PagingConstants.DefaultPageSize,
           QuerySortOrder sortOrder = QuerySortOrder.Unspecified)
        {
            switch (sortOrder)
            {
                case QuerySortOrder.Ascending:
                    return DbContext.APIMessageMonitorLogs
                        .Include(m => m.MessageType)
                        .Where(filter)
                        .OrderBy(orderBy)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
                case QuerySortOrder.Unspecified:
                case QuerySortOrder.Descending:
                default:
                    return DbContext.APIMessageMonitorLogs
                        .Include(m => m.MessageType)
                        .Where(filter)
                        .OrderByDescending(orderBy)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
            }
        }
        #endregion
    }
}
