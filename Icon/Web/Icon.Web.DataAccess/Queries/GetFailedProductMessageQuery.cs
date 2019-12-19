using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetFailedProductMessageQuery : IQueryHandler<GetFailedProductMessageParameters, List<MessageQueueProduct>>
    {
        private IconContext context;

        public GetFailedProductMessageQuery(IconContext context)
        {
            this.context = context;
        }

        public List<MessageQueueProduct> Search(GetFailedProductMessageParameters parameters)
        {

            var failedProductMessages = context.MessageQueueProduct.Where(mqp => mqp.MessageStatusId == MessageStatusTypes.Failed)
                .ToList();
            return failedProductMessages;
        }
    }
}
