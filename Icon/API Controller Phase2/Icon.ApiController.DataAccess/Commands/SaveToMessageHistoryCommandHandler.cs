using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using System;

namespace Icon.ApiController.DataAccess.Commands
{
    public class SaveToMessageHistoryCommandHandler : ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>>
    {
        private ILogger<SaveToMessageHistoryCommandHandler> logger;
        private IRenewableContext<IconContext> globalContext;

        public SaveToMessageHistoryCommandHandler(
            ILogger<SaveToMessageHistoryCommandHandler> logger,
            IRenewableContext<IconContext> globalContext)
        {
            this.logger = logger;
            this.globalContext = globalContext;
        }

        public void Execute(SaveToMessageHistoryCommand<MessageHistory> data)
        {
            globalContext.Context.MessageHistory.Add(data.Message);
            globalContext.Context.SaveChanges();

            logger.Info(String.Format("Saved message {0} to the MessageHistory table.", data.Message.MessageHistoryId));
        }
    }
}
