using System.Collections.Generic;
using InventoryProducer.Common.InstockDequeue.Model;

namespace InventoryProducer.Common.InstockDequeue
{
    public interface IInstockDequeueService
    {
        IList<InstockDequeueResult> GetDequeuedMessages();
    }
}
