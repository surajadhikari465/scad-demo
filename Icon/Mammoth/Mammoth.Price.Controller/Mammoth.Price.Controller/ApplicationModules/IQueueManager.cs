using Mammoth.Common.ControllerApplication.Models;
using System.Collections.Generic;

namespace Mammoth.Price.Controller.ApplicationModules
{
    public interface IQueueManager
    {
        List<EventQueueModel> GetEvents();
        void DeleteInProcessEvents();
    }
}
