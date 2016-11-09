using NutritionWebApi.Common;
using NutritionWebApi.Common.Interfaces;
using System.Data.SqlClient;

namespace NutritionWebApi.DataAccess.Decorators
{
    public class DbCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    {
        private ICommandHandler<TCommand> commandHandler;

        public IDbConnectionProvider DbConnectionProvider { get; set; }

        public DbCommandHandlerDecorator(
            ICommandHandler<TCommand> commandHandler,
            IDbConnectionProvider connectionProvider)
        {
            this.commandHandler = commandHandler;
            this.DbConnectionProvider = connectionProvider;
        }

        public string Execute(TCommand data)
        {
            string result;
            using (this.DbConnectionProvider.Connection = new SqlConnection(ApiConfigSettings.Instance.ConnectionString))
            {
                this.DbConnectionProvider.Connection.Open();
                result = this.commandHandler.Execute(data);
                this.DbConnectionProvider.Connection.Close();
            }
            return result;
        }
    }
}
