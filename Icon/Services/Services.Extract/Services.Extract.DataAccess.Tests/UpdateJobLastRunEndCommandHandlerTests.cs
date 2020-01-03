using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;
using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Extract.DataAccess.Commands;

namespace Services.Extract.DataAccess.Tests
{
    [TestClass]
    public class UpdateJobLastRunEndCommandHandlerTests
    {
        private UpdateJobLastRunEndCommandHandler commandHandler;
        private SqlConnection sqlConnection;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            commandHandler = new UpdateJobLastRunEndCommandHandler(sqlConnection);
        }

        [TestCleanup]
        public void Cleanup()
        {
            sqlConnection.Dispose();
            transaction.Dispose();
        }

        [TestMethod]
        public void UpdateJobLastRunEnd_JobExists_UpdatesLastRunEnd()
        {
            //Given
            int testJobScheduleId = sqlConnection.QueryFirst<int>(GetInsertJobSchedule());
            DateTime testDateTime = DateTime.Today;

            //When
            commandHandler.Execute(new UpdateJobLastRunEndCommand
            {
                JobScheduleId = testJobScheduleId,
                LastRunEndDateTime = testDateTime
            });

            //Then
            var dateTime = sqlConnection.QueryFirst<DateTime>("SELECT LastRunEndDateTimeUtc FROM app.JobSchedule WHERE JobScheduleId = @JobScheduleId", new { JobScheduleId = testJobScheduleId });
            Assert.AreEqual(testDateTime, dateTime);
        }

        private string GetInsertJobSchedule()
        {
            return @"INSERT INTO app.JobSchedule (
	                    JobName
	                    ,Region
	                    ,DestinationQueueName
	                    ,StartDateTimeUtc
	                    ,LastScheduledDateTimeUtc
	                    ,LastRunEndDateTimeUtc
	                    ,NextScheduledDateTimeUtc
	                    ,IntervalInSeconds
	                    ,Enabled
	                    ,Status
	                    ,XmlObject
	                    ,RunAdHoc
	                    ,InstanceId
	                    )
                    VALUES (
	                    'Test'
	                    ,'TT'
	                    ,''
	                    ,GETDATE()
	                    ,GETDATE()
	                    ,GETDATE()
	                    ,GETDATE()
	                    ,1
	                    ,0
	                    ,'Ready'
	                    ,''
	                    ,NULL
	                    ,NULL
	                    )

                    SELECT SCOPE_IDENTITY()";
        }
    }
}
