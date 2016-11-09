using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegionalEventController.Controller.ProcessorModules
{
    public interface IEventFinalizer
    {
        void HandleFailedEvents();
        void DeleteEvents();
    }
}
