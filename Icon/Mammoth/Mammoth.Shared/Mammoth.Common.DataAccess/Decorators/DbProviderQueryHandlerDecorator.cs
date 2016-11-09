using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.ConnectionBuilders;
using Mammoth.Common.DataAccess.DbProviders;
using System.Data.SqlClient;

namespace Mammoth.Common.DataAccess.Decorators
{
    public class DbProviderQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        private IQueryHandler<TQuery, TResult> queryHandler;
        private IDbProvider dbProvider;
        private IConnectionBuilder connectionBuilder;

        public DbProviderQueryHandlerDecorator(
            IQueryHandler<TQuery, TResult> queryHandler,
            IDbProvider dbProvider,
            IConnectionBuilder connectionBuilder)
        {
            this.queryHandler = queryHandler;
            this.dbProvider = dbProvider;
            this.connectionBuilder = connectionBuilder;
        }

        public TResult Search(TQuery parameters)
        {
            using (dbProvider.Connection = new SqlConnection(connectionBuilder.BuildConnectionString()))
            {
                dbProvider.Connection.Open();
                var results = queryHandler.Search(parameters);
                return results;
            }
        }
    }
}
