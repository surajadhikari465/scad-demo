using Dapper;
using Icon.Common.DataAccess;
using Icon.Monitoring.Common;
using System.Linq;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetApiMessageQueueIdQuery : IQueryHandler<GetApiMessageQueueIdParameters, int>
    {
        private IDbProvider db;

        public GetApiMessageQueueIdQuery(IDbProvider db)
        {
            this.db = db;
        }

        public int Search(GetApiMessageQueueIdParameters parameters)
        {
            QueueData queueData = new QueueData();
            string sql = string.Format("SELECT TOP 1 MessageQueueId FROM app.MessageQueue{0} (nolock) WHERE MessageStatusId = 1 ORDER BY MessageQueueId ASC",
                parameters.MessageQueueType);
            int result = this.db.Connection.Query<int>(sql, transaction: this.db.Transaction).FirstOrDefault();

            return result;
        }
    }
}
