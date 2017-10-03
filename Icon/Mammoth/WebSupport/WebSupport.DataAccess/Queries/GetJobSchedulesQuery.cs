using Icon.Common.DataAccess;
using Mammoth.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Queries
{
    public class GetJobSchedulesQuery : IQueryHandler<GetJobSchedulesParameters, List<JobSchedule>>
    {
        private MammothContext context;

        public GetJobSchedulesQuery(MammothContext context)
        {
            this.context = context;
        }

        public List<JobSchedule> Search(GetJobSchedulesParameters parameters)
        {
            return context.Database.SqlQuery<JobSchedule>(@"
                    SELECT [JobScheduleId]
                          ,[JobName]
                          ,[Region]
                          ,[DestinationQueueName]
                          ,[StartDateTimeUtc]
                          ,[LastRunDateTimeUtc]
                          ,[IntervalInSeconds]
                          ,[Enabled]
                          ,[XmlObject]
                      FROM [app].[JobSchedule]").ToList();
        }
    }
}
