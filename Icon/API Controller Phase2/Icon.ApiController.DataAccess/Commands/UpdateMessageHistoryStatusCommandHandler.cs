using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using System;
using Icon.ApiController.Common;
using Icon.DbContextFactory;

namespace Icon.ApiController.DataAccess.Commands
{
    public class UpdateMessageHistoryStatusCommandHandler : ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>
    {
        private ILogger<UpdateMessageHistoryStatusCommandHandler> logger;
        private IDbContextFactory<IconContext> iconContextFactory;

        public UpdateMessageHistoryStatusCommandHandler(
            ILogger<UpdateMessageHistoryStatusCommandHandler> logger,
            IDbContextFactory<IconContext> iconContextFactory)
        {
            this.logger = logger;
            this.iconContextFactory = iconContextFactory;
        }

        public void Execute(UpdateMessageHistoryStatusCommand<MessageHistory> data)
        {
            int previousStatusId = 0;

            using (var context = iconContextFactory.CreateContext())
            {
                context.MessageHistory.Attach(data.Message);

                previousStatusId = data.Message.MessageStatusId;

                data.Message.MessageStatusId = data.MessageStatusId;
                data.Message.InProcessBy = null;
                data.Message.ProcessedDate = DateTime.Now;

                context.SaveChanges();
            }

            logger.Info(string.Format("Updated message {0} from MessageStatus: {1} to MessageStatus: {2}.",
                data.Message.MessageHistoryId, previousStatusId, data.MessageStatusId));
        }
    }
}
