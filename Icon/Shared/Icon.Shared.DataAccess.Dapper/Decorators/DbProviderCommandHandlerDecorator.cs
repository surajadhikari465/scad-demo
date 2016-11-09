using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.ConnectionBuilders;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using System.Data.SqlClient;

namespace Icon.Shared.DataAccess.Dapper.Decorators
{
    public class DbProviderCommandHandlerDecorator<T> : ICommandHandler<T>
    {
        private ICommandHandler<T> commandHandler;
        private IDbProvider dbProvider;
        private IConnectionBuilder connectionBuilder;

        public DbProviderCommandHandlerDecorator(
            ICommandHandler<T> commandHandler,
            IDbProvider dbProvider,
            IConnectionBuilder connectionBuilder)
        {
            this.commandHandler = commandHandler;
            this.dbProvider = dbProvider;
            this.connectionBuilder = connectionBuilder;
        }

        public void Execute(T data)
        {
            using (dbProvider.Connection = new SqlConnection(connectionBuilder.BuildConnectionString()))
            {
                this.dbProvider.Connection.Open();
                this.commandHandler.Execute(data);
            }
        }
    }
}
