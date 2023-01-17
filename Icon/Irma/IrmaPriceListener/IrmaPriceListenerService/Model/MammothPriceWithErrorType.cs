using Icon.Esb.Schemas.Mammoth;

namespace IrmaPriceListenerService.Model
{
    public class MammothPriceWithErrorType
    {
        public MammothPriceType MammothPrice { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDetails { get; set; }
    }
}
