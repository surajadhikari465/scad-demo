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
    internal class JustInTimeMessagePublisher : IMessagePublisher
    {
        private readonly IEsbProducer justInTimeEsbProducer;
        private readonly GPMProducerServiceSettings gpmProducerServiceSettings;
        private readonly RetryPolicy sendMessageRetryPolicy;
        private readonly ILogger<JustInTimeMessagePublisher> logger;

        public JustInTimeMessagePublisher(
            IEsbProducer justInTimeEsbProducer,
            GPMProducerServiceSettings gpmProducerServiceSettings,
            ILogger<JustInTimeMessagePublisher> logger
            )
        {
            this.justInTimeEsbProducer = justInTimeEsbProducer;
            this.gpmProducerServiceSettings = gpmProducerServiceSettings;
            this.logger = logger;
            this.sendMessageRetryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(
                gpmProducerServiceSettings.SendMessageRetryCount,
                retryAttempt => TimeSpan.FromMilliseconds(gpmProducerServiceSettings.SendMessageRetryDelayInMilliseconds)
                );
            string serviceType = gpmProducerServiceSettings.ServiceType;
            var computedClientId = $"GPMService.Type-{serviceType}.{Environment.MachineName}.{Guid.NewGuid()}";
            var clientId = computedClientId.Substring(0, Math.Min(computedClientId.Length, 255));
            logger.Info($"Opening {serviceType} ESB Connection");
            justInTimeEsbProducer.OpenConnection(clientId);
            logger.Info($"{serviceType} ESB Connection Opened");
        }

        public void PublishMessage(string message, Dictionary<string, string> messageProperties)
        {
            sendMessageRetryPolicy.Execute(() =>
            {
                justInTimeEsbProducer.Send(message, messageProperties);
            });
        }
    }
}
