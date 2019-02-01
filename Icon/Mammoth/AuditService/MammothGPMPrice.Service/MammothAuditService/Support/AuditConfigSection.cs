using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Configuration;

namespace Audit
{
  public sealed class AuditConfigSection : ConfigurationSection
  {
    public static AuditConfigSection Config { get { return ConfigurationManager.GetSection("Audits") as AuditConfigSection; }}
    public IEnumerable<AuditConfigItem> SettingsList { get { return this.Settings.Cast<AuditConfigItem>(); }}

    [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
    private AditConfigConfigItems Settings
    {
      get { return (AditConfigConfigItems)this[""]; }
      set { this[""] = value; }
    }
  }
}
