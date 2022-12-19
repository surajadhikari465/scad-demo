using System;
using System.Collections.Generic;
using Icon.Esb.Producer;
using Icon.ActiveMQ.Producer;
using Icon.Logging;

namespace MammothR10Price.Publish
{
    public class MessagePublisher: IMessagePublisher
    {
        private readonly IActiveMQProducer activeMQProducer;
        private readonly IEsbProducer esbProducer;
        private readonly ILogger<MessagePublisher> logger;

        public MessagePublisher(
            IActiveMQProducer activeMQProducer,
            // Using named injection.
            // Changing the variable name would require change in SimpleInjectiorInitializer.cs file as well.
            IEsbProducer esbProducer,
            ILogger<MessagePublisher> logger
            )
        {
            this.activeMQProducer = activeMQProducer;
            this.esbProducer = esbProducer;
            this.logger = logger;
        }

        public void Publish(string message, Dictionary<string, string> messageProperties)
        {
            try
            {
                PublishToEsb(message, messageProperties);
                PublishToActiveMq(message, messageProperties);
            }
            catch (Exception e)
            {
                logger.Error($"Error trying to send data to JMS Queue: ${e}");
            }
        }

        private void PublishToEsb(string message, Dictionary<string, string> messageProperties)
        {
            esbProducer.Send(message, messageProperties);
        }

        private void PublishToActiveMq(string message, Dictionary<string, string> messageProperties)
        {
            activeMQProducer.Send(message, messageProperties);
        }
    }
}
