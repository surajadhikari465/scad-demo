using Dapper;
using Icon.Common.DataAccess;
using System.Data;
using System.Linq;

namespace WebSupport.DataAccess.Queries
{
   public class IsRegionOnGpmQuery:IQueryHandler<IsRegionOnGpmParameters,bool>
    {
        private IDbConnection connection;

        public IsRegionOnGpmQuery(IDbConnection connection)
        {
            this.connection = connection;
        }

        public bool Search(IsRegionOnGpmParameters parameters)
        {
            return connection.Query<bool>(@"
                    SELECT ISNULL((SELECT ISNULL(IsGpmEnabled, 'False') as IsGpmEnabled
                      FROM [dbo].[RegionGpmStatus]
                      WHERE Region = @Region), 'False')",
                     new { Region = parameters.Region }).First();
        }

    }
}