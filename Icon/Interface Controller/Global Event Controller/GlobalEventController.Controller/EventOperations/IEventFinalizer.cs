using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.Controller.EventOperations
{
    public interface IEventFinalizer
    {
        void HandleFailedEvents();
        void DeleteEvents();
    }
}
