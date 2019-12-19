using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetFailedLocaleMessageQuery : IQueryHandler<GetFailedLocaleMessageParameters, List<MessageQueueLocale >>
    {
        private IconContext context;

        public GetFailedLocaleMessageQuery(IconContext context)
        {
            this.context = context;
        }
        
        public List<MessageQueueLocale> Search(GetFailedLocaleMessageParameters parameters)
        {
            var failedLocaleMessages = context.MessageQueueLocale.Where(mql => mql.MessageStatusId == MessageStatusTypes.Failed)
                .ToList();
            return failedLocaleMessages;

        }
    }
}
