using Dapper;
using Icon.Common.DataAccess;
using System.Data;
using System.Linq;

namespace WebSupport.DataAccess.Queries
{
    public class DoesStoreExistQuery : IQueryHandler<DoesStoreExistParameters, bool>
    {
        private IDbConnection connection;

        public DoesStoreExistQuery(IDbConnection connection)
        {
            this.connection = connection;
        }

        public bool Search(DoesStoreExistParameters parameters)
        {
            return connection.Query<bool>($@"
                IF EXISTS (SELECT 1 FROM dbo.Locale WHERE BusinessUnitID = @BusinessUnitId)
                    SELECT CONVERT(BIT, 1)
                SELECT CONVERT(BIT, 0)",
                parameters)
                .Single();
        }
    }
}
