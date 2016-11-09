using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Common.ControllerApplication
{
    public static class ExceptionManager
    {
        private const string UnableToConnectToServerExceptionMessage = "Unable to connect";
        private const string TimeoutExceptionMessage = "Timeout";

        public static string GetErrorCode(Exception exception)
        {
            if(exception?.Message != null && exception.Message.Contains(UnableToConnectToServerExceptionMessage))
            {
                return ApplicationErrors.UnableToConnectToWebServerErrorCode;
            }
            else if(exception?.Message != null && exception.Message.Contains(TimeoutExceptionMessage))
            {
                return ApplicationErrors.TimeoutErrorCode;
            }
            else if(exception?.Message != null && exception.InnerException != null)
            {
                return GetErrorCode(exception.InnerException);
            }
            else
            {
                return ApplicationErrors.UnexpectedExceptionErrorCode;
            }
        }

        public static string GetErrorDescription(string errorCode)
        {
            if(errorCode == ApplicationErrors.InvalidDataErrorCode)
            {
                return ApplicationErrors.InvalidDataErrorDescription;
            }
            else
            {
                return errorCode;
            }
        }
    }
}
