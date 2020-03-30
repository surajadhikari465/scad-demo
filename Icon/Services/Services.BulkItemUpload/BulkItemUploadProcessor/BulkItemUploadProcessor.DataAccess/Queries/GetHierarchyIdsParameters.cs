using System.Collections.Generic;
using Icon.Common.DataAccess;

namespace BulkItemUploadProcessor.DataAccess.Queries
{
    public class GetHierarchyIdsParameters : IQuery<List<int>>
    {
        public string HierarhcyName { get; set; }
        public int HierarchyLevel { get; set; }
    }
}