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

            string sql;
            switch (parameters.MessageQueueType)
            {
                case MessageQueueTypes.Attribute:
                    sql = $"SELECT TOP 1 MessageQueueAttributeId FROM esb.MessageQueueAttribute (nolock) ORDER BY MessageQueueAttributeId ASC";
                    break;
                case MessageQueueTypes.Item:
                    sql = $"SELECT TOP 1 MessageQueueItemId FROM esb.MessageQueueItem (nolock) ORDER BY MessageQueueItemId ASC";
                    break;
                default:
                    sql = $"SELECT TOP 1 MessageQueueId FROM app.MessageQueue{parameters.MessageQueueType} (nolock) WHERE MessageStatusId = 1 ORDER BY MessageQueueId ASC";
                    break;
            }

            int result = this.db.Connection.Query<int>(sql, transaction: this.db.Transaction).FirstOrDefault();

            return result;
        }
    }
}
