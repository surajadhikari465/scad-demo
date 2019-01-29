using System.Configuration;

namespace Audit
{
  public sealed class UploadConfigItems : ConfigurationElementCollection
    {
      protected override ConfigurationElement CreateNewElement()
      {
          return new UploadConfigItem();
      }

      protected override object GetElementKey(ConfigurationElement element)
      {
          return ((UploadConfigItem)element).ProfileName;
      }
    }
}
