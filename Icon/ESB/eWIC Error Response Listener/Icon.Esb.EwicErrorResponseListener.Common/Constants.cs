
using System;

namespace Icon.Esb.EwicErrorResponseListener.Common
{
    public class Constants
    {
        public const string UnsuccessfulParse = "eWIC Error Response Listener application error: Failed to parse message.";
        public const string UnsuccessfulSave = "eWIC Error Response Listener application error: Failed to save error response.";
        public const string UnsuccessfulProcessing = "eWIC Error Response Listener application error: Failed while updating Icon message status.";
        public const string AlertNotification = "An R10 error response was received.";
        public const string ResponseReasonNoSequenceNumber = "No Sequence# Returned by R10";
        public const string ErrorResponseReceived = "An error response was received from R10.";
    }
}
