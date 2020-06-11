using System.Configuration;

namespace Services.Extract.Config
{
    public class FileDestinationConfigItem : ConfigurationElement
    {
        [ConfigurationProperty("profileName", IsKey = true, IsRequired = true)]
        public string ProfileName
        {
            get => (string)base["profileName"];
            set => base["profileName"] = value;
        }
        [ConfigurationProperty("path", IsRequired = true)]
        public string Path
        {
            get => (string)base["path"];
            set => base["path"] = value;
        }
    }
}