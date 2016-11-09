using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetIrmaItemsParameters : IQuery<List<IRMAItem>>
    {
        public int? IrmaItemId { get; set; }
        public string Identifier { get; set; }
        public string ItemDescription { get; set; }
        public string Brand { get; set; }
        public string RegionCode { get; set; }
        public bool PartialScanCode { get; set; }
        public string TaxRomanceName { get; set; }
    }
}

