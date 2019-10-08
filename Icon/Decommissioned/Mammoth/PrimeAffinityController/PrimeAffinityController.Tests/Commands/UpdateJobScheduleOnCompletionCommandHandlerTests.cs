using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class UpdateJobScheduleOnCompletionCommandHandlerTests
    {
        private UpdateJobScheduleOnCompletionCommandHandler commandHandler;
        private UpdateJobScheduleOnCompletionCommand command;
        private SqlConnection sqlConnection;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            commandHandler = new UpdateJobScheduleOnCompletionCommandHandler(sqlConnection);
            command = new UpdateJobScheduleOnCompletionCommand();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void UpdateJobScheduleOnCompletion_SetJobScheduleStatusToReady_ShouldUpdateJobScheduleStatusAndNextScheduledRunTime()
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
            Assert.AreEqual(
                command.JobSchedule.NextScheduledDateTimeUtc.AddSeconds(command.JobSchedule.IntervalInSeconds), 
                jobSchedule.NextScheduledDateTimeUtc);
            Assert.AreEqual(command.JobSchedule.NextScheduledDateTimeUtc, jobSchedule.LastScheduledDateTimeUtc);
        }

        [TestMethod]
        public void UpdateJobScheduleOnCompletion_SetJobScheduleStatusToNotReady_ShouldUpdateJobScheduleStatusAndNextScheduledRunTimeShouldStayTheSame()
        {
            //Given
            command.JobSchedule = InsertJobSchedule("FL", "Test Job");
            command.Status = JobScheduleStatuses.Failed;

            //When
            commandHandler.Execute(command);

            //Then
            var jobSchedule = sqlConnection.QuerySingle<JobScheduleModel>(
                @"SELECT * FROM app.JobSchedule WHERE JobName = @JobName",
                new { JobName = command.JobSchedule.JobName });
            Assert.AreEqual(command.Status, jobSchedule.Status);
            Assert.AreEqual(command.JobSchedule.NextScheduledDateTimeUtc, jobSchedule.NextScheduledDateTimeUtc);
            Assert.AreEqual(command.JobSchedule.NextScheduledDateTimeUtc, jobSchedule.LastScheduledDateTimeUtc);
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
