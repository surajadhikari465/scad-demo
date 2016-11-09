using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Framework;

namespace RegionalEventController.Controller.ProcessorModules
{
    public interface INewItemProcessingModule
    {
        void CreateEventQueueEntry(int eventId, string eventMessage, int eventReferenceId, string RegionCode);
        void CreateIrmaItemSubscription(string regioncode, string identifier);
        void InsertIrmaItemToIcon(IRMAItem irmaNewItem);
    }
}
