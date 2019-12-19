using System.IO;
using System.Xml.Serialization;
using Icon.Esb.MessageParsers;
using Icon.Esb.Subscriber;
using Services.Extract.Models;

namespace Services.Extract
{
    public class JobScheduleMessageParser : IMessageParser<JobSchedule>
    {
        public JobSchedule ParseMessage(IEsbMessage message)
        {
            //var xmlDocument = XDocument.Parse(message.MessageText);
            //var parsedJobSchedule = new JobSchedule();

            XmlSerializer ser = new XmlSerializer(typeof(JobSchedule));
            StringReader stringReader  = new StringReader(message.MessageText);
            JobSchedule schedule = (JobSchedule) ser.Deserialize(stringReader);

            return schedule;
        }
    }
}