using System.Configuration;
using System.Data.SqlClient;

namespace Icon.Web.Tests.Integration.TestHelpers
{
    public static class SqlConnectionBuilder
    {
        public static SqlConnection CreateIconConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
        }
    }
}