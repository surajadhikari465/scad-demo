using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonLoad.IconItemLocale
{
    public class ItemLocaleModelForIrma
    {
        /// <summary>
        /// dbo.Store
        /// </summary>
        public int BusinessUnit { get; set; }

        /// <summary>
        /// dbo.ItemIdentifier
        /// </summary>
        public string Identifier { get; set; }
        
        /// <summary>
        /// dbo.ValidatedScanCode 
        /// </summary>
        public int InforItemId { get; set; }

        /// <summary>
        /// dbo.ValidatedScanCode 
        /// </summary>
        public string ItemTypeCode { get; set; }

        /// <summary>
        /// dbo.StoreItem 
        /// </summary>
        public bool Authorized { get; set; }
        
        /// <summary>
        /// ItemOverride | Item | Price
        /// </summary>
        public bool LockedForSale { get; set; }

        /// <summary>
        /// dbo.ItemOverride | dbo.Item
        /// </summary>
        public bool? Recall { get; set; }

        /// <summary>
        /// dbo.ItemOverride | dbo.Item
        /// </summary>
        public bool Quantity_Required { get; set; }

        /// <summary>
        /// dbo.ItemOverride | dbo.Item
        /// </summary>
        public bool Price_Required { get; set; }

        /// <summary>
        /// dbo.ItemOverride | dbo.Item
        /// </summary>
        public bool? QtyProhibit { get; set; }

        /// <summary>
        /// dbo.ItemOverride | dbo.Item
        /// </summary>
        public decimal RetailSize { get; set; }

        /// <summary>
        /// dbo.ItemOverride | dbo.Item
        /// </summary>
        public bool Case_Discount { get; set; }

        /// <summary>
        /// dbo.ItemUomOverride | dbo.ItemOveride | dbo.Item => Retail_Unit_ID => dbo.ItemUnit 
        /// </summary>
        public bool Sold_By_Weight { get; set; }

        /// <summary>
        ///  dbo.ItemOveride | dbo.Item => Retail_Unit_ID => dbo.ItemUnit
        /// </summary>
        public string RetailUom { get; set; }

        /// <summary>
        /// dbo.ItemOveride | dbo.Item => Retail_Unit_ID => dbo.ItemUnit 
        /// </summary>
        public string PackageUnit { get; set; }

        /// <summary>
        /// dbo.Price
        /// </summary>
        public bool VisualVerify { get; set; }

        /// <summary>
        /// dbo.Price
        /// </summary>
        public int? PosScaleTare { get; set; }

        /// <summary>
        /// dbo.Price 
        /// </summary>
        public bool TMDiscountEligible { get; set; }

        /// <summary>
        /// dbo.Price
        /// </summary>
        public int? AgeCode { get; set; }

        /// <summary>
        /// dbo.Price
        /// </summary>
        public bool Restricted_Hours { get; set; }

        /// <summary>
        /// dbo.Price.LinkedItem => dbo.ValidatedScanCode
        /// </summary>
        public int LinkedItem_InforItemId { get; set; }

        /// <summary>
        /// dbo.Price.LinkedItem => dbo.ValidatedScanCode
        /// </summary>
        public string LinkedItem_Identifier { get; set; }

        /// <summary>
        /// dbo.Price.LinkedItem => dbo.ValidatedScanCode
        /// </summary>
        public string LinkedItem_Type { get; set; } 
    }
}
