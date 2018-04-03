using Mammoth.Common.ControllerApplication.Models;
using System.Collections.Generic;

namespace Mammoth.Price.Controller.EventProcessors
{
    public interface IEventProcessor<T>
    {
        void ProcessEvents(List<EventQueueModel> events);
    }
}
