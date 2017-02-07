using System.Data.Entity;

namespace Icon.ApiController.Common
{
    public interface IDbContextFactory<T>
        where T : DbContext, new()
    {
        T CreateContext();
        T CreateContext(string connectionStringName);
        T CreateContext(object settings);
    }
}
