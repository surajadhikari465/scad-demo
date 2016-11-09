using System.Collections.Generic;

namespace Mammoth.Price.Controller.DataAccess.Commands
{
    public class DeleteEventQueueCommand
    {
        public IEnumerable<int> QueueIds { get; set; }
    }
}
