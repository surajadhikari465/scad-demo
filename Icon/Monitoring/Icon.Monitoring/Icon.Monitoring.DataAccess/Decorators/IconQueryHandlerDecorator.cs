namespace Icon.Monitoring.DataAccess.Decorators
{
    using Icon.Common.DataAccess;
    using System.Data.SqlClient;

    public class IconQueryHandlerDecorator<TParameters, TResult> : IQueryHandler<TParameters, TResult> where TParameters : IQuery<TResult>
    {
        private IQueryHandler<TParameters, TResult> queryHandler;
        private IDbProvider dbProvider;
        private IConnectionBuilder connectionBuilder;

        public IconQueryHandlerDecorator(
            IQueryHandler<TParameters, TResult> queryHandler,
            IDbProvider dbProvider,
            IConnectionBuilder connectionBuilder)
        {
            this.queryHandler = queryHandler;
            this.dbProvider = dbProvider;
            this.connectionBuilder = connectionBuilder;
        }

        public TResult Search(TParameters parameters)
        {
            using (dbProvider.Connection = new SqlConnection(connectionBuilder.GetIconConnectionString()))
            {
                dbProvider.Connection.Open();
                var results = queryHandler.Search(parameters);
                return results;
            }
        }
    }           
}