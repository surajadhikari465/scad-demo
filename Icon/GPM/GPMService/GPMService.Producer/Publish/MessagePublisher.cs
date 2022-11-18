using GPMService.Producer.Message.Processor;
using GPMService.Producer.Settings;
using Icon.ActiveMQ.Producer;
using Icon.Esb.Producer;
using Icon.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;

namespace GPMService.Producer.Publish
{
    internal class MessagePublisher : IMessagePublisher
    {
        private readonly IActiveMQProducer activeMQProducer;
        private readonly IEsbProducer esbProducer;
        private readonly GPMProducerServiceSettings gpmProducerServiceSettings;
        private readonly RetryPolicy sendMessageRetryPolicy;
        private readonly ILogger<MessagePublisher> logger;

        public MessagePublisher(
            IActiveMQProducer activeMQProducer,
            IEsbProducer esbProducer,
            GPMProducerServiceSettings gpmProducerServiceSettings,
            ILogger<MessagePublisher> logger
            )
        {
            this.activeMQProducer = activeMQProducer;
            this.esbProducer = esbProducer;
            this.gpmProducerServiceSettings = gpmProducerServiceSettings;
            this.logger = logger;
            this.sendMessageRetryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(
                gpmProducerServiceSettings.SendMessageRetryCount,
                retryAttempt => TimeSpan.FromMilliseconds(gpmProducerServiceSettings.SendMessageRetryDelayInMilliseconds)
                );
        }

        public void PublishMessage(string xmlMessage, Dictionary<string, string> messageProperties)
        {
            try
            {
                sendMessageRetryPolicy.Execute(() => PublishToEsb(xmlMessage, messageProperties));
                sendMessageRetryPolicy.Execute(() => PublishToActiveMq(xmlMessage, messageProperties));
            }
            catch (Exception e)
            {
                logger.Error($"Error trying to send data to JMS Queue: ${e}");
            }
        }

        private void PublishToEsb(string xmlMessage, Dictionary<string, string> messageProperties)
        {
            esbProducer.Send(xmlMessage, messageProperties);
        }

        private void PublishToActiveMq(string xmlMessage, Dictionary<string, string> messageProperties)
        {
            activeMQProducer.Send(xmlMessage, messageProperties);
        }
    }
}
