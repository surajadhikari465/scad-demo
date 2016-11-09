using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GenericGetDbSet : IGenericQuery
    {
        private readonly IconContext context;

        public GenericGetDbSet(IconContext context)
        {
            this.context = context;
        }

        public List<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return this.context.Set<TEntity>().ToList();
        }
    }
}
