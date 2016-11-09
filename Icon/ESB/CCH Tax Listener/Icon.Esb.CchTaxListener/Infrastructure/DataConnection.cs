using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Icon.Esb.CchTax.Infrastructure
{
    public class DataConnection : IDataConnection
    {
        public IDbConnection Connection { get; set; }
        private IDbTransaction transaction;

        public IDbTransaction BeginTransaction()
        {
            transaction = Connection.BeginTransaction();
            return transaction;
        }

        public void UseTransaction(IDbTransaction transaction)
        {
            this.transaction = transaction;
        }

        public void SaveChanges()
        {
            if (transaction != null)
                transaction.Commit();
        }

        public IEnumerable<dynamic> Query(string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Connection.Query(sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public IEnumerable<T> Query<T>(string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Connection.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public int Execute(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Connection.Execute(sql, param, transaction, commandTimeout, commandType);
        }

        public void Dispose()
        {
            if (transaction != null)
                transaction.Dispose();
            if (Connection != null && Connection.State == ConnectionState.Open)
                Connection.Dispose();
        }
    }
}
