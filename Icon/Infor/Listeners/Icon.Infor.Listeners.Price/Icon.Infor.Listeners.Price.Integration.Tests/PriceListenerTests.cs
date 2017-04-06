using Dapper;
using FastMember;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.Price.DataAccess.Models;
using Mammoth.Common.DataAccess.DbProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Xml.Linq;

namespace Icon.Infor.Listeners.Price.Integration.Tests
{
    [TestClass]
    public class PriceListenerTests
    {
        private PriceListener listener;
        private SqlDbProvider dbProvider;
        private string region = "FL";
        private int storeBuId = 777777777;
        private int? maxItemId;
        private Item item;
        private Guid gpmId = new Guid("0415f02a-4485-4558-bedc-44eb3c7374d3");

        [TestInitialize]
        public void InitializeTest()
        {
            dbProvider = new SqlDbProvider { Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString) };
            dbProvider.Connection.Open();

            // Add Test Locale
            this.dbProvider.Connection
                .Execute($"INSERT INTO Locales_{this.region} (BusinessUnitID, StoreName, StoreAbbrev, AddedDate) VALUES ({storeBuId}, 'TEST STORE YAY', 'TES', GETDATE())",
                    this.region);

            // Add Test Items to DB
            this.maxItemId = this.dbProvider.Connection.Query<int?>("SELECT MAX(ItemID) FROM Items").FirstOrDefault();
            this.item = new TestItemBuilder().WithScanCode("30777703").WithItemId((maxItemId ?? default(int)) + 1).Build();
            AddToItemsTable(new List<Item> { this.item });
        }

        [TestCleanup]
        public void CleanupTest()
        {
            // Cleanup data that was added as part of this test.
            this.dbProvider.Connection.Execute($"DELETE FROM Items WHERE ItemID = @ItemID",
                new { ItemID = this.item.ItemID });
            this.dbProvider.Connection.Execute($"DELETE FROM dbo.Locales_{this.region} WHERE BusinessUnitID = @Bu",
                new { Bu = this.storeBuId });
            this.dbProvider.Connection.Execute($"DELETE FROM gpm.Price_{this.region} WHERE GpmID = @GpmID",
                new { GpmID = this.gpmId });
            this.dbProvider.Connection.Execute($"DELETE FROM gpm.DeletedPrices WHERE GpmID = @GpmID",
                new { GpmID = this.gpmId });
            this.dbProvider.Connection.Execute($"DELETE FROM gpm.MessageArchivePrice WHERE GpmID = @GpmID",
                new { GpmID = this.gpmId });
            this.dbProvider.Connection.Close();
        }

        [TestMethod]
        public void HandleMessage_PriceMessageWithOnePrice_AddsPriceToTheDatabase()
        {
            //Given
            //These properties match the PriceMessage.xml properties. If that message changes then so should these
            UpdateXmlWithItemId(this.item.ItemID, "TestMessages/AddPriceMessage.xml");
            int itemId = this.item.ItemID;
            int businessUnitId = this.storeBuId;
            string priceType = "REG";
            DateTime startDate = new DateTime(1970, 1, 1);
            decimal expectedPrice = 8.99m;
            Guid expectedGpmId = this.gpmId;
            int expectedCurrencyId = 1;
            string expectedPriceTypeAttribute = "EDV";
            string expectedPriceUom = "EA";
            int expectedMultiple = 1;
            DateTime? expectedEndDate = null;
            DateTime beforeTestDateTime = DateTime.Now;
            DateTime? expectedNewTagExpiration = new DateTime(2018, 3, 21);

            var container = SimpleInjectorInitializer.CreateContainer();
            listener = container.GetInstance<IListenerApplication>() as PriceListener;
            var messageText = XDocument.Load("TestMessages/AddPriceMessage.xml");
            Mock<IEsbMessage> esbMessage = new Mock<IEsbMessage>();
            esbMessage.SetupGet(m => m.MessageText).Returns(messageText.ToString());

            //When
            Assert.IsNotNull(listener);
            listener.HandleMessage(null, new EsbMessageEventArgs { Message = esbMessage.Object });

            //Then
            var sql = $@"select *
                        from gpm.Price_FL p
                        where p.ItemID = @ItemId
	                        and p.BusinessUnitID = @BusinessUnitId
	                        and p.PriceType = @PriceType
                            and p.StartDate = @StartDate";
            var actualPriceModel = this.dbProvider.Connection.Query<DbPriceModel>(sql,
                new
                {
                    ItemId = itemId,
                    BusinessUnitId = businessUnitId,
                    PriceType = priceType,
                    StartDate = startDate
                }).SingleOrDefault();
            Assert.AreEqual(itemId, actualPriceModel.ItemID, "ItemId does not match");
            Assert.AreEqual(businessUnitId, actualPriceModel.BusinessUnitID, "BusinessUnitId does not match");
            Assert.AreEqual(priceType, actualPriceModel.PriceType, "PriceType does not match");
            Assert.AreEqual(startDate.ToString(), actualPriceModel.StartDate.ToString(), "StartDate does not match");
            Assert.AreEqual(expectedGpmId, actualPriceModel.GpmID, "GpmId does not match");
            Assert.AreEqual(expectedPrice, actualPriceModel.Price, "Price does not match");
            Assert.AreEqual(expectedCurrencyId, actualPriceModel.CurrencyID, "CurrencyId does not match");
            Assert.AreEqual(expectedPriceTypeAttribute, actualPriceModel.PriceTypeAttribute, "PriceTypeAttribute does not match");
            Assert.AreEqual(expectedPriceUom, actualPriceModel.PriceUOM, "PriceUOM does not match");
            Assert.AreEqual(expectedMultiple, actualPriceModel.Multiple, "Multiple does not match");
            Assert.AreEqual(expectedEndDate.ToString(), actualPriceModel.EndDate.ToString(), "EndDate does not match");
            Assert.IsTrue(actualPriceModel.AddedDate > beforeTestDateTime, "AddedDate is not correct");
            // TODO: Once NewTagExpiration is decided update message and Assertion
            //Assert.AreEqual(expectedNewTagExpiration.ToString(), actualPriceModel.NewTagExpiration.ToString(), "NewTagExpiration does not match");

            string archiveSql = "SELECT * FROM gpm.MessageArchivePrice WHERE GpmID = @GpmID";
            var actualArchive = this.dbProvider.Connection.Query(archiveSql, new { GpmID = this.gpmId }, this.dbProvider.Transaction);
            Assert.IsNotNull(actualArchive);
        }

        //[TestMethod]
        // Need to know where ReplaceGpmId comes from in the message before running this test.
        public void HandleMessage_ReplacePriceMessageWithOnePrice_DeletesOldPriceAndAddsNewPriceToTheDatabase()
        {
            //Given
            //These properties match the PriceMessage.xml properties. If that message changes then so should these
            var itemId = 2308979;
            var businessUnitId = 10352;
            var priceType = "REG";
            var expectedPrice = 8.98m;
            using (var transaction = new TransactionScope())
            {
                var container = SimpleInjectorInitializer.CreateContainer();
                listener = container.GetInstance<IListenerApplication>() as PriceListener;
                var messageText = XDocument.Load("TestMessages/ReplacePriceMessage.xml");
                Mock<IEsbMessage> esbMessage = new Mock<IEsbMessage>();
                esbMessage.SetupGet(m => m.MessageText).Returns(messageText.ToString());

                //When
                Assert.IsNotNull(listener);
                listener.HandleMessage(null, new EsbMessageEventArgs { Message = esbMessage.Object });

                //Then
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString))
                {
                    var sql = $@"select p.Price from gpm.Price_FL p
                                where p.ItemID = {itemId}
	                                and p.BusinessUnitID = {businessUnitId}
	                                and p.PriceType = '{priceType}'";
                    var price = connection.ExecuteScalar<decimal>(sql);
                    Assert.AreEqual(expectedPrice, price);
                }
            }
        }

        private Item BuildItem()
        {
            this.maxItemId = this.dbProvider.Connection.Query<int?>("SELECT MAX(ItemID) FROM Items").FirstOrDefault();
            Item newItem = new TestItemBuilder().WithScanCode("30777703").WithItemId((maxItemId ?? default(int)) + 1).Build();
            return newItem;
        }

        private void AddToItemsTable(List<Item> items)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(this.dbProvider.Connection as SqlConnection))
            {
                using (var reader = ObjectReader.Create(
                    items,
                    nameof(Item.ItemID),
                    nameof(Item.ItemTypeID),
                    nameof(Item.ScanCode),
                    nameof(Item.HierarchyMerchandiseID),
                    nameof(Item.HierarchyNationalClassID),
                    nameof(Item.BrandHCID),
                    nameof(Item.TaxClassHCID),
                    nameof(Item.PSNumber),
                    nameof(Item.Desc_POS),
                    nameof(Item.Desc_Product),
                    nameof(Item.PackageUnit),
                    nameof(Item.RetailSize),
                    nameof(Item.RetailUOM),
                    nameof(Item.FoodStampEligible)))
                {
                    bulkCopy.DestinationTableName = "[dbo].[Items]";
                    bulkCopy.WriteToServer(reader);
                }
            }
        }

        private void UpdateXmlWithItemId(int itemId, string fileName)
        {
            var xml = XDocument.Load(fileName);
            var element = xml.Elements()
                .First(e => e.Name.LocalName == "items")
                .Elements()
                .First(fe => fe.Name.LocalName == "item")
                .Elements()
                .First(se => se.Name.LocalName == "id");
            element.SetValue(itemId);
            xml.Save(fileName);
        }

    }
}
