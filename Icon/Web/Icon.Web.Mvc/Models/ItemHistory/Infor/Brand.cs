using Newtonsoft.Json;

namespace Icon.Web.Mvc.Models
{
    public class Brand
    {
        [JsonProperty("Brand ID")]
        public string BrandId { get; set; }
        [JsonProperty("Brand Name")]
        public string BrandName { get; set; }
        [JsonProperty("Brand Abbreviation")]
        public string BrandAbbreviation { get; set; }

    }
}