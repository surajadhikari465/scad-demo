using Icon.Framework;
using RegionalEventController.DataAccess.Interfaces;
using RegionalEventController.DataAccess.Models;
using System.Collections.Generic;

namespace RegionalEventController.DataAccess.Queries
{
    public class GetValidatedItemsQuery : IQuery<Dictionary<string, int>>
    {
        public List<string> identifiers { get; set; }
    }
}
