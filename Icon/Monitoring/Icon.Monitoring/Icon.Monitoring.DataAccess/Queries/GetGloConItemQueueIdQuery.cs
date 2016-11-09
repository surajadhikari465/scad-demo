using Dapper;
using Icon.Common.DataAccess;
using Icon.Monitoring.Common;
using System.Linq;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetGloConItemQueueIdQuery : IQueryHandler<GetGloConItemQueueIdParameters, int>
    {
        private IDbProvider db;

        public GetGloConItemQueueIdQuery(IDbProvider db)
        {
            this.db = db;
        }

        public int Search(GetGloConItemQueueIdParameters parameters)
        {
            QueueData queueData = new QueueData();
            string sql = string.Format("SELECT TOP 1 QueueID FROM app.EventQueue WHERE ProcessFailedDate IS NULL order by QueueID asc");
            int result = this.db.Connection.Query<int>(sql, transaction: this.db.Transaction).FirstOrDefault();

            return result;
        }
    }
}
