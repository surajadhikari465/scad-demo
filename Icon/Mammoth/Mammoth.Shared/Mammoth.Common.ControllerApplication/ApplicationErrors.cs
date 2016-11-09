using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Common.ControllerApplication
{
    public static class ApplicationErrors
    {
        public const string UnprocessableEventCode = "UnprocessableEvent";
        public const string UnprocessableEventDescription = "Event could not be processed by the Mammoth Controller because the query used to pull the data from IRMA did not return all data for this event.";
        public const string UnableToConnectToWebServerErrorCode = "UnableToConnectToWebServer";
        public const string UnexpectedExceptionErrorCode = "UnexpectedExceptionError";
        public const string TimeoutErrorCode = "TimeoutError";
        public const string InvalidDataErrorCode = "InvalidData";
        public const string InvalidDataErrorDescription = "The item being processed did not have store specific data. This row will be deleted and the data will be processed again when the following fields are populated: Authorized, CaseDiscount (IBM_Discount), or TM Discount (Discountable).";
    }
}
