using Dapper;
using Icon.Monitoring.DataAccess.Model;
using Icon.Common.DataAccess;
using System.Linq;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetIconDeadLetterQuery : IQueryHandler<GetIconDeadLetterParameters, int>
    {
        private IDbProvider db;

        public GetIconDeadLetterQuery(IDbProvider db)
        {
            this.db = db;
        }

        public int Search(GetIconDeadLetterParameters parameters)
        {
            string sql = @"select count(*) from esb.MessageDeadLetterQueue WHERE InsertDateUTC > @lastMonitorDate";
            var result = this.db.Connection.QueryFirst<int>(sql,
                new { lastMonitorDate = parameters.LastMonitorDate },
                transaction: this.db.Transaction);
            return result;
        }
    }
}
