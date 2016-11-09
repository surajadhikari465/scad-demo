using Icon.Esb;
using Icon.Esb.Factory;
using Icon.Framework;
using System.Collections.Generic;

namespace Icon.Ewic.Transmission.Producers
{
    public class EwicMessageProducer : IMessageProducer
    {
        private IEsbConnectionFactory producerFactory;

        public EwicMessageProducer(IEsbConnectionFactory producerFactory)
        {
            this.producerFactory = producerFactory;
        }

        public void SendMessages(List<MessageHistory> messages)
        {
            using (var producer = producerFactory.CreateProducer(EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("producer")))
            {
                producer.OpenConnection();

                foreach (var message in messages)
                {
                    producer.Send(message.Message, new Dictionary<string, string> { { "TransactionID", message.MessageHistoryId.ToString() } });
                }
            };
        }
    }
}
