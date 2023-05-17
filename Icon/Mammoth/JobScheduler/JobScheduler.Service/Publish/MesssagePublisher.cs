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

        private readonly IS3Facade s3Facade;
        private readonly JobSchedulerServiceSettings jobSchedulerServiceSettings;
        private readonly ILogger<MesssagePublisher> logger;
        private readonly RetryPolicy sendMessageRetryPolicy;
        private readonly IDictionary<string, string> consumerServiceS3BucketMap;
        private readonly IDictionary<string, string> queueNameConsumerServiceMap;

        public MesssagePublisher(
            IS3Facade s3Facade,
            JobSchedulerServiceSettings jobSchedulerServiceSettings,
            ILogger<MesssagePublisher> logger
            )
        {
            this.s3Facade = s3Facade;
            this.jobSchedulerServiceSettings = jobSchedulerServiceSettings;
            this.logger = logger;
            this.sendMessageRetryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(
                jobSchedulerServiceSettings.SendMessageRetryCount,
                retryAttempt => TimeSpan.FromMilliseconds(jobSchedulerServiceSettings.SendMessageRetryDelayInMilliseconds)
                );
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
        }

        public void PublishMessage(string queueName, string message, Dictionary<string, string> messageProperties)
        {
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

    }
}
