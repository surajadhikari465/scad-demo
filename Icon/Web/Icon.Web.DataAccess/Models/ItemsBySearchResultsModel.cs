using System.Collections.Generic;

namespace Icon.Web.DataAccess.Models
{
    public class ItemsBySearchResultsModel
    {
        public List<ItemSearchModel> Items { get; set; }
        public int ItemsCount { get; set; }
    }
}
