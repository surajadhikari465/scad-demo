namespace Icon.Monitoring.DataAccess.Decorators
{
    using System.Data.SqlClient;

    using Icon.Common.DataAccess;
    using Icon.Monitoring.DataAccess.Queries;
    using Common.Enums;

    public class IrmaQueryHandlerDecorator<TParameters, TResult> : IQueryByRegionHandler<TParameters, TResult> where TParameters : IQuery<TResult>
    {
        private IQueryByRegionHandler<TParameters, TResult> queryHandler;
        private IDbProvider dbProvider;
        private IConnectionBuilder connectionBuilder;

        public IrmaRegions TargetRegion { get; set; }

        public IrmaQueryHandlerDecorator(
            IQueryByRegionHandler<TParameters, TResult> queryHandler,
            IDbProvider dbProvider,
            IConnectionBuilder connectionBuilder)
        {
            this.queryHandler = queryHandler;
            this.dbProvider = dbProvider;
            this.connectionBuilder = connectionBuilder;
        }

        public TResult Search(TParameters parameters)
        {
            this.queryHandler.TargetRegion = TargetRegion;

            using (dbProvider.Connection = new SqlConnection(
                connectionBuilder.GetIrmaConnectionStringForRegion(this.TargetRegion.ToString())))
            {
                dbProvider.Connection.Open();
                var results = this.queryHandler.Search(parameters);
                return results;
            }
        }
    }
}
