using System;
using System.Collections.Generic;

namespace InventoryProducer.Producer.Publish
{
    public interface IMessagePublisher
    {
        void PublishMessage(string message, Dictionary<string, string> messageProperties, Action onSuccess, Action<Exception> onFailure);
    }
}
