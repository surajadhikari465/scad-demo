using Icon.ApiController.DataAccess.Commands;
using Icon.Common;
using Icon.Common.Context;
using Icon.Common.DataAccess;
using Mammoth.Common.DataAccess;
using Mammoth.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.ApiController.DataAccess.Commands
{
    public class AssociateMessageToQueueCommandHandler<T> : ICommandHandler<AssociateMessageToQueueCommand<T, MessageHistory>> where T : class, IMessageQueue
    {
        private IRenewableContext<MammothContext> globalContext;

        public AssociateMessageToQueueCommandHandler(IRenewableContext<MammothContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public void Execute(AssociateMessageToQueueCommand<T, MessageHistory> data)
        {
            DateTime timestamp = DateTime.Now;
            if (globalContext.Context.Database.CurrentTransaction != null)
            {
                using (var sqlBulkCopy = new SqlBulkCopy(globalContext.Context.Database.Connection as SqlConnection,
                    SqlBulkCopyOptions.Default,
                    globalContext.Context.Database.CurrentTransaction.UnderlyingTransaction as SqlTransaction))
                {
                    sqlBulkCopy.DestinationTableName = "Staging.esb.MessageQueueStaging";
                    var table = data.QueuedMessages
                        .Select(m => new
                        {
                            m.MessageQueueId,
                            Timestamp = timestamp
                        }).ToDataTable();
                    sqlBulkCopy.WriteToServer(table);
                }
            }
            else
            {
                using (var sqlBulkCopy = new SqlBulkCopy(globalContext.Context.Database.Connection as SqlConnection))
                {
                    sqlBulkCopy.DestinationTableName = "Staging.esb.MessageQueueStaging";
                    var table = data.QueuedMessages
                        .Select(m => new
                        {
                            m.MessageQueueId,
                            Timestamp = timestamp
                        }).ToDataTable();
                    sqlBulkCopy.WriteToServer(table);
                }
            }

            SqlParameter messageHistoryIdParameter = new SqlParameter("MessageHistoryId", data.MessageHistory.MessageHistoryId);
            messageHistoryIdParameter.DbType = DbType.Int32;

            SqlParameter messageStatusIdParameter = new SqlParameter("MessageStatusId", MessageStatusTypes.Associated);
            messageStatusIdParameter.DbType = DbType.Int32;

            SqlParameter timestampParameter = new SqlParameter("Timestamp", timestamp);
            timestampParameter.DbType = DbType.DateTime2;

            string sql = string.Format(@"UPDATE esb.{0}
                           SET MessageHistoryId = @MessageHistoryId,
                               MessageStatusId = @MessageStatusId
                           WHERE EXISTS
                           (
	                           SELECT 1 
	                           FROM Staging.esb.MessageQueueStaging mqs
	                           WHERE esb.{0}.MessageQueueId = mqs.MessageQueueId
		                           AND mqs.Timestamp = @Timestamp
                           )", typeof(T).Name);
            globalContext.Context.Database.ExecuteSqlCommand(sql, messageHistoryIdParameter, messageStatusIdParameter, timestampParameter);

            globalContext.Context.Database.ExecuteSqlCommand(
                "DELETE Staging.esb.MessageQueueStaging WHERE Timestamp = @Timestamp",
                new SqlParameter("Timestamp", timestamp) { DbType = DbType.DateTime2 });
        }
    }
}
