using System.Configuration;

namespace Services.Extract.Config
{
    public class S3CredentialConfigItems : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new S3CredentialConfigItem();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((S3CredentialConfigItem) element).ProfileName;
        }
    }
}