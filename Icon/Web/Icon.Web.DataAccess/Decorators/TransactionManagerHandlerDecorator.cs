using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;

namespace Icon.Web.DataAccess.Decorators
{
    public class TransactionManagerHandlerDecorator<TManager> : IManagerHandler<TManager>
    {
        private readonly IManagerHandler<TManager> managerHandler;
        private IconContext context;

        public TransactionManagerHandlerDecorator(IManagerHandler<TManager> managerHandler, IconContext context)
        {
            this.managerHandler = managerHandler;
            this.context = context;
        }

        public void Execute(TManager manager)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    managerHandler.Execute(manager);
                    transaction.Commit();
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
