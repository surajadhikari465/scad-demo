using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.Extract.DataAccess.Commands;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;

namespace Services.Extract.DataAccess.Tests.Commands
{
    [TestClass]
    public class UpdateJobStatusCommandHandlerTests
    {
        private UpdateJobStatusCommandHandler commandHandler;
        private SqlConnection sqlConnection;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            commandHandler = new UpdateJobStatusCommandHandler(sqlConnection);
        }

        [TestCleanup]
        public void Cleanup()
        {
            sqlConnection.Dispose();
            transaction.Dispose();
        }

        [TestMethod]
        public void UpdateJobStatus_JobExists_UpdatesStatus()
        {
            //Given
            int testJobScheduleId = sqlConnection.QueryFirst<int>(GetInsertJobSchedule());

            //When
            commandHandler.Execute(new UpdateJobStatusCommand
            {
                JobScheduleId = testJobScheduleId,
                Status = "running"
            });

            //Then
            var status = sqlConnection.QueryFirst<string>("SELECT Status FROM app.JobSchedule WHERE JobScheduleId = @JobScheduleId", new { JobScheduleId = testJobScheduleId });
            Assert.AreEqual("running", status);
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
