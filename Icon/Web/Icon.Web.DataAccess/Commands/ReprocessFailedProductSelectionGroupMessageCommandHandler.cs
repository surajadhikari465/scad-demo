using Icon.Common.DataAccess;
using Icon.Framework;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class ReprocessFailedProductSelectionGroupMessageCommandHandler : ICommandHandler<ReprocessFailedProductSelectionGroupMessageCommand>
    {
        private IconContext context;

        public ReprocessFailedProductSelectionGroupMessageCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(ReprocessFailedProductSelectionGroupMessageCommand data)
        {
            var failedProductSelectionGroupMessages = context.MessageQueueProductSelectionGroup.Where(ip => data.MessageQueueIds.Contains(ip.MessageQueueId));
            foreach (var failedProductSelectionGroupMessage in failedProductSelectionGroupMessages)
            {
                failedProductSelectionGroupMessage.MessageStatusId = 1;
                failedProductSelectionGroupMessage.MessageHistoryId = null;
                failedProductSelectionGroupMessage.InProcessBy = null;
                failedProductSelectionGroupMessage.ProcessedDate = null;
            }
            context.SaveChanges();
        }
    }
    
}
