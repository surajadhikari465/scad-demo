using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Web.Common.Cache;

namespace Icon.Web.DataAccess.Decorators
{
    public class TransactionCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    {
        private readonly ICommandHandler<TCommand> commandHandler;
        private IconContext context;
        private ICache cache;

        public TransactionCommandHandlerDecorator(ICommandHandler<TCommand> commandHandler, IconContext context, ICache cache)
        {
            this.commandHandler = commandHandler;
            this.context = context;
            this.cache = cache;
        }

        public void Execute(TCommand command)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    commandHandler.Execute(command);
                    transaction.Commit();
                    if (CacheRefreshNeededHandler.HiearchyCacheRefreshNeededHandlerList.Contains(commandHandler.GetType().Name))
                    {
                        cache.Remove("GetHierarchyLineageParameters");
                    }
                    if (CacheRefreshNeededHandler.AgencyCacheRefreshNeededHandlerList.Contains(commandHandler.GetType().Name))
                    {
                        cache.Remove("GetCertificationAgenciesParameters");
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
