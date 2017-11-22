using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.ConnectionBuilders;
using Mammoth.Common.DataAccess.DbProviders;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Mammoth.Common.DataAccess.Decorators
{
    public class DbProviderQueryHandlerDecoratorAsync<TQuery, TResult> : IQueryHandlerAsync<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        private IQueryHandlerAsync<TQuery, TResult> queryHandler;
        private IDbProvider dbProvider;
        private IConnectionBuilder connectionBuilder;

        public DbProviderQueryHandlerDecoratorAsync(
            IQueryHandlerAsync<TQuery, TResult> asyncQueryHandler,
            IDbProvider dbProvider,
            IConnectionBuilder connectionBuilder)
        {
            this.queryHandler = asyncQueryHandler;
            this.dbProvider = dbProvider;
            this.connectionBuilder = connectionBuilder;
        }

        public async Task<TResult> QueryAsync(TQuery parameters)
        {
            using (dbProvider.Connection = new SqlConnection(connectionBuilder.BuildConnectionString()))
            {
                dbProvider.Connection.Open();
                var results = await queryHandler.QueryAsync(parameters);
                return results;
            }
        }
    }
}
