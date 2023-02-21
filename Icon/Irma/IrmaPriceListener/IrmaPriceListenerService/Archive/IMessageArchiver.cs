using Icon.Esb.Schemas.Mammoth;
using IrmaPriceListenerService.Model;
using System.Collections.Generic;
using Wfm.Aws.ExtendedClient.SQS.Model;

namespace IrmaPriceListenerService.Archive
{
    public interface IMessageArchiver
    {
        void ArchivePriceMessage(SQSExtendedClientReceiveModel message, IList<MammothPriceType> mammothPrices, IList<MammothPriceWithErrorType> mammothPriceWithErrors);
    }
}
