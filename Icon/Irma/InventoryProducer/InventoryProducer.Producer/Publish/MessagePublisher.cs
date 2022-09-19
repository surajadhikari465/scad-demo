using System;
using System.Collections.Generic;
using Icon.ActiveMQ.Producer;
using Icon.Esb.Producer;

namespace InventoryProducer.Producer.Publish
{
    public class MessagePublisher: IMessagePublisher
    {

        private readonly IActiveMQProducer activeMQProducer;
        private readonly IEsbProducer esbProducer;

        public MessagePublisher(IActiveMQProducer activeMQProducer, IEsbProducer esbProducer)
        {
            this.activeMQProducer = activeMQProducer;
            this.esbProducer = esbProducer;
        }

        public void PublishMessage(string xmlMessage, Dictionary<string, string> messageProperties, 
            Action onSuccess=null, Action<Exception> onFailure=null)
        {
            try
            {
                PublishToActiveMq(xmlMessage, messageProperties);
                PublishToEsb(xmlMessage, messageProperties);
                onSuccess?.Invoke();
            }
            catch(Exception ex)
            {
                onFailure?.Invoke(ex);
            }
        }

        private void PublishToEsb(string xmlMessage, Dictionary<string, string> messageProperties)
        {
            esbProducer.Send(xmlMessage, messageProperties);
        }

        private void PublishToActiveMq(string xmlMessage, Dictionary<string,string> messageProperties)
        {
            activeMQProducer.Send(xmlMessage, messageProperties);
        }
    }
}
