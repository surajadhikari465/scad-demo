
namespace Icon.Ewic.Models
{
    public class EwicExclusionMessageModel
    {
        public int ActionTypeId { get; set; }
        public string ScanCode { get; set; }
        public string AgencyId { get; set; }
        public string ProductDescription { get; set; }
        public int MessageHistoryId { get; set; }
        public string SerializedMessage { get; set; }
    }
}
