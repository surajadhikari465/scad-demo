using GPMService.Producer.Model;
using Icon.Esb;
using System.IO;
using System.Xml.Serialization;

namespace GPMService.Producer.Message.Parser
{
    internal class ExpiringTprMessageParser : IMessageParser<JobSchedule>
    {
        private readonly XmlSerializer serializer;
        private TextReader textReader;

        public ExpiringTprMessageParser()
        {
            serializer = new XmlSerializer(typeof(JobSchedule));
        }
        public JobSchedule ParseMessage(ReceivedMessage receivedMessage)
        {
            JobSchedule expiringTprJobScheduleParsed;
            using (textReader = new StringReader(Utility.RemoveUnusableCharactersFromXml(receivedMessage.esbMessage.MessageText)))
            {
                expiringTprJobScheduleParsed = serializer.Deserialize(textReader) as JobSchedule;
            }
            return expiringTprJobScheduleParsed;
        }
    }
}
