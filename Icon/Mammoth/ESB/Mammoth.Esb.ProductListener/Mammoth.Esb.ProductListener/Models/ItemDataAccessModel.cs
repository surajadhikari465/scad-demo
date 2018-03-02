using System;

namespace Mammoth.Esb.ProductListener.Models
{
    public class ItemDataAccessModel
    {
        public int ItemID { get; set; }
        public int ItemTypeID { get; set; }
        public string ItemTypeCode { get; set; }
        public string ScanCode { get; set; }
        public int HierarchyMerchandiseID { get; set; }
        public int HierarchyNationalClassID { get; set; }
        public int BrandHCID { get; set; }
        public int TaxClassHCID { get; set; }
        public int PSNumber { get; set; }
        public string Desc_Product { get; set; }
        public string Desc_POS { get; set; }
        public int PackageUnit { get; set; }
        public int RetailSize { get; set; }
        public string RetailUOM { get; set; }
        public bool FoodStampEligible { get; set; }
        public string Desc_CustomerFriendly { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
