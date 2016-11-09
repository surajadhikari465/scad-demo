using System.Collections.Generic;
using TlogController.DataAccess.Models;

namespace TlogController.DataAccess.BulkCommands
{
    public class BulkUpdateSalesSumByitemCommand
    {
        public List<ItemMovementToIrma> ItemMovementsToIrma { get; set; }
    }
}
