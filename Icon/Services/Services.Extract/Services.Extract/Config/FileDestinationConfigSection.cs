using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Services.Extract.Config
{
    public class FileDestinationConfigSection : ConfigurationSection
    {
        public static FileDestinationConfigSection Config => ConfigurationManager.GetSection("FileDestinations") as FileDestinationConfigSection;
        
        public IEnumerable<FileDestinationConfigItem> SettingsList => Settings.Cast<FileDestinationConfigItem>();
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        private FileDestinationConfigItems Settings
        {
            get => (FileDestinationConfigItems)this[""];
            set => this[""] = value;
        }
    }
}