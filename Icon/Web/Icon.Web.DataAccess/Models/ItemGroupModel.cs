using System;

namespace Icon.Web.DataAccess.Models
{
    /// <summary>
    /// Sku/Priceline Model.
    /// </summary>
    public class ItemGroupModel
    {
        public const int ItemGroupTypeIdSku = 1;

        /// <summary>
        /// Gets or sets the Item Group Id.
        /// </summary>
        public int ItemGroupId { get; set; }

        /// <summary>
        /// Gets or sets the Item Group Type Id.
        /// </summary>
        public ItemGroupTypeId ItemGroupTypeId { get; set; }

        /// <summary>
        /// Gets or sets the Sku Description.
        /// </summary>
        public string SKUDescription { get; set; }

        /// <summary>
        /// Gets or sets the Price Line Description.
        /// </summary>
        public string PriceLineDescription { get; set; }

        /// <summary>
        /// Gets or sets the Price Line Size.
        /// </summary>
        public string PriceLineSize { get; set; }

        /// <summary>
        /// Gets or sets the Price Line UOM
        /// </summary>
        public string PriceLineUOM { get; set; }

        /// <summary>
        /// Gets or sets the Scan Code.
        /// </summary>
        public string ScanCode { get; set; }

        /// <summary>
        /// Sets or sets the number of items in the itemGroup.
        /// </summary>
        public int? ItemCount { get; set; }

        /// <summary>
        /// Gets or sets the ItemGroupAttributesJson
        /// </summary>
        public string ItemGroupAttributesJson { get; set; }
    }
}
