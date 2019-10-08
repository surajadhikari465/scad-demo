using Dapper;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PrimeAffinityController.Commands;
using PrimeAffinityController.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;
using static PrimeAffinityController.Constants.ApplicationConstants;

namespace PrimeAffinityController.Tests.Queries
{
    [TestClass]
    public class UpdateJobStatusCommandHandlerTests
    {
        private UpdateJobStatusCommandHandler commandHandler;
        private UpdateJobStatusCommand command;
        private SqlConnection sqlConnection;
        private TransactionScope transaction;
        private Mock<ILogger<UpdateJobStatusCommandHandler>> logger;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            logger = new Mock<ILogger<UpdateJobStatusCommandHandler>>();
            commandHandler = new UpdateJobStatusCommandHandler(sqlConnection, logger.Object);
            command = new UpdateJobStatusCommand();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void UpdateJobStatus_SetJobScheduleStatusToReady_ShouldUpdateJobScheduleStatusToReady()
        {
            //Given
            command.JobSchedule = InsertJobSchedule("FL", "Test Job");
            command.Status = JobScheduleStatuses.Ready;

            //When
            commandHandler.Execute(command);

            //Then
            var jobSchedule = sqlConnection.QuerySingle<JobScheduleModel>(
                @"SELECT * FROM app.JobSchedule WHERE JobName = @JobName",
                new { JobName = command.JobSchedule.JobName });
            Assert.AreEqual(command.Status, jobSchedule.Status);
        }

        private JobScheduleModel InsertJobSchedule(string region, string jobName)
        {
            JobScheduleModel jobSchedule = new JobScheduleModel
            {
                DestinationQueueName = null,
                Enabled = true,
                IntervalInSeconds = 1111,
                JobName = jobName,
                LastRunEndDateTimeUtc = DateTime.Today.AddDays(-10),
                LastScheduledDateTimeUtc = DateTime.Today.AddDays(-9),
                NextScheduledDateTimeUtc = DateTime.Today.AddDays(-8),
                Region = region,
                StartDateTimeUtc = DateTime.Today.AddDays(-7),
                Status = JobScheduleStatuses.Running,
                XmlObject = null
            };
            var jobScheduleId = sqlConnection.QueryFirst<int>(
                @"  INSERT INTO [app].[JobSchedule]
                               ([JobName]
                               ,[Region]
                               ,[DestinationQueueName]
                               ,[StartDateTimeUtc]
                               ,[LastScheduledDateTimeUtc]
                               ,[LastRunEndDateTimeUtc]
                               ,[NextScheduledDateTimeUtc]
                               ,[IntervalInSeconds]
                               ,[Enabled]
                               ,[Status]
                               ,[XmlObject])
                         VALUES
                               (@JobName
                               ,@Region
                               ,@DestinationQueueName
                               ,@StartDateTimeUtc
                               ,@LastScheduledDateTimeUtc
                               ,@LastRunEndDateTimeUtc
                               ,@NextScheduledDateTimeUtc
                               ,@IntervalInSeconds
                               ,@Enabled
                               ,@Status
                               ,@XmlObject)

                    SELECT SCOPE_IDENTITY()",
                                    jobSchedule);

            jobSchedule.JobScheduleId = jobScheduleId;
            return jobSchedule;
        }
    }
}