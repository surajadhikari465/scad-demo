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
            string sql = string.Format("SELECT COUNT(*) FROM app.MessageQueue{0} (nolock) WHERE RegionCode ='{1}'  and processedDate IS NULL",
                                        parameters.MessageQueueType, parameters.RegionCode);
            int result = this.db.Connection.Query<int>(sql, transaction: this.db.Transaction).FirstOrDefault();

            return result;
        }
    }
}
