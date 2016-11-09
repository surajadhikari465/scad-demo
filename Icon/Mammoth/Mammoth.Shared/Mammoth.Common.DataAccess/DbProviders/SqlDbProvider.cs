using System.Data;

namespace Mammoth.Common.DataAccess.DbProviders
{
    public class SqlDbProvider : IDbProvider
    {
        public IDbConnection Connection { get; set; }
        public IDbTransaction Transaction { get; set; }
    }
}
