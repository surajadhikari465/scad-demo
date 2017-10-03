using Dapper;
using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSupport.DataAccess.Commands
{
    public class UpdateJobScheduleCommandHandler : ICommandHandler<UpdateJobScheduleCommand>
    {
        private IDbConnection connection;

        public UpdateJobScheduleCommandHandler(IDbConnection connection)
        {
            this.connection = connection;
        }

        public void Execute(UpdateJobScheduleCommand data)
        {
            connection.Execute(@"
                UPDATE [app].[JobSchedule]
                   SET [JobName] = @JobName
                      ,[Region] = @Region
                      ,[DestinationQueueName] = @DestinationQueueName
                      ,[StartDateTimeUtc] = @StartDateTimeUtc
                      ,[IntervalInSeconds] = @IntervalInSeconds
                      ,[Enabled] = @Enabled
                      ,[XmlObject] = @XmlObject
                WHERE JobScheduleId = @JobScheduleId",
                data.JobSchedule);
        }
    }
}
