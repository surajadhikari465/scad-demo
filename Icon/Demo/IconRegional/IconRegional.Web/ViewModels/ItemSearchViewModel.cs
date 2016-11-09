using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IconRegional.Web.ViewModels
{
    public class ItemSearchViewModel
    {
        public string ScanCodes { get; set; }
        public int Page { get; set; }
        public int Take => 10;
        public int Skip => Page * Take;
    }
}
