using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
