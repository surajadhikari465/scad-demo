using System;
using System.Collections.Generic;
using Icon.Web.DataAccess.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Icon.Web.Tests.Integration.TestHelpers;
using System.Data;
using Dapper;
using Icon.Web.DataAccess.Models;
using Icon.Web.Common.Utility;
using System.Transactions;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class UpdateSkuCommandHandlerTests
    {
        private IDbConnection db;
        private UpdateSkuCommandHandler updateSkuCommandHandler;
        private int newItemGroupId;
        private TransactionScope transaction;
        private int itemGroupTypeId;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            this.db = SqlConnectionBuilder.CreateIconConnection();
            updateSkuCommandHandler = new UpdateSkuCommandHandler(this.db);
            itemGroupTypeId = GetSkuItemGroupTypeId();
            InsertItemGroup(itemGroupTypeId);
            GetNewtemGroupId();
        }
        [TestMethod]
        public void UpdateSkuDescription_SkuDescriptionIsModified()
        {
            updateSkuCommandHandler.Execute(new UpdateSkuCommand { ModifiedBy = "Test", ModifiedDateTimeUtc = DateTime.UtcNow.ToFormattedDateTimeString(), SkuDescription = "Test", SkuId = newItemGroupId });

            var updatedSkuDescription = GetSkuDescriptionById(newItemGroupId);

            Assert.AreEqual(updatedSkuDescription, "Test");
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.transaction.Dispose();
            this.db.Close();
        }

        private SkuModel GetSkuById(int skuId)
        {
            //string sql = @"SELECT * from attributes where attributeId = @attributeId";
            string sql = @"SELECT *
                            FROM dbo.itemGroup a 
                        where a.itemGroupId = @itemGroupId";

            SkuModel skuModel = this.db.Query<SkuModel>(sql, new { itemGroupId = skuId }).FirstOrDefault();

            return skuModel;
        }

        private int GetSkuItemGroupTypeId()
        {
            string sql = @"SELECT TOP 1 itemGroupTypeId from itemGroupType where itemGroupTypeName = 'SKU'";
            int itemGroupTypeId = this.db.Query<int>(sql).First();

            return itemGroupTypeId;
        }

        private int GetNewtemGroupId()
        {
            string sql = @"SELECT TOP 1 itemgroupId from itemgroup where LastModifiedBy = 'TestUserTesting'";
            newItemGroupId = this.db.Query<int>(sql).First();

            return newItemGroupId;
        }
        private void InsertItemGroup(int itemGroupTypeId)
        {
            string ItemGroupDescription = "{ \"SkuDescription\":\"Deposit Bottle 24 Pack3\" }";
            string sql = @"
                        INSERT INTO [dbo].[itemgroup]
                                   ([ItemGroupTypeId]
                                   ,[ItemGroupAttributesJson]
                                   ,[LastModifiedBy])
                            VALUES
                            (
                                 @itemGroupTypeId,
                                 @ItemGroupDescription,
                                'TestUserTesting'
                            )";

            int affectedRows = this.db.Execute(sql, new { itemGroupTypeId = itemGroupTypeId, ItemGroupDescription = ItemGroupDescription });
        }

        private string GetSkuDescriptionById(int newItemGroupId)
        {
            string sql = @"SELECT TOP 1 json_value(ItemGroupAttributesJson,'$.SkuDescription') from Itemgroup where ItemGroupId =@newItemGroupId";
            return this.db.Query<string>(sql, new { newItemGroupId = newItemGroupId }).First();
        }
    }
}