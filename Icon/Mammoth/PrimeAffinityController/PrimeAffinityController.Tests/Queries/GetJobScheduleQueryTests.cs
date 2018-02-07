using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrimeAffinityController.Models;
using PrimeAffinityController.Queries;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;
using static PrimeAffinityController.Constants.ApplicationConstants;

namespace PrimeAffinityController.Tests.Queries
{
    [TestClass]
    public class GetJobScheduleQueryTests
    {
        private GetJobScheduleQuery query;
        private GetJobScheduleParameters parameters;
        private SqlConnection sqlConnection;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);

            query = new GetJobScheduleQuery(sqlConnection);
            parameters = new GetJobScheduleParameters();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void GetJobSchedule_JobScheduleExists_ShouldReturnJobSchedule()
        {
            //Given
            parameters.Region = "FL";
            parameters.JobName = "TestJob";
            var testJobSchedule = InsertJobSchedule(parameters.Region, parameters.JobName);

            //When
            var result = query.Search(parameters);

            //Then
            Assert.IsNotNull(result);
            Assert.AreEqual(parameters.Region, result.Region);
            Assert.AreEqual(parameters.JobName, result.JobName);
            Assert.AreEqual(testJobSchedule.DestinationQueueName, result.DestinationQueueName);
            Assert.AreEqual(testJobSchedule.Enabled, result.Enabled);
            Assert.AreEqual(testJobSchedule.IntervalInSeconds, result.IntervalInSeconds);
            Assert.AreEqual(testJobSchedule.LastRunEndDateTimeUtc, result.LastRunEndDateTimeUtc);
            Assert.AreEqual(testJobSchedule.LastScheduledDateTimeUtc, result.LastScheduledDateTimeUtc);
            Assert.AreEqual(testJobSchedule.NextScheduledDateTimeUtc, result.NextScheduledDateTimeUtc);
            Assert.AreEqual(testJobSchedule.Region, result.Region);
            Assert.AreEqual(testJobSchedule.StartDateTimeUtc, result.StartDateTimeUtc);
            Assert.AreEqual(testJobSchedule.Status, result.Status);
            Assert.AreEqual(testJobSchedule.XmlObject, result.XmlObject);
        }

        [TestMethod]
        public void GetJobSchedule_JobScheduleExistsAndOtherJobSchedulesExist_ShouldReturnOnlyOneJobSchedule()
        {
            //Given
            parameters.Region = "FL";
            parameters.JobName = "TestJob";
            var testJobSchedule = InsertJobSchedule(parameters.Region, parameters.JobName);
            InsertJobSchedule(parameters.Region, "TEST");
            InsertJobSchedule("X2", parameters.JobName);
            InsertJobSchedule("X2", "TEST");

            //When
            var result = query.Search(parameters);

            //Then
            Assert.IsNotNull(result);
            Assert.AreEqual(parameters.Region, result.Region);
            Assert.AreEqual(parameters.JobName, result.JobName);
            Assert.AreEqual(testJobSchedule.DestinationQueueName, result.DestinationQueueName);
            Assert.AreEqual(testJobSchedule.Enabled, result.Enabled);
            Assert.AreEqual(testJobSchedule.IntervalInSeconds, result.IntervalInSeconds);
            Assert.AreEqual(testJobSchedule.LastRunEndDateTimeUtc, result.LastRunEndDateTimeUtc);
            Assert.AreEqual(testJobSchedule.LastScheduledDateTimeUtc, result.LastScheduledDateTimeUtc);
            Assert.AreEqual(testJobSchedule.NextScheduledDateTimeUtc, result.NextScheduledDateTimeUtc);
            Assert.AreEqual(testJobSchedule.Region, result.Region);
            Assert.AreEqual(testJobSchedule.StartDateTimeUtc, result.StartDateTimeUtc);
            Assert.AreEqual(testJobSchedule.Status, result.Status);
            Assert.AreEqual(testJobSchedule.XmlObject, result.XmlObject);
        }

        [TestMethod]
        public void GetJobSchedule_JobScheduleDoesNotExists_ShouldNotReturnJobSchedule()
        {
            //Given
            parameters.Region = "FL";
            parameters.JobName = "TestJob";
            InsertJobSchedule(parameters.Region, "TEST");
            InsertJobSchedule("X2", parameters.JobName);
            InsertJobSchedule("X2", "TEST");

            //When
            var result = query.Search(parameters);

            //Then
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetJobSchedule_JobScheduleDoesNotExistsAndOtherJobSchedulesExist_ShouldNotReturnJobSchedule()
        {
            //Given
            parameters.Region = "FL";
            parameters.JobName = "TestJob";

            //When
            var result = query.Search(parameters);

            //Then
            Assert.IsNull(result);
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
                Status = JobScheduleStatuses.Ready,
                XmlObject = null
            };
            sqlConnection.Execute(
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
                               ,@XmlObject)",
                                    jobSchedule);
            return jobSchedule;
        }
    }
}
