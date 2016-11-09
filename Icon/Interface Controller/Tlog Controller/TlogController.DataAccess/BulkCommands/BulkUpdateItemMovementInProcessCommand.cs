using Icon.Framework;
using System.Collections.Generic;
using TlogController.DataAccess.Interfaces;

namespace TlogController.DataAccess.BulkCommands
{
    public class BulkUpdateItemMovementInProcessCommand : IQuery<List<ItemMovement>>
    {
        public int MaxTransaction { get; set; }
        public string Instance { get; set; }
    }
}
