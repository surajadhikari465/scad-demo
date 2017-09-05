namespace Mammoth.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Item
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ItemID { get; set; }

        public int? ItemTypeID { get; set; }

        [StringLength(13)]
        public string ScanCode { get; set; }

        public int? HierarchyMerchandiseID { get; set; }

        public int? HierarchyNationalClassID { get; set; }

        public int? BrandHCID { get; set; }

        public int? TaxClassHCID { get; set; }

        public int? PSNumber { get; set; }

        [StringLength(255)]
        public string Desc_Product { get; set; }

        [StringLength(255)]
        public string Desc_POS { get; set; }

        [StringLength(255)]
        public string PackageUnit { get; set; }

        [StringLength(255)]
        public string RetailSize { get; set; }

        [StringLength(255)]
        public string RetailUOM { get; set; }

        public bool? FoodStampEligible { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime AddedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
