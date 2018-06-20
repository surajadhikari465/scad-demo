using System.Data;

namespace Icon.Esb.R10Listener.Infrastructure.DataAccess
{
    public interface IDbFactory
    {
        IDbConnection CreateConnection(string name);
    }
}
