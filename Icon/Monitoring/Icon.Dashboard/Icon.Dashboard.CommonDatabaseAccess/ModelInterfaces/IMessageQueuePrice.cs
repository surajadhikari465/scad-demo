using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.CommonDatabaseAccess
{
    public interface IMessageQueuePrice
    {
        int MessageQueueId { get; set; }
        int MessageTypeId { get; set; }
        int MessageStatusId { get; set; }
        int? MessageHistoryId { get; set; }
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
        string UomCode { get; set; }
        string UomName { get; set; }
        string CurrencyCode { get; set; }
        decimal? Price { get; set; }
        int? Multiple { get; set; }
        decimal? SalePrice { get; set; }
        int? SaleMultiple { get; set; }
        DateTime? SaleStartDate { get; set; }
        DateTime? SaleEndDate { get; set; }
        decimal? PreviousSalePrice { get; set; }
        int? PreviousSaleMultiple { get; set; }
        DateTime? PreviousSaleStartDate { get; set; }
        DateTime? PreviousSaleEndDate { get; set; }
        int? InProcessBy { get; set; }
        DateTime? ProcessedDate { get; set; }
    }
}
