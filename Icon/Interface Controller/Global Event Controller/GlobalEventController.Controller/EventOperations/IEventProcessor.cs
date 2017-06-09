using Icon.Framework;
using System.Collections.Generic;

namespace GlobalEventController.Controller.EventOperations
{
    public interface IEventProcessor
    {
        void ProcessBrandNameUpdateEvents();
        void ProcessTaxEvents();
        void ProcessBrandDeleteEvents();
        void ProcessNationalClassAddOrUpdateEvents();
        void ProcessNationalClassDeleteEvents();
    }
}
