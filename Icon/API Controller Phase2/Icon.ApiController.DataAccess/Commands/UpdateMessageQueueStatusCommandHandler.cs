using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using System;
using Icon.DbContextFactory;
using System.Data.Entity;

namespace Icon.ApiController.DataAccess.Commands
{
    public class UpdateMessageQueueStatusCommandHandler<T> : ICommandHandler<UpdateMessageQueueStatusCommand<T>> where T : class, IMessageQueue
    {
        private ILogger<UpdateMessageQueueStatusCommandHandler<T>> logger;
        private IDbContextFactory<IconContext> iconContextFactory;

        public UpdateMessageQueueStatusCommandHandler(
            ILogger<UpdateMessageQueueStatusCommandHandler<T>> logger,
            IDbContextFactory<IconContext> iconContextFactory)
        {
            this.logger = logger;
            this.iconContextFactory = iconContextFactory;
        }

        public void Execute(UpdateMessageQueueStatusCommand<T> data)
        {
            using (var context = iconContextFactory.CreateContext())
            {
                if (data.QueuedMessages == null || data.QueuedMessages.Count == 0)
                {
                    logger.Warn("UpdateMessageQueueStatus was called with a null or empty list.  Check the calling method for errors.");
                    return;
                }

                logger.Info(string.Format("Updating MessageStatusId from {0} to {1} for {2} queued message(s).", data.QueuedMessages[0], data.MessageStatusId, data.QueuedMessages.Count));
                
                var set = GetDbSet(context);
                foreach (var messageToUpdate in data.QueuedMessages)
                {
                    set.Attach(messageToUpdate);
                    messageToUpdate.MessageStatusId = data.MessageStatusId;

                    if (data.ResetInProcessBy)
                    {
                        messageToUpdate.InProcessBy = null;
                    }
                }

                context.SaveChanges();
            }
        }

        private DbSet<T> GetDbSet(IconContext context)
        {
            return context.Set<T>();
        }
    }
}
