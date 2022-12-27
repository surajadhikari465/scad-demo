using System.Collections.Generic;

namespace Icon.ActiveMQ.Producer
{
    public interface IActiveMQDynamicProducer: IActiveMQConnection
    {
        void OpenConnection(string clientId, int maxRetries);
        void Send(string queueName, string message, Dictionary<string, string> messageProperties = null);
    }
}
