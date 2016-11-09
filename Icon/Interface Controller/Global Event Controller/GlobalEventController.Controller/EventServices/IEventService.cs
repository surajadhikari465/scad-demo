
using GlobalEventController.Common;
using Icon.Framework;
using System.Collections.Generic;
namespace GlobalEventController.Controller.EventServices
{
    public interface IEventService
    {
        int? ReferenceId { get; set; }
        string Message { get; set; }
        string Region { get; set; }
        int EventTypeId { get; set; }
        List<ScanCode> ScanCodes { get; set; }
        List<RegionalItemMessageModel> RegionalItemMessage { get; set; }
        void Run();
    }
}
