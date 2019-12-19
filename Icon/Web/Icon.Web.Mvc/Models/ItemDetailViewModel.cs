using System.Collections.Generic;

namespace Icon.Web.Mvc.Models
{
    public class ItemDetailViewModel
    {
        public ItemViewModel ItemViewModel { get; set; }
        public IEnumerable<AttributeViewModel> Attributes { get; set; }

        public ItemHistoryViewModel ItemHistoryModel { get; set; }
    }
}