using Icon.Esb;
using Icon.Esb.MessageParsers;
using Icon.Esb.Schemas.Mammoth;
using Icon.Esb.Subscriber;
using MammothR10Price.Serializer;
using System.IO;

namespace MammothR10Price.Message.Parser
{
    internal class MammothPriceParser : IMessageParser<MammothPricesType>
    {
        private readonly ISerializer<MammothPricesType> serializer;

        public MammothPriceParser(ISerializer<MammothPricesType> serializer)
        {
            this.serializer = serializer;
        }

        public MammothPricesType ParseMessage(IEsbMessage receivedMessage)
        {
            MammothPricesType parsedPrice;
            using (TextReader textReader = new StringReader(Utility.RemoveUnusableCharactersFromXml(receivedMessage.MessageText)))
            {
                parsedPrice = serializer.Deserialize(textReader);
            }
            return parsedPrice;
        }
    }
}
