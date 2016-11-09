using System;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Models
{
    public partial class Item
    {
        public Item()
        {
            this.ItemAttributes_Ext = new List<ItemAttributes_Ext>();
        }

        public int ItemID { get; set; }
        public Nullable<int> ItemTypeID { get; set; }
        public string ScanCode { get; set; }
        public Nullable<int> HierarchyMerchandiseID { get; set; }
        public Nullable<int> HierarchyNationalClassID { get; set; }
        public Nullable<int> BrandHCID { get; set; }
        public Nullable<int> TaxClassHCID { get; set; }
        public Nullable<int> PSNumber { get; set; }
        public string Desc_Product { get; set; }
        public string Desc_POS { get; set; }
        public string PackageUnit { get; set; }
        public string RetailSize { get; set; }
        public string RetailUOM { get; set; }
        public Nullable<bool> FoodStampEligible { get; set; }
        public System.DateTime AddedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public virtual ICollection<ItemAttributes_Ext> ItemAttributes_Ext { get; set; }
    }
}
