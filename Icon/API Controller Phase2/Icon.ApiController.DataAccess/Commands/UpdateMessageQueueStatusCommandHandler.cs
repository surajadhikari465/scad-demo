using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using System;

namespace Icon.ApiController.DataAccess.Commands
{
    public class UpdateMessageQueueStatusCommandHandler<T> : ICommandHandler<UpdateMessageQueueStatusCommand<T>> where T : class, IMessageQueue
    {
        private ILogger<UpdateMessageQueueStatusCommandHandler<T>> logger;
        private IRenewableContext<IconContext> globalContext;

        public UpdateMessageQueueStatusCommandHandler(
            ILogger<UpdateMessageQueueStatusCommandHandler<T>> logger,
            IRenewableContext<IconContext> globalContext)
        {
            this.logger = logger;
            this.globalContext = globalContext;
        }

        public void Execute(UpdateMessageQueueStatusCommand<T> data)
        {
            if (data.QueuedMessages == null || data.QueuedMessages.Count == 0)
            {
                logger.Warn("UpdateMessageQueueStatus was called with a null or empty list.  Check the calling method for errors.");
                return;
            }

            logger.Info(String.Format("Updating MessageStatusId from {0} to {1} for {2} queued message(s).", data.QueuedMessages[0], data.MessageStatusId, data.QueuedMessages.Count));

            foreach (var messageToUpdate in data.QueuedMessages)
            {
                messageToUpdate.MessageStatusId = data.MessageStatusId;

                if (data.ResetInProcessBy)
                {
                    messageToUpdate.InProcessBy = null;
                }
            }

            globalContext.Context.SaveChanges();
        }
    }
}
