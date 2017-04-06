using Icon.Esb.Schemas.Wfm.ContractTypes;
using System;

namespace Icon.Infor.Listeners.Price.Models
{
    public class PriceModel
    {
        public Guid GpmId { get; set; }
        public ActionEnum Action { get; set; }
        public int ItemId { get; set; }
        public int BusinessUnitId { get; set; }
        public PriceTypeIdType PriceType { get; set; }
        public PriceTypeIdType PriceTypeAttribute { get; set; }
        public string SellableUom { get; set; }
        public decimal Price { get; set; }
        public int Multiple { get; set; }
        public string CurrencyCode { get; set; }
        public int CurrencyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? ReplacedGpmId { get; set; }
        public DateTime? NewTagExpiration { get; set; }
        public string Region { get; set; }
        public DateTime AddedDate { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDetails { get; set; }
    }
}
