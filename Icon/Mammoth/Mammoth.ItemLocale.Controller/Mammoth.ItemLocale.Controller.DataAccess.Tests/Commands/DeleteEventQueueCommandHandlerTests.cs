using Dapper;
using Mammoth.Common.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Mammoth.ItemLocale.Controller.DataAccess.Commands;
using Mammoth.Common.DataAccess.DbProviders;

namespace Mammoth.ItemLocale.Controller.DataAccess.Tests.Commands
{
    [TestClass]
    public class DeleteEventQueueCommandHandlerTests
    {
        private DeleteEventQueueCommandHandler commandHandler;
        private SqlDbProvider dbProvider;

        [TestInitialize]
        public void Initialize()
        {
            dbProvider = new SqlDbProvider();
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ItemCatalog_FL"].ConnectionString);
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
            var queueIds = dbProvider.Connection.Query<int>(@"
                declare @QueueIds table(QueueId int)

                insert into mammoth.ItemLocaleChangeQueue(Item_Key, Identifier, EventTypeID, EventReferenceID, InsertDate)
                    output inserted.QueueId 
                        into @QueueIds
                values (@Item_Key, @Identifier, @EventTypeId, @EventReferenceId, @InsertDate)
                
                select * from @QueueIds",
                new { Item_Key = item.Item_Key, Identifier = item.Identifier, EventTypeId = IrmaEventTypes.ItemLocaleAddOrUpdate, EventReferenceId = testEventReferenceId, InsertDate = DateTime.Now },
                dbProvider.Transaction);

            Assert.AreEqual(1, dbProvider.Connection.Query<int>(@"select count(*) from mammoth.ItemLocaleChangeQueue where EventReferenceID = @Id", new { Id = testEventReferenceId }, transaction: dbProvider.Transaction).First());

            //When
            commandHandler.Execute(new DeleteEventQueueCommand
            {
                QueueIds = queueIds.ToList()
            });

            //Then
            Assert.AreEqual(0, dbProvider.Connection.Query<int>(@"select count(*) from mammoth.ItemLocaleChangeQueue where EventReferenceID = @Id", new { Id = testEventReferenceId }, transaction: dbProvider.Transaction).First());
        }
    }
}
