using Icon.Dvs.MessageParser;
using Icon.Dvs.Model;
using Icon.Esb.Schemas.Mammoth;

namespace MammothR10Price.Message.Parser
{
    public class MammothPriceParser : MessageParserBase<MammothPricesType, MammothPricesType>
    {
        public override MammothPricesType ParseMessage(DvsMessage message)
        {
            MammothPricesType parsedPrice = DeserializeMessage(message);
            return parsedPrice;
        }
    }
}
