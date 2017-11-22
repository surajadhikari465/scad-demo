using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.Common;
using MammothWebApi.Service.Services;
using System.Data.SqlClient;

namespace MammothWebApi.Service.Decorators
{
    public class DbConnectionServiceDecorator<T> : IUpdateService<T> where T: class
    {
        private IUpdateService<T> service;
        private IServiceSettings settings;
        private IDbProvider db;

        public DbConnectionServiceDecorator(IUpdateService<T> service,
            IDbProvider db,
            IServiceSettings settings)
        {
            this.service = service;
            this.settings = settings;
            this.db = db;
        }

        public void Handle(T data)
        {
            using (this.db.Connection = new SqlConnection(this.settings.ConnectionString))
            {
                this.db.Connection.Open();
                this.service.Handle(data);
            }
        }
    }
}
