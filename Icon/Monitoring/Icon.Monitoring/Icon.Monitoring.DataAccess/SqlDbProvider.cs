using System.Data;

namespace Icon.Monitoring.DataAccess
{
    public class SqlDbProvider : IDbProvider
    {
        public IDbConnection Connection { get; set; }
        public IDbTransaction Transaction { get; set; }
    }
}
