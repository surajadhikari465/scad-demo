using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vim.Common.DataAccess;
using Vim.Common.DataAccess.Commands;
using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace Vim.Common.DataAcess.Tests.Commands
{
    [TestClass]
    public class UpdateEventQueueInProcessCommandHandlerTests
    {
        private UpdateEventQueueInProcessCommandHandler commandHandler;
        private SqlDbProvider dbProvider;

        [TestInitialize]
        public void Initialize()
        {
            dbProvider = new SqlDbProvider();
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
            dbProvider.Connection.Open();
            dbProvider.Transaction = dbProvider.Connection.BeginTransaction();

            commandHandler = new UpdateEventQueueInProcessCommandHandler(dbProvider);
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbProvider.Transaction.Rollback();
            dbProvider.Transaction.Dispose();
            dbProvider.Connection.Dispose();
        }

        [TestMethod]
        public void UpdateChangeQueue_LocaleAddEventType_ShouldMarkLocaleAddEventQueues()
        {
            //Given
            List<EventQueueModel> queueModels = new List<EventQueueModel>();
            for (int i = 0; i < 100; i++)
            {
                queueModels.Add(new EventQueueModel { EventTypeId = EventTypes.LocaleAdd, EventReferenceId = 1, InsertDate = DateTime.Now });
            }
            dbProvider.Connection.Execute(
                @"insert into vim.EventQueue(EventTypeId, EventReferenceId, InsertDate)
                    values (@EventTypeId, @EventReferenceId, @InsertDate)",
                queueModels,
                transaction: dbProvider.Transaction);

            //When
            commandHandler.Execute(new UpdateEventQueueInProcessCommand
            {
                EventTypeIds = new List<int> { EventTypes.LocaleAdd },
                Instance = 100,
                MaxNumberOfRowsToMark = 100
            });

            //Then
            var numberOfItemsWithInstanceId = dbProvider.Connection.Query<int>(
                @"select count(*) from vim.EventQueue 
                    where EventTypeId = @EventTypeId 
                    and InProcessBy = @InstanceId",
                new { EventTypeId = EventTypes.LocaleAdd, InstanceId = 100 },
                transaction: dbProvider.Transaction);
            Assert.AreEqual(100, numberOfItemsWithInstanceId.First());
        }

        [TestMethod]
        public void UpdateChangeQueue_localeUpdateEventType_ShouldMarkLocaleAddEventQueues()
        {
            //Given
            List<EventQueueModel> queueModels = new List<EventQueueModel>();
            for (int i = 0; i < 100; i++)
            {
                queueModels.Add(new EventQueueModel { EventTypeId = EventTypes.LocaleUpdate, EventReferenceId = 1, InsertDate = DateTime.Now });
            }
            dbProvider.Connection.Execute(
                @"insert into vim.EventQueue(EventTypeId, EventReferenceId, InsertDate)
                    values (@EventTypeId, @EventReferenceId, @InsertDate)",
                queueModels,
                transaction: dbProvider.Transaction);

            //When
            commandHandler.Execute(new UpdateEventQueueInProcessCommand
            {
                EventTypeIds = new List<int> { EventTypes.LocaleUpdate },
                Instance = 100,
                MaxNumberOfRowsToMark = 100
            });

            //Then
            var numberOfItemsWithInstanceId = dbProvider.Connection.Query<int>(
                @"select count(*) from vim.EventQueue 
                    where EventTypeId = @EventTypeId 
                    and InProcessBy = @InstanceId",
                new { EventTypeId = EventTypes.LocaleUpdate, InstanceId = 100 },
                transaction: dbProvider.Transaction);
            Assert.AreEqual(100, numberOfItemsWithInstanceId.First());
        }
    }
}
