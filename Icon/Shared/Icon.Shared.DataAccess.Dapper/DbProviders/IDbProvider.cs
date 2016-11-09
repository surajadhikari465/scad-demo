using System.Data;

namespace Icon.Shared.DataAccess.Dapper.DbProviders
{
    public interface IDbProvider
    {
        IDbConnection Connection { get; set; }
        IDbTransaction Transaction { get; set; }
    }
}
