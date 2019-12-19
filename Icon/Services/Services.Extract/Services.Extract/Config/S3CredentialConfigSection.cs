using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Services.Extract.Config
{
    public class S3CredentialConfigSection : ConfigurationSection
    {
        public static S3CredentialConfigSection Config => ConfigurationManager.GetSection("S3Credentials") as S3CredentialConfigSection;
        public IEnumerable<S3CredentialConfigItem> SettingsList => Settings.Cast<S3CredentialConfigItem>();
        
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        private S3CredentialConfigItems Settings
        {
            get => (S3CredentialConfigItems) this[""];
            set => this[""] = value;
        }
    }
}