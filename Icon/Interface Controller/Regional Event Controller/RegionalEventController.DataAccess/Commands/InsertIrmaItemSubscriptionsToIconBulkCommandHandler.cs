using Icon.Framework;
using Icon.Logging;
using RegionalEventController.Common;
using RegionalEventController.DataAccess.Interfaces;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RegionalEventController.DataAccess.Commands
{
    public class InsertIrmaItemSubscriptionsToIconBulkCommandHandler : IBulkCommandHandler<InsertIrmaItemSubscriptionsToIconBulkCommand>
    {
        private ILogger<InsertIrmaItemSubscriptionsToIconBulkCommandHandler> logger;
        private IconContext context;

        public InsertIrmaItemSubscriptionsToIconBulkCommandHandler(ILogger<InsertIrmaItemSubscriptionsToIconBulkCommandHandler> logger, IconContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public int Execute(InsertIrmaItemSubscriptionsToIconBulkCommand command)
        {
            int returnVal = 0;
            if (command.IrmaNewItemSubscriptions == null || command.IrmaNewItemSubscriptions.Count == 0)
            {
                logger.Warn("InsertIrmaItemSubscriptionsToIconBulkCommand was called with a null or empty list.  Check execution logic in the calling method.");
                return returnVal;
            }

            SqlParameter messagesParameter = new SqlParameter("IRMAItemSubscriptions", SqlDbType.Structured);
            messagesParameter.TypeName = "app.IRMAItemSubscriptionType";

            messagesParameter.Value = command.IrmaNewItemSubscriptions.ConvertAll(s => new
            {
                Regioncode = s.regioncode,
                Identifier = s.identifier
            }).ToDataTable();

            string sql = "EXEC app.InsertIRMAItemSubscriptions @IRMAItemSubscriptions";

            returnVal = context.Database.ExecuteSqlCommand(sql, messagesParameter);

            logger.Info(String.Format("Successfully generated {0} IRMAItemSubscription entries.",
                returnVal.ToString()));

            return returnVal;
        }
    }
}
