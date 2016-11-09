using Dapper;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vim.Common.DataAccess.Commands;
using Vim.Common.DataAccess;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Vim.Common.DataAcess.Tests.Commands
{
    [TestClass]
    public class UpdateFailedEventQueueCommandHandlerTests
    {
        private UpdateFailedEventQueueCommandHandler commandHandler;
        private SqlDbProvider dbProvider;

        [TestInitialize]
        public void InitializeTest()
        {
            this.dbProvider = new SqlDbProvider();
            this.dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
            this.dbProvider.Connection.Open();
            this.dbProvider.Transaction = this.dbProvider.Connection.BeginTransaction();

            this.commandHandler = new UpdateFailedEventQueueCommandHandler(this.dbProvider);
        }

        [TestCleanup]
        public void CleanupTests()
        {
            this.dbProvider.Transaction.Rollback();
            this.dbProvider.Connection.Dispose();
        }

        [TestMethod]
        public void UpdateFailedEventsCommand_GivenAListOfExistingQueueIds_ShouldSetProcessFailedDataToNotNull()
        {
            // Given
            var addEventQueue = new List<EventQueueModel>();
            addEventQueue.Add(new EventQueueModel { EventTypeId = 1, EventReferenceId = 1, EventMessage = "111122223333" });
            addEventQueue.Add(new EventQueueModel { EventTypeId = 1, EventReferenceId = 1, EventMessage = "111122223334" });
            addEventQueue.Add(new EventQueueModel { EventTypeId = 1, EventReferenceId = 1, EventMessage = "111122223335" });

            string sql = @"INSERT INTO vim.EventQueue (EventTypeId, EventReferenceId, EventMessage)
                            VALUES (@EventTypeId, @EventReferenceId, @EventMessage);
                            SELECT CAST(SCOPE_IDENTITY() as int);";

            List<int> idList = new List<int>();
            foreach (var row in addEventQueue)
            {
                idList.Add(this.dbProvider.Connection.Query<int>(sql,
                    new { EventTypeId = row.EventTypeId, EventReferenceId = row.EventReferenceId, EventMessage = row.EventMessage },
                    transaction: this.dbProvider.Transaction).Single());
            }


            // When
            var parameters = new UpdateFailedEventQueueCommand { QueueIds = idList };
            this.commandHandler.Execute(parameters);

            // Then
            IEnumerable<EventQueueModel> actualRows = this.dbProvider.Connection.Query<EventQueueModel>(@"SELECT * FROM vim.EventQueue WHERE QueueId IN @Id",
                new { Id = idList },
                transaction: this.dbProvider.Transaction);

            foreach (var row in actualRows)
            {
                Assert.IsTrue(row.ProcessedFailedDate != null);
            }
        }

        [TestMethod]
        public void UpdateFailedEventsCommand_ListOfQueueIDs_NumberOfRetryIncrementedAndProcessedFailedDateUpdated()
        {
            // Given
            var addEventQueue = new List<EventQueueModel>();
            DateTime today = DateTime.Now;
            addEventQueue.Add(new EventQueueModel { EventTypeId = 1, EventReferenceId = 1, EventMessage = "111122223333", NumberOfRetry = 1, ProcessedFailedDate = today });
            addEventQueue.Add(new EventQueueModel { EventTypeId = 1, EventReferenceId = 1, EventMessage = "111122223334", NumberOfRetry = 2, ProcessedFailedDate = today });
            addEventQueue.Add(new EventQueueModel { EventTypeId = 1, EventReferenceId = 1, EventMessage = "111122223335", NumberOfRetry = null, ProcessedFailedDate = null });

            string sql = @"INSERT INTO vim.EventQueue (EventTypeId, EventReferenceId, EventMessage, NumberOfRetry, ProcessedFailedDate)
                            VALUES (@EventTypeId, @EventReferenceId, @EventMessage, @NumberOfRetry, @ProcessedFailedDate);
                            SELECT CAST(SCOPE_IDENTITY() as int);";

            List<int> idList = new List<int>();
            foreach (var row in addEventQueue)
            {
                idList.Add(this.dbProvider.Connection.Query<int>(sql,
                    new
                    {
                        EventTypeId = row.EventTypeId,
                        EventReferenceId = row.EventReferenceId,
                        EventMessage = row.EventMessage,
                        NumberOfRetry = row.NumberOfRetry,
                        ProcessedFailedDate = row.ProcessedFailedDate
                    },
                    transaction: this.dbProvider.Transaction).Single());
            }


            // When
            var parameters = new UpdateFailedEventQueueCommand { QueueIds = idList };
            this.commandHandler.Execute(parameters);

            // Then
            List<EventQueueModel> actualRows = this.dbProvider.Connection.Query<EventQueueModel>(@"SELECT * FROM vim.EventQueue WHERE QueueId IN @Id",
                new { Id = idList },
                transaction: this.dbProvider.Transaction).ToList();

            for (int i = 0; i < addEventQueue.Count; i++)
            {
                int retryNumber = addEventQueue[i].NumberOfRetry != null ? addEventQueue[i].NumberOfRetry.Value : -1;
                Assert.AreEqual(retryNumber + 1, actualRows[i].NumberOfRetry);
                Assert.IsTrue(actualRows[i].ProcessedFailedDate != null && actualRows[i].ProcessedFailedDate > today);
            }
        }
    }
}

