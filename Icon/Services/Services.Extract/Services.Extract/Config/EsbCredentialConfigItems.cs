using System.Configuration;

namespace Services.Extract.Config
{
    public class EsbCredentialConfigItems : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new EsbCredentialConfigItem();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EsbCredentialConfigItem)element).ProfileName;
        }
    }
}
