using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.IconDatabaseAccess.Repositories;
using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.IconDatabaseAccess
{
    public class IconDataService : IIconDatabaseService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;

        public IAppLoggingRepository LoggingRepository { get; private set; }

        public IApiJobMonitoringRepository ApiJobMonitoringRepository { get; private set; }

        public IconDataService()
        {
            // build our dependencies by hand for now - DI to come
            _dbContextScopeFactory = new DbContextScopeFactory();
            var ambientDbContextLocator = new AmbientDbContextLocator();
            LoggingRepository = new IconAppLogRepository(ambientDbContextLocator);
            ApiJobMonitoringRepository = new IconApiJobMonitoringRepository(ambientDbContextLocator);
        }

        public IconDataService(IDbContextScopeFactory dbContextScopeFactory, IAppLoggingRepository loggingRepository, IApiJobMonitoringRepository apiJobMonitoringRepository)
        {
            if (dbContextScopeFactory == null) throw new ArgumentNullException(nameof(dbContextScopeFactory));
            if (loggingRepository == null) throw new ArgumentNullException(nameof(loggingRepository));
            if (apiJobMonitoringRepository == null) throw new ArgumentNullException(nameof(apiJobMonitoringRepository));

            _dbContextScopeFactory = dbContextScopeFactory;
            LoggingRepository = loggingRepository;
            ApiJobMonitoringRepository = apiJobMonitoringRepository;
        }

        public IApp GetApp(string appName)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                return LoggingRepository.GetApp(appName);
            }
        }

        public IApp GetApp(int appID)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                return LoggingRepository.GetApp(appID);
            }
        }

        public IEnumerable<IApp> GetApps()
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                var apps = LoggingRepository.GetApps();
                return apps;
            }
        }

        public IAppLog GetSingleAppLog(int appLogId)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                var appLog = LoggingRepository.GetSingleAppLog(appLogId);
                return appLog;
            }
        }

        public IEnumerable<IAppLog> GetPagedAppLogs(
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                var pagedData = LoggingRepository.GetPagedAppLogs(page, pageSize);
                return pagedData;
            }
        }

        public IEnumerable<IAppLog> GetPagedAppLogs(
            Expression<Func<IAppLog, int>> orderBy,
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize,
            QuerySortOrder sortOrder = QuerySortOrder.Unspecified)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                var pagedData = LoggingRepository.GetPagedAppLogs(orderBy, page, pageSize, sortOrder);
                return pagedData;
            }
        }

        public IEnumerable<IAppLog> GetPagedAppLogsByApp(
            string appName,
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                var pagedData = LoggingRepository.GetPagedAppLogsByApp(appName, page, pageSize);
                return pagedData;
            }
        }

        public IEnumerable<IAppLog> GetPagedAppLogsByApp(
            string appName,
            Expression<Func<IAppLog, int>> orderBy,
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize,
            QuerySortOrder sortOrder = QuerySortOrder.Unspecified)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                var pagedData = LoggingRepository.GetPagedAppLogsByApp(appName, orderBy, page, pageSize, sortOrder);
                return pagedData;
            }
        }

        public IEnumerable<IAppLog> GetPagedAppLogsWithFilter(
           Expression<Func<IAppLog, bool>> filter,
           int page = PagingConstants.DefaultPage,
           int pageSize = PagingConstants.DefaultPageSize,
           QuerySortOrder sortOrder = QuerySortOrder.Unspecified)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                var pagedData = LoggingRepository.GetPagedAppLogsWithFilter(filter, page, pageSize, sortOrder);
                return pagedData;
            }
        }

        public IAppLogSummary GetRecentLogEntriesReport(int appID, TimeSpan timePeriod, LoggingLevel loggingLevel = LoggingLevel.Error)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                return LoggingRepository.GetRecentLogEntriesForApp(appID, timePeriod, loggingLevel);
            }
        }

        public IAppLogSummary GetRecentLogEntriesReport(string appName, TimeSpan timePeriod, LoggingLevel loggingLevel = LoggingLevel.Error)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                return LoggingRepository.GetRecentLogEntriesForApp(appName, timePeriod, loggingLevel);
            }
        }        

        public IEnumerable<IAPIMessageMonitorLog> GetPagedApiJobSummaries(
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                return ApiJobMonitoringRepository.GetPagedApiJobSummaries(page, pageSize);
            }
        }

        public IEnumerable<IAPIMessageMonitorLog> GetPagedApiJobSummariesByMessageType(
            string messageType,
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                return ApiJobMonitoringRepository.GetPagedApiJobSummariesByMessageType(messageType, page, pageSize);
            }
        }

        public IApiJobSummary GetApiJobSummaryReport(string messageType, DateTime startTime, DateTime endTime)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                return ApiJobMonitoringRepository.GetApiJobSummaryReport(messageType, startTime, endTime);
            }
        }

        public Dictionary<string, int> GetPendingMessageCountByMessageType()
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                return ApiJobMonitoringRepository.GetPendingMessageCountByMessageType();
            }
        }
    }
}
