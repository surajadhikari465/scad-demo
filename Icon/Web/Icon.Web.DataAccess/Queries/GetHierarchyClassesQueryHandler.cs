using Dapper;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Data;

namespace Icon.Web.DataAccess.Queries
{
    public class GetHierarchyClassesQueryHandler : IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>>
    {
        private IDbConnection dbConnection;

        public GetHierarchyClassesQueryHandler(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public IEnumerable<HierarchyClassModel> Search(GetHierarchyClassesParameters parameters)
        {
            return dbConnection.Query<HierarchyClassModel>("dbo.GetHierarchyClasses",
                parameters,
                commandType: CommandType.StoredProcedure);
        }
    }
}
