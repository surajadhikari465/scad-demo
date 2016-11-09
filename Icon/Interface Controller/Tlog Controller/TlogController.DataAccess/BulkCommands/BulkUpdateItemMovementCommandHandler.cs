using Icon.Logging;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TlogController.DataAccess.Interfaces;
using TlogController.Common;
using TlogController.DataAccess.Models;

namespace TlogController.DataAccess.BulkCommands
{
    public class BulkUpdateItemMovementCommandHandler : IBulkCommandHandler<BulkUpdateItemMovementCommand>
    {
        private ILogger<BulkUpdateItemMovementCommandHandler> logger;
        private IconContext context;

        public BulkUpdateItemMovementCommandHandler(ILogger<BulkUpdateItemMovementCommandHandler> logger, IconContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public int Execute(BulkUpdateItemMovementCommand command)
        {
            int returnVal = 0;
            if (command.ItemMovementTransactionData == null || command.ItemMovementTransactionData.Count == 0)
            {
                logger.Warn("BulkUpdateItemMovementCommandHandler was called with an empty or null list.  Check the execution path in the calling method for potential errors.");
            }
            else
            {
                List<ItemMovementTransaction> processedItemMovementData = command.ItemMovementTransactionData.Select(s => s).Where(s => s.Processed == true).ToList();

                if (processedItemMovementData != null && processedItemMovementData.Count > 0)
                    returnVal = DeleteProcessedQueueEntries(processedItemMovementData);

                List<ItemMovementTransaction> failToBeProcessedItemMovementData = command.ItemMovementTransactionData.Select(s => s).Where(s => s.Processed == false).ToList();

                if (failToBeProcessedItemMovementData != null && failToBeProcessedItemMovementData.Count > 0)
                    returnVal = UpdateProcessedQueueEntries(failToBeProcessedItemMovementData);

                string firstMsgId = command.ItemMovementTransactionData[0].ESBMessageID;
                string lastMsgId = command.ItemMovementTransactionData[command.ItemMovementTransactionData.Count - 1].ESBMessageID;

                logger.Info(String.Format("Successfully cleaned {0} Icon ItemMovement transaction messages {1} through {2}.",
                    command.ItemMovementTransactionData.Count, firstMsgId, lastMsgId));
            }
            return returnVal;
        }

        private int UpdateProcessedQueueEntries(List<ItemMovementTransaction> failToBeProcessedEntries)
        {
            SqlParameter queueIDsParameter = new SqlParameter("ESBMessageIds", SqlDbType.Structured);
            queueIDsParameter.TypeName = "app.ESBMessageIdType";

            queueIDsParameter.Value = failToBeProcessedEntries.ConvertAll(m => new
            {
                ESBMessageIds = m.ESBMessageID
            }).ToDataTable();

            SqlParameter failedDateParameter = new SqlParameter("FailedDate", SqlDbType.DateTime2);
            failedDateParameter.Value = DateTime.Now;

            string sql = "EXEC app.MassUpdateItemMovement @ESBMessageIds, @FailedDate";

            return context.Database.ExecuteSqlCommand(sql, queueIDsParameter, failedDateParameter);
        }

        private int DeleteProcessedQueueEntries(List<ItemMovementTransaction> processedEntries)
        {
            SqlParameter queueIDsParameter = new SqlParameter("ESBMessageIds", SqlDbType.Structured);
            queueIDsParameter.TypeName = "app.ESBMessageIdType";

            queueIDsParameter.Value = processedEntries.ConvertAll(m => new
            {
                ESBMessageIds = m.ESBMessageID
            }).ToDataTable();

            SqlParameter failedDateParameter = new SqlParameter("FailedDate", SqlDbType.DateTime2);
            failedDateParameter.Value = DBNull.Value;

            string sql = "EXEC app.MassUpdateItemMovement @ESBMessageIds, @FailedDate";

            return context.Database.ExecuteSqlCommand(sql, queueIDsParameter, failedDateParameter);
        }
    }
}
