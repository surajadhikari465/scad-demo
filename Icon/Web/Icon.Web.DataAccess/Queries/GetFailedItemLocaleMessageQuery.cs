using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetFailedItemLocaleMessageQuery : IQueryHandler<GetFailedItemLocaleMessageParameters, List<MessageQueueItemLocale>>
    {
        private IconContext context;

        public GetFailedItemLocaleMessageQuery(IconContext context)
        {
            this.context = context;
        }

        public List<MessageQueueItemLocale> Search(GetFailedItemLocaleMessageParameters parameters)
        {
            var failedItemLocaleMessages = context.MessageQueueItemLocale.Where(mqi => mqi.MessageStatusId == MessageStatusTypes.Failed)
                .ToList();
            return failedItemLocaleMessages;
        }
    }
}
