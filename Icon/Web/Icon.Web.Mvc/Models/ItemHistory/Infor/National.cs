using Newtonsoft.Json;

namespace Icon.Web.Mvc.Models
{
    public class National
    {
        [JsonProperty("Family")]
        public HierarchyItem Family { get; set; }
        [JsonProperty("Category")]
        public HierarchyItem Category { get; set; }
        [JsonProperty("Sub Category")]
        public HierarchyItem SubCategory { get; set; }
        [JsonProperty("Class")]
        public HierarchyItem Class { get; set; }

    }
}