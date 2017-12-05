using System;

namespace Mammoth.Esb.ProductListener.Models
{
    public class GlobalAttributesModel
    {
        public int ItemID { get; set; }
        public int ItemTypeID { get; set; }
        public string ScanCode { get; set; }
        public int? SubBrickID { get; set; }
        public int? NationalClassID { get; set; }
        public int? BrandHCID { get; set; }
        public int? TaxClassHCID { get; set; }
        public string Desc_Product { get; set; }
        public string Desc_POS { get; set; }
        public string PackageUnit { get; set; }
        public string RetailSize { get; set; }
        public string RetailUOM { get; set; }
        public int? PSNumber { get; set; }
        public bool? FoodStampEligible { get; set; }
        public string Desc_CustomerFriendly { get; set; }
        public string MessageTaxClassHCID { get; set; }
    }
}