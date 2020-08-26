using System;
using System.Data;
using System.Linq;
using System.Transactions;
using Dapper;
using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Models;
using Icon.Web.Tests.Integration.TestHelpers;
using Icon.Web.Tests.Integration.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class AddMessageQueueItemByHierarchyClassIdCommadHandlerTests
    {
        private AddMessageQueueItemByHierarchyClassIdCommadHandler commandHandler;
        private TransactionScope transaction;
        private IDbConnection db;
        private ItemTestHelper itemTestHelper;

        [TestInitialize]
        public void Initialize()
        {
            this.transaction = new TransactionScope();
            this.db = SqlConnectionBuilder.CreateIconConnection();
            this.commandHandler = new AddMessageQueueItemByHierarchyClassIdCommadHandler(this.db);
            this.itemTestHelper = new ItemTestHelper();
            this.itemTestHelper.Initialize(this.db);
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.transaction.Dispose();
            this.db.Close();
            this.db.Dispose();
        }

        [TestMethod]
        public void AddMessageQueueItemByHierarchyClass_HierarchyClassId_AddsItemIdsAssociatedToHierarchyClassId()
        {
            // Given
            var commandParameters = new AddMessageQueueItemByHierarchyClassIdCommand { HierarchyClassId = itemTestHelper.TestItem.MerchandiseHierarchyClassId };
            DateTime utcNow = DateTime.UtcNow.AddMinutes(-1); // Keep 1 minute to avoid probles with Server time synchronization.

            // When
            this.commandHandler.Execute(commandParameters);

            // Then
            var queuedItem = this.db
                .Query<TestMessageQueueItemModel>("SELECT ItemID as ItemId, InsertDateUtc, EsbReadyDateTimeUtc FROM esb.MessageQueueItem WHERE ItemID = @itemId",
                    new { itemId = itemTestHelper.TestItem.ItemId })
                .FirstOrDefault();
            Assert.AreEqual(itemTestHelper.TestItem.ItemId, queuedItem.ItemId);
            Assert.IsTrue(queuedItem.InsertDateUtc >= utcNow);
            Assert.IsTrue(queuedItem.EsbReadyDateTimeUtc >= utcNow);

        }

        [TestMethod]
        public void AddMessageQueueItemByHierarchyClass_CouponItemAssociatedToHierarchyClassId_DoesNotAddMessageQueueRecord()
        {
            // Given
            var commandParameters = new AddMessageQueueItemByHierarchyClassIdCommand { HierarchyClassId = itemTestHelper.TestItem.MerchandiseHierarchyClassId };
            ItemDbModel itemDbModel = new ItemDbModel
            {
                ItemId = itemTestHelper.TestItem.ItemId,
                ItemTypeId = ItemTypes.Coupon
            };
            DateTime utcNow = DateTime.UtcNow;
            this.itemTestHelper.TestItem.ItemTypeId = ItemTypes.Coupon;
            this.itemTestHelper.UpdateItemType(itemDbModel);

            // When
            this.commandHandler.Execute(commandParameters);

            // Then
            var queuedItem = this.db
                .Query<TestMessageQueueItemModel>("SELECT ItemID as ItemId, InsertDateUtc, EsbReadyDateTimeUtc FROM esb.MessageQueueItem WHERE ItemID = @itemId",
                    new { itemId = itemTestHelper.TestItem.ItemId })
                .FirstOrDefault();
            Assert.IsNull(queuedItem);

        }
    }
}
