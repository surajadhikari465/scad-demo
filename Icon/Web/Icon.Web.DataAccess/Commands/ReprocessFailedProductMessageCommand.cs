using System.Collections.Generic;

namespace Icon.Web.DataAccess.Commands
{
    public class ReprocessFailedProductMessageCommand
    {
        public List<int> MessageQueueIds { get; set; }
    }
}
