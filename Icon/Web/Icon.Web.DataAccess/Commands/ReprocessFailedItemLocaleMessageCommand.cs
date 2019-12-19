using System.Collections.Generic;

namespace Icon.Web.DataAccess.Commands
{
    public class ReprocessFailedItemLocaleMessageCommand
    {
        public List<int> MessageQueueIds { get; set; }
    }
}
