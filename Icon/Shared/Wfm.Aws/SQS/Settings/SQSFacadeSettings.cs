using Icon.Common;
using System;
using Wfm.Aws.ConfigurationReader;

namespace Wfm.Aws.SQS.Settings
{
    public class SQSFacadeSettings
    {
        public string AwsAccessKey { get; set; }
        public string AwsSecretKey { get; set; }
        public string AwsRegion { get; set; }

        public static SQSFacadeSettings CreateSettingsFromNamedConfig(string configurationName)
        {
            var sqsFacadeConfigurations = WfmAwsConfigurationReader.GetConfig();
            if (sqsFacadeConfigurations == null)
            {
                throw new ArgumentException($"Could not find {Constants.NamedConfigurationProperties.WfmAwsConfigurations}");
            }
            var namedConfiguration = sqsFacadeConfigurations.SQSFacadeConfigurations[configurationName];
            if (namedConfiguration == null)
            {
                throw new ArgumentException($"Could not find '{configurationName}' in {Constants.NamedConfigurationProperties.SQSFacadeConfigurations}");
            }

            return new SQSFacadeSettings
            {
                AwsAccessKey = namedConfiguration.AwsAccessKey,
                AwsSecretKey = namedConfiguration.AwsSecretKey,
                AwsRegion = namedConfiguration.AwsRegion
            };
        }

        public static SQSFacadeSettings CreateSettingsFromConfig()
        {
            return new SQSFacadeSettings
            {
                AwsAccessKey = AppSettingsAccessor.GetStringSetting(Constants.ConfigurationProperties.AwsAccessKey),
                AwsSecretKey = AppSettingsAccessor.GetStringSetting(Constants.ConfigurationProperties.AwsSecretKey),
                AwsRegion = AppSettingsAccessor.GetStringSetting(Constants.ConfigurationProperties.AwsRegion)
            };
        }
    }
}
