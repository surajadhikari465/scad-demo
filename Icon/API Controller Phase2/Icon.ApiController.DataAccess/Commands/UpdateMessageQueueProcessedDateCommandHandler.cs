using Icon.ApiController.Common;
using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Data;
using System.Data.SqlClient;
using Icon.DbContextFactory;

namespace Icon.ApiController.DataAccess.Commands
{
    public class UpdateMessageQueueProcessedDateCommandHandler<T> : ICommandHandler<UpdateMessageQueueProcessedDateCommand<T>> where T : class, IMessageQueue
    {
        private ILogger<UpdateMessageQueueProcessedDateCommandHandler<T>> logger;
        private IDbContextFactory<IconContext> iconContextFactory;

        public UpdateMessageQueueProcessedDateCommandHandler(
            ILogger<UpdateMessageQueueProcessedDateCommandHandler<T>> logger, 
            IDbContextFactory<IconContext> iconContextFactory)
        {
            this.logger = logger;
            this.iconContextFactory = iconContextFactory;
        }

        public void Execute(UpdateMessageQueueProcessedDateCommand<T> data)
        {
            if (data.MessagesToUpdate == null || data.MessagesToUpdate.Count == 0)
            {
                logger.Warn("UpdateMessageQueueProcessedDateCommandHandler was called with a null or empty list.  Check the calling method for errors.");
                return;
            }

            SqlParameter tableNameParameter = new SqlParameter("MessageQueueTable", SqlDbType.NVarChar);
            tableNameParameter.Value = typeof(T).Name;

            SqlParameter messagesParameter = new SqlParameter("MessagesToUpdate", SqlDbType.Structured);
            messagesParameter.TypeName = "app.MessageQueueType";

            SqlParameter dateParameter = new SqlParameter("ProcessedDate", SqlDbType.DateTime2);
            dateParameter.Value = data.ProcessedDate;
            
            messagesParameter.Value = data.MessagesToUpdate.ConvertAll(m => new
                {
                    MessageQueueId = m.MessageQueueId
                }).ToDataTable();

            string sql = "EXEC app.UpdateMessageQueueProcessedDate @MessageQueueTable, @MessagesToUpdate, @ProcessedDate";

            using (var context = iconContextFactory.CreateContext())
            {
                context.Database.ExecuteSqlCommand(sql, tableNameParameter, messagesParameter, dateParameter);
            }

            logger.Info(string.Format("Successfully updated {0} MessageQueue record(s) with a ProcessedDate of {1}.",
                data.MessagesToUpdate.Count, data.ProcessedDate));
        }
    }
}
