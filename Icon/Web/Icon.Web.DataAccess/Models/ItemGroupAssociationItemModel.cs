namespace Icon.Web.DataAccess.Models
{
    /// <summary>
    /// ItemGroup Association Item Model.
    /// Used to partial search an item
    /// </summary>
    public class ItemGroupAssociationItemModel
    {
        /// <summary>
        /// Item Id.
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// Scan Code.
        /// </summary>
        public string ScanCode { get; set; }

        /// <summary>
        /// Item Customer Friendly Description
        /// </summary>
        public string CustomerFriendlyDescription { get; set; }

        /// <summary>
        /// Current Item Group.
        /// </summary>
        public int ItemGroupId { get; set; }

        /// <summary>
        /// SKU description (null for Priceline)
        /// </summary>
        public string SKUDescription { get; set; }

        /// <summary>
        /// Price line description (null for Sku)
        /// </summary>
        public string PriceLineDescription { get; set; }

        /// <summary>
        /// The item is primary in the item group.
        /// </summary>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Number of items in the item group.
        /// </summary>
        public int? ItemGroupItemCount { get; set; }
    }
}
