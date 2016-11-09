using Icon.RenewableContext;
using Icon.Framework;
using Icon.Logging;
using MoreLinq;
using PushController.DataAccess.Interfaces;
using System;
using System.Data;
using System.Data.SqlClient;

namespace PushController.DataAccess.Commands
{
    public class DeleteItemSubscriptionCommandHandler : ICommandHandler<DeleteItemSubscriptionCommand>
    {
        private ILogger<DeleteItemSubscriptionCommandHandler> logger;
        private IRenewableContext<IconContext> context;

        public DeleteItemSubscriptionCommandHandler(ILogger<DeleteItemSubscriptionCommandHandler> logger, IRenewableContext<IconContext> context)
        {
            this.logger = logger;
            this.context = context;
        }

        public void Execute(DeleteItemSubscriptionCommand command)
        {
            if (command.Subscriptions == null || command.Subscriptions.Count == 0)
            {
                logger.Warn("DeleteItemSubscriptionCommandHandler was called with a null or empty list.");
                return;
            }

            SqlParameter messagesParameter = new SqlParameter("IrmaSubscriptionList", SqlDbType.Structured);
            messagesParameter.TypeName = "app.IRMAItemSubscriptionType";

            messagesParameter.Value = command.Subscriptions.ConvertAll(m => new
                {
                   Regioncode = m.regioncode,
                   Identifier = m.identifier
                }).ToDataTable();

            string sql = "EXEC app.DeleteIrmaSubscriptions @IrmaSubscriptionList";

            context.Context.Database.ExecuteSqlCommand(sql, messagesParameter);

            logger.Info(String.Format("Successfully removed {0} subscriptions.", command.Subscriptions.Count));
        }
    }
}
