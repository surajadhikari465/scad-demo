using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mammoth.ItemLocale.Controller.DataAccess.Commands;
using System.Data.SqlClient;
using System.Configuration;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Common.DataAccess.Models;
using System.Collections.Generic;
using Dapper;
using System.Linq;

namespace Mammoth.ItemLocale.Controller.DataAccess.Tests.Commands
{
    [TestClass]
    public class ArchiveEventsCommandHandlerTests
    {
        private ArchiveEventsCommandHandler commandHandler;
        private ArchiveEventsCommand command;
        private SqlDbProvider dbProvider;
        private string testContext = "Unit Test Context";
        private int? maxHistoryId;

        [TestInitialize]
        public void Initialize()
        {
            dbProvider = new SqlDbProvider();
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ItemCatalog_FL"].ConnectionString);
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

        [TestMethod]
        public void ArchiveEvents_Given10RecordtWith20000CharacterTextField_ShouldAdd10RecordsToHistoryTable()
        {
            //Given
            var expectedNumberOfRecords = 10;
            command.Events = BuildChangeQueueHistoryModels(expectedNumberOfRecords);
            foreach (var item in command.Events)
            {
                item.Context += new string('s', 20000);
            }

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
