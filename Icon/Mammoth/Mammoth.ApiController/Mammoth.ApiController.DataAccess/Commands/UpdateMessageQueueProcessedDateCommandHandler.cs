using Icon.ApiController.DataAccess.Commands;
using Icon.Common;
using Icon.Common.DataAccess;
using Icon.DbContextFactory;
using Mammoth.Framework;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Mammoth.ApiController.DataAccess.Commands
{
    public class UpdateMessageQueueProcessedDateCommandHandler<T> : ICommandHandler<UpdateMessageQueueProcessedDateCommand<T>> where T : class, IMessageQueue
    {
        private IDbContextFactory<MammothContext> mammothContextFactory;

        public UpdateMessageQueueProcessedDateCommandHandler(IDbContextFactory<MammothContext> mammothContextFactory)
        {
            this.mammothContextFactory = mammothContextFactory;
        }

        public void Execute(UpdateMessageQueueProcessedDateCommand<T> data)
        {
            Guid transactionId = Guid.NewGuid();
            DateTime timestamp = DateTime.Now;

            using (var context = mammothContextFactory.CreateContext())
            {
                if (context.Database.Connection.State == ConnectionState.Closed)
                {
                    context.Database.Connection.Open();
                }
                using (var sqlBulkCopy = new SqlBulkCopy(context.Database.Connection as SqlConnection))
                {
                    sqlBulkCopy.DestinationTableName = "stage.MessageQueue";
                    var table = data.MessagesToUpdate
                        .Select(m => new
                        {
                            m.MessageQueueId,
                            Timestamp = timestamp,
                            TransactionId = transactionId
                        }).ToDataTable();
                    sqlBulkCopy.WriteToServer(table);
                }

                SqlParameter processedDateParameter = new SqlParameter("ProcessedDate", data.ProcessedDate);
                processedDateParameter.DbType = DbType.DateTime2;

                SqlParameter transactionIdParameterForUpdate = new SqlParameter("TransactionId", transactionId);
                transactionIdParameterForUpdate.DbType = DbType.Guid; ;

                string sql = string.Format(@"UPDATE esb.{0}
                           SET ProcessedDate = @ProcessedDate,
                               InProcessBy = NULL
                           WHERE EXISTS
                           (
	                           SELECT 1 
	                           FROM stage.MessageQueue mqs
	                           WHERE esb.{0}.MessageQueueId = mqs.MessageQueueId
		                           AND mqs.TransactionId = @TransactionId
                           )", typeof(T).Name);
                context.Database.ExecuteSqlCommand(sql, processedDateParameter, transactionIdParameterForUpdate);

                SqlParameter transactionIdParameterForDelete = new SqlParameter("TransactionId", transactionId);
                transactionIdParameterForDelete.DbType = DbType.Guid;

                context.Database.ExecuteSqlCommand(
                    "DELETE stage.MessageQueue WHERE TransactionId = @TransactionId", transactionIdParameterForDelete);
            }
        }
    }
}
