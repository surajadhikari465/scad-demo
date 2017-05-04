using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using System;

namespace Icon.Infor.Listeners.Price.DataAccess.Decorators
{
    public class TransactionCommandHandlerDecorator<T> : ICommandHandler<T>
    {
        private ICommandHandler<T> commandHandler;
        private IDbProvider db;

        public TransactionCommandHandlerDecorator(ICommandHandler<T> commandHandler, IDbProvider db)
        {
            this.commandHandler = commandHandler;
            this.db = db;
        }

        public void Execute(T data)
        {
            using (this.db.Transaction = this.db.Connection.BeginTransaction())
            {
                try
                {
                    this.commandHandler.Execute(data);
                    this.db.Transaction.Commit();
                }
                catch (Exception)
                {
                    this.db.Transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
