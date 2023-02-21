using System;
using Wfm.Aws.ExtendedClient.SQS.Model;

namespace IrmaPriceListenerService.Archive
{
    public interface IErrorEventPublisher
    {
        void PublishErrorMessage(SQSExtendedClientReceiveModel message, Exception ex);
    }
}
