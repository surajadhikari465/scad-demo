using Dapper;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Models;
using Icon.Web.Tests.Integration.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class PublishItemUpdatesCommandHandlerTests
    {
        private PublishItemUpdatesCommandHandler commandHandler;
        private PublishItemUpdatesCommand command;
        private SqlConnection dbConnection;
        private TransactionScope transaction;
        private ItemTestHelper itemTestHelper;
        private string testRegionCode1 = "T1";
        private string testRegionCode2 = "T2";

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            dbConnection = SqlConnectionBuilder.CreateIconConnection();
            commandHandler = new PublishItemUpdatesCommandHandler(dbConnection);
            command = new PublishItemUpdatesCommand();

            itemTestHelper = new ItemTestHelper();
            itemTestHelper.Initialize(dbConnection);

            if(dbConnection.QueryFirstOrDefault<int?>("SELECT EventId FROM app.EventType WHERE EventName = 'Item Update'") == null)
            {
                dbConnection.Execute("INSERT INTO app.EventType(EventName) VALUES ('Item Update')");
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void PublishItemUpdates_ScanCodeFound_CreatesDatabaseRecordInMessageQueueItemTableAndEventsInEventQueueTable()
        {
            //Given
            dbConnection.Execute(@"
                INSERT INTO app.IRMAItemSubscription(RegionCode, Identifier, InsertDate, DeleteDate)
                VALUES (@RegionCode1, @ScanCode, GETDATE(), NULL),
                       (@RegionCode2, @ScanCode, GETDATE(), NULL)",
                new
                {
                    RegionCode1 = testRegionCode1,
                    RegionCode2 = testRegionCode2,
                    itemTestHelper.TestItem.ScanCode
                });
            command.ScanCodes = new List<string>
            {
                itemTestHelper.TestItem.ScanCode
            };

            //When
            commandHandler.Execute(command);

            //Then
            var itemUpdateEventTypeId = dbConnection.QueryFirst<int>("SELECT EventId FROM app.EventType WHERE EventName = 'Item Update'");
            var events = dbConnection.Query<EventQueueModel>(@"
                SELECT 
                    QueueId,
                    EventId,
                    EventMessage,
                    EventReferenceId,
                    RegionCode,
                    InsertDate,
                    ProcessFailedDate,
                    InProcessBy
                FROM app.EventQueue
                WHERE EventMessage = @ScanCode",
                new
                {
                    itemTestHelper.TestItem.ScanCode
                });

            Assert.AreEqual(2, events.Count());
            Assert.IsTrue(events.All(
                e => e.EventMessage == itemTestHelper.TestItem.ScanCode
                    && e.EventReferenceId == itemTestHelper.TestItem.ItemId
                    && e.EventId == itemUpdateEventTypeId));
            Assert.AreEqual(1, events.Count(e => e.RegionCode == testRegionCode1));
            Assert.AreEqual(1, events.Count(e => e.RegionCode == testRegionCode2));

            var messageQueueItem = dbConnection.QuerySingleOrDefault<MessageQueueItemModel>(@"
                SELECT * FROM esb.MessageQueueItem WHERE ItemId = @ItemId",
               new { itemTestHelper.TestItem.ItemId });

            Assert.IsNotNull(messageQueueItem);
        }

        [TestMethod]
        public void PublishItemUpdates_IrmaItemSubscriptionIsDeleted_CreatesDatabaseRecordInMessageQueueItemTableAndEventsInEventQueueTableForItemItemSubscriptionsThatAreNotDeleted()
        {
            //Given
            dbConnection.Execute(@"
                INSERT INTO app.IRMAItemSubscription(RegionCode, Identifier, InsertDate, DeleteDate)
                VALUES (@RegionCode1, @ScanCode, GETDATE(), GETDATE()),
                       (@RegionCode2, @ScanCode, GETDATE(), NULL)",
                new
                {
                    RegionCode1 = testRegionCode1,
                    RegionCode2 = testRegionCode2,
                    itemTestHelper.TestItem.ScanCode
                });
            command.ScanCodes = new List<string>
            {
                itemTestHelper.TestItem.ScanCode
            };

            //When
            commandHandler.Execute(command);

            //Then
            var itemUpdateEventTypeId = dbConnection.QueryFirst<int>("SELECT EventId FROM app.EventType WHERE EventName = 'Item Update'");
            var events = dbConnection.Query<EventQueueModel>(@"
                SELECT 
                    QueueId,
                    EventId,
                    EventMessage,
                    EventReferenceId,
                    RegionCode,
                    InsertDate,
                    ProcessFailedDate,
                    InProcessBy
                FROM app.EventQueue
                WHERE EventMessage = @ScanCode",
                new
                {
                    itemTestHelper.TestItem.ScanCode
                });

            Assert.AreEqual(1, events.Count());
            Assert.IsTrue(events.All(
                e => e.EventMessage == itemTestHelper.TestItem.ScanCode
                    && e.EventReferenceId == itemTestHelper.TestItem.ItemId
                    && e.EventId == itemUpdateEventTypeId));
            Assert.AreEqual(1, events.Count(e => e.RegionCode == testRegionCode2));

            var messageQueueItem = dbConnection.QuerySingleOrDefault<MessageQueueItemModel>(@"
                SELECT * FROM esb.MessageQueueItem WHERE ItemId = @ItemId",
               new { itemTestHelper.TestItem.ItemId });

            Assert.IsNotNull(messageQueueItem);
        }
    }
}
