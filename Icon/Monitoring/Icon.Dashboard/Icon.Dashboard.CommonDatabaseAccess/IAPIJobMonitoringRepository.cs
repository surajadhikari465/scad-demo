using Icon.Dashboard.CommonDatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.CommonDatabaseAccess
{
    public interface IApiJobMonitoringRepository
    {
        IEnumerable<IAPIMessageMonitorLog> GetPagedApiJobSummaries(
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize);

        IEnumerable<IAPIMessageMonitorLog> GetPagedApiJobSummariesByMessageType(
            string messageType,
            int page = PagingConstants.DefaultPage,
            int pageSize = PagingConstants.DefaultPageSize);

        IApiJobSummary GetApiJobSummaryReport(string messageType, DateTime startTime, DateTime endTime);

        Dictionary<string, int> GetPendingMessageCountByMessageType();
    }
}
