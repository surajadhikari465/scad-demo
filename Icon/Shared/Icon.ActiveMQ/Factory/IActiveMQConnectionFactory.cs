using System;
using Icon.ActiveMQ.Producer;

namespace Icon.ActiveMQ.Factory
{
    public interface IActiveMQConnectionFactory
    {
        ActiveMQConnectionSettings Settings { get; set; }
        IActiveMQProducer CreateProducer(string clientId, bool openConnection = true);
    }
}
