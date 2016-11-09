using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetPluRequestsQuery : IQueryHandler<GetPluRequestsParameters, List<PLURequest>>
    {
        private readonly IconContext context;

        public GetPluRequestsQuery(IconContext context)
        {
            this.context = context;
        }

        public List<PLURequest> Search(GetPluRequestsParameters parameters)
        {
            SqlParameter requestStatus = new SqlParameter("@requestStatus", SqlDbType.VarChar);
            requestStatus.Value = parameters.requestStatus.ToString();

            string sql = @"app.GetPLURequestBySearchParams @requestStatus";
            var dbResult = this.context.Database.SqlQuery<PLURequest>(sql, requestStatus);
            return dbResult.ToList();
        }
    }
}

