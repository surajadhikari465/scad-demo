using GlobalEventController.Common;
using GlobalEventController.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubteamEventController.DataAccess.BulkCommands
{
    public class BulkGetItemsWithSubTeamQuery : IQuery<List<ItemSubTeamModel>>
    {
        public List<string> ScanCodes { get; set; }
    }
}
