using Icon.ApiController.DataAccess.Commands;
using Icon.Common.DataAccess;
using Icon.DbContextFactory;
using Mammoth.Framework;

namespace Mammoth.ApiController.DataAccess.Commands
{
    public class UpdateMessageQueueStatusCommandHandler<T> : ICommandHandler<UpdateMessageQueueStatusCommand<T>> where T : class, IMessageQueue
    {
        private IDbContextFactory<MammothContext> mammothContextFactory;

        public UpdateMessageQueueStatusCommandHandler(IDbContextFactory<MammothContext> mammothContextFactory)
        {
            this.mammothContextFactory = mammothContextFactory;
        }

        public void Execute(UpdateMessageQueueStatusCommand<T> data)
        {
            using (var context = mammothContextFactory.CreateContext())
            {
                var set = context.Set<T>();
                foreach (var messageQueue in data.QueuedMessages)
                {
                    set.Attach(messageQueue);
                    messageQueue.MessageStatusId = data.MessageStatusId;

                    if (data.ResetInProcessBy)
                    {
                        messageQueue.InProcessBy = null;
                    }
                }

                context.SaveChanges();
            }
        }
    }
}
