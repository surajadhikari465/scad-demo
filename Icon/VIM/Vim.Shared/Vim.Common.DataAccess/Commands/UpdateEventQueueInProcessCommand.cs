using System.Collections.Generic;

namespace Vim.Common.DataAccess.Commands
{
    public class UpdateEventQueueInProcessCommand
    {
        public int MaxNumberOfRowsToMark { get; set; }
        public int Instance { get; set; }
        public List<int> EventTypeIds { get; set; }
    }
}
