namespace Icon.Monitoring.DataAccess.Decorators
{
    using Icon.Common.DataAccess;
    using Queries;
    using System.Data.SqlClient;

    public class MammothQueryHandlerDecorator<TParameters, TResult> : IQueryHandlerMammoth<TParameters, TResult> where TParameters : IQuery<TResult>
    {
        private IQueryHandlerMammoth<TParameters, TResult> queryHandler;
        private IDbProvider dbProvider;
        private IConnectionBuilder connectionBuilder;

        public MammothQueryHandlerDecorator(
            IQueryHandlerMammoth<TParameters, TResult> queryHandler,
            IDbProvider dbProvider,
            IConnectionBuilder connectionBuilder)
        {
            this.queryHandler = queryHandler;
            this.dbProvider = dbProvider;
            this.connectionBuilder = connectionBuilder;
        }

        public TResult Search(TParameters parameters)
        {
            using (dbProvider.Connection = new SqlConnection(connectionBuilder.GetMammothConnectionString()))
            {
                dbProvider.Connection.Open();
                var results = queryHandler.Search(parameters);
                return results;
            }
        }
    }
}
