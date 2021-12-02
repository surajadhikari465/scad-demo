using Icon.ActiveMQ.Producer;
using System;

namespace Icon.ActiveMQ.Factory
{
    public class ActiveMQConnectionFactory
    {
        public ActiveMQConnectionSettings Settings { get; set; }

        public ActiveMQConnectionFactory(ActiveMQConnectionSettings settings)
        {
            Settings = settings;
        }

        public ActiveMQProducer CreateProducer(string clientId, bool openConnection = true)
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
