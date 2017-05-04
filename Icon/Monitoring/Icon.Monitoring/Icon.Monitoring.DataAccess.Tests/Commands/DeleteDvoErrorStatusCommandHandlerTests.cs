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
    public class DeleteDvoErrorStatusCommandHandlerTests
    {
        private DeleteDvoErrorStatusCommandHandler commandHandler;
        private DeleteDvoErrorStatusCommand command;
        private SqlDbProvider db;

        [TestInitialize]
        public void Initialize()
        {
            db = new SqlDbProvider();
            db.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();

            commandHandler = new DeleteDvoErrorStatusCommandHandler(db);
            command = new DeleteDvoErrorStatusCommand();
        }

        [TestCleanup]
        public void Cleanup()
        {
            db.Transaction.Rollback();
            db.Connection.Close();
        }

        [TestMethod]
        public void DeleteDvoErrorStatus_StatusExistsForRegion_DeletesErrorStatusForRegion()
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
            Assert.IsNull(db.Connection.Query(selectSql, transaction: db.Transaction).SingleOrDefault());
        }

        [TestMethod]
        public void DeleteDvoErrorStatus_StatusExistsForDifferentRegion_DoesNotDeletesErrorStatusForDifferentRegion()
        {
            //Given
            command.Region = "FL";

            string insertSql = "insert monitor.JobStatus(JobName, Status, Region) values(@JobName, @Status, @Region)";
            db.Connection.Execute(
                sql: insertSql,
                param: new List<dynamic>
                {
                    new { JobName = Common.Constants.CustomJobNames.DvoJobName, Status = "TEST", Region = command.Region },
                    new { JobName = Common.Constants.CustomJobNames.DvoJobName, Status = "TEST", Region = "SW" },
                    new { JobName = Common.Constants.CustomJobNames.DvoJobName, Status = "TEST", Region = "MW" },
                    new { JobName = Common.Constants.CustomJobNames.DvoJobName, Status = "TEST", Region = "MA" }
                },
                transaction: db.Transaction);

            Assert.AreEqual(db.Connection.Query<int>("select count(*) from monitor.JobStatus", transaction: db.Transaction).First(), 4, "Test data does not have the expected size");

            //When
            commandHandler.Execute(command);

            //Then
            string selectSql = $"select * from monitor.JobStatus where Region = '{command.Region}' and JobName = '{Common.Constants.CustomJobNames.DvoJobName}'";
            dynamic status = db.Connection.Query(selectSql, transaction: db.Transaction).SingleOrDefault();
            Assert.IsNull(status);
            Assert.AreEqual(db.Connection.Query<int>("select count(*) from monitor.JobStatus", transaction: db.Transaction).First(), 3);
        }
    }
}
