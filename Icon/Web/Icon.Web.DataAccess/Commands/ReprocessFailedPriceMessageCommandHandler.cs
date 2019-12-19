using Icon.Common.DataAccess;
using Icon.Framework;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class ReprocessFailedPriceMessageCommandHandler : ICommandHandler<ReprocessFailedPriceMessageCommand>
    {
        private IconContext context;

        public ReprocessFailedPriceMessageCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(ReprocessFailedPriceMessageCommand data)
        {
            var failedPriceMessages = context.MessageQueuePrice.Where(mp => data.MessageQueueIds.Contains(mp.MessageQueueId));
            foreach (var failedPriceMessage in failedPriceMessages)
            {
                failedPriceMessage.MessageStatusId = 1;
                failedPriceMessage.MessageHistoryId = null;
                failedPriceMessage.InProcessBy = null;
                failedPriceMessage.ProcessedDate = null;
            }
            context.SaveChanges();
        }
    }
}
