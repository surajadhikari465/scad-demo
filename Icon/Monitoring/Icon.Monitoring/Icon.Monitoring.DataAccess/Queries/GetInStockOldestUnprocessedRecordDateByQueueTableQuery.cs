using Dapper;
using Icon.Common.DataAccess;
using Icon.Monitoring.Common.Enums;
using System;
using System.Linq;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetInStockOldestUnprocessedRecordDateByQueueTableQuery : IQueryByRegionHandler<GetInStockOldestUnprocessedRecordDateByQueueTableParameters, DateTime?>
    {
        private IDbProvider db;
        public IrmaRegions TargetRegion { get; set; }

        public GetInStockOldestUnprocessedRecordDateByQueueTableQuery(IDbProvider db)
        {
            this.db = db;
        }

        public DateTime? Search(GetInStockOldestUnprocessedRecordDateByQueueTableParameters parameters)
        {
            string sql = string.Format("SELECT TOP 1 MessageTimestampUtc FROM {0} (nolock) order by queueId asc",
                                        parameters.QueueTableName);
            DateTime? result = this.db.Connection.Query<DateTime?>(sql, transaction: this.db.Transaction).FirstOrDefault();

            return result;
        }
    }
}
