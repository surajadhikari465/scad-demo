using System.Collections.Generic;
using Icon.Common;
using Wfm.Aws.S3;
using Wfm.Aws.S3.Settings;

namespace WebSupport.Clients
{
    public class DvsNearRealTimePriceClient: IDvsNearRealTimePriceClient
    {
        private IS3Facade s3Facade;
        private string dvsNrtGpmPriceBucket;

        public DvsNearRealTimePriceClient()
        {
            s3Facade = new S3Facade(S3FacadeSettings.CreateSettingsFromNamedConfig("MammothGpmS3Settings"));
            string dvsAwsAccount = AppSettingsAccessor.GetStringSetting("DvsAwsAccount");
            dvsNrtGpmPriceBucket = $"gpmsourcebucket-{dvsAwsAccount}";
        }

        public void Send(string message, string messageId, Dictionary<string, string> messageProperties)
        {
            s3Facade.PutObject(dvsNrtGpmPriceBucket, messageId, message, messageProperties);
        }
    }
}