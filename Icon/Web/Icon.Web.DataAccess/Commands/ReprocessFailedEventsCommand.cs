using System.Collections.Generic;

namespace Icon.Web.DataAccess.Commands
{
    public class ReprocessFailedEventsCommand
    {
        public List<int> EventQueueIds { get; set; }
    }
}
