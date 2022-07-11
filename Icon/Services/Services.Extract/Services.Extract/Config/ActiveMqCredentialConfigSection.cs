using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Services.Extract.Config
{
    public class ActiveMqCredentialConfigSection: ConfigurationSection
    {
        public static ActiveMqCredentialConfigSection Config => ConfigurationManager.GetSection("ActiveMqCredentials") as ActiveMqCredentialConfigSection;

        public IEnumerable<ActiveMqCredentialConfigItem> SettingsList => Settings.Cast<ActiveMqCredentialConfigItem>();

        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        private ActiveMqCredentialConfigItems Settings
        {
            get => (ActiveMqCredentialConfigItems)this[""];
            set => this[""] = value;
        }
    }
}
