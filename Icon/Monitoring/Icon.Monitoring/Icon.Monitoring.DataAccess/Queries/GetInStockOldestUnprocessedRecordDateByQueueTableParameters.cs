using Icon.Common.DataAccess;
using System;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetInStockOldestUnprocessedRecordDateByQueueTableParameters : IQuery<DateTime?>
    {
        public string QueueTableName { get; set; }
    }
}
