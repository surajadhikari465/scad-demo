using Icon.Esb.Schemas.Wfm.Contracts;

namespace Mammoth.PrimeAffinity.Library.MessageBuilders
{
    public class PrimeAffinityMessageModel
    {
        public ActionEnum MessageAction { get; set; }
        public string Region { get; set; }
        public int ItemID { get; set; }
        public int BusinessUnitID { get; set; }
        public string ScanCode { get; set; }
        public string ItemTypeCode { get; set; }
        public string StoreName { get; set; }
        public object InternalPriceObject { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDetails { get; set; }
    }
}
