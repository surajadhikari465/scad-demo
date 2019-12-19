using Newtonsoft.Json;

namespace Icon.Web.Mvc.Models
{
    public class Tax
    {
        [JsonProperty("Tax Hier ID")]
        public string TaxHierarchyId { get; set; }
        [JsonProperty("Tax Class ID")]
        public string TaxClassId { get; set; }
        [JsonProperty("Tax Class")]
        public string TaxClass { get; set; }
        [JsonProperty("Tax Abbreviation")]
        public string TaxAbbreviation { get; set; }
        [JsonProperty("Tax Romance")]
        public string TaxRomance { get; set; }
    }
}