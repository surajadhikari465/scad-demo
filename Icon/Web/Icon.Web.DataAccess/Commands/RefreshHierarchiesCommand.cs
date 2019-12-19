using System.Collections.Generic;

namespace Icon.Web.DataAccess.Commands
{
    public class RefreshHierarchiesCommand
    {
        public List<int> HierarchyClassIds { get; set; }
        public bool IsManufacturerHierarchyMessage { get; set; }
    }
}
