using Icon.ActiveMQ.Producer;
using System;

namespace Icon.ActiveMQ.Factory
{
    public class ActiveMQConnectionFactory: IActiveMQConnectionFactory
    {
        public ActiveMQConnectionSettings Settings { get; set; }

        public ActiveMQConnectionFactory(ActiveMQConnectionSettings settings)
        {
            Settings = settings;
        }

        public IActiveMQProducer CreateProducer(string clientId, bool openConnection = true)
        {
            ActiveMQProducer producer = new ActiveMQProducer(Settings);
            if (openConnection)
            {
                producer.OpenConnection(clientId);
            }
            return producer;
        }

        public static ActiveMQProducer CreateProducer(ActiveMQConnectionSettings settings)
        {
            return new ActiveMQProducer(settings);
        }
    }
}
