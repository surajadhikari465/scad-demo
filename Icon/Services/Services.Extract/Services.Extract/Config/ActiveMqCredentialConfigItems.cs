using System.Configuration;

namespace Services.Extract.Config
{
    public class ActiveMqCredentialConfigItems: ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ActiveMqCredentialConfigItem();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ActiveMqCredentialConfigItem)element).ProfileName;
        }
    }
}
