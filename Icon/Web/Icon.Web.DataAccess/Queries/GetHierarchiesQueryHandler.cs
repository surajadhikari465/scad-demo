using Dapper;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Data;

namespace Icon.Web.DataAccess.Queries
{
    public class GetHierarchiesQueryHandler : IQueryHandler<GetHierarchiesParameters, IEnumerable<HierarchyModel>>
    {
        private IDbConnection dbConnection;

        public GetHierarchiesQueryHandler(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public IEnumerable<HierarchyModel> Search(GetHierarchiesParameters parameters)
        {
            return dbConnection.Query<HierarchyModel>(@"
                SELECT 
                    h.hierarchyID AS HierarchyId,
                    h.hierarchyName AS HierarchyName
                FROM dbo.Hierarchy h");
        }
    }
}
