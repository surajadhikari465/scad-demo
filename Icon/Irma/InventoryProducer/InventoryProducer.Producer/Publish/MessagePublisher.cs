using System;
using System.Collections.Generic;
using Icon.ActiveMQ.Producer;

namespace InventoryProducer.Producer.Publish
{
    public class MessagePublisher: IMessagePublisher
    {

        private readonly IActiveMQProducer activeMQProducer;

        public MessagePublisher(IActiveMQProducer activeMQProducer)
        {
            this.activeMQProducer = activeMQProducer;
        }

        public void PublishMessage(string xmlMessage, Dictionary<string, string> messageProperties, 
            Action onSuccess=null, Action<Exception> onFailure=null)
        {
            try
            {
                PublishToActiveMq(xmlMessage, messageProperties);
                onSuccess?.Invoke();
            }
            catch(Exception ex)
            {
                onFailure?.Invoke(ex);
            }
        }

        private void PublishToActiveMq(string xmlMessage, Dictionary<string,string> messageProperties)
        {
            activeMQProducer.Send(xmlMessage, messageProperties);
        }
    }
}
