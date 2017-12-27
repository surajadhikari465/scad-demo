using System;

namespace MammothWebApi.DataAccess.Models
{
    public partial class ItemAttributes_Locale
    {
        public string Region { get; set; }
        public int ItemAttributeLocaleID { get; set; }
        public int ItemID { get; set; }
        public int BusinessUnitID { get; set; }
        public bool Discount_Case { get; set; }
        public bool Discount_TM { get; set; }
        public int? Restriction_Age { get; set; }
        public bool Restriction_Hours { get; set; }
        public bool Authorized { get; set; }
        public bool Discontinued { get; set; }
        public bool LocalItem { get; set; }
        public bool ScaleItem { get; set; }
        public bool OrderedByInfor { get; set; }
        public string DefaultScanCode { get; set; }
        public string LabelTypeDesc { get; set; }
        public string Product_Code { get; set; }
        public string RetailUnit { get; set; }
        public string Sign_Desc { get; set; }
        public string Locality { get; set; }
        public string Sign_RomanceText_Long { get; set; }
        public string Sign_RomanceText_Short { get; set; }
        public string AltRetailUOM { get; set; }
        public decimal AltRetailSize { get; set; }
        public decimal Msrp { get; set; }
        public System.DateTime AddedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
