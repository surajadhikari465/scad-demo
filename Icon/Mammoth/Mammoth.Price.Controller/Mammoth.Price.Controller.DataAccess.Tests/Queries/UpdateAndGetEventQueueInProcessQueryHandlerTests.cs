using Dapper;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Price.Controller.DataAccess.Commands;
using Mammoth.Price.Controller.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Mammoth.Price.Controller.DataAccess.Tests.Commands
{
    [TestClass]
    public class UpdateAndGetEventQueueInProcessQueryHandlerTests
    {
        private UpdateAndGetEventQueueInProcessQuery queryHandler;
        private SqlDbProvider dbProvider;

        [TestInitialize]
        public void Initialize()
        {
            dbProvider = new SqlDbProvider();
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ItemCatalog_RM"].ConnectionString);
            dbProvider.Connection.Open();
            dbProvider.Transaction = dbProvider.Connection.BeginTransaction();

            queryHandler = new UpdateAndGetEventQueueInProcessQuery(dbProvider);
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbProvider.Transaction.Rollback();
            dbProvider.Transaction.Dispose();
            dbProvider.Connection.Dispose();
        }

        [TestMethod]
        public void UpdateChangeQueue_EventsExistWithNullInProcessBy_ShouldMarkEventRecordsAsInProcess()
        {
            //Given
            string sql = @"INSERT INTO mammoth.PriceChangeQueue (Item_Key, Identifier, EventTypeId)
                            VALUES (@Item_Key, @Identifier, @EventTypeId);
                            SELECT CAST(SCOPE_IDENTITY() as int);";

            List<int> idList = new List<int>();
            foreach (var item in this.dbProvider.Connection.Query<dynamic>("SELECT TOP 100 Item_Key, Identifier FROM ItemIdentifier", transaction: dbProvider.Transaction))
            {
                idList.Add(this.dbProvider.Connection.Query<int>(sql,
                        new { Item_Key = item.Item_Key, Identifier = item.Identifier, EventTypeId = IrmaEventTypes.Price },
                        transaction: this.dbProvider.Transaction).Single());
            }

            //When
            var result = queryHandler.Search(new UpdateAndGetEventQueueInProcessParameters
            {
                Instance = 100,
                MaxNumberOfRowsToMark = 100
            });

            //Then
            var numberOfItemsWithInstanceId = dbProvider.Connection.Query<int>(
                @"select count(*) from mammoth.PriceChangeQueue 
                    where EventTypeId = @EventTypeId 
                    and InProcessBy = @InstanceId",
                new { EventTypeId = IrmaEventTypes.Price, InstanceId = 100 },
                transaction: dbProvider.Transaction);
            Assert.AreEqual(100, numberOfItemsWithInstanceId.First());
            Assert.AreEqual(100, result.Count());
        }
    }
}
