using System.Data;

namespace Vim.Common.DataAccess
{
    public class SqlDbProvider : IDbProvider
    {
        public IDbConnection Connection { get; set; }
        public IDbTransaction Transaction { get; set; }
    }
}
