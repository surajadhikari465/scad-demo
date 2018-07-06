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
        private string testContext = "Testing Context";
        private int? maxHistoryId;

        [TestInitialize]
        public void Initialize()
        {
            dbProvider = new SqlDbProvider();
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ItemCatalog_MA"].ConnectionString);
            dbProvider.Connection.Open();

            this.commandHandler = new ArchiveEventsCommandHandler(dbProvider);
            this.command = new ArchiveEventsCommand();
            this.maxHistoryId = dbProvider.Connection.Query<int?>("SELECT MAX(HistoryId) FROM mammoth.ChangeQueueHistory").First();
            this.maxHistoryId = this.maxHistoryId.HasValue ? this.maxHistoryId : 0;
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbProvider.Connection.Execute("DELETE mammoth.ChangeQueueHistory WHERE HistoryId > @MaxHistoryId", new { MaxHistoryId = this.maxHistoryId });
            dbProvider.Connection.Close();
            dbProvider.Connection.Dispose();
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
                "SELECT * FROM mammoth.ChangeQueueHistory h WHERE h.HistoryId > @MaxHistoryId", 
                new { MaxHistoryId = this.maxHistoryId });

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
                "SELECT * FROM mammoth.ChangeQueueHistory h WHERE h.HistoryId > @MaxHistoryId",
                new { MaxHistoryId = this.maxHistoryId });

            Assert.AreEqual(expectedNumberOfRecords, numberOfHistoryRecords.Count());
        }

        private List<ChangeQueueHistoryModel> BuildChangeQueueHistoryModels(int numberOfModels)
        {
            var events = new List<ChangeQueueHistoryModel>();
            for (int i = 0; i < numberOfModels; i++)
            {
                events.Add(new ChangeQueueHistoryModel
                {
                    QueueID = i+1,
                    Store_No = i+1,
                    Item_Key = i+1,
                    EventReferenceId = i+1,
                    Identifier = (i+1).ToString(),
                    ErrorCode = (i+1).ToString(),
                    ErrorDetails = (i+1).ToString(),
                    EventTypeId = 1,
                    MachineName = "test machine",
                    InsertDate = DateTime.Now,
                    QueueInsertDate = DateTime.Now,
                    Context = testContext
                });
            }

            return events;
        }
    }
}
