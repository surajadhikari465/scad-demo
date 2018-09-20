using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KitBuilderWebApi.QueryParameters
{
    public class KitSearchParameters : BaseParameters
    {
        public string ItemDescription { get; set; }
        public string ItemScanCode { get; set; }
        public string LinkGroupName { get; set; }
    }
}
