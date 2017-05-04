using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Monitoring.DataAccess.Decorators
{
    public class IconCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    {
        private ICommandHandler<TCommand> commandHandler;
        private IDbProvider dbProvider;
        private IConnectionBuilder connectionBuilder;

        public IconCommandHandlerDecorator(
            ICommandHandler<TCommand> commandHandler,
            IDbProvider dbProvider,
            IConnectionBuilder connectionBuilder)
        {
            this.commandHandler = commandHandler;
            this.dbProvider = dbProvider;
            this.connectionBuilder = connectionBuilder;
        }

        public void Execute(TCommand command)
        {
            using (dbProvider.Connection = new SqlConnection(connectionBuilder.GetIconConnectionString()))
            {
                dbProvider.Connection.Open();
                commandHandler.Execute(command);
            }
        }
    }
}
