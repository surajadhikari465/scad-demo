using Icon.Esb;
using Icon.Esb.Schemas.Mammoth;
using IrmaPriceListenerService.Service.Parser;
using IrmaPriceListenerService.Serializer;
using System.IO;
using Wfm.Aws.ExtendedClient.SQS.Model;

namespace IrmaPriceListenerService.Parser
{
    public class MammothPriceParser : IMessageParser<MammothPricesType>
    {
        private readonly ISerializer<MammothPricesType> serializer;
        private TextReader textReader;

        public MammothPriceParser(ISerializer<MammothPricesType> serializer)
        {
            this.serializer = serializer;
        }
        public MammothPricesType ParseMessage(SQSExtendedClientReceiveModel receivedMessage)
        {
            MammothPricesType parsedPrice;
            using (textReader = new StringReader(Utility.RemoveUnusableCharactersFromXml(receivedMessage.S3Details[0].Data)))
            {
                parsedPrice = serializer.Deserialize(textReader);
            }
            return parsedPrice;
        }
    }
}
