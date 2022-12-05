using System.Collections.Generic;
using Icon.Esb.Schemas.Mammoth;

namespace MammothR10Price.Message.Archive
{
    public interface IMessageArchiver
    {
        void ArchiveMessage(IList<MammothPriceType> mammothPrices, string itemPriceXml, IDictionary<string, string> messageProperties);
    }
}
