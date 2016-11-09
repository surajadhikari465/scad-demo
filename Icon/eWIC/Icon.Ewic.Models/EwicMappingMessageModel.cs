
namespace Icon.Ewic.Models
{
    public class EwicMappingMessageModel
    {
        public int ActionTypeId { get; set; }
        public string AplScanCode { get; set; }
        public string WfmScanCode { get; set; }
        public string AgencyId { get; set; }
        public string ProductDescription { get; set; }
        public int MessageHistoryId { get; set; }
        public string SerializedMessage { get; set; }
    }
}
