using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Icon.Esb.R10Listener.Infrastructure.DataAccess
{
    public class DbFactory : IDbFactory
    {
        public IDbConnection CreateConnection(string name)
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings[name].ConnectionString);
        }
    }
}
