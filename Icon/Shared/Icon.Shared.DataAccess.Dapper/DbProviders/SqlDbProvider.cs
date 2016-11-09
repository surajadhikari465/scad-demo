using System.Data;

namespace Icon.Shared.DataAccess.Dapper.DbProviders
{
    public class SqlDbProvider : IDbProvider
    {
        public IDbConnection Connection { get; set; }
        public IDbTransaction Transaction { get; set; }
    }
}
