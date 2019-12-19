using System.Collections.Generic;

namespace Icon.Web.Mvc.Models
{
    public class ItemSearchGridResponseViewModel
    {
        public List<ItemViewModel> Records { get; set; }
        public int TotalRecordsCount { get; set; }
    }
}
