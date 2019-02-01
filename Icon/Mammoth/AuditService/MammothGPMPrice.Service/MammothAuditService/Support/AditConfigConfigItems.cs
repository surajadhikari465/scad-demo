using System.Configuration;

namespace Audit
{
  public sealed class AditConfigConfigItems : ConfigurationElementCollection
    {
      protected override ConfigurationElement CreateNewElement()
      {
          return new AuditConfigItem();
      }

      protected override object GetElementKey(ConfigurationElement element)
      {
          return ((AuditConfigItem)element).Name;
      }
    }
}