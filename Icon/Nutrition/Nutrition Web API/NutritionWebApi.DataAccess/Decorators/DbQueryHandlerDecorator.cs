using NutritionWebApi.Common;
using System.Data.SqlClient;

namespace NutritionWebApi.DataAccess.Decorators
{
    public class DbQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        public IQueryHandler<TQuery, TResult> queryHandler { get; set; }
        public IDbConnectionProvider DbConnectionProvider { get; set; }

        public DbQueryHandlerDecorator(IQueryHandler<TQuery, TResult> queryHandler, IDbConnectionProvider DbConnectionProvider)
        {
            this.queryHandler = queryHandler;
            this.DbConnectionProvider = DbConnectionProvider;
        }

        public TResult Search(TQuery parameters)
        {

            using (this.DbConnectionProvider.Connection = new SqlConnection(ApiConfigSettings.Instance.ConnectionString))
            {
                this.DbConnectionProvider.Connection.Open();
                TResult result = this.queryHandler.Search(parameters);
                this.DbConnectionProvider.Connection.Close();
                return result;
            }
        }
    }
}
