using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Configuration;

namespace Audit
{
  public class UploadConfigSection : ConfigurationSection
  {
    public static UploadConfigSection Config { get { return ConfigurationManager.GetSection("Upload") as UploadConfigSection; }}
    public IEnumerable<UploadConfigItem> SettingsList { get { return this.Settings.Cast<UploadConfigItem>(); }}

    [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
    private UploadConfigItems Settings
    {
      get { return (UploadConfigItems)this[""]; }
      set { this[""] = value; }
    }
  }
}
