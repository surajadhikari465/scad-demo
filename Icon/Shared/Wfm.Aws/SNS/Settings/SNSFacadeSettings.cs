using Icon.Common;
using System;
using Wfm.Aws.ConfigurationReader;

namespace Wfm.Aws.SNS.Settings
{
    public class SNSFacadeSettings
    {
        public string AwsAccessKey { get; set; }
        public string AwsSecretKey { get; set; }
        public string AwsRegion { get; set; }

        public static SNSFacadeSettings CreateSettingsFromNamedConfig(string configurationName)
        {
            var snsExtendedClientConfiguration = WfmAwsConfigurationReader.GetConfig();
            if (snsExtendedClientConfiguration == null)
            {
                throw new ArgumentException($"Could not find {Constants.NamedConfigurationProperties.WfmAwsConfigurations}");
            }
            var namedConfiguration = snsExtendedClientConfiguration.SNSExtendedClientConfigurations[configurationName];
            if (namedConfiguration == null)
            {
                throw new ArgumentException($"Could not find '{configurationName}' in {Constants.NamedConfigurationProperties.SNSFacadeConfigurations}");
            }

            return new SNSFacadeSettings
            {
                AwsAccessKey = namedConfiguration.AwsAccessKey,
                AwsSecretKey = namedConfiguration.AwsSecretKey,
                AwsRegion = namedConfiguration.AwsRegion
            };
        }

        public static SNSFacadeSettings CreateSettingsFromConfig()
        {
            return new SNSFacadeSettings
            {
                AwsAccessKey = AppSettingsAccessor.GetStringSetting(Constants.ConfigurationProperties.AwsAccessKey),
                AwsSecretKey = AppSettingsAccessor.GetStringSetting(Constants.ConfigurationProperties.AwsSecretKey),
                AwsRegion = AppSettingsAccessor.GetStringSetting(Constants.ConfigurationProperties.AwsRegion)
            };
        }
    }
}
