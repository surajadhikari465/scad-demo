using System;
using GlobalEventController.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GlobalEventController.Controller.EventServices
{
    public interface IBulkItemSubTeamEventService
    {
        int? ReferenceId { get; set; }
        string Message { get; set; }
        string Region { get; set; }
        List<ItemSubTeamModel> ItemSubTeamModelList { get; set; }
        void Run();
    }
}
