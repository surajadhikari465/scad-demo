using System.Collections.Generic;
using TIBCO.EMS;

namespace Mammoth.PrimeAffinity.Library.Esb
{
    public class CacheEsbProducer : ICacheEsbProducer
    {
        private MessageProducer messageProducer;
        private Session session;

        public CacheEsbProducer(MessageProducer messageProducer, Session session)
        {
            this.messageProducer = messageProducer;
            this.messageProducer.DeliveryMode = DeliveryMode.PERSISTENT;
            this.session = session;
        }

        public void Send(string message, Dictionary<string, string> messageProperties = null)
        {
            TextMessage textMessage = session.CreateTextMessage(message);

            Send(textMessage, messageProperties);
        }

        public void Send(string message, string messageId, Dictionary<string, string> messageProperties = null)
        {
            TextMessage textMessage = session.CreateTextMessage(message);
            textMessage.MessageID = messageId;

            Send(textMessage, messageProperties);
        }

        private void Send(TextMessage textMessage, Dictionary<string, string> messageProperties = null)
        {
            if (messageProperties != null)
            {
                foreach (var property in messageProperties)
                {
                    textMessage.SetStringProperty(property.Key, property.Value);
                }
            }

            messageProducer.Send(textMessage);
        }

        public void Dispose()
        {
            messageProducer.Close();
            session.Close();
        }
    }
}
