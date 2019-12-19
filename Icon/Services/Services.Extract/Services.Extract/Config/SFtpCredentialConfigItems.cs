using System.Configuration;

namespace Services.Extract.Config
{
    public class SFtpCredentialConfigItems : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SFtpCredentialConfigItem();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SFtpCredentialConfigItem)element).ProfileName;
        }
    }
}