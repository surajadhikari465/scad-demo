using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace WebSupport.Models
{
    [XmlRoot(Namespace = "http://schemas.wfm.com/Mammoth/JobSchedule/V1", IsNullable = false)]
    public class JobScheduleModel
    {
        [XmlElement]
        public int JobScheduleId { get; set; }
        [XmlElement]
        public string JobName { get; set; }
        [XmlElement]
        public string Region { get; set; }
        [XmlElement]
        public string DestinationQueueName { get; set; }
        [XmlElement]
        [DisplayFormat(DataFormatString = "{0:"+ ValidationConstants.XmlDateTimeFormat + "}", ApplyFormatInEditMode = true)]
        public DateTime StartDateTimeUtc { get; set; }
        [XmlElement]
        public DateTime? LastRunDateTimeUtc { get; set; }
        [XmlElement]
        public int IntervalInSeconds { get; set; }
        [XmlElement]
        public bool Enabled { get; set; }
        [XmlElement]
        public string XmlObject { get; set; }
    }
}