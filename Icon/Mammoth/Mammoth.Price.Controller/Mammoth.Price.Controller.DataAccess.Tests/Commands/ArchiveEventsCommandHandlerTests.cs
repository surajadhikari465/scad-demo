using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mammoth.Price.Controller.DataAccess.Commands;
using Mammoth.Common.DataAccess.DbProviders;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using Mammoth.Common.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.Price.Controller.DataAccess.Tests.Commands
{
    [TestClass]
    public class ArchiveEventsCommandHandlerTests
    {
        private ArchiveEventsCommandHandler commandHandler;
        private ArchiveEventsCommand command;
        private SqlDbProvider dbProvider;
        private string testContext = "Test Context";

        [TestInitialize]
        public void Initialize()
        {
            dbProvider = new SqlDbProvider();
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ItemCatalog_FL"].ConnectionString);
            dbProvider.Connection.Open();

            this.commandHandler = new ArchiveEventsCommandHandler(dbProvider);
            this.command = new ArchiveEventsCommand();
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbProvider.Connection.Execute("DELETE mammoth.ChangeQueueHistory WHERE Context = @TestContext", new { TestContext = testContext });
        }

        [TestMethod]
        public void ArchiveEvents_GivenNoChangeHistoryModels_ShouldNotAddToHistoryTable()
        {
            //Given
            command.Events = new List<ChangeQueueHistoryModel>();

            //When
            commandHandler.Execute(command);

            //Then
            var numberOfHistoryRecords = dbProvider.Connection.Query<ChangeQueueHistoryModel>(
                "SELECT * FROM mammoth.ChangeQueueHistory h WHERE h.Context = @TestContext", 
                new { TestContext = testContext });

            Assert.AreEqual(0, numberOfHistoryRecords.Count());
        }

        [TestMethod]
        public void ArchiveEvents_Given10ChangeHistoryModels_ShouldAdd10RecordsToHistoryTable()
        {
            //Given
            var expectedNumberOfRecords = 10;
            command.Events = BuildChangeQueueHistoryModels(expectedNumberOfRecords);

            //When
            commandHandler.Execute(command);

            //Then
            var numberOfHistoryRecords = dbProvider.Connection.Query<ChangeQueueHistoryModel>(
                "SELECT * FROM mammoth.ChangeQueueHistory h WHERE h.Context = @TestContext",
                new { TestContext = testContext });

            Assert.AreEqual(expectedNumberOfRecords, numberOfHistoryRecords.Count());
        }

        private List<ChangeQueueHistoryModel> BuildChangeQueueHistoryModels(int numberOfModels)
        {
            var events = new List<ChangeQueueHistoryModel>();
            for (int i = 0; i < numberOfModels; i++)
            {
                events.Add(new ChangeQueueHistoryModel
                {
                    QueueID = i,
                    Store_No = i,
                    Item_Key = i,
                    EventReferenceId = i,
                    Identifier = i.ToString(),
                    ErrorCode = i.ToString(),
                    ErrorDetails = i.ToString(),
                    EventTypeId = i,
                    MachineName = i.ToString(),
                    InsertDate = DateTime.Now,
                    QueueInsertDate = DateTime.Now,
                    Context = testContext
                });
            }

            return events;
        }
    }
}
