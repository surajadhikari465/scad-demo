using System.Collections.Generic;

namespace JobScheduler.Service.Publish
{
    internal interface IMessagePublisher
    {
        void PublishMessage(string queueName, string message, Dictionary<string, string> messageProperties);
    }
}
