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
    public class GetFailedHierarchyMessageQuery : IQueryHandler<GetFailedHierarchyMessageParameters, List<MessageQueueHierarchy>>
    {
        private IconContext context;

        public GetFailedHierarchyMessageQuery(IconContext context)
        {
            this.context = context;
        }

        public List<MessageQueueHierarchy> Search(GetFailedHierarchyMessageParameters parameters)
        {
            var failedHierarchyMessages = context.MessageQueueHierarchy.Where(mqh => mqh.MessageStatusId == MessageStatusTypes.Failed)
                .ToList();
            return failedHierarchyMessages;
        }
    }
}
