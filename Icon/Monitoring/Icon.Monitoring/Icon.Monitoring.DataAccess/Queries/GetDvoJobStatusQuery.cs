using Dapper;
using Icon.Common.DataAccess;
using Icon.Monitoring.Common.Dvo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Monitoring.DataAccess.Queries
{
    public class GetDvoJobStatusQuery : IQueryHandler<GetDvoJobStatusParameters, List<DvoRegionalJobStatus>>
    {
        private IDbProvider db;

        public GetDvoJobStatusQuery(IDbProvider db)
        {
            this.db = db;
        }

        public List<DvoRegionalJobStatus> Search(GetDvoJobStatusParameters parameters)
        {
            if (parameters.Regions != null && parameters.Regions.Any())
            {
                string sql = $@"select * from monitor.JobStatus
                                where JobName = '{Common.Constants.CustomJobNames.DvoJobName}'
                                and Region in ({string.Join(",", parameters.Regions.Select(r => "'" + r + "'"))})";

                return this.db.Connection.Query<dynamic>(
                    sql,
                    transaction: db.Transaction)
                    .Select(s => new DvoRegionalJobStatus
                    {
                        Region = s.Region
                    }).ToList();
            }
            else
            {
                return new List<DvoRegionalJobStatus>();
            }
        }
    }
}
