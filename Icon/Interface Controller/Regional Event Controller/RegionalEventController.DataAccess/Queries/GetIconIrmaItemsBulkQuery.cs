using Icon.Framework;
using System.Collections.Generic;
using RegionalEventController.DataAccess.Interfaces;
namespace RegionalEventController.DataAccess.Queries
{
    public class GetIconIrmaItemsBulkQuery : IQuery<Dictionary<string, int>>
    {
        public List<string> identifiers { get; set; }
    }
}
