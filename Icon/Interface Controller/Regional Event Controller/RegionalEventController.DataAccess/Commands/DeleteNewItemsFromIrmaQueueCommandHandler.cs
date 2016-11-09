using Icon.Logging;
using Irma.Framework;
using RegionalEventController.Common;
using RegionalEventController.DataAccess.Interfaces;
using RegionalEventController.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace RegionalEventController.DataAccess.Commands
{
    public class DeleteNewItemsFromIrmaQueueCommandHandler : IBulkCommandHandler<DeleteNewItemsFromIrmaQueueCommand>
    {
        private ILogger<DeleteNewItemsFromIrmaQueueCommandHandler> logger;
        private IrmaContext context;
        public DeleteNewItemsFromIrmaQueueCommandHandler(ILogger<DeleteNewItemsFromIrmaQueueCommandHandler> logger, IrmaContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public int Execute(DeleteNewItemsFromIrmaQueueCommand command)
        {
            int returnVal = 0;
            if (command.NewIrmaItems == null || command.NewIrmaItems.Count == 0)
            {
                logger.Warn("DeleteNewItemsFromIrmaQueueCommandHandler was called with an empty or null list.  Check the execution path in the calling method for potential errors.");
            }
            else
            {
                List<IrmaNewItem> ProcessedNewItems = command.NewIrmaItems.Select(s => s).Where(s => s.ProcessedByController == true).ToList();

                if (ProcessedNewItems != null && ProcessedNewItems.Count > 0)
                    returnVal= DeleteProcessedQueueEntries(ProcessedNewItems);

                List<IrmaNewItem> FailToBeProcessedNewItems = command.NewIrmaItems.Select(s => s).Where(s => s.ProcessedByController == false).ToList();

                if (FailToBeProcessedNewItems != null && FailToBeProcessedNewItems.Count > 0)
                    returnVal = UpdateProcessedQueueEntries(FailToBeProcessedNewItems);

                int firstQId = command.NewIrmaItems[0].QueueId;
                int lastQId = command.NewIrmaItems[command.NewIrmaItems.Count - 1].QueueId;

                logger.Info(String.Format("Successfully cleaned {0} IconItemChangeQueue entries {1} through {2}.",
                    command.NewIrmaItems.Count, firstQId, lastQId));
            }
            return returnVal;
        }

        private int UpdateProcessedQueueEntries(List<IrmaNewItem> failToBeProcessedEntries)
        {
            SqlParameter queueIDsParameter = new SqlParameter("QIDs", SqlDbType.Structured);
            queueIDsParameter.TypeName = "dbo.IconItemChangeQueueIdType";

            queueIDsParameter.Value = failToBeProcessedEntries.ConvertAll(m => new
            {
                QID = m.QueueId
            }).ToDataTable();

            SqlParameter failedDateParameter = new SqlParameter("FailedDate", SqlDbType.DateTime2);
            failedDateParameter.Value = DateTime.Now;

            string sql = "EXEC dbo.MassUpdateIconItemChangeQueue @QIDs, @FailedDate";

            return context.Database.ExecuteSqlCommand(sql, queueIDsParameter, failedDateParameter);
        }

        private int DeleteProcessedQueueEntries(List<IrmaNewItem> processedEntries)
        {
            SqlParameter queueIDsParameter = new SqlParameter("QIDs", SqlDbType.Structured);
            queueIDsParameter.TypeName = "dbo.IconItemChangeQueueIdType";

            queueIDsParameter.Value = processedEntries.ConvertAll(m => new
            {
               QID  = m.QueueId
            }).ToDataTable();

            SqlParameter failedDateParameter = new SqlParameter("FailedDate", SqlDbType.DateTime2);
            failedDateParameter.Value = DBNull.Value;

            string sql = "EXEC dbo.MassUpdateIconItemChangeQueue @QIDs, @FailedDate";

            return context.Database.ExecuteSqlCommand(sql, queueIDsParameter, failedDateParameter);
        }
    }
}
