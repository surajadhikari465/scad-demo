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
    public class UpdateItemTypeByHierarchyClassCommandHandlerTests
    {
        private UpdateItemTypeByHierarchyClassCommandHandler commandHandler;
        private UpdateItemTypeByHierarchyClassCommand command;
        private IDbConnection db;
        private ItemTestHelper itemHelper;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            this.db = SqlConnectionBuilder.CreateIconConnection();
            this.command = new UpdateItemTypeByHierarchyClassCommand();
            this.commandHandler = new UpdateItemTypeByHierarchyClassCommandHandler(this.db);
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
        public void UpdateItemTypeByHierarchyClassCommand_ItemsAssociatedToSubBrick_UpdatesItemTypeId()
        {
            // Given
            DateTime now = DateTime.Now;
            int expectedFinancialId = this.db
                .Query<int>("SELECT hc.hierarchyClassId AS SubTeamId FROM HierarchyClass hc JOIN Hierarchy h ON hc.hierarchyID = h.hierarchyID WHERE hierarchyName = 'Financial'")
                .FirstOrDefault();
            int expectedItemTypeId = ItemTypes.Deposit;
            string expectedUserName = "User For Integration Test";
            string expectedDateTime = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:ss.ffffffZ");

            this.command.HierarchyClassId = this.itemHelper.TestHierarchyClasses
                .First(hc => hc.Key == HierarchyNames.Merchandise)
                .Value[0]
                .HierarchyClassId;
            this.command.ItemTypeId = expectedItemTypeId;
            this.command.UserName = expectedUserName;
            this.command.ModifiedDateTimeUtc = expectedDateTime;

            // When
            this.commandHandler.Execute(this.command);

            // Then
            var item = this.db.Query<ItemDbModel>("SELECT * FROM ItemView i WHERE i.ItemID = @itemID", new { itemID = this.itemHelper.TestItem.ItemId }).First();
            Dictionary<string,string> attributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(item.ItemAttributesJson);
            Assert.AreEqual(expectedItemTypeId, item.ItemTypeId);
            Assert.AreEqual(expectedUserName, attributes["ModifiedBy"]);
            Assert.AreEqual(expectedDateTime,attributes["ModifiedDateTimeUtc"]);
        }
    }
}
