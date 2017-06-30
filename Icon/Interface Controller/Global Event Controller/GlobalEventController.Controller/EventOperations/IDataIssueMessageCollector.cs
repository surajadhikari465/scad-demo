using GlobalEventController.Common;
using System.Collections.Generic;

namespace GlobalEventController.Controller.EventOperations
{
    public interface IDataIssueMessageCollector
    {
        List<RegionalItemMessageModel> Message { get; set; }
        void SendDataIssueMessage();
    }
}
