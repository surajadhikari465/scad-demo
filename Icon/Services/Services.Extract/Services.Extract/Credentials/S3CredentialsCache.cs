using System.Collections.Generic;
using System.Linq;
using Services.Extract.Config;

namespace Services.Extract.Credentials
{
    public class S3CredentialsCache : IS3CredentialsCache
    {
        public Dictionary<string, S3Credential> Credentials { get; set; }

        public void Refresh()
        {
            Credentials = S3CredentialConfigSection.Config.SettingsList.ToDictionary(d => d.ProfileName, d => new S3Credential(d.AccessKey, d.SecretKey, d.BucketName, d.BucketRegion));
        }
    }
}