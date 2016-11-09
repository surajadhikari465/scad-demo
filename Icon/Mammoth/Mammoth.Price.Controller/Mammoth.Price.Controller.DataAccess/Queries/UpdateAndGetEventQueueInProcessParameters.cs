using Mammoth.Common.ControllerApplication.Models;
using Mammoth.Common.DataAccess.CommandQuery;
using System.Collections.Generic;

namespace Mammoth.Price.Controller.DataAccess.Queries
{
    public class UpdateAndGetEventQueueInProcessParameters : IQuery<List<EventQueueModel>>
    {
        public int MaxNumberOfRowsToMark { get; set; }
        public int Instance { get; set; }
    }
}
