using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Icon.DbContextFactory;

namespace Icon.ApiController.DataAccess.Queries
{
    public class GetNextAvailableBusinessUnitQuery : IQueryHandler<GetNextAvailableBusinessUnitParameters, int?>
    {
        private IDbContextFactory<IconContext> iconContextFactory;

        public GetNextAvailableBusinessUnitQuery(IDbContextFactory<IconContext> iconContextFactory)
        {
            this.iconContextFactory = iconContextFactory;
        }

        public int? Search(GetNextAvailableBusinessUnitParameters parameters)
        {
            SqlParameter instanceId = new SqlParameter("InstanceId", SqlDbType.Int);
            instanceId.Value = parameters.InstanceId;

            string sql = $"EXEC app.{parameters.MessageQueueName}GetBusinessUnitToProcess @InstanceId";

            using (var context = iconContextFactory.CreateContext())
            {
                return context.Database.SqlQuery<int?>(sql, instanceId).FirstOrDefault();
            }
        }
    }
}
