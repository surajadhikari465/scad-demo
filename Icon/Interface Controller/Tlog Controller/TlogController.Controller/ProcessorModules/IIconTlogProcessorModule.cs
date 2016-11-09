using System.Collections.Generic;
using TlogController.DataAccess.Models;

namespace TlogController.Controller.ProcessorModules
{
    public interface IIconTlogProcessorModule
    {
        void LoadBusinessUnitToRegionCodeMapping();
        void DeleteProcessedItemMovement(List<ItemMovementTransaction> data);
        List<IrmaTlog> GroupTlogEntiesByRegion();
    }
}
