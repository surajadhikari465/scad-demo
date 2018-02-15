using System;

namespace Icon.Monitoring.DataAccess.Model
{
    public class JobSchedule
    {
        public int JobScheduleId { get; set; }
        public string JobName { get; set; }
        public string Region { get; set; }
        public string DestinationQueueName { get; set; }
        public DateTime StartDateTimeUtc { get; set; }
        public DateTime? LastScheduledDateTimeUtc { get; set; }
        public DateTime? LastRunEndDateTimeUtc { get; set; }
        public DateTime NextScheduledDateTimeUtc { get; set; }
        public int IntervalInSeconds { get; set; }
        public bool Enabled { get; set; }
        public string Status { get; set; }
        public string XmlObject { get; set; }
    }
}
