using System;

namespace PushController.Common
{
    // This model mirrors ItemPrice, with the addition of the IrmaPushId property.  There is no FK relationship between IRMAPush and ItemPrice, so this model will
    // allow a connection to be maintained between the two during UDM processing.

    public class ItemPriceModel
    {
        public int IrmaPushId { get; set; }
        public int ItemId { get; set; }
        public int LocaleId { get; set; }
        public int ItemPriceTypeId { get; set; }
        public int UomId { get; set; }
        public int CurrencyTypeId { get; set; }
        public decimal ItemPriceAmount { get; set; }
        public int? BreakPointStartQuantity { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
