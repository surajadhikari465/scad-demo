using System.Collections.Generic;

namespace Icon.Esb.Services.ConfirmationBod
{
  public class ConfirmationBodEsbRequest
    {
        public string BodId { get; set; }
        public string OriginalMessage { get; set; }
        public ConfirmationBodEsbErrorTypes ErrorType { get; set; }
        public string ErrorReasonCode { get; set; }
        public string ErrorDescription { get; set; }
        public string TenantId { get; set; }
        public Dictionary<string, string> EsbMessageProperties { get; set; }
    }
}
