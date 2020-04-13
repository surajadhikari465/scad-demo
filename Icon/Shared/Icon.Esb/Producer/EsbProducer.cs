using System;
using System.Collections.Generic;
using TIBCO.EMS;

namespace Icon.Esb.Producer
{
    public class EsbProducer : EsbConnection, IEsbProducer
    {
        private MessageProducer producer;

        public EsbProducer(EsbConnectionSettings settings) : base(settings) { }
        

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

        public void Send(byte[] bytes, string messageId, Dictionary<string, string> messageProperties = null)
        {
            BytesMessage bytesMessage = session.CreateBytesMessage();
            bytesMessage.WriteBytes(bytes);

            bytesMessage.MessageID = messageId;

            Send(bytesMessage, messageProperties);
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

            producer.Send(textMessage);
        }

        private void Send(BytesMessage byteMessage, Dictionary<string, string> messageProperties = null)
        {
            if (messageProperties != null)
            {
                foreach (var property in messageProperties)
                {
                    byteMessage.SetStringProperty(property.Key, property.Value);
                }
            }

            producer.Send(byteMessage);
        }

        public override void OpenConnection(string clientId)
        {
            base.OpenConnection(clientId);
            producer = session.CreateProducer(destination);
            producer.DeliveryMode = DeliveryMode.PERSISTENT;
        }

        public override void Dispose()
        {
            if (producer != null)
            {
                producer.Close();
            }

            base.Dispose();
        }
    }
}
