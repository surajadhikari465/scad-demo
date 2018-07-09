using Mammoth.Common.ControllerApplication.Models;
using Mammoth.Price.Controller.DataAccess.Models;
using System.Collections.Generic;

namespace Mammoth.Price.Controller.DataAccess.Commands
{
    public class ReprocessFailedCancelAllSalesEventsCommand
    {
        public string Region { get; set; }
        public List<EventQueueModel> Events { get; set; }
        public List<CancelAllSalesEventModel> CancelAllSales { get; set; }
    }
}
