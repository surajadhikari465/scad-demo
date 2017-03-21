using Dapper;
using Icon.Common.DataAccess;
using Icon.Monitoring.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetApiMessageUnprocessedRowCountQuery : IQueryHandler<GetApiMessageUnprocessedRowCountParameters, int>
    {
        private IDbProvider db;

        public GetApiMessageUnprocessedRowCountQuery(IDbProvider db)
        {
            this.db = db;
        }

        public int Search(GetApiMessageUnprocessedRowCountParameters parameters)
        {
            QueueData queueData = new QueueData();
            string sql = string.Format("SELECT count(*) FROM app.MessageQueue{0} (nolock) WHERE processedDate is null",
                                        parameters.MessageQueueType);
            int result = this.db.Connection.Query<int>(sql, transaction: this.db.Transaction).FirstOrDefault();

            return result;
        }
    }
}
