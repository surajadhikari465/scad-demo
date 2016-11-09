using System.Collections.Generic;
using TlogController.DataAccess.Models;

namespace TlogController.DataAccess.BulkCommands
{
    public class BulkUpdateItemMovementCommand
    {
        public List<ItemMovementTransaction> ItemMovementTransactionData { get; set; }
    }
}
