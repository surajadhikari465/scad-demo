using System.Collections.Generic;

namespace Icon.ApiController.DataAccess.Commands
{
    public class UpdateStagedProductStatusCommand
    {
        public List<int> PublishedHierarchyClasses { get; set; }
    }
}
