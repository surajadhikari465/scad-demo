using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceController.Common;

namespace GlobalEventController.Controller.EventServices
{
    public interface IEventServiceProvider
    {
        IEventService GetEventService(Enums.EventNames eventName, string region);
        IBulkEventService GetBulkItemEventService(string region);
        IEventService GetBrandNameUpdateEventService(Enums.EventNames eventName, string region);
        IEventService GetTaxEventService(Enums.EventNames eventName, string region);
        IBulkItemSubTeamEventService GetBulkItemSubTeamEventService(string region);
        IEventService GetSubTeamEventService(Enums.EventNames eventName, string region);
        IEventService GetItemSubTeamEventService(Enums.EventNames eventName, string region);
        IBulkEventService GetBulkItemNutriFactsEventService(string region);
        void RefreshContexts();
    }
}
