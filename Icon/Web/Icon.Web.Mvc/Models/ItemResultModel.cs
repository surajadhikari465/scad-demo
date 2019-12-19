using System.Collections.Generic;

namespace Icon.Web.Mvc.Models
{
    public class ItemResultModel
    {
        public int TotalRecordsCount { get; set; }
        public string Query { get; set; }
        public List<Dictionary<string,object>> Records { get; set; }
    }
}