using System.Collections.Generic;

namespace Icon.Web.DataAccess.Commands
{
    public class ReprocessFailedHierarchyMessageCommand
    {
        public List<int> MessageQueueIds { get; set; }
    }
}
