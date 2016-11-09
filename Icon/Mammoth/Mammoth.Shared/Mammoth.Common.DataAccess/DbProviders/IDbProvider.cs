using System.Data;

namespace Mammoth.Common.DataAccess.DbProviders
{
    public interface IDbProvider
    {
        IDbConnection Connection { get; set; }
        IDbTransaction Transaction { get; set; }
    }
}
