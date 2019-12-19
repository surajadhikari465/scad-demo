using Icon.Common.DataAccess;
using Icon.Framework;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class ReprocessFailedItemLocaleMessageCommandHandler : ICommandHandler<ReprocessFailedItemLocaleMessageCommand>
    {
        private IconContext context;

        public ReprocessFailedItemLocaleMessageCommandHandler(IconContext uow)
        {
            this.context = uow;
        }

        public void Execute(ReprocessFailedItemLocaleMessageCommand data)
        {
            var failedItemLocaleMessages = context.MessageQueueItemLocale.Where(ip => data.MessageQueueIds.Contains(ip.MessageQueueId));
            foreach (var failedItemLocaleMessage in failedItemLocaleMessages)
            {
                failedItemLocaleMessage.MessageStatusId = 1;
                failedItemLocaleMessage.MessageHistoryId = null;
                failedItemLocaleMessage.InProcessBy = null;
                failedItemLocaleMessage.ProcessedDate = null;
            }
            context.SaveChanges();
        }
    }
}
