using System.Collections.Generic;
using Icon.Esb.Schemas.Mammoth;

namespace MammothR10Price.Model
{
    public class PriceMessageDetail
    {
        public string ItemID { get; set; }
        public string BusinessUnitID { get; set; }
        public string MessageID { get; set; }
        public IDictionary<string, string> MessageHeaders { get; set; }
        public MammothPriceType MammothPrice { get; set; }
        public string MammothMessageID { get; set; }
    }
}
