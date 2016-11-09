using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vim.Common.DataAccess.Commands
{
    public class UpdateFailedEventQueueCommand
    {
        public IEnumerable<int> QueueIds { get; set; }
    }
}
