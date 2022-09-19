using System;
using InventoryProducer.Common.InstockDequeue.Model;

namespace InventoryProducer.Producer.Publish
{
    public interface IErrorEventPublisher
    {
        void PublishErrorEventToMammoth(InstockDequeueResult instockDequeueResult, Exception exception);
    }
}
