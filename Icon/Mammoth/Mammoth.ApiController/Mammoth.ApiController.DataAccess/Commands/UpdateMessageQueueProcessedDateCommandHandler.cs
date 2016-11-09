using Icon.ApiController.DataAccess.Commands;
using Icon.Common;
using Icon.Common.Context;
using Icon.Common.DataAccess;
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
    public class UpdateMessageQueueProcessedDateCommandHandler<T> : ICommandHandler<UpdateMessageQueueProcessedDateCommand<T>> where T : class, IMessageQueue
    {
        private IRenewableContext<MammothContext> globalContext;

        public UpdateMessageQueueProcessedDateCommandHandler(IRenewableContext<MammothContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public void Execute(UpdateMessageQueueProcessedDateCommand<T> data)
        {
            DateTime timestamp = DateTime.Now;
            if (globalContext.Context.Database.CurrentTransaction != null)
            {
                using (var sqlBulkCopy = new SqlBulkCopy(globalContext.Context.Database.Connection as SqlConnection,
                    SqlBulkCopyOptions.Default,
                    globalContext.Context.Database.CurrentTransaction.UnderlyingTransaction as SqlTransaction))
                {
                    sqlBulkCopy.DestinationTableName = "Staging.esb.MessageQueueStaging";
                    var table = data.MessagesToUpdate
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
                    var table = data.MessagesToUpdate
                        .Select(m => new
                        {
                            m.MessageQueueId,
                            Timestamp = timestamp
                        }).ToDataTable();
                    sqlBulkCopy.WriteToServer(table);
                }
            }

            SqlParameter processedDateParameter = new SqlParameter("ProcessedDate", data.ProcessedDate);
            processedDateParameter.DbType = DbType.DateTime2;

            SqlParameter timestampParameter = new SqlParameter("Timestamp", timestamp);
            timestampParameter.DbType = DbType.DateTime2;

            string sql = string.Format(@"UPDATE esb.{0}
                           SET ProcessedDate = @ProcessedDate,
                               InProcessBy = NULL
                           WHERE EXISTS
                           (
	                           SELECT 1 
	                           FROM Staging.esb.MessageQueueStaging mqs
	                           WHERE esb.{0}.MessageQueueId = mqs.MessageQueueId
		                           AND mqs.Timestamp = @Timestamp
                           )", typeof(T).Name);
            globalContext.Context.Database.ExecuteSqlCommand(sql, processedDateParameter, timestampParameter);

            globalContext.Context.Database.ExecuteSqlCommand(
                "DELETE Staging.esb.MessageQueueStaging WHERE Timestamp = @Timestamp",
                new SqlParameter("Timestamp", timestamp) { DbType = DbType.DateTime2 });
        }
    }
}
