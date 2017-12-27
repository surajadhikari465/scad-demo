using Icon.Common.DataAccess;
using System.Collections.Generic;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Queries
{
    public class GetGpmPricesParameters : IQuery<List<GpmPrice>>
    {
        public string Region { get; set; }
        public string BusinessUnitId { get; set; }
        public string ScanCode { get; set; }
    }
}