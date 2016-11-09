using Icon.ApiController.Common;
using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Icon.ApiController.DataAccess.Commands
{
    public class AssociateMessageToQueueCommandHandler<T> : ICommandHandler<AssociateMessageToQueueCommand<T, MessageHistory>> where T : class, IMessageQueue
    {
        private ILogger<AssociateMessageToQueueCommandHandler<T>> logger;
        private IRenewableContext<IconContext> globalContext;

        public AssociateMessageToQueueCommandHandler(
            ILogger<AssociateMessageToQueueCommandHandler<T>> logger,
            IRenewableContext<IconContext> globalContext)
        {
            this.logger = logger;
            this.globalContext = globalContext;
        }

        public void Execute(AssociateMessageToQueueCommand<T, MessageHistory> data)
        {
            if (data.QueuedMessages == null || data.QueuedMessages.Count == 0)
            {
                logger.Warn("AssociateMessageToQueueCommandHandler.Execute() was called with a null or empty list.  Check calling method for possible logic errors.");
                return;
            }

            SqlParameter tableNameParameter = new SqlParameter("MessageQueueTable", SqlDbType.NVarChar);
            tableNameParameter.Value = typeof(T).Name;

            SqlParameter messagesParameter = new SqlParameter("MessagesToUpdate", SqlDbType.Structured);
            messagesParameter.TypeName = "app.MessageQueueType";

            SqlParameter messageHistoryParameter = new SqlParameter("MessageHistoryId", SqlDbType.Int);
            messageHistoryParameter.Value = data.MessageHistory.MessageHistoryId;

            SqlParameter messageStatusParameter = new SqlParameter("MessageStatusId", SqlDbType.Int);
            messageStatusParameter.Value = MessageStatusTypes.Associated;

            messagesParameter.Value = data.QueuedMessages.ConvertAll(m => new
                {
                    MessageQueueId = m.MessageQueueId
                }).ToDataTable();

            string sql = "EXEC app.AssociateMessageToQueue @MessageQueueTable, @MessagesToUpdate, @MessageHistoryId, @MessageStatusId";

            globalContext.Context.Database.ExecuteSqlCommand(sql, tableNameParameter, messagesParameter, messageHistoryParameter, messageStatusParameter);

            logger.Info(String.Format("Associated MessageHistoryId {0} to {1} MessageQueue record(s).",
                data.MessageHistory.MessageHistoryId, data.QueuedMessages.Count));
        }
    }
}
