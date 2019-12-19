using System.Collections.Generic;

namespace Icon.Web.DataAccess.Commands
{
    public class ReprocessFailedPriceMessageCommand
    {
        public List<int> MessageQueueIds { get; set; }
    }
}
