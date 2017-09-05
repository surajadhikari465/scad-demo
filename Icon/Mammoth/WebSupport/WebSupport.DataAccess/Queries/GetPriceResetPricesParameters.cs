using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Queries
{
    public class GetPriceResetPricesParameters : IQuery<List<PriceResetPrice>>
    {
        public string Region { get; set; }
        public List<string> BusinessUnitIds { get; set; }
        public List<string> ScanCodes { get; set; }
    }
}
