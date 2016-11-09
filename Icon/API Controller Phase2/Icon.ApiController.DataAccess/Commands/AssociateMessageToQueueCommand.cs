using System.Collections.Generic;

namespace Icon.ApiController.DataAccess.Commands
{
    public class AssociateMessageToQueueCommand<TMessageQueue, TMessageHistory>
    {
        public List<TMessageQueue> QueuedMessages { get; set; }
        public TMessageHistory MessageHistory { get; set; }
    }
}
