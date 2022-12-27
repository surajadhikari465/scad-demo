using System.Collections.Generic;

namespace JobScheduler.Service.Publish
{
    internal interface IMessagePublisher
    {
        void PublishMessage(string message, Dictionary<string, string> messageProperties);
    }
}
