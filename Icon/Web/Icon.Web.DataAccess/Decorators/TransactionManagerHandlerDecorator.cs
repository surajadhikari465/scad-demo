using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.Common.Cache;

namespace Icon.Web.DataAccess.Decorators
{
    public class TransactionManagerHandlerDecorator<TManager> : IManagerHandler<TManager>
    {
        private readonly IManagerHandler<TManager> managerHandler;
        private IconContext context;
        private ICache cache;

        public TransactionManagerHandlerDecorator(IManagerHandler<TManager> managerHandler, IconContext context, ICache cache)
        {
            this.managerHandler = managerHandler;
            this.context = context;
            this.cache = cache;
        }

        public void Execute(TManager manager)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    managerHandler.Execute(manager);
                    transaction.Commit();

                    if (CacheRefreshNeededHandler.HiearchyCacheRefreshNeededHandlerList.Contains(managerHandler.GetType().Name))
                    {
                        cache.Remove("GetHierarchyLineageParameters");
                    }
                    if (CacheRefreshNeededHandler.AgencyCacheRefreshNeededHandlerList.Contains(managerHandler.GetType().Name))
                    {
                        cache.Remove("GetCertificationAgenciesParameters");
                    }
                }
                catch (CommandException)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
