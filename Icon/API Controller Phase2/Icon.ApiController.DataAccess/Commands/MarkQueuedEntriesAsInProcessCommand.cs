using System.Collections.Generic;

namespace Icon.ApiController.DataAccess.Commands
{
    public class MarkQueuedEntriesAsInProcessCommand<T>
    {
        public int LookAhead { get; set; }
        public int Instance { get; set; }
        public int BusinessUnit { get; set; }
    }
}
