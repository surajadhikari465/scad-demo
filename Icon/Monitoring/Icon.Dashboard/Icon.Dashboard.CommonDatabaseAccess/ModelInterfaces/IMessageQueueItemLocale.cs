using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.CommonDatabaseAccess
{
    public interface IMessageQueueItemLocale
    {
        int MessageQueueId { get; set; }
        int MessageTypeId { get; set; }
        int MessageStatusId { get; set; }
        int? MessageHistoryId { get; set; }
        int MessageActionId { get; set; }
        int IRMAPushID { get; set; }
        DateTime InsertDate { get; set; }
        string RegionCode { get; set; }
        int BusinessUnit_ID { get; set; }
        int ItemId { get; set; }
        string ItemTypeCode { get; set; }
        string ItemTypeDesc { get; set; }
        int LocaleId { get; set; }
        string LocaleName { get; set; }
        int ScanCodeId { get; set; }
        string ScanCode { get; set; }
        int ScanCodeTypeId { get; set; }
        string ScanCodeTypeDesc { get; set; }
        string ChangeType { get; set; }
        bool LockedForSale { get; set; }
        bool Recall { get; set; }
        bool TMDiscountEligible { get; set; }
        bool Case_Discount { get; set; }
        int? AgeCode { get; set; }
        bool Restricted_Hours { get; set; }
        bool Sold_By_Weight { get; set; }
        bool ScaleForcedTare { get; set; }
        bool Quantity_Required { get; set; }
        bool Price_Required { get; set; }
        bool QtyProhibit { get; set; }
        bool VisualVerify { get; set; }
        string LinkedItemScanCode { get; set; }
        string PreviousLinkedItemScanCode { get; set; }
        int? PosScaleTare { get; set; }
        int? InProcessBy { get; set; }
        DateTime? ProcessedDate { get; set; }
    }
}
