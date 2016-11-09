using Icon.Framework;
using Icon.Logging;
using RegionalEventController.Common;
using RegionalEventController.DataAccess.Interfaces;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RegionalEventController.DataAccess.Commands
{
    public class InsertEventQueuesToIconBulkCommandHandler : IBulkCommandHandler<InsertEventQueuesToIconBulkCommand>
    {
        private ILogger<InsertEventQueuesToIconBulkCommandHandler> logger;
        private IconContext context;

        public InsertEventQueuesToIconBulkCommandHandler(ILogger<InsertEventQueuesToIconBulkCommandHandler> logger, IconContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public int Execute(InsertEventQueuesToIconBulkCommand command)
        {
            int returnVal = 0;
            if (command.EventQueueEntries == null || command.EventQueueEntries.Count == 0)
            {
                logger.Warn("InsertEventQueuesToIconBulkCommandHandler was called with a null or empty list.  Check execution logic in the calling method.");
                return returnVal;
            }

            SqlParameter messagesParameter = new SqlParameter("EventQueueEntries", SqlDbType.Structured);
            messagesParameter.TypeName = "app.EventQueueType";

            messagesParameter.Value = command.EventQueueEntries.ConvertAll(e => new
            {
                EventId = e.EventId,
                EventMessage = e.EventMessage,
                EventReferenceId = e.EventReferenceId,
                RegionCode = e.RegionCode
            }).ToDataTable();

            string sql = "EXEC app.InsertEventQueueEntries @EventQueueEntries";

            returnVal = context.Database.ExecuteSqlCommand(sql, messagesParameter);

            logger.Info(String.Format("Successfully generated {0} EventQueue entries.",
                returnVal.ToString()));

            return returnVal;
        }
    }
}
