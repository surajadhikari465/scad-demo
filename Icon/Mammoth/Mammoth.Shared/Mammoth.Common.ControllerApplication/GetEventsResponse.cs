using Mammoth.Common.ControllerApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Common.ControllerApplication
{
    public class ChangeQueueEvents<T>
    {
        public ChangeQueueEvents()
        {
            QueuedEvents = new List<EventQueueModel>();
            EventModels = new List<T>();
        }

        public List<EventQueueModel> QueuedEvents { get; set; }
        public List<T> EventModels { get; set; }

        public void ClearEvents()
        {
            if (QueuedEvents != null)
            {
                QueuedEvents.Clear();
            }
            if (EventModels != null)
            {
                EventModels.Clear();
            }
        }
    }
}
