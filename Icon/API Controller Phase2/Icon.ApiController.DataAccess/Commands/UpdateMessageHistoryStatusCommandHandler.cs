using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using System;

namespace Icon.ApiController.DataAccess.Commands
{
    public class UpdateMessageHistoryStatusCommandHandler : ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>
    {
        private ILogger<UpdateMessageHistoryStatusCommandHandler> logger;
        private IRenewableContext<IconContext> globalContext;

        public UpdateMessageHistoryStatusCommandHandler(
            ILogger<UpdateMessageHistoryStatusCommandHandler> logger,
            IRenewableContext<IconContext> globalContext)
        {
            this.logger = logger;
            this.globalContext = globalContext;
        }

        public void Execute(UpdateMessageHistoryStatusCommand<MessageHistory> data)
        {
            int previousStatusId = data.Message.MessageStatusId;

            data.Message.MessageStatusId = data.MessageStatusId;
            data.Message.InProcessBy = null;
            data.Message.ProcessedDate = DateTime.Now;

            globalContext.Context.SaveChanges();

            logger.Info(String.Format("Updated message {0} from MessageStatus: {1} to MessageStatus: {2}.",
                data.Message.MessageHistoryId, previousStatusId, data.MessageStatusId));
        }
    }
}
