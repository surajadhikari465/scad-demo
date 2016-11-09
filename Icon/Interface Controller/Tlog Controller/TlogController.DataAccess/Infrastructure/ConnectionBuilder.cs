using System;
using System.Collections.Generic;
using System.Configuration;

namespace TlogController.DataAccess.Infrastructure
{
    public static class ConnectionBuilder
    {
        private static List<string> connectionStrings;

        public static List<string> BuildConnections()
        {
            connectionStrings = new List<string>();

            ConnectionStringSettingsCollection appConfigConnectionStrings = ConfigurationManager.ConnectionStrings;
            foreach (ConnectionStringSettings connectionString in appConfigConnectionStrings)
            {
                if (connectionString.Name.StartsWith("ItemCatalog"))
                {
                    connectionStrings.Add(connectionString.ConnectionString);
                }
            }

            return connectionStrings;
        }

        public static string GetConnection(string regionCode)
        {
            string regionalConnectionString = String.Empty;

            if (!String.IsNullOrEmpty(regionCode))
            {
                ConnectionStringSettingsCollection appConfigConnectionStrings = ConfigurationManager.ConnectionStrings;
                foreach (ConnectionStringSettings connectionString in appConfigConnectionStrings)
                {
                    if (connectionString.Name.StartsWith("ItemCatalog") && connectionString.Name.EndsWith(regionCode))
                    {
                        regionalConnectionString = connectionString.ConnectionString;
                        break;
                    }
                }
            }

            return regionalConnectionString;
        }
    }
}
