using Icon.Monitoring.Common;
using Icon.Monitoring.Common.Enums;
using System.Linq;
using Dapper;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetMammothItemLocaleChangeQueueIdQuery : IQueryByRegionHandler<GetMammothItemLocaleChangeQueueIdQueryParameters, int>
    {
        private IDbProvider db;
        public IrmaRegions TargetRegion { get; set; }

        public GetMammothItemLocaleChangeQueueIdQuery(IDbProvider db)
        {
            this.db = db;
        }

        public int Search(GetMammothItemLocaleChangeQueueIdQueryParameters parameters)
        {
            QueueData queueData = new QueueData();
            string sql = string.Format("SELECT TOP 1 QueueID FROM mammoth.ItemLocaleChangeQueue WHERE ProcessFailedDate IS NULL ORDER BY QueueID ASC",
                parameters.EmptyParameter);
            int result = this.db.Connection.Query<int>(sql, transaction: this.db.Transaction).FirstOrDefault();

            return result;
        }
    }
}
