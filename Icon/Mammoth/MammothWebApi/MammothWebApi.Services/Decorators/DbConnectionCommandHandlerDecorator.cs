using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.Common;
using System.Data.SqlClient;

namespace MammothWebApi.Service.Decorators
{
    public class DbConnectionCommandHandlerDecorator<T> : ICommandHandler<T> where T: class
    {
        private ICommandHandler<T> commandHandler;
        private IServiceSettings settings;
        private IDbProvider db;

        public DbConnectionCommandHandlerDecorator(ICommandHandler<T> service,
            IDbProvider db,
            IServiceSettings settings)
        {
            this.commandHandler = service;
            this.settings = settings;
            this.db = db;
        }

        public void Execute(T command)
        {
            using (this.db.Connection = new SqlConnection(this.settings.ConnectionString))
            {
                this.db.Connection.Open();
                this.commandHandler.Execute(command);
            }
        }
    }
}
