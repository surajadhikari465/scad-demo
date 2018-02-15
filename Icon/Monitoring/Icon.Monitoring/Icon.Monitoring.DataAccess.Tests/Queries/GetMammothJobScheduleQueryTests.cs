using Dapper;
using Icon.Monitoring.DataAccess.Model;
using Icon.Monitoring.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;

namespace Icon.Monitoring.DataAccess.Tests.Queries
{
    [TestClass]
    public class GetMammothJobScheduleQueryTests
    {
        private SqlDbProvider provider;
        private GetMammothJobScheduleQuery query;
        private GetMammothJobScheduleParameters parameters;
        private SqlConnection sqlConnection;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            provider = new SqlDbProvider();
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            provider.Connection = sqlConnection;

            query = new GetMammothJobScheduleQuery(provider);
            parameters = new GetMammothJobScheduleParameters();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void GetMammothJobSchedule_JobScheduleExists_ShouldReturnJobSchedule()
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
        public void GetMammothJobSchedule_JobScheduleExistsAndOtherJobSchedulesExist_ShouldReturnOnlyOneJobSchedule()
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
        public void GetMammothJobSchedule_JobScheduleDoesNotExists_ShouldNotReturnJobSchedule()
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
        public void GetMammothJobSchedule_JobScheduleDoesNotExistsAndOtherJobSchedulesExist_ShouldNotReturnJobSchedule()
        {
            //Given
            parameters.Region = "FL";
            parameters.JobName = "TestJob";

            //When
            var result = query.Search(parameters);

            //Then
            Assert.IsNull(result);
        }

        private JobSchedule InsertJobSchedule(string region, string jobName)
        {
            JobSchedule jobSchedule = new JobSchedule
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
                Status = "ready",
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
