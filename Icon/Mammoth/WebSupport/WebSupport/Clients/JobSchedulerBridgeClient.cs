using Icon.Common;
using System.Collections.Generic;
using WebSupport.Models;
using Wfm.Aws.S3;
using Wfm.Aws.S3.Settings;
using Constants = WebSupport.Helpers.Constants;

namespace WebSupport.Clients
{
    public class JobSchedulerBridgeClient: IJobSchedulerBridgeClient
    {
        private string awsAccountId;
        private IS3Facade s3Facade;

        private Dictionary<string, string> queueNameConsumerS3Map = new Dictionary<string, string>()
        {
            { Constants.ConsumerQueues.ExtractServiceQueue, Constants.ConsumerS3Buckets.ExtractServiceBucketName },
            { Constants.ConsumerQueues.ActivePriceServiceQueue , Constants.ConsumerS3Buckets.ActivePriceServiceBucketName },
            { Constants.ConsumerQueues.ExpiringTprServiceQueue , Constants.ConsumerS3Buckets.ExpiringTprServiceBucketName }
        };

        public JobSchedulerBridgeClient() 
        {
            S3FacadeSettings settings = S3FacadeSettings.CreateSettingsFromNamedConfig("JobSchedulerS3Settings");
            s3Facade = new S3Facade(settings);
            awsAccountId = AppSettingsAccessor.GetStringSetting("JobSchedulerBridgeAwsAccount");
        }

        public void Send(JobScheduleModel request, string message, string messageId, Dictionary<string, string> messageProperties)
        {
            s3Facade.PutObject(
                $"{queueNameConsumerS3Map[request.DestinationQueueName]}-{awsAccountId}",
                messageId,
                message,
                messageProperties
            );
        }
    }
}