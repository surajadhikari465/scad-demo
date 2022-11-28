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
    internal class NearRealTimeMessagePublisher : IMessagePublisher
    {
        private readonly IActiveMQProducer activeMQProducer;
        private readonly IEsbProducer nearRealTimeEsbProducer;
        private readonly GPMProducerServiceSettings gpmProducerServiceSettings;
        private readonly RetryPolicy sendMessageRetryPolicy;
        private readonly ILogger<NearRealTimeMessagePublisher> logger;

        public NearRealTimeMessagePublisher(
            IActiveMQProducer activeMQProducer,
            // Using named injection.
            // Changing the variable name would require change in SimpleInjectiorInitializer.cs file as well.
            IEsbProducer nearRealTimeEsbProducer,
            GPMProducerServiceSettings gpmProducerServiceSettings,
            ILogger<NearRealTimeMessagePublisher> logger
            )
        {
            this.activeMQProducer = activeMQProducer;
            this.nearRealTimeEsbProducer = nearRealTimeEsbProducer;
            this.gpmProducerServiceSettings = gpmProducerServiceSettings;
            this.logger = logger;
            this.sendMessageRetryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(
                gpmProducerServiceSettings.SendMessageRetryCount,
                retryAttempt => TimeSpan.FromMilliseconds(gpmProducerServiceSettings.SendMessageRetryDelayInMilliseconds)
                );
        }

        public void PublishMessage(string message, Dictionary<string, string> messageProperties)
        {
            try
            {
                sendMessageRetryPolicy.Execute(() => PublishToEsb(message, messageProperties));
                sendMessageRetryPolicy.Execute(() => PublishToActiveMq(message, messageProperties));
            }
            catch (Exception e)
            {
                logger.Error($"Error trying to send data to JMS Queue: ${e}");
            }
        }

        private void PublishToEsb(string message, Dictionary<string, string> messageProperties)
        {
            nearRealTimeEsbProducer.Send(message, messageProperties);
        }

        private void PublishToActiveMq(string message, Dictionary<string, string> messageProperties)
        {
            activeMQProducer.Send(message, messageProperties);
        }
    }
}
