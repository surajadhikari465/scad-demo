using Newtonsoft.Json;

namespace Icon.Web.Mvc.Models
{
    public class Merchandise
    {
        [JsonProperty("Family")]
        public HierarchyItem Family { get; set; }

        [JsonProperty("Segment")]
        public HierarchyItem Segment { get; set; }

        [JsonProperty("Class")]
        public HierarchyItem Class { get; set; }

        [JsonProperty("GS1 Brick")]
        public HierarchyItem GS1Brick { get; set; }

        [JsonProperty("Sub Brick")]
        public HierarchyItem SubBrick { get; set; }
    }
}