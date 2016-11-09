using System.Collections.Generic;

namespace Vim.Common.DataAccess.Commands
{
    public class DeleteEventQueueCommand
    {
        public IEnumerable<int> QueueIds { get; set; }
    }
}

