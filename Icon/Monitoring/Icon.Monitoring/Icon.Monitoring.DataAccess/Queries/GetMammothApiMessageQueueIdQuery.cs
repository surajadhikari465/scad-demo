namespace Icon.Monitoring.DataAccess.Queries
{
    using Dapper;
    using Icon.Monitoring.Common;
    using System.Linq;

    public class GetMammothApiMessageQueueIdQuery : IQueryHandlerMammoth<GetApiMessageQueueIdParameters, int>
    {
        private IDbProvider db;

        public GetMammothApiMessageQueueIdQuery(IDbProvider db)
        {
            this.db = db;
        }

        public int Search(GetApiMessageQueueIdParameters parameters)
        {
            QueueData queueData = new QueueData();
            string sql = string.Format("SELECT TOP 1 MessageQueueId FROM esb.MessageQueue{0} (nolock) WHERE MessageStatusId = 1 ORDER BY MessageQueueId ASC",
                parameters.MessageQueueType);
            int result = this.db.Connection.Query<int>(sql, transaction: this.db.Transaction).FirstOrDefault();

            return result;
        }
    }
}
