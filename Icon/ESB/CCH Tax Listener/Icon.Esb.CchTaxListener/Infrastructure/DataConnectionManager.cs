using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.CchTax.Infrastructure
{
    public class DataConnectionManager : IDataConnectionManager
    {
        public IDataConnection Connection { get; set; }

        public IDataConnection InitializeConnection(string connectionString)
        {
            DataConnection connection = new DataConnection
            {
                Connection = new SqlConnection(connectionString) 
            };
            connection.Connection.Open();

            Connection = connection;

            return connection;
        }
    }
}
