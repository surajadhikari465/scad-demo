using GPMService.Producer.Model;
using GPMService.Producer.Serializer;
using Icon.Esb;
using Icon.Esb.Schemas.Mammoth;
using System.IO;

namespace GPMService.Producer.Message.Parser
{
    internal class ExpiringTprMessageParser : IMessageParser<JobSchedule>
    {
        private readonly ISerializer<JobSchedule> serializer;
        private TextReader textReader;

        public ExpiringTprMessageParser(ISerializer<JobSchedule> serializer)
        {
            this.serializer = serializer;
        }
        public JobSchedule ParseMessage(ReceivedMessage receivedMessage)
        {
            JobSchedule expiringTprJobScheduleParsed;
            using (textReader = new StringReader(Utility.RemoveUnusableCharactersFromXml(receivedMessage.sqsExtendedClientMessage.S3Details[0].Data)))
            {
                expiringTprJobScheduleParsed = serializer.Deserialize(textReader);
            }
            return expiringTprJobScheduleParsed;
        }
    }
}
