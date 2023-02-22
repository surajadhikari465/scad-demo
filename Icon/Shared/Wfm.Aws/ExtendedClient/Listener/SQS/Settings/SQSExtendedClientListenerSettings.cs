using Icon.Common;
using System;
using Wfm.Aws.ConfigurationReader;

namespace Wfm.Aws.ExtendedClient.Listener.SQS.Settings
{
    public class SQSExtendedClientListenerSettings
    {
        public string SQSListenerApplicationName { get; set; }
        public string SQSListenerQueueUrl { get; set; }
        public int SQSListenerTimeoutInSeconds { get; set; }
        public int SQSListenerPollIntervalInSeconds { get; set; }
        public int SQSListenerSafeStopCheckInSeconds { get; set; }
        public bool SQSListenerSafeStopCheckEnabled { get; set; }

        public static SQSExtendedClientListenerSettings CreateSettingsFromNamedConfig(string configurationName)
        {
            var sqsExtendedClientListenerConfigurations = WfmAwsConfigurationReader.GetConfig();
            if (sqsExtendedClientListenerConfigurations == null)
            {
                throw new ArgumentException($"Could not find {Constants.NamedConfigurationProperties.WfmAwsConfigurations}");
            }
            var namedConfiguration = sqsExtendedClientListenerConfigurations.SQSExtendedClientListenerConfigurations[configurationName];
            if (namedConfiguration == null)
            {
                throw new ArgumentException($"Could not find '{configurationName}' in {Constants.NamedConfigurationProperties.SQSExtendedClientListenerConfigurations}");
            }

            return new SQSExtendedClientListenerSettings
            {
                SQSListenerApplicationName = namedConfiguration.SQSListenerApplicationName,
                SQSListenerQueueUrl = namedConfiguration.SQSListenerQueueUrl,
                SQSListenerTimeoutInSeconds = namedConfiguration.SQSListenerTimeoutInSeconds,
                SQSListenerPollIntervalInSeconds = namedConfiguration.SQSListenerPollIntervalInSeconds,
                SQSListenerSafeStopCheckInSeconds = namedConfiguration.SQSListenerSafeStopCheckInSeconds,
                SQSListenerSafeStopCheckEnabled = namedConfiguration.SQSListenerSafeStopCheckEnabled
            };
        }

        public static SQSExtendedClientListenerSettings CreateSettingsFromConfig()
        {
            return new SQSExtendedClientListenerSettings
            {
                SQSListenerApplicationName = AppSettingsAccessor.GetStringSetting(Constants.ConfigurationProperties.SQSListenerApplicationName),
                SQSListenerQueueUrl = AppSettingsAccessor.GetStringSetting(Constants.ConfigurationProperties.SQSListenerQueueUrl),
                SQSListenerTimeoutInSeconds = AppSettingsAccessor.GetIntSetting(Constants.ConfigurationProperties.SQSListenerTimeoutInSeconds, 15),
                SQSListenerPollIntervalInSeconds = AppSettingsAccessor.GetIntSetting(Constants.ConfigurationProperties.SQSListenerPollIntervalInSeconds, 30),
                SQSListenerSafeStopCheckInSeconds = AppSettingsAccessor.GetIntSetting(Constants.ConfigurationProperties.SQSListenerSafeStopCheckInSeconds, 15),
                SQSListenerSafeStopCheckEnabled = AppSettingsAccessor.GetBoolSetting(Constants.ConfigurationProperties.SQSListenerSafeStopCheckEnabled, false),
            };
        }
    }
}
