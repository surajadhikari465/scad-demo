using Dapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetHealthCheckQuery : IQueryHandler<GetHealthCheckParameters, int>
    {
        private IDbConnection connection;

        public GetHealthCheckQuery(IDbConnection connection)
        {
            this.connection = connection;
        }

        public int Search(GetHealthCheckParameters parameters)
        {
            return connection.Query<int>($@"SELECT TOP 1 CheckId FROM app.HealthCheck").Single();
        }
    }
}
