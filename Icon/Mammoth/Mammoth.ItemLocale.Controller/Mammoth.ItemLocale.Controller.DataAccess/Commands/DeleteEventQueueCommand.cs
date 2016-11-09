using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.ItemLocale.Controller.DataAccess.Commands
{
    public class DeleteEventQueueCommand
    {
        public IEnumerable<int> QueueIds { get; set; }
    }
}
