using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Linq;
using Icon.ApiController.Common;
using Icon.DbContextFactory;

namespace Icon.ApiController.DataAccess.Commands
{
    public class MarkUnsentMessagesAsInProcessCommandHandler : ICommandHandler<MarkUnsentMessagesAsInProcessCommand>
    {
        private ILogger<MarkUnsentMessagesAsInProcessCommandHandler> logger;
        private IDbContextFactory<IconContext> iconContextFactory;

        public MarkUnsentMessagesAsInProcessCommandHandler(
            ILogger<MarkUnsentMessagesAsInProcessCommandHandler> logger,
            IDbContextFactory<IconContext> iconContextFactory)
        {
            this.logger = logger;
            this.iconContextFactory = iconContextFactory;
        }

        public void Execute(MarkUnsentMessagesAsInProcessCommand data)
        {
            if (data.MiniBulkLimitMessageHistory < 1)
            {
                logger.Error(string.Format("The mini-bulk limit for MessageHistory is {0}.  This will prevent the controller from finding any unsent messages to process.",
                    data.MiniBulkLimitMessageHistory));
                return;
            }

            using (var context = iconContextFactory.CreateContext())
            {
                bool controllerAlreadyHasUnsentMessagesInProcess = context.MessageHistory.Any(mh => mh.InProcessBy == data.Instance);

                if (!controllerAlreadyHasUnsentMessagesInProcess)
                {
                    logger.Info(string.Format("Checking for unsent messages to mark as in process using a batch size of {0}...", data.MiniBulkLimitMessageHistory));
                    context.MarkUnsentMessagesAsInProcess(data.MiniBulkLimitMessageHistory, data.MessageTypeId, data.Instance);
                }
            }
        }
    }
}
