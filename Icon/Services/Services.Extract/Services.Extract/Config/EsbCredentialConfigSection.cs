using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Services.Extract.Config
{
    public class EsbCredentialConfigSection : ConfigurationSection
    {
        public static EsbCredentialConfigSection Config => ConfigurationManager.GetSection("EsbCredentials") as EsbCredentialConfigSection;
        public IEnumerable<EsbCredentialConfigItem> SettingsList => Settings.Cast<EsbCredentialConfigItem>();

        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        private EsbCredentialConfigItems Settings
        {
            get => (EsbCredentialConfigItems)this[""];
            set => this[""] = value;
        }
    }
}
