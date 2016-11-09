using System.Data;

namespace Icon.Monitoring.DataAccess
{
    public interface IDbProvider
    {
        IDbConnection Connection { get; set; }
        IDbTransaction Transaction { get; set; }
    }
}
