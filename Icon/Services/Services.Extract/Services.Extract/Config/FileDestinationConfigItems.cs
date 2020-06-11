using System.Configuration;

namespace Services.Extract.Config
{
    public class FileDestinationConfigItems : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FileDestinationConfigItem();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FileDestinationConfigItem)element).ProfileName;
        }
    }
}