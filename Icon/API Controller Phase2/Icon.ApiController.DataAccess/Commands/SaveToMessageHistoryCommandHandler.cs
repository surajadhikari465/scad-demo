using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.DbContextFactory;

namespace Icon.ApiController.DataAccess.Commands
{
    public class SaveToMessageHistoryCommandHandler : ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>>
    {
        private ILogger<SaveToMessageHistoryCommandHandler> logger;
        private IDbContextFactory<IconContext> iconContextFactory;

        public SaveToMessageHistoryCommandHandler(
            ILogger<SaveToMessageHistoryCommandHandler> logger,
            IDbContextFactory<IconContext> iconContextFactory)
        {
            this.logger = logger;
            this.iconContextFactory = iconContextFactory;
        }

        public void Execute(SaveToMessageHistoryCommand<MessageHistory> data)
        {
            using (var context = iconContextFactory.CreateContext())
            {
                context.MessageHistory.Add(data.Message);
                context.SaveChanges();
            }
            logger.Info(string.Format("Saved message {0} to the MessageHistory table.", data.Message.MessageHistoryId));
        }
    }
}
