using Icon.Common.DataAccess;
using Icon.Framework;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class ReprocessFailedHierarchyMessageCommandHandler : ICommandHandler<ReprocessFailedHierarchyMessageCommand>
    {
        private IconContext context;

        public ReprocessFailedHierarchyMessageCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(ReprocessFailedHierarchyMessageCommand data)
        {
            var failedPriceMessages = context.MessageQueueHierarchy.Where(mh => data.MessageQueueIds.Contains(mh.MessageQueueId));
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
