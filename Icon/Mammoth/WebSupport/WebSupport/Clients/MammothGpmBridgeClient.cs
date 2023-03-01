using Icon.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using WebSupport.DataAccess;
using Wfm.Aws.ExtendedClient.Serializer;
using Wfm.Aws.ExtendedClient.SQS;
using Wfm.Aws.S3;
using Wfm.Aws.S3.Settings;
using Wfm.Aws.SQS;
using Wfm.Aws.SQS.Settings;

namespace WebSupport.Clients
{
    public class MammothGpmBridgeClient: IMammothGpmBridgeClient
    {
        private string awsAccountId;
        private string awsRegion;
        private ISQSExtendedClient sqsExtendedClient;
        private IS3Facade s3Facade;

        public MammothGpmBridgeClient()
        {
            this.awsAccountId = AppSettingsAccessor.GetStringSetting("MammothGpmBridgeAwsAccount");
            this.s3Facade = new S3Facade(S3FacadeSettings.CreateSettingsFromNamedConfig("MammothGpmS3Settings"));
            var sqsFacadeSettings = SQSFacadeSettings.CreateSettingsFromNamedConfig("MammothGpmSqsSettings");
            ISQSFacade sqsFacade = new SQSFacade(sqsFacadeSettings);
            this.sqsExtendedClient = new SQSExtendedClient(
                sqsFacade, 
                s3Facade, 
                new ExtendedClientMessageSerializer()
            );
            this.awsRegion = sqsFacadeSettings.AwsRegion;
        }

        public void SendToJustInTimeConsumers(string message, Dictionary<string, string> messageProperties, string irmaRegion, string system) 
        {
            string s3Bucket = $"{Helpers.Constants.ConsumerS3Buckets.MammothGpmJitBucket}-{awsAccountId}";
            string messageId = Guid.NewGuid().ToString();

            if (PriceRefreshConstants.R10.Equals(system))
            {
                string queueUrl = $"https://sqs.{awsRegion}.amazonaws.com/{awsAccountId}/MammothGpmMammothR10Queue";
                
                sqsExtendedClient.SendMessage(
                    queueUrl,
                    s3Bucket,
                    $"MammothWebSupport/{DateTime.UtcNow.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture)}/{messageId}",
                    message,
                    RemoveNullSqsAttributes(messageProperties)
                );
            }
            else if(PriceRefreshConstants.IRMA.Equals(system))
            {
                if (RegionNameConstants.TS.Equals(irmaRegion))
                {
                    // ESB Logic - all the 365 stores are stores in RM region in IRMA.
                    irmaRegion = RegionNameConstants.RM;
                }

                string queueUrl = $"https://sqs.{awsRegion}.amazonaws.com/{awsAccountId}/MammothGpmIrmaQueue-{irmaRegion}";

                sqsExtendedClient.SendMessage(
                    queueUrl,
                    s3Bucket,
                    $"MammothWebSupport/{DateTime.UtcNow.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture)}/{messageId}",
                    message,
                    RemoveNullSqsAttributes(messageProperties)
                );
            }
            else
            {
                throw new ArgumentException($"{system} is not a valid downstream system for price refreshes.", nameof(system));
            }
        }

        public void SendToGpmProcessBod(string message, string messageId, Dictionary<string, string> messageProperties)
        {
            string s3Bucket = $"{Helpers.Constants.ConsumerS3Buckets.ProcessBodBucket}-{awsAccountId}";
            s3Facade.PutObject(
                s3Bucket,
                $"MammothWebSupport/{DateTime.UtcNow.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture)}/{messageId}",
                message,
                messageProperties
                );
        }

        // Null is not supported as a valid value in SQS attributes, hence removing them in case it's sent to the client
        private static Dictionary<string, string> RemoveNullSqsAttributes(Dictionary<string, string> properties)
        {
            Dictionary<string, string> newProperties = new Dictionary<string, string>();
            foreach (var key in properties.Keys)
            {
                if (properties[key] != null)
                {
                    newProperties[key] = properties[key];
                }
            }
            return newProperties;
        }
    }
}