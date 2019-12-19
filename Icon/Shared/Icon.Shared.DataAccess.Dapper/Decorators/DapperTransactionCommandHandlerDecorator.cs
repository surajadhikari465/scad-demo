using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using System;

namespace Icon.Shared.DataAccess.Dapper.Decorators
{
    public class DapperTransactionCommandHandlerDecorator<TData> : ICommandHandler<TData>
    {
        private readonly IDbProvider db;
        private ICommandHandler<TData> commandHandler;

        public DapperTransactionCommandHandlerDecorator(IDbProvider dbProvider, ICommandHandler<TData> commandHandler)
        {
            this.db = dbProvider;
            this.commandHandler = commandHandler;
        }

        public void Execute(TData data)
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
