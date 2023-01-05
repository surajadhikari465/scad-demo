using System.Collections.Generic;

namespace MammothR10Price.Publish
{
    public interface IErrorEventPublisher
    {
        void PublishErrorEvent(
            string applicationName,
            string messageId,
            IDictionary<string, string> messageProperties,
            string message,
            string errorCode,
            string errorDetails,
            string errorSeverity = Constants.ErrorSeverity.Error
        );
    }
}
