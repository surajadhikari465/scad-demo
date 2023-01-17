using Icon.Dvs.Model;
using System;

namespace IrmaPriceListenerService.Archive
{
    public interface IErrorEventPublisher
    {
        void PublishErrorMessage(DvsMessage message, Exception ex);
    }
}
