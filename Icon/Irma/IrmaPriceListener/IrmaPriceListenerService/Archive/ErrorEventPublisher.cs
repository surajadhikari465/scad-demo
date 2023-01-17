using Icon.Dvs.Model;
using System;

namespace IrmaPriceListenerService.Archive
{
    public class ErrorEventPublisher : IErrorEventPublisher
    {
        public void PublishErrorMessage(DvsMessage message, Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
