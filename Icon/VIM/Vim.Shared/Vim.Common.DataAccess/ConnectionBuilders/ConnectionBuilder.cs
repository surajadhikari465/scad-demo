using System.Configuration;

namespace Vim.Common.DataAccess.ConnectionBuilders
{
    public class ConnectionBuilder : IConnectionBuilder
    {
        private string connectionStringKey;
        private string connectionString;

        public ConnectionBuilder(string connectionStringKey)
        {
            this.connectionStringKey = connectionStringKey;
            this.connectionString = ConfigurationManager.ConnectionStrings[connectionStringKey].ConnectionString;
        }

        public string BuildConnectionString()
        {
            return connectionString;
        }
    }
}
