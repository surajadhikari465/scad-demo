using Dapper;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Price.Controller.DataAccess.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Mammoth.Price.Controller.DataAccess.Tests.Commands
{
    [TestClass]
    public class DeleteEventQueueCommandHandlerTests
    {
        private DeleteEventQueueCommandHandler commandHandler;
        private SqlDbProvider dbProvider;
        private int testInstance = 99;

        [TestInitialize]
        public void Initialize()
        {
            dbProvider = new SqlDbProvider();
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ItemCatalog_MA"].ConnectionString);
            dbProvider.Connection.Open();
            dbProvider.Transaction = dbProvider.Connection.BeginTransaction();

            commandHandler = new DeleteEventQueueCommandHandler(dbProvider);
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbProvider.Transaction.Rollback();
            dbProvider.Transaction.Dispose();
            dbProvider.Connection.Dispose();
        }

        [TestMethod]
        public void DeleteEventQueueData_DataExist_ShouldDeleteData()
        {
            //Given
            int testEventReferenceId = 1234567;
            var item = dbProvider.Connection.Query<dynamic>("select top 1 Item_Key, Identifier from ItemIdentifier", transaction: dbProvider.Transaction).First();
            dbProvider.Connection.Query<int>(@"
                insert into mammoth.PriceChangeQueue(Item_Key, Identifier, EventTypeID, EventReferenceID, InProcessBy, InsertDate)
                values (@Item_Key, @Identifier, @EventTypeId, @EventReferenceId, @InProcessBy, @InsertDate)",
                new
                {
                    item.Item_Key,
                    item.Identifier,
                    EventTypeId = IrmaEventTypes.Price,
                    EventReferenceId = testEventReferenceId,
                    InProcessBy = testInstance,
                    InsertDate = DateTime.Now
                },
                dbProvider.Transaction);

            //When
            commandHandler.Execute(new DeleteEventQueueCommand
            {
                Instance = testInstance
            });

            //Then
            var eventCount = dbProvider.Connection.QueryFirst<int>(
                @"select count(*) from mammoth.PriceChangeQueue where InProcessBy = @Instance",
                new { Instance = testInstance },
                transaction: dbProvider.Transaction);
            Assert.AreEqual(0, eventCount);
        }
    }
}
