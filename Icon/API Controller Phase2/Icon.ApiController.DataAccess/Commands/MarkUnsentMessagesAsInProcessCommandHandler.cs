using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Linq;

namespace Icon.ApiController.DataAccess.Commands
{
    public class MarkUnsentMessagesAsInProcessCommandHandler : ICommandHandler<MarkUnsentMessagesAsInProcessCommand>
    {
        private ILogger<MarkUnsentMessagesAsInProcessCommandHandler> logger;
        private IRenewableContext<IconContext> globalContext;

        public MarkUnsentMessagesAsInProcessCommandHandler(
            ILogger<MarkUnsentMessagesAsInProcessCommandHandler> logger, 
            IRenewableContext<IconContext> globalContext)
        {
            this.logger = logger;
            this.globalContext = globalContext;
        }

        public void Execute(MarkUnsentMessagesAsInProcessCommand data)
        {
            if (data.MiniBulkLimitMessageHistory < 1)
            {
                logger.Error(String.Format("The mini-bulk limit for MessageHistory is {0}.  This will prevent the controller from finding any unsent messages to process.",
                    data.MiniBulkLimitMessageHistory));
                return;
            }

            bool controllerAlreadyHasUnsentMessagesInProcess = globalContext.Context.MessageHistory.Any(mh => mh.InProcessBy == data.Instance);

            if (!controllerAlreadyHasUnsentMessagesInProcess)
            {
                logger.Info(String.Format("Checking for unsent messages to mark as in process using a batch size of {0}...", data.MiniBulkLimitMessageHistory));
                globalContext.Context.MarkUnsentMessagesAsInProcess(data.MiniBulkLimitMessageHistory, data.MessageTypeId, data.Instance);
            }
        }
    }
}
