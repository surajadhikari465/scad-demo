using System.Collections.Generic;
using System.Linq;
using Icon.Common;

namespace Icon.Web.Mvc.Models
{
    public class ItemEditViewModel
    {
        public ItemViewModel ItemViewModel { get; set; }
        public List<AttributeViewModel> Attributes { get; set; }
        public bool Success { get; set; }
        public ItemHistoryViewModel ItemHistoryModel { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public Dictionary<string, string> OrderOfFields { get; set; }
    }
}