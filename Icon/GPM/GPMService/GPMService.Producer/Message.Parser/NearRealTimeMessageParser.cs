using GPMService.Producer.Model;
using System.IO;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Esb;
using GPMService.Producer.Serializer;

namespace GPMService.Producer.Message.Parser
{
    internal class NearRealTimeMessageParser : IMessageParser<items>
    {
        private readonly ISerializer<items> serializer;
        private TextReader textReader;

        public NearRealTimeMessageParser(ISerializer<items> serializer)
        {
            this.serializer = serializer;
        }
        public items ParseMessage(ReceivedMessage receivedMessage)
        {
            items gpmParsed;
            using (textReader = new StringReader(Utility.RemoveUnusableCharactersFromXml(receivedMessage.sqsExtendedClientMessage.S3Details[0].Data)))
            {
                gpmParsed = serializer.Deserialize(textReader);
            }
            return gpmParsed;
        }
    }
}
