using System;

namespace Icon.Web.DataAccess.Models
{
    /// <summary>
    /// Sku Model.
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
        public string ItemGroupAttributesJson { get; set; }

        /// <summary>
        /// Gets or sets the Scan Code.
        /// </summary>
        public string ScanCode { get; set; }
    }
}
