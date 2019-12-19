using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.NewItem.Models
{
    public class EventQueueModel
    {
        public int QueueId { get; set; }
        public int EventId { get; set; }
        public string EventMessage { get; set; }
        public int EventReferenceId { get; set; }
        public string RegionCode { get; set; }
    }
}
