using System.Configuration;

namespace Services.Extract.Config
{
    public class SFtpCredentialConfigItem : ConfigurationElement
    {
        [ConfigurationProperty("profileName", IsKey = true, IsRequired = true)]
        public string ProfileName
        {
            get => (string)base["profileName"];
            set => base["profileName"] = value;
        }
        [ConfigurationProperty("host", IsRequired = true)]
        public string Host
        {
            get => (string)base["host"];
            set => base["host"] = value;
        }
        [ConfigurationProperty("username", IsRequired = true)]
        public string Username
        {
            get => (string)base["username"];
            set => base["username"] = value;
        }
        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get => (string)base["password"];
            set => base["password"] = value;
        }
    }
}