namespace Icon.Web.DataAccess.Models
{
    public class RegionalSettingsModel
    {

        public int RegionalSettingsId { get; set; }
        public string RegionCode { get; set; }
        public string SectionName { get; set; }
        public string KeyName { get; set; }
        public string Description { get; set; }
        public bool Value { get; set; }

    }
}
