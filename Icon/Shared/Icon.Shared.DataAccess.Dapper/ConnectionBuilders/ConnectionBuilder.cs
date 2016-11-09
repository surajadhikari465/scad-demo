using System.Configuration;

namespace Icon.Shared.DataAccess.Dapper.ConnectionBuilders
{
    public class ConnectionBuilder : IConnectionBuilder
    {
        private string connectionStringKey;
        private string connectionString;

        public ConnectionBuilder(string connectionStringKey)
        {
            this.connectionStringKey = connectionStringKey;
            connectionString = ConfigurationManager.ConnectionStrings[connectionStringKey].ConnectionString;
        }

        public string BuildConnectionString()
        {
            return connectionString;
        }
    }
}
