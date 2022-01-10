using System;
using System.Collections.Generic;

namespace Icon.ActiveMQ.Producer
{
    public interface IActiveMQProducer: IActiveMQConnection
    {
        void Send(string message, Dictionary<string, string> messageProperties = null);

        void Send(string message, string messageId, Dictionary<string, string> messageProperties = null);

        void OpenConnection(string clientId, int maxRetries);
    }
}
