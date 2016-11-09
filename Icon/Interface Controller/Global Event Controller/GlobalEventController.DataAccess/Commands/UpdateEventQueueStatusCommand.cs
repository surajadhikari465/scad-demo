using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.Commands
{
    public class UpdateEventQueueStatusCommand
    {
        public List<int> EventQueueIdList { get; set; }
        public int Instance { get; set; }
    }
}
