using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Mvc.Models
{
    public class ItemSearchGridResponseViewModel
    {
        public List<ItemViewModel> Records { get; set; }
        public int TotalRecordsCount { get; set; }
    }
}
