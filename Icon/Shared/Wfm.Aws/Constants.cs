namespace Wfm.Aws
{
    public class Constants
    {
        public struct ConfigurationProperties
        {
            public const string AwsAccessKey = "AwsAccessKey";
            public const string AwsSecretKey = "AwsSecretKey";
            public const string AwsRegion = "AwsRegion";
            public const string SQSListenerApplicationName = "SQSListenerApplicationName";
            public const string SQSListenerQueueUrl = "SQSListenerQueueUrl";
            public const string SQSListenerTimeoutInSeconds = "SQSListenerTimeoutInSeconds";
            public const string SQSListenerPollIntervalInSeconds = "SQSListenerPollIntervalInSeconds";
            public const string SQSListenerSafeStopCheckInSeconds = "SQSListenerSafeStopCheckInSeconds";
            public const string SQSListenerSafeStopCheckEnabled = "SQSListenerSafeStopCheckEnabled";
        }
        public struct NamedConfigurationProperties
        {
            public const string WfmAwsConfigurations = "wfmAwsConfigurations";
            public const string S3FacadeConfigurations = "s3FacadeConfigurations";
            public const string S3FacadeConfiguration = "s3FacadeConfiguration";
            public const string SNSFacadeConfigurations = "snsFacadeConfigurations";
            public const string SNSFacadeConfiguration = "snsFacadeConfiguration";
            public const string SQSFacadeConfigurations = "sqsFacadeConfigurations";
            public const string SQSFacadeConfiguration = "sqsFacadeConfiguration";
            public const string SQSExtendedClientListenerConfigurations = "sqsExtendedClientListenerConfigurations";
            public const string SQSExtendedClientListenerConfiguration = "sqsExtendedClientListenerConfiguration";
            public const string Name = "name";
            public const string AwsAccessKey = "awsAccessKey";
            public const string AwsSecretKey = "awsSecretKey";
            public const string AwsRegion = "awsRegion";
            public const string SQSListenerApplicationName = "sqsListenerApplicationName";
            public const string SQSListenerQueueUrl = "sqsListenerQueueUrl";
            public const string SQSListenerTimeoutInSeconds = "sqsListenerTimeoutInSeconds";
            public const string SQSListenerPollIntervalInSeconds = "sqsListenerPollIntervalInSeconds";
            public const string SQSListenerSafeStopCheckInSeconds = "sqsListenerSafeStopCheckInSeconds";
            public const string SQSListenerSafeStopCheckEnabled = "sqsListenerSafeStopCheckEnabled";
        }
        public struct EventSources
        {
            public const string SQS = "SQS";
            public const string SNS = "SNS";
        }
    }
}
