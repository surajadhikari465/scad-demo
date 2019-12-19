using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetFailedProductSelectionGroupMessageQuery : IQueryHandler<GetFailedProductSelectionGroupMessageParameters, List<MessageQueueProductSelectionGroup>>
    {
        private IconContext context;

        public GetFailedProductSelectionGroupMessageQuery(IconContext context)
        {
            this.context = context;
        }

        public List<MessageQueueProductSelectionGroup> Search(GetFailedProductSelectionGroupMessageParameters parameters)
        {
            var failedProductSelectionGroupMessages = context.MessageQueueProductSelectionGroup.Where(mqp => mqp.MessageStatusId == MessageStatusTypes.Failed)
                .ToList();
            return failedProductSelectionGroupMessages;
        }

    }
}
