namespace Icon.Web.DataAccess.Models
{
    /// <summary>
    /// ItemGroupMember database data model.
    /// </summary>
    public class ItemGroupMember
    {
        /// <summary>
        /// ItemGroupId.
        /// </summary>
        public int ItemGroupId { get; set; }

        /// <summary>
        /// ItemId.
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// Scan Code of the item.
        /// </summary>
        public string ScanCode { get; set; }

        /// <summary>
        /// True if this is the Primary item.
        /// </summary>
        public bool IsPrimary {get; set;}

        /// <summary>
        /// Product Description.
        /// </summary>
        public string ProductDescription { get; set; }

        /// <summary>
        /// Customer Friendly Description.
        /// </summary>
        public string CustomerFriendlyDescription { get; set; }

        /// <summary>
        /// Item Pack.
        /// </summary>
        public string ItemPack { get; set; }

        /// <summary>
        /// Item Retail Size.
        /// </summary>
        public string RetailSize { get; set; }

        /// <summary>
        /// Item Unit of Measure.
        /// </summary>
        public string UOM { get; set; }
    }
}
