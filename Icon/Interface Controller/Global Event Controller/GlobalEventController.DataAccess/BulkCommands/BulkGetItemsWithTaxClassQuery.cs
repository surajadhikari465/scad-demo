using GlobalEventController.Common;
using GlobalEventController.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.DataAccess.BulkCommands
{
    public class BulkGetItemsWithTaxClassQuery : IQuery<List<ValidatedItemModel>>
    {
        public List<ValidatedItemModel> ValidatedItems { get; set; }
    }
}
