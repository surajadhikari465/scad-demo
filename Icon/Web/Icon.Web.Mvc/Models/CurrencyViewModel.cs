
namespace Icon.Web.Mvc.Models
{
    public class CurrencyViewModel
    {
        public int CurrencyTypeID { get; set; }
        public string CurrencyTypeCode { get; set; }
        public string CurrencyTypeDesc { get; set; }
        public string IssuingEntity { get; set; }
        public int? NumericCode { get; set; }
        public int? MinorUnit { get; set; }
        public string Symbol { get; set; }
    }
}
