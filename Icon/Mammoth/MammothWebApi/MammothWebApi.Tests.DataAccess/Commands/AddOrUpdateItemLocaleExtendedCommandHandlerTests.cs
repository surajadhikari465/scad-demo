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
using System.Data.SqlTypes;

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
        private List<int> itemIDs
        {
            get
            {
                return this.items?.Select(item => item.ItemID).ToList();
            }
        }

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

            // Add rows to staging table (no existing extended attributes for item-locale)
            for (int i = 0; i < this.items.Count; i++)
            {
                expectedItems.Add(CreateStagingItemLocaleExtendedModel(transactionId, this.items[i].ScanCode, now, "USA"));
            }
            AddRowsToItemLocaleExtendedStagingTable(expectedItems);

            AssertNoExistingExtendedAttributesForItems(this.itemIDs);

            AddOrUpdateItemLocaleExtendedCommand command = new AddOrUpdateItemLocaleExtendedCommand();
            command.Timestamp = now;
            command.TransactionId = transactionId;
            command.Region = this.region;

            // When
            this.commandHandler.Execute(command);

            // Then
            var actual = GetItemExtendedAttributesList(this.itemIDs);
            Assert.AreEqual(expectedItems.Count, actual.Count, $"Expected {expectedItems.Count} Extended Attribute records to be created but found {actual.Count}.");
            for (int i = 0; i < expectedItems.Count; i++)
            {
                AssertPropertiesMatchStaged(expectedItems[i], actual[i]);
                Assert.AreEqual(new SqlDateTime(now).Value, actual[i].AddedDate, "The AddedDate is not the expected value.");
                Assert.IsNull(actual[i].ModifiedDate, "The Modified Date is not null");
            }
        }

        [TestMethod]
        public void AddOrUpdateItemLocaleExtended_ExistingItemLocaleExtendedRowsInStagingWithMatchingTransactionId_UpdatesRowsInRegionalItemLocaleExtendedTable()
        {
            // Given
            DateTime now = DateTime.Now;
            DateTime addedDate = new DateTime(2015, 5, 15);
            var existingItemAttributesExt = new List<ItemAttributes_Locale_Ext>();
            var expectedItems = new List<StagingItemLocaleExtendedModel>();
            Guid transactionId = Guid.NewGuid();

            for (int i = 0; i < this.items.Count; i++)
            {
                // Add existing rows to ItemAttributes_Locale_Ext table
                existingItemAttributesExt.Add(
                    CreateItemAttributes_Locale_ExtModel(this.items[i].ItemID, addedDate, "Canada"));
                // Add rows to staging table
                expectedItems.Add(
                    CreateStagingItemLocaleExtendedModel(transactionId, this.items[i].ScanCode, now, "USA"));
            }
            AddRowsToItemAttributesLocaleExtendedTable(existingItemAttributesExt, this.region);
            AddRowsToItemLocaleExtendedStagingTable(expectedItems);
            
            // setup command parameters
            AddOrUpdateItemLocaleExtendedCommand command = new AddOrUpdateItemLocaleExtendedCommand();
            command.Timestamp = now;
            command.TransactionId = transactionId;
            command.Region = this.region;

            // When
            this.commandHandler.Execute(command);

            // Then
            var actual = GetItemExtendedAttributesList(this.itemIDs);

            for (int i = 0; i < expectedItems.Count; i++)
            {
                AssertPropertiesMatchStaged(expectedItems[i], actual[i]);
                Assert.AreEqual(new SqlDateTime(addedDate).Value, actual[i].AddedDate, "The AddedDate is not the expected value.");
                Assert.AreEqual(new SqlDateTime(now).Value, actual[i].ModifiedDate, "The Modified Date did not match.");
            }
        }

        [TestMethod]
        public void AddOrUpdateItemLocaleExtended_NewItemLocaleExtendedRowsInStagingWithNonMatchingTransactionId_DoesNotAddsRowsInRegionalItemLocaleExtendedTable()
        {
            // Given
            DateTime now = DateTime.Now;
            Guid transactionId = Guid.NewGuid();
            var expectedItems = new List<StagingItemLocaleExtendedModel>();

            // Add rows to staging table (no existing extended attributes for item-locale)
            for (int i = 0; i < this.items.Count; i++)
            {
                expectedItems.Add(CreateStagingItemLocaleExtendedModel(transactionId, this.items[i].ScanCode, now, "USA"));
            }
            AddRowsToItemLocaleExtendedStagingTable(expectedItems);

            AssertNoExistingExtendedAttributesForItems(this.itemIDs);

            AddOrUpdateItemLocaleExtendedCommand command = new AddOrUpdateItemLocaleExtendedCommand();
            command.Timestamp = now;
            command.TransactionId = Guid.NewGuid();
            command.Region = this.region;

            // When
            this.commandHandler.Execute(command);

            // Then
            var actualRowCount = GetItemExtendedAttributesCount(this.itemIDs);
            Assert.AreEqual(0, actualRowCount,
                $"Rows were added to the ItemAttributes_Locale_{this.region}_Ext table then they should not have been because of non - matching transaction ID.");
        }

        [TestMethod]
        public void AddOrUpdateItemLocaleExtended_ExistingItemLocaleExtendedRowsInStagingWithNonMatchingTransactionId_DoesNotUpdatesRowsInRegionalItemLocaleExtendedTable()
        {
            // Given
            DateTime now = DateTime.Now;
            Guid transactionId = Guid.NewGuid();
            DateTime addedDate = new DateTime(2015, 5, 15);
            var existingItemAttributesExt = new List<ItemAttributes_Locale_Ext>();
            var expectedItems = new List<StagingItemLocaleExtendedModel>();

            for (int i = 0; i < this.items.Count; i++)
            {
                // Add existing rows to ItemAttributes_Locale_Ext table
                existingItemAttributesExt.Add(
                    CreateItemAttributes_Locale_ExtModel(this.items[i].ItemID, addedDate, "Canada"));
                // Add rows to staging table
                expectedItems.Add(
                    CreateStagingItemLocaleExtendedModel(transactionId, this.items[i].ScanCode, now, "USA"));
            }
            AddRowsToItemAttributesLocaleExtendedTable(existingItemAttributesExt, this.region);
            AddRowsToItemLocaleExtendedStagingTable(expectedItems);            

            // setup command parameters
            AddOrUpdateItemLocaleExtendedCommand command = new AddOrUpdateItemLocaleExtendedCommand();
            command.Timestamp = now;
            command.TransactionId = Guid.NewGuid(); // different Guid
            command.Region = this.region;

            // When
            this.commandHandler.Execute(command);

            // Then
            var actual = GetItemExtendedAttributesList(this.itemIDs);

            for (int i = 0; i < expectedItems.Count; i++)
            {
                AssertPropertiesMatch(existingItemAttributesExt[i], actual[i]);
            }
        }

        [TestMethod]
        public void AddOrUpdateItemLocaleExtended_ExistingItemLocaleExtendedRowsInStagingWithNullAttributeValues_DeletesExistingItemLocaleExtendedRows()
        {
            // Given
            DateTime now = DateTime.Now;
            DateTime addedDate = new DateTime(2015, 5, 15);
            var existingItemAttributesExt = new List<ItemAttributes_Locale_Ext>();
            var expectedItems = new List<StagingItemLocaleExtendedModel>();
            Guid transactionId = Guid.NewGuid();

            for (int i = 0; i < this.items.Count; i++)
            {
                // Add existing rows to ItemAttributes_Locale_Ext table
                existingItemAttributesExt.Add(
                    CreateItemAttributes_Locale_ExtModel(this.items[i].ItemID, addedDate, "Canada"));
                // Add rows to staging table
                expectedItems.Add(
                    CreateStagingItemLocaleExtendedModel(transactionId, this.items[i].ScanCode, now, null));
            }
            AddRowsToItemAttributesLocaleExtendedTable(existingItemAttributesExt, this.region);
            AddRowsToItemLocaleExtendedStagingTable(expectedItems);            

            // setup command parameters
            AddOrUpdateItemLocaleExtendedCommand command = new AddOrUpdateItemLocaleExtendedCommand();
            command.Timestamp = now;
            command.TransactionId = transactionId;
            command.Region = this.region;

            // When
            this.commandHandler.Execute(command);

            // Then
            var actualCount = GetItemExtendedAttributesCount(this.itemIDs);
            Assert.AreEqual(0, actualCount, "The existing NULL rows were not deleted as expected.");
        }

        [TestMethod]
        public void AddOrUpdateItemLocaleExtended_NewItemLocaleExtendedRowsInStagingWithNullAttributeValue_DoesNotAddRowsInRegionalItemLocaleExtendedTable()
        {
            // Given
            DateTime now = DateTime.Now;
            Guid transactionId = Guid.NewGuid();
            var expectedItems = new List<StagingItemLocaleExtendedModel>();
            
            for (int i = 0; i < this.items.Count; i++)
            {
                // Add rows to staging table
                expectedItems.Add(
                    CreateStagingItemLocaleExtendedModel(transactionId, this.items[i].ScanCode, now, null));
            }
            AddRowsToItemLocaleExtendedStagingTable(expectedItems);

            AssertNoExistingExtendedAttributesForItems(this.itemIDs);

            AddOrUpdateItemLocaleExtendedCommand command = new AddOrUpdateItemLocaleExtendedCommand();
            command.Timestamp = now;
            command.TransactionId = transactionId;
            command.Region = this.region;

            // When
            this.commandHandler.Execute(command);

            // Then
            var actualCount = GetItemExtendedAttributesCount(this.itemIDs);
            Assert.AreEqual(0, actualCount, "The rows with NULL values were added when they should not have been.");
        }

        [TestMethod]
        public void AddOrUpdateItemLocaleExtended_MixedDataInStaging_ModifiesExpectedRowsInRegionalItemLocaleTable()
        {
            // Given
            DateTime now = DateTime.Now;
            DateTime addedDate = new DateTime(2015, 5, 15);
            var existingItemAttributesExt = new List<ItemAttributes_Locale_Ext>();
            var stagedItemModels = new List<StagingItemLocaleExtendedModel>();
            int expectedCountAfterUpdate = 2;
            Guid transactionId = Guid.NewGuid();

            // item #1 to delete (null attribute value in staging)
            existingItemAttributesExt.Add(
                CreateItemAttributes_Locale_ExtModel(this.items[0].ItemID, addedDate, "Canada"));
            stagedItemModels.Add(
                CreateStagingItemLocaleExtendedModel(transactionId, this.items[0].ScanCode, now, null));

            // item #2 to insert (no pre-existing record)
            stagedItemModels.Add(
                CreateStagingItemLocaleExtendedModel(transactionId, this.items[1].ScanCode, now, "USA"));

            // item #3 to update (altered attribute value)
            existingItemAttributesExt.Add(
                CreateItemAttributes_Locale_ExtModel(this.items[2].ItemID, addedDate, "Canada"));
            stagedItemModels.Add(
                CreateStagingItemLocaleExtendedModel(transactionId, this.items[2].ScanCode, now, "USA"));

            AddRowsToItemAttributesLocaleExtendedTable(existingItemAttributesExt, this.region);
            AddRowsToItemLocaleExtendedStagingTable(stagedItemModels);
            AssertNoExistingExtendedAttributesForItems(new List<int> { this.items[1].ItemID });

            // setup command parameters
            AddOrUpdateItemLocaleExtendedCommand command = new AddOrUpdateItemLocaleExtendedCommand();
            command.Timestamp = now;
            command.TransactionId = transactionId;
            command.Region = this.region;

            // When
            this.commandHandler.Execute(command);

            // Then
            //first item ext attributes should be gone, one should be inserted, and one should be updated
            var actual = GetItemExtendedAttributesList(this.itemIDs);
            Assert.IsNotNull(actual, "Should be some Extended Attribute data for items.");
            Assert.AreEqual(expectedCountAfterUpdate, actual.Count, $"Expected {expectedCountAfterUpdate} Extended Attribute records.");
            Assert.AreEqual(0, actual.Count(x => x.ItemID == this.items[0].ItemID), "Extended Attributes should have been deleted for this item-locale.");

            var insertedItemLocaleAttributes = actual.First(x => x.ItemID == this.items[1].ItemID);
            AssertPropertiesMatchStaged(stagedItemModels[1], insertedItemLocaleAttributes);
            Assert.AreEqual(new SqlDateTime(now).Value, insertedItemLocaleAttributes.AddedDate, "The Extended Attribute should have been added during this test.");
            Assert.IsNull(insertedItemLocaleAttributes.ModifiedDate, "The Extended Attribute should not have been modified");

            var updatedItemLocaleAttributes = actual.First(x => x.ItemID == this.items[2].ItemID);
            AssertPropertiesMatchStaged(stagedItemModels[2], updatedItemLocaleAttributes);
            Assert.AreEqual(new SqlDateTime(addedDate).Value, updatedItemLocaleAttributes.AddedDate, "The Extended Attribute should have kept the same AddedDate during this test.");
            Assert.AreEqual(new SqlDateTime(now).Value, updatedItemLocaleAttributes.ModifiedDate, "The Extended Attribute should have been modified during this test.");
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

        private int GetItemExtendedAttributesCount(IEnumerable<int> itemIDs)
        {
            return GetItemExtendedAttributes(itemIDs).Count();
        }

        private List<ItemAttributes_Locale_Ext> GetItemExtendedAttributesList(IEnumerable<int> itemIDs)
        {
            return GetItemExtendedAttributes(itemIDs).ToList();
        }
        private IEnumerable<ItemAttributes_Locale_Ext> GetItemExtendedAttributes(IEnumerable<int> itemIDs)
        {
            return GetItemExtendedAttributes(itemIDs, this.region, this.locale.LocaleID);
        }

        private IEnumerable<ItemAttributes_Locale_Ext> GetItemExtendedAttributes(IEnumerable<int> itemIDs, string region, int localeID)
        {
            string queryString = $"SELECT * FROM ItemAttributes_Locale_{region}_Ext WHERE ItemID IN @Items AND LocaleID = @LocaleID";
            var queryArgs = new { Items = itemIDs, LocaleID = localeID };
            return this.db.Connection.Query<ItemAttributes_Locale_Ext>(queryString, queryArgs, this.db.Transaction);
        }
        private void AssertNoExistingExtendedAttributesForItems(IEnumerable<int> itemIDs)
        {
            int existingRowCount = GetItemExtendedAttributesCount(itemIDs);
            Assert.AreEqual(0, existingRowCount,
                $"The test data is not setup correctly. Rows already exist in the ItemAttributes_Locale_{this.region}_Ext table, and the test is testing the addition of these rows.");
        }

        private StagingItemLocaleExtendedModel CreateStagingItemLocaleExtendedModel( Guid transactionId,
            string scanCode, DateTime timeStamp = default(DateTime), string attributeValue = null, int attributeId = Attributes.Origin)
        {
            var model = new StagingItemLocaleExtendedModel();

            model.Region = string.IsNullOrEmpty(this.region) ? "SW" : this.region;
            model.BusinessUnitId = this.locale.BusinessUnitID;
            model.ScanCode = scanCode;
            model.TransactionId = transactionId == default(Guid) ? Guid.NewGuid() : transactionId;
            model.Timestamp = timeStamp == default(DateTime) ? DateTime.Now : timeStamp;
            model.AttributeValue = attributeValue;
            model.AttributeId = attributeId;

            return model;
        }

        private ItemAttributes_Locale_Ext CreateItemAttributes_Locale_ExtModel(int itemID, DateTime addedDate,
            string attributeValue = null, int attributeId = Attributes.Origin, DateTime modifiedDate = default(DateTime))
        {
            var model = new ItemAttributes_Locale_Ext();

            model.Region = this.region;
            model.LocaleID = this.locale.LocaleID;
            model.ItemID = itemID;
            model.AttributeID = attributeId;
            model.AttributeValue = string.IsNullOrEmpty(attributeValue) ? "Canada" : attributeValue;
            model.AddedDate = addedDate;
            if (modifiedDate == default(DateTime))
            {
                model.ModifiedDate = null;
            }
            else
            {
                model.ModifiedDate = modifiedDate;
            }

            return model;
        }

        private void AssertPropertiesMatch<T>(T expected, T actual)
            where T : ItemAttributes_Locale_Ext
        {
            Assert.AreEqual(expected.Region, actual.Region, "Region did not match.");
            Assert.AreEqual(expected.LocaleID, actual.LocaleID, "Region did not match.");
            Assert.AreEqual(expected.ItemID, actual.ItemID, "ModifiedDate did not match.");
            Assert.AreEqual(expected.AttributeID, actual.AttributeID, "AttributeId did not match.");
            Assert.AreEqual(expected.AttributeValue, actual.AttributeValue, "AttributeValue did not match.");
            Assert.AreEqual(expected.AddedDate, actual.AddedDate, "AddedDate did not match.");
            Assert.AreEqual(expected.ModifiedDate, actual.ModifiedDate, "ModifiedDate did not match.");
        }

        private void AssertPropertiesMatchStaged<T, U>(T expected, U actual, int expectedLocaleID, IEnumerable<int> expectedItemIds)
            where T : StagingItemLocaleExtendedModel
            where U : ItemAttributes_Locale_Ext
        {
            Assert.AreEqual(expected.Region, actual.Region, "Region did not match.");
            Assert.AreEqual(expectedLocaleID, actual.LocaleID, "LocaleID did not match.");
            Assert.AreEqual(expected.AttributeId, actual.AttributeID, "AttributeId did not match.");
            Assert.AreEqual(expected.AttributeValue, actual.AttributeValue, "AttributeValue did not match.");
            Assert.IsTrue(expectedItemIds.Contains(actual.ItemID), "The expected ItemIDs were not used.");
        }

        private void AssertPropertiesMatchStaged<T, U>(T expected, U actual)
            where T : StagingItemLocaleExtendedModel
            where U : ItemAttributes_Locale_Ext
        {
            AssertPropertiesMatchStaged(expected, actual, this.locale.LocaleID, this.itemIDs);
        }
    }
}
