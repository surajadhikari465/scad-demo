using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.DataAccess.Models
{
    public class MessageQueuePrice
    {
        public int MessageQueueId { get; set; }
        public int MessageTypeId { get; set; }
        public int MessageStatusId { get; set; }
        public int? MessageHistoryId { get; set; }
        public int MessageActionId { get; set; }
        public DateTime InsertDate { get; set; }
        public int ItemId { get; set; }
        public string ItemTypeCode { get; set; }
        public string ItemTypeDesc { get; set; }
        public int BusinessUnitId { get; set; }
        public string LocaleName { get; set; }
        public string ScanCode { get; set; }
        public string UomCode { get; set; }
        public string CurrencyCode { get; set; }
        public string PriceTypeCode { get; set; }
        public string SubPriceTypeCode { get; set; }
        public decimal Price { get; set; }
        public int Multiple { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? InProcessBy { get; set; }
        public DateTime? ProcessedDate { get; set; }
    }
}
