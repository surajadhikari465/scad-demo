using System.Configuration;

namespace Audit
{
  public sealed class AuditConfigItem : ConfigurationElement
  {
    [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
    public string Name
    {
        get { return (string)base["name"]; }
        set { base["name"] = value; }
    }

    [ConfigurationProperty("proc", IsRequired = true)]
    public string Proc
    {
        get { return (string)base["proc"]; }
        set { base["proc"] = value; }
    }

    [ConfigurationProperty("excludeRegions", IsRequired = false)]
    public string ExcludeRegions
    {
        get { return (string)base["excludeRegions"]; }
        set { base["excludeRegions"] = value; }
    }

    [ConfigurationProperty("isActive", IsRequired = true)]
    public bool IsActive
    {
        get { return (bool)base["isActive"]; }
        set { base["isActive"] = value; }
    }

		[ConfigurationProperty("isGlobal", IsRequired = true)]
    public bool IsGlobal
    {
        get { return (bool)base["isGlobal"]; }
        set { base["isGlobal"] = value; }
    }

    [ConfigurationProperty("fileName", IsRequired = true)]
    public string FileName
    {
        get { return (string)base["fileName"]; }
        set { base["fileName"] = value; }
    }

    [ConfigurationProperty("delimiter", IsRequired = true)]
    public char Delimiter
    {
        get { return (char)base["delimiter"]; }
        set { base["delimiter"] = value; }
    }

    [ConfigurationProperty("intervalMinutes", IsRequired = true)]
    public int Interval
    {
        get { return (int)base["intervalMinutes"]; }
        set { base["intervalMinutes"] = value; }
    }

    [ConfigurationProperty("profileName", IsRequired = true)]
    public string ProfileName
    {
        get { return (string)base["profileName"]; }
        set { base["profileName"] = value; }
    }

		[ConfigurationProperty("groupSize", IsRequired = true)]
    public int GroupSize
    {
        get { return (int)base["groupSize"]; }
        set { base["groupSize"] = value; }
    }
  }
}