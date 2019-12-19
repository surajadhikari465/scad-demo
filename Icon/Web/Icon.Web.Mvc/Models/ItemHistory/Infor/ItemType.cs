using Icon.Web.DataAccess.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Icon.Web.Mvc.Models
{

    public class ItemType
    {
        [JsonProperty("Non Merch Trait")]
        public string NonMerchTrait { get; set; }
        [JsonProperty("Item Type")]
        public string ItemTypeCode { get; set; }
    }
}