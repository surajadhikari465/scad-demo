using Mammoth.Common.ControllerApplication.Models;
using Mammoth.Common.DataAccess.CommandQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.ItemLocale.Controller.DataAccess.Queries
{
    public class UpdateAndGetEventQueueInProcessQuery : IQuery<List<EventQueueModel>>
    {
        public int MaxNumberOfRowsToMark { get; set; }
        public int Instance { get; set; }
    }
}
