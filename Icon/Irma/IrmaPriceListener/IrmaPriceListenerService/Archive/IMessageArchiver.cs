using Icon.Dvs.Model;
using Icon.Esb.Schemas.Mammoth;
using IrmaPriceListenerService.Model;
using System.Collections.Generic;

namespace IrmaPriceListenerService.Archive
{
    public interface IMessageArchiver
    {
        void ArchivePriceMessage(DvsMessage message, IList<MammothPriceType> mammothPrices, IList<MammothPriceWithErrorType> mammothPriceWithErrors);
    }
}
