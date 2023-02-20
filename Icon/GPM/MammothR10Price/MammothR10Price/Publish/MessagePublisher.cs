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
            IEsbProducer esbProducer,
            ILogger<MessagePublisher> logger
            )
        {
            this.activeMQProducer = activeMQProducer;
            this.esbProducer = esbProducer;
            this.logger = logger;
            var computedClientId = $"MammothR10PriceService.{Environment.MachineName}.{Guid.NewGuid()}";
            var clientId = computedClientId.Substring(0, Math.Min(computedClientId.Length, 255));
            logger.Info("Opening ESB producer connection");
            this.esbProducer.OpenConnection(clientId);
            logger.Info("ESB producer connection opened");
            logger.Info("Opening ActiveMQ producer connection");
            this.activeMQProducer.OpenConnection(clientId);
            logger.Info("ActiveMQ producer connection opened");
        }

        public void Publish(string message, Dictionary<string, string> messageProperties)
        {
            PublishToActiveMq(message, messageProperties);
            PublishToEsb(message, messageProperties);
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
