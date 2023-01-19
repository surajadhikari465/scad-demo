using Icon.Common;
using System;
using Wfm.Aws.ConfigurationReader;

namespace Wfm.Aws.S3.Settings
{
    public class S3FacadeSettings
    {
        public string AwsAccessKey { get; set; }
        public string AwsSecretKey { get; set; }
        public string AwsRegion { get; set; }

        public static S3FacadeSettings CreateSettingsFromNamedConfig(string configurationName)
        {
            var s3FacadeConfiguration = WfmAwsConfigurationReader.GetConfig();
            if (s3FacadeConfiguration == null)
            {
                throw new ArgumentException($"Could not find {Constants.NamedConfigurationProperties.WfmAwsConfigurations}");
            }
            var namedConfiguration = s3FacadeConfiguration.BasicAWSConfigurations[configurationName];
            if (namedConfiguration == null)
            {
                throw new ArgumentException($"Could not find '{configurationName}' in {Constants.NamedConfigurationProperties.S3FacadeConfigurations}");
            }
            return new S3FacadeSettings
            {
                AwsAccessKey = namedConfiguration.AwsAccessKey,
                AwsSecretKey = namedConfiguration.AwsSecretKey,
                AwsRegion = namedConfiguration.AwsRegion
            };
        }

        public static S3FacadeSettings CreateSettingsFromConfig()
        {
            return new S3FacadeSettings
            {
                AwsAccessKey = AppSettingsAccessor.GetStringSetting(Constants.ConfigurationProperties.AwsAccessKey),
                AwsSecretKey = AppSettingsAccessor.GetStringSetting(Constants.ConfigurationProperties.AwsSecretKey),
                AwsRegion = AppSettingsAccessor.GetStringSetting(Constants.ConfigurationProperties.AwsRegion)
            };
        }
    }
}
