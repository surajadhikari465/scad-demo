using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Icon.Common.DataAccess;

namespace BulkItemUploadProcessor.DataAccess.Queries
{
    public class GetHierarchyIdsQueryHandler : IQueryHandler<GetHierarchyIdsParameters, List<int>>
    {
        private readonly IDbConnection Connection;
        public GetHierarchyIdsQueryHandler(IDbConnection connection)
        {
            Connection = connection;
        }

        public List<int> Search(GetHierarchyIdsParameters parameters)
        {
            var query = "select HierarchyClassId from HierarchyClass hc inner join Hierarchy h on hc.HierarchyId = h.HierarchyId where h.Hierarchyname = @HierarchyName and hierarchyLevel = @HierarchyLevel";
            var ids = Connection.Query<int>(query, new { HierarchyName = parameters.HierarhcyName, HierarchyLevel  = parameters.HierarchyLevel});
            return ids.ToList();
        }
    }
}