using System.Collections.Generic;

namespace Icon.Esb.R10Listener.Models
{
    public class R10MessageResponseModel
    {
        public int MessageHistoryId { get; set; }
        public bool RequestSuccess { get; set; }
        public bool SystemError { get; set; }
        public string FailureReasonCode { get; set; }
        public string ResponseText { get; set; }
        public IEnumerable<BusinessErrorModel> BusinessErrors { get; set; }
    }
}
