using Icon.Common.DataAccess;
using Icon.Framework;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class ReprocessFailedLocaleMessageCommandHandler : ICommandHandler<ReprocessFailedLocaleMessageCommand>
    {
        private IconContext context;

        public ReprocessFailedLocaleMessageCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(ReprocessFailedLocaleMessageCommand data)
        {
            var failedLocaleMessages = context.MessageQueueLocale.Where(ml => data.MessageQueueIds.Contains(ml.MessageQueueId));
            foreach (var failedLocaleMessage in failedLocaleMessages)
            {
                failedLocaleMessage.MessageStatusId = 1;
                failedLocaleMessage.MessageHistoryId = null;
                failedLocaleMessage.InProcessBy = null;
                failedLocaleMessage.ProcessedDate = null;

            }
            context.SaveChanges();
        }
    }
}
