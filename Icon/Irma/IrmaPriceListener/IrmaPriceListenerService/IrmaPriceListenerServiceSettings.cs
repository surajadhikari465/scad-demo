using Icon.Common;
using Wfm.Aws.ExtendedClient.Listener.SQS.Settings;

namespace IrmaPriceListenerService
{
    public class IrmaPriceListenerServiceSettings
    {
        public string IrmaRegionCode { get; set; }
        public string ApplicationName { get => GetApplicationName(); }
        public string AwsAccountId { get; set; }
        public string AwsRegion { get; set; }
        public string SqsQueueUrl { get => GetSqsQueueUrl(); }
        public int PollInterval { get; set; } = 30;

        private string GetSqsQueueUrl()
        {
            return $"https://sqs.{AwsRegion}.amazonaws.com/{AwsAccountId}/MammothGpmIrmaQueue-{IrmaRegionCode}";
        }

        private string GetApplicationName()
        {
            return $"IrmaPriceListenerService-{IrmaRegionCode}";
        }

        public SQSExtendedClientListenerSettings GetGpmBridgeListenerSettings()
        {
            return new SQSExtendedClientListenerSettings
            {
                SQSListenerQueueUrl = SqsQueueUrl,
                SQSListenerApplicationName = ApplicationName,
                SQSListenerPollIntervalInSeconds = PollInterval
            };
        }

        public static IrmaPriceListenerServiceSettings CreateFromConfig()
        {
            return new IrmaPriceListenerServiceSettings()
            {
                AwsAccountId = AppSettingsAccessor.GetStringSetting("AwsAccountId"),
                AwsRegion = AppSettingsAccessor.GetStringSetting("AwsRegion"),
                IrmaRegionCode = AppSettingsAccessor.GetStringSetting("IrmaRegionCode")
            };
        }
    }
}
