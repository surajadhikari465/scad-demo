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
    public class CreateJobScheduleCommandHandler : ICommandHandler<CreateJobScheduleCommand>
    {
        private IDbConnection connection;

        public CreateJobScheduleCommandHandler(IDbConnection connection)
        {
            this.connection = connection;
        }

        public void Execute(CreateJobScheduleCommand data)
        {
            connection.Execute(
                @"INSERT INTO [app].[JobSchedule]
                           ([JobName]
                           ,[Region]
                           ,[DestinationQueueName]
                           ,[StartDateTimeUtc]
                           ,[IntervalInSeconds]
                           ,[Enabled]
                           ,[XmlObject])
                     VALUES
                           (@JobName
                           ,@Region
                           ,@DestinationQueueName
                           ,@StartDateTimeUtc
                           ,@IntervalInSeconds
                           ,@Enabled
                           ,@XmlObject)",
                data.JobSchedule);
        }
    }
}
