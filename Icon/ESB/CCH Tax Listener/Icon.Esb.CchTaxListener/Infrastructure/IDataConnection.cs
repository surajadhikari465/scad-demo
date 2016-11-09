using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.CchTax.Infrastructure
{
    public interface IDataConnection : IDisposable
    {
        IDbConnection Connection { get; set; }
        IDbTransaction BeginTransaction();
        void SaveChanges();
        IEnumerable<dynamic> Query(string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);
        IEnumerable<T> Query<T>(string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);
        int Execute(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null);
    }
}
