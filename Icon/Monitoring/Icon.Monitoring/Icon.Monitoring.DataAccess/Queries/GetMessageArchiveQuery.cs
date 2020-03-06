using Dapper;
using Icon.Common.DataAccess;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetMessageArchiveQuery : IQueryHandler<GetMessageArchiveParameters, int>
    {
        private IDbProvider db;

        public GetMessageArchiveQuery(IDbProvider db)
        {
            this.db = db;
        }

        public int Search(GetMessageArchiveParameters parameters)
        {
            string sql = @"select count(*) from esb.MessageArchive WHERE InsertDateUTC > @lastMonitorDate";
            var result = this.db.Connection.QueryFirst<int>(sql,
                new { lastMonitorDate = parameters.LastMonitorDate },
                transaction: this.db.Transaction);
            return result;
        }
    }
}