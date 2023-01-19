using Icon.Esb.Producer;
using Icon.Logging;
using JobScheduler.Service.Helper;
using JobScheduler.Service.Settings;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Globalization;
using Wfm.Aws.S3;

namespace JobScheduler.Service.Publish
{
    internal class MesssagePublisher : IMessagePublisher
    {

        // Using named injection for IEsbProducer variables.
        // Changing the variable name would require change in SimpleInjectiorInitializer.cs file as well.
        private readonly IEsbProducer extractServiceEsbProducer;
        private readonly IEsbProducer activePriceServiceEsbProducer;
        private readonly IEsbProducer expiringTprServiceEsbProducer;
        private readonly IS3Facade s3Facade;
        private readonly JobSchedulerServiceSettings jobSchedulerServiceSettings;
        private readonly ILogger<MesssagePublisher> logger;
        private readonly RetryPolicy sendMessageRetryPolicy;
        private readonly IDictionary<string, IEsbProducer> consumerServiceEsbProducerMap;
        private readonly IDictionary<string, string> consumerServiceS3BucketMap;
        private readonly IDictionary<string, string> queueNameConsumerServiceMap;

        public MesssagePublisher(
            IEsbProducer extractServiceEsbProducer,
            IEsbProducer activePriceServiceEsbProducer,
            IEsbProducer expiringTprServiceEsbProducer,
            IS3Facade s3Facade,
            JobSchedulerServiceSettings jobSchedulerServiceSettings,
            ILogger<MesssagePublisher> logger
            )
        {
            this.extractServiceEsbProducer = extractServiceEsbProducer;
            this.activePriceServiceEsbProducer = activePriceServiceEsbProducer;
            this.expiringTprServiceEsbProducer = expiringTprServiceEsbProducer;
            this.s3Facade = s3Facade;
            this.jobSchedulerServiceSettings = jobSchedulerServiceSettings;
            this.logger = logger;
            this.sendMessageRetryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(
                jobSchedulerServiceSettings.SendMessageRetryCount,
                retryAttempt => TimeSpan.FromMilliseconds(jobSchedulerServiceSettings.SendMessageRetryDelayInMilliseconds)
                );
            consumerServiceEsbProducerMap = new Dictionary<string, IEsbProducer>()
            {
                { Constants.ConsumerServices.ExtractService, extractServiceEsbProducer },
                { Constants.ConsumerServices.ActivePriceService, activePriceServiceEsbProducer },
                { Constants.ConsumerServices.ExpiringTprService, expiringTprServiceEsbProducer }
            };
            consumerServiceS3BucketMap = new Dictionary<string, string>()
            {
                { Constants.ConsumerServices.ExtractService, Constants.ConsumerS3Buckets.ExtractServiceBucketName },
                { Constants.ConsumerServices.ActivePriceService, Constants.ConsumerS3Buckets.ActivePriceServiceBucketName },
                { Constants.ConsumerServices.ExpiringTprService, Constants.ConsumerS3Buckets.ExpiringTprServiceBucketName }
            };
            queueNameConsumerServiceMap = new Dictionary<string, string>()
            {
                { Constants.ConsumerQueues.ExtractServiceQueue, Constants.ConsumerServices.ExtractService },
                { Constants.ConsumerQueues.ActivePriceServiceQueue , Constants.ConsumerServices.ActivePriceService },
                { Constants.ConsumerQueues.ExpiringTprServiceQueue , Constants.ConsumerServices.ExpiringTprService }
            };
            OpenEsbConnection();
        }

        public void PublishMessage(string queueName, string message, Dictionary<string, string> messageProperties)
        {
            sendMessageRetryPolicy.Execute(() =>
            {
                consumerServiceEsbProducerMap[queueNameConsumerServiceMap[queueName]].Send(message, messageProperties);
            });
            sendMessageRetryPolicy.Execute(() =>
            {
                s3Facade.PutObject(
                    $"{consumerServiceS3BucketMap[queueNameConsumerServiceMap[queueName]]}-{jobSchedulerServiceSettings.AwsAccountId}",
                    $"{System.DateTime.UtcNow.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture)}/{Guid.NewGuid()}",
                    message,
                    messageProperties
                    );
            });
        }

        private void OpenEsbConnection()
        {
            foreach (KeyValuePair<string, IEsbProducer> entry in consumerServiceEsbProducerMap)
            {
                var computedClientId = $"JobScheduler.Type-{entry.Key}.{Environment.MachineName}.{Guid.NewGuid()}";
                var clientId = computedClientId.Substring(0, Math.Min(computedClientId.Length, 255));
                logger.Info($"Opening JobScheduler-{entry.Key} ESB Connection");
                entry.Value.OpenConnection(clientId);
                logger.Info($"JobScheduler-{entry.Key} ESB Connection Opened");
            }
        }
    }
}
