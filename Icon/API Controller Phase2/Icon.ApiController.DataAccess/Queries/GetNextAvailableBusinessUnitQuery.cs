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

namespace Icon.ApiController.DataAccess.Queries
{
    public class GetNextAvailableBusinessUnitQuery : IQueryHandler<GetNextAvailableBusinessUnitParameters, int?>
    {
        private IRenewableContext<IconContext> globalContext;

        public GetNextAvailableBusinessUnitQuery(IRenewableContext<IconContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public int? Search(GetNextAvailableBusinessUnitParameters parameters)
        {
            SqlParameter instanceId = new SqlParameter("InstanceId", SqlDbType.Int);
            instanceId.Value = parameters.InstanceId;

            string sql = $"EXEC app.{parameters.MessageQueueName}GetBusinessUnitToProcess @InstanceId";

            return globalContext.Context.Database.SqlQuery<int?>(sql, instanceId).FirstOrDefault();
        }
    }
}
