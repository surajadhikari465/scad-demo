using GlobalEventController.Common;
using GlobalEventController.DataAccess.Infrastructure;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.BulkCommands
{
    public class BulkGetValidatedItemsQuery : IQuery<List<ValidatedItemModel>>
    {
        public List<EventQueue> Events { get; set; }
    }
}
