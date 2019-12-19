using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System;

namespace Icon.Web.DataAccess.Decorators
{
    public class TransactionCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    {
        private readonly ICommandHandler<TCommand> commandHandler;
        private IconContext context;

        public TransactionCommandHandlerDecorator(ICommandHandler<TCommand> commandHandler, IconContext context)
        {
            this.commandHandler = commandHandler;
            this.context = context;
        }

        public void Execute(TCommand command)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    commandHandler.Execute(command);
                    transaction.Commit();
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
