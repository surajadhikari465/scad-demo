using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using Dapper;
using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Models;
using Icon.Web.Tests.Integration.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class UpdateItemProhibitDiscountByHierarchyClassCommandHandlerTests
    {
        private UpdateItemProhibitDiscountByHierarchyClassCommandHandler commandHandler;
        private UpdateItemProhibitDiscountByHierarchyClassCommand command;
        private IDbConnection db;
        private ItemTestHelper itemHelper;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            this.db = SqlConnectionBuilder.CreateIconConnection();
            this.command = new UpdateItemProhibitDiscountByHierarchyClassCommand();
            this.commandHandler = new UpdateItemProhibitDiscountByHierarchyClassCommandHandler(this.db);
            this.itemHelper = new ItemTestHelper();
            this.itemHelper.Initialize(this.db);
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.transaction.Dispose();
            this.db.Close();
        }

        [TestMethod]
        public void UpdateItemProhibitDiscountByHierarchyClassCommand_ItemsAssociatedToSubBrick_UpdatesAttributes()
        {
            // Given
            DateTime now = DateTime.Now;
            string expectedProhibitDiscount = "true";
            string expectedUserName = "User For Integration Test";
            string expectedModifiedDateTimeUtc = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:ss.ffffff");

            this.command.HierarchyClassId = this.itemHelper.TestHierarchyClasses
                .First(hc => hc.Key == HierarchyNames.Merchandise)
                .Value[0]
                .HierarchyClassId;
            this.command.ProhibitDiscount = expectedProhibitDiscount;
            this.command.UserName = expectedUserName;
            this.command.ModifiedDateTimeUtc = expectedModifiedDateTimeUtc;

            // When
            this.commandHandler.Execute(this.command);

            // Then
            var item = this.db.Query<ItemDbModel>("SELECT * FROM ItemView i WHERE i.ItemID = @itemID", new { itemID = this.itemHelper.TestItem.ItemId }).First();
            Dictionary<string, string> attributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(item.ItemAttributesJson);
            Assert.AreEqual(expectedProhibitDiscount, attributes["ProhibitDiscount"]);
            Assert.AreEqual(expectedUserName, attributes["ModifiedBy"]);
            Assert.AreEqual(expectedModifiedDateTimeUtc, attributes["ModifiedDateTimeUtc"]);
        }
    }
}
