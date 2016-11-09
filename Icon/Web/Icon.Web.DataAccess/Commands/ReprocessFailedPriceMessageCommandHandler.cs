using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
