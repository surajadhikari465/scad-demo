using Icon.Common;
using Icon.Dvs;
using System;

namespace IrmaPriceListenerService
{
    public class IrmaPriceListenerServiceSettings
    {
        public string IrmaRegionCode { get; set; }
        public string ApplicationName { get => GetApplicationName(); }
        public string AwsAccountId { get; set; }
        public string AwsRegion { get; set; }
        public string AwsAccessKey { get; set; }
        public string AwsSecretKey { get; set; }
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

        public DvsListenerSettings GetDvsListenerSettings()
        {
            return new DvsListenerSettings
            {
                SqsQueueUrl = SqsQueueUrl,
                AwsAccessKey = AwsAccessKey,
                AwsSecretKey = AwsSecretKey,
                ListenerApplicationName = ApplicationName,
                Region = AwsRegion,
                PollInterval = PollInterval
            };
        }

        public static IrmaPriceListenerServiceSettings CreateFromConfig()
        {
            return new IrmaPriceListenerServiceSettings()
            {
                AwsAccessKey = AppSettingsAccessor.GetStringSetting("AwsAccessKey"),
                AwsSecretKey = AppSettingsAccessor.GetStringSetting("AwsSecretKey"),
                AwsAccountId = AppSettingsAccessor.GetStringSetting("AwsAccountId"),
                AwsRegion = AppSettingsAccessor.GetStringSetting("AwsRegion"),
                IrmaRegionCode = AppSettingsAccessor.GetStringSetting("IrmaRegionCode")
            };
        }
    }
}
