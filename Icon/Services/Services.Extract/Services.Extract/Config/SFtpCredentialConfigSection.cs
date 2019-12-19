using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Services.Extract.Config
{
    public class SFtpCredentialConfigSection : ConfigurationSection
    {
        public static SFtpCredentialConfigSection Config => ConfigurationManager.GetSection("SFtpCredentials") as SFtpCredentialConfigSection;
        public IEnumerable<SFtpCredentialConfigItem> SettingsList => Settings.Cast<SFtpCredentialConfigItem>();

        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        private SFtpCredentialConfigItems Settings
        {
            get => (SFtpCredentialConfigItems)this[""];
            set => this[""] = value;
        }
    }
}