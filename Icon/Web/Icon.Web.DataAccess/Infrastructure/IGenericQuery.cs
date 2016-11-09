using System.Collections.Generic;

namespace Icon.Web.DataAccess.Infrastructure
{
    public interface IGenericQuery
    {
        List<TEntity> GetAll<TEntity>() where TEntity : class;
    }
}
