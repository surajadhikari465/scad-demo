using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.NewItem.Models
{
    public class NewItemModel
    {
        public string Region { get; set; }
        public int QueueId { get; set; }
        public int ItemKey { get; set; }
        public string ScanCode { get; set; }
        public bool IsRetailSale { get; set; }
        public bool IsDefaultIdentifier { get; set; }
        public string ItemDescription { get; set; }
        public string PosDescription { get; set; }
        public string BrandName { get; set; }
        public int? IconBrandId { get; set; }
        public decimal PackageUnit { get; set; }
        public decimal RetailSize { get; set; }
        public string RetailUom { get; set; }
        public bool FoodStampEligible { get; set; }
        public string SubTeamName { get; set; }
        public string SubTeamNumber { get; set; }
        public string NationalClassCode { get; set; }
        public string TaxClassCode { get; set; }
        public bool Organic { get; set; }
		public string CustomerFriendlyDescription { get; set; }
		public DateTime QueueInsertDate { get; set; }
        public int? IconItemId { get; set; }
        public bool MessageSentToInfor { get; set; }
        public int? MessageHistoryId { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDetails { get; set; }
    }
}
