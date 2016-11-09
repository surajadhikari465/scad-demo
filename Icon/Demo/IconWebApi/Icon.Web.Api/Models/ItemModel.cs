using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Api.Models
{
    public class ItemModel
    {
        public string ScanCode { get; set; }
        public string Brand { get; set; }
        public string ProductDescription { get; set; }
        public string PosDescription { get; set; }
        public string PackageUnit { get; set; }
        public bool FoodStampEligible { get; set; }
        public string PosScaleTare { get; set; }
        public string RetailSize { get; set; }
        public string RetailUom { get; set; }
        public string MerchandiseHierarchyName { get; set; }
        public string NationalHierarchyName { get; set; }
        public string TaxHierarchyName { get; set; }
    }
}