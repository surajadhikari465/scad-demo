using GPMService.Producer.Model;
using Icon.Esb;
using System.IO;
using System.Xml.Serialization;

namespace GPMService.Producer.Message.Parser
{
    internal class ActivePriceMessageParser : IMessageParser<JobSchedule>
    {
        private readonly XmlSerializer serializer;
        private TextReader textReader;

        public ActivePriceMessageParser()
        {
            serializer = new XmlSerializer(typeof(JobSchedule));
        }
        public JobSchedule ParseMessage(ReceivedMessage receivedMessage)
        {
            JobSchedule activePriceJobScheduleParsed;
            using (textReader = new StringReader(Utility.RemoveUnusableCharactersFromXml(receivedMessage.esbMessage.MessageText)))
            {
                activePriceJobScheduleParsed = serializer.Deserialize(textReader) as JobSchedule;
            }
            return activePriceJobScheduleParsed;
        }
    }
}
