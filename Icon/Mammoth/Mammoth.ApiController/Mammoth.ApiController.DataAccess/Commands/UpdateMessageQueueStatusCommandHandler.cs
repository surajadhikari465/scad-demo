using Icon.ApiController.DataAccess.Commands;
using Icon.Common.Context;
using Icon.Common.DataAccess;
using Mammoth.Framework;

namespace Mammoth.ApiController.DataAccess.Commands
{
    public class UpdateMessageQueueStatusCommandHandler<T> : ICommandHandler<UpdateMessageQueueStatusCommand<T>> where T : class, IMessageQueue
    {
        private IRenewableContext<MammothContext> globalContext;

        public UpdateMessageQueueStatusCommandHandler(IRenewableContext<MammothContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public void Execute(UpdateMessageQueueStatusCommand<T> data)
        {
            foreach (var messageQueue in data.QueuedMessages)
            {
                messageQueue.MessageStatusId = data.MessageStatusId;

                if (data.ResetInProcessBy)
                {
                    messageQueue.InProcessBy = null;
                }
            }

            globalContext.Context.SaveChanges();
        }
    }
}
