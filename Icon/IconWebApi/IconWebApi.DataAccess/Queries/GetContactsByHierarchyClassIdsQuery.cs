using Icon.Common.DataAccess;
using IconWebApi.DataAccess.Models;
using System.Collections.Generic;

namespace IconWebApi.DataAccess.Queries
{
    public class GetContactsByHierarchyClassIdsQuery : IQuery<IEnumerable<AssociatedContact>>
    {
        public List<int> HierarchyClassIds { get; set; }
    }
}