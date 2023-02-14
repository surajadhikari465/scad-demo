using System.IO;
using System.Xml.Serialization;
using Icon.Esb;
using Services.Extract.Models;

namespace Services.Extract.Message.Parser
{
    internal class JobScheduleMessageParser : IMessageParser<JobSchedule>
    {
        public JobSchedule ParseMessage(string receivedMessage)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(JobSchedule));
            StringReader stringReader  = new StringReader(Utility.RemoveUnusableCharactersFromXml(receivedMessage));
            JobSchedule jobScheduleParsed = (JobSchedule)serializer.Deserialize(stringReader);

            return jobScheduleParsed;
        }
    }
}