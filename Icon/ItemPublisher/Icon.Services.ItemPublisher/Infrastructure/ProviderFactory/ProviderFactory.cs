using Icon.Logging;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Icon.Services.ItemPublisher.Infrastructure
{
    public class ProviderFactory : IProviderFactory
    {
        private SqlDbProvider provider;
        private string connectionString;
        private readonly ILogger<ProviderFactory> logger;

        public IDbTransaction Transaction { get; private set; }

        public ProviderFactory(string connectionString,
            ILogger<ProviderFactory> logger = null)
        {
            this.connectionString = connectionString;
            this.logger = logger;
        }

        public SqlDbProvider Provider
        {
            get
            {
                if (this.provider == null ||
                    provider.Connection.State == System.Data.ConnectionState.Broken ||
                    provider.Connection.State == System.Data.ConnectionState.Closed)
                {
                    provider?.Connection?.Close();

                    var connection = new SqlConnection(connectionString);
                    connection.Open();
                    this.provider = new SqlDbProvider()
                    {
                        Connection = connection
                    };
                }

                return this.provider;
            }
        }

        /// <summary>
        /// Starts a transaction on the connection and sets the transaction property on the property to the newly created transaction
        /// throws exceptions of the connection is null or a transaction has already been started.
        /// </summary>
        public void BeginTransaction()
        {
            if (this.Provider?.Connection == null)
            {
                throw new NullReferenceException("Connection must be created first");
            }

            if (this.Transaction != null)
            {
                throw new Exception("Transaction has already been started");
            }

            this.Transaction = this.Provider.Connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
            this.Provider.Transaction = this.Transaction;
        }

        /// <summary>
        /// If a transaction has been started it is rolled back. Throws exceptions if the connection is null
        /// or a transcation has not been started.
        /// </summary>
        public void RollbackTransaction()
        {
            if (this.Provider?.Connection == null)
            {
                throw new NullReferenceException("Connection must be created first");
            }

            if (this.Transaction == null)
            {
                logger.Debug($"Transaction has not been started. Connection State: {Provider.Connection.State}");
            }
            else
            {
                this.Transaction.Rollback();
                this.Transaction = null;
            }
        }

        public void CommitTransaction()
        {
            if (this.Provider?.Connection == null)
            {
                throw new NullReferenceException("Connection must be created first");
            }

            if (this.Transaction == null)
            {
                throw new Exception("Transaction has not been started");
            }

            this.Transaction.Commit();
            this.Transaction = null;
        }
    }
}