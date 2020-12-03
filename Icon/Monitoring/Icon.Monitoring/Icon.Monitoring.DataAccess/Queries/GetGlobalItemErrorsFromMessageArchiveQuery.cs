using System.CodeDom;
using Dapper;
using Icon.Common.DataAccess;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetGlobalItemErrorsFromMessageArchiveQuery : IQueryHandlerMammoth<GetGlobalItemErrorsFromMessageArchaiveParameters, int>
    {
        private IDbProvider db;
        private const string FailedErrorStatusId = "3";
        private const string GlobalItemMessageTypeId = "8";

        public GetGlobalItemErrorsFromMessageArchiveQuery(IDbProvider db)
        {
            this.db = db;
        }

        public int Search(GetGlobalItemErrorsFromMessageArchaiveParameters parameters)
        {
            string sql = @" SELECT  count(*)
                            FROM    esb.MessageArchive
                            WHERE   MessageStatusID = @errorStatusId
	                                AND MessageTypeID = @globalItemMessageTypeId
	                                AND InsertDateUTC > @lastMonitorDate";
            var result = this.db.Connection.QueryFirst<int>(sql,
                new { errorStatusId = FailedErrorStatusId,
                    globalItemMessageTypeId= GlobalItemMessageTypeId,
                    lastMonitorDate = parameters.LastMonitorDate
                }, transaction: this.db.Transaction);
            return result;
        }
    }
}