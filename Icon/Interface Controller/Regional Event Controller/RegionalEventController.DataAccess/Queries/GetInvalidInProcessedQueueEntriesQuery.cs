using System.Collections.Generic;
using RegionalEventController.DataAccess.Interfaces;

namespace RegionalEventController.DataAccess.Queries
{
    public class GetInvalidInProcessedQueueEntriesQuery : IQuery<List<int>> 
    {
        public List<int> queueIds { get; set; }
    }
}
