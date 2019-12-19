using Newtonsoft.Json;

namespace Icon.Web.Mvc.Models
{
    public class Financial
    {
        [JsonProperty("Subteam")]
        public string Subteam { get;set;}

        [JsonProperty("Financial Hier ID")]
        public string FinancialHierarchyId { get; set; }

        [JsonProperty("Subteam Name")]
        public string SubteamName { get; set; }
    }
}