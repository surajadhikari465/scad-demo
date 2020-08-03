namespace Icon.Web.DataAccess.Models
{
    /// <summary>
    /// Item Group Association Auto-Complete Data.
    /// Data used by the AddToItem AutoComplete UI.
    /// All properties in camel case for Json serialization..
    /// </summary>
    public class ItemGroupAssociationAutoCompleteData
    {
        /// <summary>
        /// Item Id.
        /// </summary>
        public int itemId { get; set; }

        /// <summary>
        /// Scan Code.
        /// </summary>
        public string scanCode { get; set; }

        /// <summary>
        /// Item Description.
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// Current ItemGroup ID for the Item.
        /// </summary>
        public int previousPriceLineId { get; set; }

        /// <summary>
        /// Current ItemGroup Descrition for the Item.
        /// </summary>
        public string previousPriceLine { get; set; }

        /// <summary>
        /// True if the item is currently the primary.
        /// </summary>
        public bool isPrimary { get; set; }

        /// <summary>
        /// True if this is the last item in its current item group.
        /// </summary>
        public bool isLastInpreviousPriceLine { get; set; }
    }
}
