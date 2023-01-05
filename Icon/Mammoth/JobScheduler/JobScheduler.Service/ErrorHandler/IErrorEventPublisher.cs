using JobScheduler.Service.Helper;
using System.Collections.Generic;

namespace JobScheduler.Service.ErrorHandler
{
    internal interface IErrorEventPublisher
    {
        void PublishErrorEvent(
            string applicationName,
            string messageID,
            Dictionary<string, string> messageProperties,
            string message,
            string errorCode,
            string errorDetails,
            string errorSeverity = Constants.ErrorSeverity.Error
            );
    }
}
