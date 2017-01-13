using Dapper;
using Icon.Monitoring.DataAccess.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Monitoring.DataAccess.Tests.Commands
{
    [TestClass]
    public class AddDvoErrorStatusCommandHandlerTests
    {
        private AddDvoErrorStatusCommandHandler commandHandler;
        private AddDvoErrorStatusCommand command;
        private SqlDbProvider db;

        [TestInitialize]
        public void Initialize()
        {
            db = new SqlDbProvider();
            db.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();

            commandHandler = new AddDvoErrorStatusCommandHandler(db);
            command = new AddDvoErrorStatusCommand();
        }

        [TestCleanup]
        public void Cleanup()
        {
            db.Transaction.Rollback();
            db.Connection.Close();
        }

        [TestMethod]
        public void AddDvoErrorStatus_StatusDoesNotExistForRegion_AddsErrorStatusForRegion()
        {
            //Given
            command.Region = "FL";
            string sql = $"select * from monitor.JobStatus where Region = '{command.Region}' and JobName = '{Common.Constants.CustomJobNames.DvoJobName}'";

            Assert.IsFalse(db.Connection.Query(sql, transaction: db.Transaction).Any(), $"A failed job status already exists for '{command.Region}', cannot begin test.");

            //When
            commandHandler.Execute(command);

            //Then
            Assert.IsNotNull(db.Connection.Query(sql, transaction: db.Transaction).SingleOrDefault());
        }

        [TestMethod]
        public void AddDvoErrorStatus_StatusForRegionExists_DoesNotAddDuplicateErrorStatus()
        {
            //Given
            command.Region = "FL";
            string insertSql = "insert monitor.JobStatus(JobName, Status, Region) values(@JobName, @Status, @Region)";
            db.Connection.Execute(
                sql: insertSql,
                param: new { JobName = Common.Constants.CustomJobNames.DvoJobName, Status = "TEST", Region = command.Region },
                transaction: db.Transaction);

            //When
            commandHandler.Execute(command);

            //Then
            string selectSql = $"select * from monitor.JobStatus where Region = '{command.Region}' and JobName = '{Common.Constants.CustomJobNames.DvoJobName}'";
            dynamic status = db.Connection.Query(selectSql, transaction: db.Transaction).SingleOrDefault();
            Assert.IsNotNull(status);
            Assert.AreEqual("TEST", status.Status);
        }
    }
}
