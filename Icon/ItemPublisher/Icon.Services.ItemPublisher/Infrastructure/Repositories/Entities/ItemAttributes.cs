using System;

namespace Icon.Services.NewItem.Repositories.Entities
{
    public class ItemAttributes
    {
        public int ItemId { get; set; }
        public string ProductDescription { get; set; }
        public string POSDescription { get; set; }
        public string CustomerFriendlyDescription { get; set; }
        public short PackageUnit { get; set; }
        public decimal RetailSize { get; set; }
        public string RetailUOM { get; set; }
        public bool FoodStampEligible { get; set; }
        public decimal POSScaleTare { get; set; }
        public bool ProhibitDiscount { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime SysStartTime { get; set; }
        public DateTime SysEndTime { get; set; }
    }
}