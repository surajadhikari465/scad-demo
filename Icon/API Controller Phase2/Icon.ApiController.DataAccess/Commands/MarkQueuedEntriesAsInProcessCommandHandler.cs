using Icon.ApiController.Common;
using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Linq;

namespace Icon.ApiController.DataAccess.Commands
{
    public class MarkQueuedEntriesAsInProcessCommandHandler<T> : ICommandHandler<MarkQueuedEntriesAsInProcessCommand<T>> where T : class, IMessageQueue
    {
        private ILogger<MarkQueuedEntriesAsInProcessCommandHandler<T>> logger;
        private IRenewableContext<IconContext> globalContext;

        public MarkQueuedEntriesAsInProcessCommandHandler(
            ILogger<MarkQueuedEntriesAsInProcessCommandHandler<T>> logger, 
            IRenewableContext<IconContext> globalContext)
        {
            this.logger = logger;
            this.globalContext = globalContext;
        }

        public void Execute(MarkQueuedEntriesAsInProcessCommand<T> data)
        {
            if (data.LookAhead == default(int))
            {
                logger.Error("The look-ahead value is zero.  This will prevent the controller from finding any queued entries to process.");
                return;
            }

            var messageQueueTable = globalContext.Context.Set<T>();

            int currentMessagesInProcess = messageQueueTable.Count(q => q.InProcessBy == data.Instance);

            if (currentMessagesInProcess < data.LookAhead)
            {
                int newMessagesToMark = data.LookAhead - currentMessagesInProcess;

                if (data.BusinessUnit == default(int))
                {
                    logger.Info(String.Format("Controller {0} has {1} records in process and will attempt to mark {2} additional records.  The look-ahead value is {3}.",
                    ControllerType.Instance.ToString(), currentMessagesInProcess, newMessagesToMark.ToString(), data.LookAhead.ToString()));
                }
                else
                {
                    logger.Info(String.Format("Controller {0} has {1} records in process and will attempt to mark {2} additional records for business unit {3}.  The look-ahead value is {4}.",
                    ControllerType.Instance.ToString(), currentMessagesInProcess, newMessagesToMark.ToString(), data.BusinessUnit.ToString(), data.LookAhead.ToString()));
                }

                globalContext.Context.MarkMessageQueueEntriesAsInProcess(typeof(T).Name, newMessagesToMark, data.Instance, data.BusinessUnit);
            }
        }
    }
}
