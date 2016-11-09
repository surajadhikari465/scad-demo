using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.Service.Decorators
{
    public class DbConnectionQueryHandlerDecorator<T, U> : IQueryHandler<T, U> where T : class, IQuery<U>
    {
        private IQueryHandler<T, U> queryHandler;
        private IServiceSettings settings;
        private IDbProvider db;

        public DbConnectionQueryHandlerDecorator(IQueryHandler<T, U> queryHandler,
            IServiceSettings settings,
            IDbProvider db)
        {
            this.queryHandler = queryHandler;
            this.settings = settings;
            this.db = db;
        }

        public U Search(T parameters)
        {
            using (this.db.Connection = new SqlConnection(this.settings.ConnectionString))
            {
                this.db.Connection.Open();
                return queryHandler.Search(parameters);
            }
        }
    }
}