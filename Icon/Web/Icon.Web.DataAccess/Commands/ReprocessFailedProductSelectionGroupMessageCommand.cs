using System.Collections.Generic;

namespace Icon.Web.DataAccess.Commands
{
    public class ReprocessFailedProductSelectionGroupMessageCommand
    {
        public List<int> MessageQueueIds { get; set; }
    }
}
