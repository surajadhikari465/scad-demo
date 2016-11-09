
namespace Icon.Esb.EwicErrorResponseListener.Common.Models
{
    public class EwicErrorResponseModel
    {
        public int MessageHistoryId { get; set; }
        public bool RequestSuccess { get; set; }
        public bool SystemError { get; set; }
        public string ResponseText { get; set; }
        public string ResponseReason { get; set; }
    }
}
