using Mammoth.Common.DataAccess.DbProviders;
using Dapper;
using Mammoth.Common.DataAccess;
using MammothWebApi.DataAccess.Commands;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.Tests.DataAccess.ModelBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace MammothWebApi.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class AddOrUpdateItemLocaleExtendedCommandHandlerTests
    {
        private AddOrUpdateItemLocaleExtendedCommandHandler commandHandler;
        private SqlDbProvider db;
        private DateTime timestamp;
        private Locales locale;
        private string region;
        private List<Item> items;
        private int? maxItemId;

        [TestInitialize]
        public void InitializeTests()
        {
            this.timestamp = DateTime.Now;
            string connectionString = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;
            this.db = new SqlDbProvider();
            this.db.Connection = new SqlConnection(connectionString);
            this.db.Connection.Open();
            this.db.Transaction = this.db.Connection.BeginTransaction();

            this.commandHandler = new AddOrUpdateItemLocaleExtendedCommandHandler(this.db);

            // Add Test Locale
            this.region = "MW";
            this.locale = new Locales
            {
                BusinessUnitID = 77777,
                StoreAbbrev = "TEST",
                StoreName = "TEST STORE NAME 1"
            };
            AddLocaleToDb(locale, this.region);
            this.locale = this.db.Connection.Query<Locales>(String.Format("SELECT * FROM Locales_{0} WHERE BusinessUnitID = @BusinessUnitID", this.region),
                new { BusinessUnitID = this.locale.BusinessUnitID }, this.db.Transaction).First();

            // Add Test Items
            this.maxItemId = this.db.Connection.Query<int?>("SELECT MAX(ItemID) FROM Items", transaction: this.db.Transaction).FirstOrDefault();
            this.items = new List<Item>();
            this.items.Add(new TestItemBuilder().WithScanCode("111111777771").WithItemId((maxItemId ?? default(int)) + 1).Build());
            this.items.Add(new TestItemBuilder().WithScanCode("111111777772").WithItemId((maxItemId ?? default(int)) + 2).Build());
            this.items.Add(new TestItemBuilder().WithScanCode("111111777773").WithItemId((maxItemId ?? default(int)) + 3).Build());
            AddToItemsTable(this.items);

        }

        [TestCleanup]
        public void CleanupTest()
        {
            if (this.db.Transaction != null)
            {
                this.db.Transaction.Rollback();
                this.db.Transaction.Dispose();
            }
            this.db.Connection.Dispose();
        }

        [TestMethod]
        public void AddOrUpdateItemLocaleExtended_NewItemLocaleExtendedRowsInStagingWithMatchingTransactionId_AddsRowsInRegionalItemLocaleExtendedTable()
        {
            // Given
            DateTime now = DateTime.Now;
            Guid transactionId = Guid.NewGuid();

            var expectedItems = new List<StagingItemLocaleExtendedModel>();
            for (int i = 0; i < this.items.Count; i++)
            {
                StagingItemLocaleExtendedModel model = new StagingItemLocaleExtendedModel
                {
                    AttributeId = Attributes.Origin,
                    AttributeValue = "USA",
                    ScanCode = this.items[i].ScanCode,
                    BusinessUnitId = this.locale.BusinessUnitID,
                    Region = this.region,
                    Timestamp = now,
                    TransactionId = transactionId
                };
                expectedItems.Add(model);
            }
            AddRowsToItemLocaleExtendedStagingTable(expectedItems);

            int exisitingRows = this.db.Connection
                .Query<int>(String.Format("SELECT COUNT(*) FROM ItemAttributes_Locale_{0}_Ext WHERE ItemID IN @Items AND LocaleID = @LocaleID", this.region),
                            new { Items = this.items.Select(item => item.ItemID), LocaleID = this.locale.LocaleID },
                            this.db.Transaction)
                .First();

            if (exisitingRows > 0)
            {
                Assert.Fail(String.Format(@"The test data is not setup correctly. Rows already exist in the ItemAttributes_Locale_{0}_Ext table,
                            and the test is testing the addition of these rows.", this.region));
            }

            AddOrUpdateItemLocaleExtendedCommand command = new AddOrUpdateItemLocaleExtendedCommand();
            command.Timestamp = now;
            command.TransactionId = transactionId;
            command.Region = this.region;

            // When
            this.commandHandler.Execute(command);

            // Then
            var actual = this.db.Connection
                .Query<ItemAttributes_Locale_Ext>(String.Format("SELECT * FROM ItemAttributes_Locale_{0}_Ext WHERE ItemID IN @Items AND LocaleID = @LocaleID", this.region),
                            new { Items = this.items.Select(i => i.ItemID), LocaleID = this.locale.LocaleID },
                            this.db.Transaction)
                .ToList();

            for (int i = 0; i < expectedItems.Count; i++)
            {
                Assert.AreEqual(expectedItems[i].AttributeId, actual[i].AttributeID, "AttributeId did not match.");
                Assert.AreEqual(expectedItems[i].AttributeValue, actual[i].AttributeValue, "AttributeValue did not match.");
                Assert.AreEqual(this.locale.LocaleID, actual[i].LocaleID, "LocaleID did not match.");
                Assert.AreEqual(expectedItems[i].Region, actual[i].Region, "Region did not match.");
                Assert.IsTrue(actual[i].AddedDate.ToString() == now.ToString(), "The AddedDate is not the expected value.");
                Assert.IsNull(actual[i].ModifiedDate, "The Modified Date is not null");
                Assert.IsTrue(this.items.Select(e => e.ItemID).Contains(actual[i].ItemID), "The expected ItemIDs were not used.");
            }
        }

        [TestMethod]
        public void AddOrUpdateItemLocaleExtended_ExistingItemLocaleExtendedRowsInStagingWithMatchingTransactionId_UpdatesRowsInRegionalItemLocaleExtendedTable()
        {
            // Given
            DateTime now = DateTime.Now;
            DateTime addedDate = new DateTime(2015, 5, 15);

            // Add existing rows to ItemAttributes_Locale_Ext table
            var existingItemAttributesExt = new List<ItemAttributes_Locale_Ext>();
            for (int i = 0; i < this.items.Count; i++)
            {
                ItemAttributes_Locale_Ext itemAttributeExt = new ItemAttributes_Locale_Ext
                {
                    Region = this.region,
                    ItemID = this.items[i].ItemID,
                    LocaleID = this.locale.LocaleID,
                    AddedDate = addedDate,
                    ModifiedDate = null,
                    AttributeID = Attributes.Origin,
                    AttributeValue = "Canada"
                };
                existingItemAttributesExt.Add(itemAttributeExt);
            }
            AddRowsToItemAttributesLocaleExtendedTable(existingItemAttributesExt, this.region);

            // Add rows to staging
            Guid transactionId = Guid.NewGuid();
            var expectedItems = new List<StagingItemLocaleExtendedModel>();
            for (int i = 0; i < this.items.Count; i++)
            {
                StagingItemLocaleExtendedModel model = new StagingItemLocaleExtendedModel
                {
                    AttributeId = Attributes.Origin,
                    AttributeValue = "USA",
                    ScanCode = this.items[i].ScanCode,
                    BusinessUnitId = this.locale.BusinessUnitID,
                    Region = this.region,
                    Timestamp = now,
                    TransactionId = transactionId
                };
                expectedItems.Add(model);
            }
            AddRowsToItemLocaleExtendedStagingTable(expectedItems);

            // setup command parameters
            AddOrUpdateItemLocaleExtendedCommand command = new AddOrUpdateItemLocaleExtendedCommand();
            command.Timestamp = now;
            command.TransactionId = transactionId;
            command.Region = this.region;

            // When
            this.commandHandler.Execute(command);

            // Then
            var actual = this.db.Connection
                .Query<ItemAttributes_Locale_Ext>(String.Format("SELECT * FROM ItemAttributes_Locale_{0}_Ext WHERE ItemID IN @Items AND LocaleID = @LocaleID", this.region),
                            new { Items = this.items.Select(i => i.ItemID), LocaleID = this.locale.LocaleID },
                            this.db.Transaction)
                .ToList();

            for (int i = 0; i < expectedItems.Count; i++)
            {
                Assert.AreEqual(expectedItems[i].AttributeId, actual[i].AttributeID, "AttributeId did not match.");
                Assert.AreEqual(expectedItems[i].AttributeValue, actual[i].AttributeValue, "AttributeValue did not match.");
                Assert.AreEqual(this.locale.LocaleID, actual[i].LocaleID, "LocaleID did not match.");
                Assert.AreEqual(expectedItems[i].Region, actual[i].Region, "Region did not match.");
                Assert.AreEqual(addedDate.ToString(), actual[i].AddedDate.ToString(), "The AddedDate is not the expected value.");
                Assert.IsTrue(actual[i].ModifiedDate.ToString() == now.ToString(), "The Modified Date did not match.");
                Assert.IsTrue(this.items.Select(e => e.ItemID).Contains(actual[i].ItemID), "The expected ItemIDs were not used.");
            }
        }

        [TestMethod]
        public void AddOrUpdateItemLocaleExtended_NewItemLocaleExtendedRowsInStagingWithNonMatchingTransactionId_DoesNotAddsRowsInRegionalItemLocaleExtendedTable()
        {
            // Given
            DateTime now = DateTime.Now;
            Guid transactionId = Guid.NewGuid();

            var expectedItems = new List<StagingItemLocaleExtendedModel>();
            for (int i = 0; i < this.items.Count; i++)
            {
                StagingItemLocaleExtendedModel model = new StagingItemLocaleExtendedModel
                {
                    AttributeId = Attributes.Origin,
                    AttributeValue = "USA",
                    ScanCode = this.items[i].ScanCode,
                    BusinessUnitId = this.locale.BusinessUnitID,
                    Region = this.region,
                    Timestamp = now,
                    TransactionId = transactionId
                };
                expectedItems.Add(model);
            }
            AddRowsToItemLocaleExtendedStagingTable(expectedItems);

            int exisitingRows = this.db.Connection
                .Query<int>(String.Format("SELECT COUNT(*) FROM ItemAttributes_Locale_{0}_Ext WHERE ItemID IN @Items AND LocaleID = @LocaleID", this.region),
                            new { Items = this.items.Select(item => item.ItemID), LocaleID = this.locale.LocaleID },
                            this.db.Transaction)
                .First();

            if (exisitingRows > 0)
            {
                Assert.Fail(String.Format(@"The test data is not setup correctly. Rows already exist in the ItemAttributes_Locale_{0}_Ext table,
                            and the test is testing the addition of these rows.", this.region));
            }

            AddOrUpdateItemLocaleExtendedCommand command = new AddOrUpdateItemLocaleExtendedCommand();
            command.Timestamp = now;
            command.TransactionId = Guid.NewGuid();
            command.Region = this.region;

            // When
            this.commandHandler.Execute(command);

            // Then
            var actual = this.db.Connection
                .Query<ItemAttributes_Locale_Ext>(String.Format("SELECT * FROM ItemAttributes_Locale_{0}_Ext WHERE ItemID IN @Items AND LocaleID = @LocaleID", this.region),
                            new { Items = this.items.Select(i => i.ItemID), LocaleID = this.locale.LocaleID },
                            this.db.Transaction)
                .ToList();

            Assert.IsTrue(actual.Count == 0, String.Format(@"Rows were added to the ItemAttributes_Locale_{0}_Ext table then they should not have been
                because of non-matching timestamp.", this.region));
        }

        [TestMethod]
        public void AddOrUpdateItemLocaleExtended_ExistingItemLocaleExtendedRowsInStagingWithNonMatchingTransactionId_DoesNotUpdatesRowsInRegionalItemLocaleExtendedTable()
        {
            // Given
            DateTime now = DateTime.Now;
            Guid transactionId = Guid.NewGuid();
            DateTime addedDate = new DateTime(2015, 5, 15);

            // Add existing rows to ItemAttributes_Locale_Ext table
            var existingItemAttributesExt = new List<ItemAttributes_Locale_Ext>();
            for (int i = 0; i < this.items.Count; i++)
            {
                ItemAttributes_Locale_Ext itemAttributeExt = new ItemAttributes_Locale_Ext
                {
                    Region = this.region,
                    ItemID = this.items[i].ItemID,
                    LocaleID = this.locale.LocaleID,
                    AddedDate = addedDate,
                    ModifiedDate = null,
                    AttributeID = Attributes.Origin,
                    AttributeValue = "Canada",
                };
                existingItemAttributesExt.Add(itemAttributeExt);
            }
            AddRowsToItemAttributesLocaleExtendedTable(existingItemAttributesExt, this.region);

            // Add rows to staging
            var expectedItems = new List<StagingItemLocaleExtendedModel>();
            for (int i = 0; i < this.items.Count; i++)
            {
                StagingItemLocaleExtendedModel model = new StagingItemLocaleExtendedModel
                {
                    AttributeId = Attributes.Origin,
                    AttributeValue = "USA",
                    ScanCode = this.items[i].ScanCode,
                    BusinessUnitId = this.locale.BusinessUnitID,
                    Region = this.region,
                    Timestamp = now,
                    TransactionId = transactionId
                };
                expectedItems.Add(model);
            }
            AddRowsToItemLocaleExtendedStagingTable(expectedItems);

            // setup command parameters
            AddOrUpdateItemLocaleExtendedCommand command = new AddOrUpdateItemLocaleExtendedCommand();
            command.Timestamp = now;
            command.TransactionId = Guid.NewGuid(); // different Guid
            command.Region = this.region;

            // When
            this.commandHandler.Execute(command);

            // Then
            var actual = this.db.Connection
                .Query<ItemAttributes_Locale_Ext>(String.Format("SELECT * FROM ItemAttributes_Locale_{0}_Ext WHERE ItemID IN @Items AND LocaleID = @LocaleID", this.region),
                            new { Items = this.items.Select(i => i.ItemID), LocaleID = this.locale.LocaleID },
                            this.db.Transaction)
                .ToList();

            for (int i = 0; i < expectedItems.Count; i++)
            {
                Assert.AreEqual(existingItemAttributesExt[i].AttributeID, actual[i].AttributeID, "AttributeId did not match.");
                Assert.AreEqual(existingItemAttributesExt[i].AttributeValue, actual[i].AttributeValue, "AttributeValue did not match.");
                Assert.AreEqual(this.locale.LocaleID, actual[i].LocaleID, "LocaleID did not match.");
                Assert.AreEqual(existingItemAttributesExt[i].Region, actual[i].Region, "Region did not match.");
                Assert.AreEqual(addedDate.ToString(), actual[i].AddedDate.ToString(), "The AddedDate is not the expected value.");
                Assert.AreEqual(existingItemAttributesExt[i].ModifiedDate.ToString(), actual[i].ModifiedDate.ToString(), "The Modified Date did not match.");
                Assert.IsTrue(this.items.Select(e => e.ItemID).Contains(actual[i].ItemID), "The expected ItemIDs were not used.");
            }
        }

        [TestMethod]
        public void AddOrUpdateItemLocaleExtended_ExistingItemLocaleExtendedRowsInStagingWithNullAttributeValues_DeletesExistingItemLocaleExtendedRows()
        {
            // Given
            DateTime now = DateTime.Now;
            DateTime addedDate = new DateTime(2015, 5, 15);

            // Add existing rows to ItemAttributes_Locale_Ext table
            var existingItemAttributesExt = new List<ItemAttributes_Locale_Ext>();
            for (int i = 0; i < this.items.Count; i++)
            {
                ItemAttributes_Locale_Ext itemAttributeExt = new ItemAttributes_Locale_Ext
                {
                    Region = this.region,
                    ItemID = this.items[i].ItemID,
                    LocaleID = this.locale.LocaleID,
                    AddedDate = addedDate,
                    ModifiedDate = null,
                    AttributeID = Attributes.Origin,
                    AttributeValue = "Canada"
                };
                existingItemAttributesExt.Add(itemAttributeExt);
            }
            AddRowsToItemAttributesLocaleExtendedTable(existingItemAttributesExt, this.region);

            // Add rows to staging
            Guid transactionId = Guid.NewGuid();
            var expectedItems = new List<StagingItemLocaleExtendedModel>();
            for (int i = 0; i < this.items.Count; i++)
            {
                StagingItemLocaleExtendedModel model = new StagingItemLocaleExtendedModel
                {
                    AttributeId = Attributes.Origin,
                    AttributeValue = null,
                    ScanCode = this.items[i].ScanCode,
                    BusinessUnitId = this.locale.BusinessUnitID,
                    Region = this.region,
                    Timestamp = now,
                    TransactionId = transactionId
                };
                expectedItems.Add(model);
            }
            AddRowsToItemLocaleExtendedStagingTable(expectedItems);

            // setup command parameters
            AddOrUpdateItemLocaleExtendedCommand command = new AddOrUpdateItemLocaleExtendedCommand();
            command.Timestamp = now;
            command.TransactionId = transactionId;
            command.Region = this.region;

            // When
            this.commandHandler.Execute(command);

            // Then
            var actual = this.db.Connection
                .Query<ItemAttributes_Locale_Ext>(String.Format("SELECT * FROM ItemAttributes_Locale_{0}_Ext WHERE ItemID IN @Items AND LocaleID = @LocaleID", this.region),
                            new { Items = this.items.Select(i => i.ItemID), LocaleID = this.locale.LocaleID },
                            this.db.Transaction)
                .ToList();

            Assert.AreEqual(0, actual.Count, "The existing NULL rows were not deleted as expected.");
        }

        [TestMethod]
        public void AddOrUpdateItemLocaleExtended_NewItemLocaleExtendedRowsInStagingWithNullAttributeValue_DoesNotAddRowsInRegionalItemLocaleExtendedTable()
        {
            // Given
            DateTime now = DateTime.Now;

            var expectedItems = new List<StagingItemLocaleExtendedModel>();
            for (int i = 0; i < this.items.Count; i++)
            {
                StagingItemLocaleExtendedModel model = new StagingItemLocaleExtendedModel
                {
                    AttributeId = Attributes.Origin,
                    AttributeValue = null,
                    ScanCode = this.items[i].ScanCode,
                    BusinessUnitId = this.locale.BusinessUnitID,
                    Region = this.region,
                    Timestamp = now
                };
                expectedItems.Add(model);
            }
            AddRowsToItemLocaleExtendedStagingTable(expectedItems);

            int exisitingRows = this.db.Connection
                .Query<int>(String.Format("SELECT COUNT(*) FROM ItemAttributes_Locale_{0}_Ext WHERE ItemID IN @Items AND LocaleID = @LocaleID", this.region),
                            new { Items = this.items.Select(item => item.ItemID), LocaleID = this.locale.LocaleID },
                            this.db.Transaction)
                .First();

            if (exisitingRows > 0)
            {
                Assert.Fail(String.Format(@"The test data is not setup correctly. Rows already exist in the ItemAttributes_Locale_{0}_Ext table,
                            and the test is testing the addition of these rows.", this.region));
            }

            AddOrUpdateItemLocaleExtendedCommand command = new AddOrUpdateItemLocaleExtendedCommand();
            command.Timestamp = now;
            command.Region = this.region;

            // When
            this.commandHandler.Execute(command);

            // Then
            var actual = this.db.Connection
                .Query<ItemAttributes_Locale_Ext>(String.Format("SELECT * FROM ItemAttributes_Locale_{0}_Ext WHERE ItemID IN @Items AND LocaleID = @LocaleID", this.region),
                            new { Items = this.items.Select(i => i.ItemID), LocaleID = this.locale.LocaleID },
                            this.db.Transaction)
                .ToList();

            Assert.AreEqual(0, actual.Count, "The rows with NULL values were added when they should not have been.");
        }

        private void AddLocaleToDb(Locales locale, string region)
        {
            string sql = String.Format(@"INSERT INTO dbo.Locales_{0}
                                        (
	                                        BusinessUnitID,
	                                        StoreName,
	                                        StoreAbbrev
                                        )
                                        VALUES
                                        (
	                                        @BusinessUnitID,
	                                        @StoreName,
	                                        @StoreAbbrev
                                        )", region);

            int affectedRows = this.db.Connection.Execute(sql, locale, transaction: this.db.Transaction);
        }

        private void AddToItemsTable(List<Item> items)
        {
            string sql = @"INSERT INTO Items
                            (
	                            ItemID,
	                            ItemTypeID,
	                            ScanCode,
	                            HierarchyMerchandiseID,
	                            HierarchyNationalClassID,
	                            BrandHCID,
	                            TaxClassHCID,
	                            PSNumber,
	                            Desc_POS,
	                            Desc_Product,
	                            PackageUnit,
	                            RetailSize,
	                            RetailUOM,
	                            FoodStampEligible
                            )
                            VALUES
                            (
	                            @ItemID,
	                            @ItemTypeID,
	                            @ScanCode,
	                            @HierarchyMerchandiseID,
	                            @HierarchyNationalClassID,
	                            @BrandHCID,
	                            @TaxClassHCID,
	                            @PSNumber,
	                            @Desc_POS,
	                            @Desc_Product,
	                            @PackageUnit,
	                            @RetailSize,
	                            @RetailUOM,
	                            @FoodStampEligible
                            )";
            int affectedRows = this.db.Connection.Execute(sql, items, transaction: this.db.Transaction);
        }

        private void AddRowsToItemLocaleExtendedStagingTable(List<StagingItemLocaleExtendedModel> itemLocalesExtended)
        {
            string sql = @" INSERT INTO stage.ItemLocaleExtended
                            (
                                Region,
	                            ScanCode,
	                            BusinessUnitId,
	                            AttributeId,
	                            AttributeValue,
	                            Timestamp,
                                TransactionId
                            )
                            VALUES
                            (
	                            @Region,
	                            @ScanCode,
	                            @BusinessUnitId,
	                            @AttributeId,
	                            @AttributeValue,
	                            @Timestamp,
                                @TransactionId
                            )";

            int affectedRows = this.db.Connection.Execute(sql, itemLocalesExtended, this.db.Transaction);
        }

        private void AddRowsToItemAttributesLocaleExtendedTable(List<ItemAttributes_Locale_Ext> existingItemAttributesExt, string region)
        {
            string sql = String.Format(@" INSERT INTO ItemAttributes_Locale_{0}_Ext
                            (
	                            ItemID,
	                            LocaleID,
	                            AttributeID,
	                            AttributeValue,
	                            AddedDate
                            )
                            VALUES
                            (
	                            @ItemID,
	                            @LocaleID,
	                            @AttributeID,
	                            @AttributeValue,
	                            @AddedDate
                            )", region);

            int affectedRows = this.db.Connection.Execute(sql, existingItemAttributesExt, this.db.Transaction);
        }
    }
}
