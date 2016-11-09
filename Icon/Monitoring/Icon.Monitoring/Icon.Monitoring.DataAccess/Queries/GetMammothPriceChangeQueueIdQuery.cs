namespace Icon.Monitoring.DataAccess.Queries
{
    using Common.Enums;
    using Dapper;
    using Icon.Monitoring.Common;
    using System.Linq;

    public class GetMammothPriceChangeQueueIdQuery : IQueryByRegionHandler<GetMammothPriceChangeQueueIdQueryParameters, int>
    {
        private IDbProvider db;
        public IrmaRegions TargetRegion { get; set; }

        public GetMammothPriceChangeQueueIdQuery(IDbProvider db)
        {
            this.db = db;
        }

        public int Search(GetMammothPriceChangeQueueIdQueryParameters parameters)
        {
            QueueData queueData = new QueueData();
            string sql = string.Format("SELECT TOP 1 QueueID FROM mammoth.PriceChangeQueue WHERE ProcessFailedDate IS NULL ORDER BY QueueID ASC",
                parameters.EmptyParameter);
            int result = this.db.Connection.Query<int>(sql, transaction: this.db.Transaction).FirstOrDefault();

            return result;
        }
    }
}
