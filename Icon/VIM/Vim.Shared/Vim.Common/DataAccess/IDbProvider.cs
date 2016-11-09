using System.Data;

namespace Vim.Common.DataAccess
{
    public interface IDbProvider
    {
        IDbConnection Connection { get; set; }
        IDbTransaction Transaction { get; set; }
    }
}
