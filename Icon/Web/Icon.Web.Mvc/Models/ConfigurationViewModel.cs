using Icon.Web.DataAccess.Models;

namespace Icon.Web.Mvc.Models
{
    public class ConfigurationViewModel
    {
        public int RegionalSettingsId { get; set; }
        public string Description { get; set; }
        public string RegionCode { get; set; }
        public string KeyName { get; set; }
        public string SectionName { get; set; }
        public bool Value { get; set; }        
        public ConfigurationViewModel() {}

        public ConfigurationViewModel(RegionalSettingsModel irmaItem)
        {
            this.RegionalSettingsId = irmaItem.RegionalSettingsId;
            this.Description = irmaItem.Description;
            this.KeyName = irmaItem.KeyName;
            this.RegionCode = irmaItem.RegionCode;
            this.SectionName = irmaItem.SectionName;
            this.Value = irmaItem.Value;
        }
      
    }
}