using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;
using System.Linq;

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
