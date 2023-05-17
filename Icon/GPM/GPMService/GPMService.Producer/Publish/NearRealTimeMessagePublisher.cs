﻿using GPMService.Producer.Settings;
using Icon.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Globalization;
using Wfm.Aws.S3;

namespace GPMService.Producer.Publish
{
    internal class NearRealTimeMessagePublisher : IMessagePublisher
    {
        private readonly IS3Facade s3Facade;
        private readonly GPMProducerServiceSettings gpmProducerServiceSettings;
        private readonly RetryPolicy sendMessageRetryPolicy;
        private readonly ILogger<NearRealTimeMessagePublisher> logger;

        public NearRealTimeMessagePublisher(
            IS3Facade s3Facade,
            GPMProducerServiceSettings gpmProducerServiceSettings,
            ILogger<NearRealTimeMessagePublisher> logger
            )
        {
            this.s3Facade = s3Facade;
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
        }

        public void PublishMessage(string message, Dictionary<string, string> messageProperties)
        {
            try
            {
                sendMessageRetryPolicy.Execute(() => PublishToS3(message, messageProperties));
            }
            catch (Exception e)
            {
                logger.Error($"Error trying to send data to JMS Queue: {e}");
            }
        }

        private void PublishToS3(string message, Dictionary<string, string> messageProperties)
        {
            s3Facade.PutObject(
                gpmProducerServiceSettings.DvsGpmSourceBucket,
                $"NearRealTime/{DateTime.UtcNow.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture)}/{Guid.NewGuid()}",
                message,
                messageProperties
                );
        }
    }
}
