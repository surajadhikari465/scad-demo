using Icon.Common.DataAccess;
using Icon.Framework;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class ReprocessFailedProductMessageCommandHandler : ICommandHandler<ReprocessFailedProductMessageCommand>
    {

        private IconContext context;

        public ReprocessFailedProductMessageCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(ReprocessFailedProductMessageCommand data)
        {
            var failedProductMessages = context.MessageQueueProduct.Where(ip => data.MessageQueueIds.Contains(ip.MessageQueueId));
            foreach (var failedProductMessage in failedProductMessages)
            {
                failedProductMessage.MessageStatusId = 1;
                failedProductMessage.MessageHistoryId = null;
                failedProductMessage.InProcessBy = null;
                failedProductMessage.ProcessedDate = null;
            }
            context.SaveChanges();
        }
    }
}
