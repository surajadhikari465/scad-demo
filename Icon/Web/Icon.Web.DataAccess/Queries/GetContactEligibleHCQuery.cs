using System.Collections.Generic;
using Icon.Common.DataAccess;
using Dapper;
using System.Data;


namespace Icon.Web.DataAccess.Queries
{
    public class GetContactEligibleHCQuery : IQueryHandler<GetContactEligibleHCParameters, HashSet<int>>
    {
        private readonly IDbConnection db;

        public GetContactEligibleHCQuery(IDbConnection db)
        {
           this.db = db;
        }

        public HashSet<int> Search(GetContactEligibleHCParameters data)
        {
            return new HashSet<int>(this.db.Query<int>(@"SELECT hc.hierarchyClassID
              FROM dbo.HierarchyClass hc
              JOIN dbo.Hierarchy h on h.hierarchyID = hc.hierarchyID
              WHERE h.hierarchyName in('Brands','Manufacturer');", commandType: CommandType.Text));
        }
    }
}
