using System;
using System.Collections.Generic;
using Icon.ActiveMQ;
using Icon.ActiveMQ.Producer;

namespace WebSupport.Clients
{
    public class DvsNearRealTimePriceClient: IDvsNearRealTimePriceClient
    {
        private IActiveMQProducer producer;

        public DvsNearRealTimePriceClient()
        {
            producer = new ActiveMQProducer(ActiveMQConnectionSettings.CreateSettingsFromConfig());
        }

        public void Send(string message, string messageId, Dictionary<string, string> messageProperties)
        {
            producer.Send(message, messageId, messageProperties);
        }
    }
}