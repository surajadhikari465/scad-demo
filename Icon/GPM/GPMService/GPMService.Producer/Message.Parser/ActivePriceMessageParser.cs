using GPMService.Producer.Model;
using GPMService.Producer.Serializer;
using Icon.Esb;
using Icon.Esb.Schemas.Mammoth;
using System.IO;

namespace GPMService.Producer.Message.Parser
{
    internal class ActivePriceMessageParser : IMessageParser<JobSchedule>
    {
        private readonly ISerializer<JobSchedule> serializer;
        private TextReader textReader;

        public ActivePriceMessageParser(ISerializer<JobSchedule> serializer)
        {
            this.serializer = serializer;
        }
        public JobSchedule ParseMessage(ReceivedMessage receivedMessage)
        {
            JobSchedule activePriceJobScheduleParsed;
            using (textReader = new StringReader(Utility.RemoveUnusableCharactersFromXml(receivedMessage.esbMessage.MessageText)))
            {
                activePriceJobScheduleParsed = serializer.Deserialize(textReader);
            }
            return activePriceJobScheduleParsed;
        }
    }
}
