using Dapper;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.DataAccess.Commands;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.Tests.DataAccess.ModelBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;

namespace MammothWebApi.Tests.DataAccess.Commands
{
    [TestClass]
    public class AddOrUpdateItemLocaleSupplierCommandHandlerTests
    {
        private AddOrUpdateItemLocaleSupplierCommandHandler commandHandler;
        private SqlDbProvider db;
        private DateTime timestamp;
        private Locales locale;
        private string region;
        private List<Item> items;
        private int? maxItemId;

        private int bizUnitID
        {
            get { return locale == null ? 0 : this.locale.BusinessUnitID; }
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

            this.commandHandler = new AddOrUpdateItemLocaleSupplierCommandHandler(this.db);

            // Add Test Locale
            this.region = "SW";
            this.locale = new Locales
            {
                BusinessUnitID = 77777,
                LocaleID = 70000,
                StoreAbbrev = "TEST",
                StoreName = "TEST STORE NAME 1"
            };
            AddLocaleToDb(locale, this.region);

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
        public void AddOrUpdateItemLocaleSupplierCommand_NewItemLocaleDataInStaging_AddsRowsInRegionalItemLocaleSupplierTable()
        {
            // Given
            List<Item> existingItems = this.db.Connection
                .Query<Item>(@"SELECT * FROM Items WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = this.items.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            DateTime now = DateTime.Now;
            Guid transactionId = Guid.NewGuid();
            var expectedItemLocaleSuppliers = new List<StagingItemLocaleSupplierModel>();
            for (int i = 0; i < existingItems.Count; i++)
            {
                expectedItemLocaleSuppliers.Add(new TestStagingItemLocaleSupplierModelBuilder()
                    .WithRegion(this.region)
                    .WithBusinessUnit(this.bizUnitID)
                    .WithScanCode(existingItems[i].ScanCode)
                    .WithTimestamp(now)
                    .WithTransactionId(transactionId)
                    .Build());
            }

            AddItemsToStaging(expectedItemLocaleSuppliers);

            // When
            AddOrUpdateItemLocaleSupplierCommand command = new AddOrUpdateItemLocaleSupplierCommand { Region = region, Timestamp = now, TransactionId = transactionId };
            this.commandHandler.Execute(command);

            // Then
            var actualItemLocaleSuppliers = this.db.Connection
                .Query<ItemLocale_Supplier>("SELECT * FROM ItemLocale_Supplier_SW WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = existingItems.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            for (int i = 0; i < expectedItemLocaleSuppliers.Count; i++)
            {
                AssertPropertiesMatchStaged(this.bizUnitID, expectedItemLocaleSuppliers[i], actualItemLocaleSuppliers[i]);
            }
        }

        [TestMethod]
        public void AddOrUpdateItemLocaleSupplierCommand_ExistingItemLocaleDataInStaging_UpdatesRowsInRegionalItemLocaleSupplierTable()
        {
            // Given
            DateTime addedDate = DateTime.Now;

            List<Item> existingItems = this.db.Connection
                .Query<Item>(@"SELECT * FROM Items WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = this.items.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            var existingItemLocaleSuppliers = new List<ItemLocale_Supplier>();
            for (int i = 0; i < existingItems.Count; i++)
            {
                existingItemLocaleSuppliers.Add(new TestItemLocale_SupplierBuilder()
                    .WithRegion(this.region)
                    .WithBusinessUnit(this.bizUnitID)
                    .WithItemId(existingItems[i].ItemID)
                    .WithAddedDateUtc(addedDate)
                    .Build());
            }

            AddItemLocalesToDatabase(existingItemLocaleSuppliers, this.region);

            DateTime now = DateTime.Now;
            Guid transactionId = Guid.NewGuid();
            var expectedItemLocaleSuppliers = new List<StagingItemLocaleSupplierModel>();
            for (int i = 0; i < existingItems.Count; i++)
            {
                expectedItemLocaleSuppliers.Add(new TestStagingItemLocaleSupplierModelBuilder()
                    .WithRegion(this.region)
                    .WithBusinessUnit(this.bizUnitID)
                    .WithScanCode(existingItems[i].ScanCode)
                    .WithTimestamp(now)
                    .WithTransactionId(transactionId)
                    .WithIrmaVendorKey(i + existingItemLocaleSuppliers[i].IrmaVendorKey)
                    .WithSupplierCaseSize(i + existingItemLocaleSuppliers[i].SupplierCaseSize)
                    .WithSupplierItemId(i + existingItemLocaleSuppliers[i].SupplierItemID)
                    .WithSupplierName(i + existingItemLocaleSuppliers[i].SupplierName)
                    .Build());
            }

            AddItemsToStaging(expectedItemLocaleSuppliers);

            // When
            AddOrUpdateItemLocaleSupplierCommand command = new AddOrUpdateItemLocaleSupplierCommand { Region = this.region, Timestamp = now, TransactionId = transactionId };
            this.commandHandler.Execute(command);

            // Then
            var actualItemLocaleSuppliers = this.db.Connection
                .Query<ItemLocale_Supplier>(String.Format("SELECT * FROM ItemLocale_Supplier_{0} WHERE ItemID IN @ItemIDs", this.region),
                    new { ItemIDs = existingItems.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            for (int i = 0; i < expectedItemLocaleSuppliers.Count; i++)
            {
                AssertPropertiesMatchStaged(this.bizUnitID, expectedItemLocaleSuppliers[i], actualItemLocaleSuppliers[i]);
                Assert.AreEqual(addedDate.ToShortTimeString(), actualItemLocaleSuppliers[i].AddedDateUtc.ToShortTimeString(), "The AddedDate does not equal the expected added date.");
            }
        }

        [TestMethod]
        public void AddOrUpdateItemLocaleSupplierCommand_NewItemLocaleDataInStagingTimestampNotMatching_DoesAddRowsInRegionalItemLocaleSupplierTable()
        {
            // Given
            List<Item> existingItems = this.db.Connection
                .Query<Item>(@"SELECT * FROM Items WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = this.items.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            DateTime now = DateTime.Now;
            var expectedItemLocaleSuppliers = new List<StagingItemLocaleSupplierModel>();
            for (int i = 0; i < existingItems.Count; i++)
            {
                expectedItemLocaleSuppliers.Add(new TestStagingItemLocaleSupplierModelBuilder()
                    .WithRegion(this.region)
                    .WithBusinessUnit(this.bizUnitID)
                    .WithScanCode(existingItems[i].ScanCode)
                    .WithTimestamp(now)
                    .Build());
            }

            AddItemsToStaging(expectedItemLocaleSuppliers);

            // When
            AddOrUpdateItemLocaleSupplierCommand command = new AddOrUpdateItemLocaleSupplierCommand { Region = region, Timestamp = DateTime.UtcNow };
            this.commandHandler.Execute(command);

            // Then
            var actualItemLocaleSuppliers = this.db.Connection
                .Query<ItemLocale_Supplier>("SELECT * FROM ItemLocale_Supplier_SW WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = existingItems.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            Assert.IsTrue(actualItemLocaleSuppliers.Count == 0, "The ItemLocale_Supplier rows were added when they should not have been.");
        }

        [TestMethod]
        public void AddOrUpdateItemLocaleSupplierCommand_ExistingItemLocaleDataInStagingTimestampNotMatching_DoesNotUpdateRowsInRegionalItemLocaleSupplierTable()
        {
            // Given
            var existingTimestamp = DateTime.Now;
            var nonmatchingTimestamp = DateTime.UtcNow;
            List<Item> existingItems = this.db.Connection
                .Query<Item>(@"SELECT * FROM Items WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = this.items.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            var expectedItemLocaleSuppliers = new List<ItemLocale_Supplier>();
            for (int i = 0; i < existingItems.Count; i++)
            {
                expectedItemLocaleSuppliers.Add(new TestItemLocale_SupplierBuilder()
                    .WithRegion(this.region)
                    .WithBusinessUnit(this.bizUnitID)
                    .WithItemId(existingItems[1].ItemID)
                    .WithAddedDateUtc(existingTimestamp)
                    .Build());
            }

            AddItemLocalesToDatabase(expectedItemLocaleSuppliers, this.region);

            DateTime now = DateTime.Now;
            var stagedItemLocaleSuppliers = new List<StagingItemLocaleSupplierModel>();
            for (int i = 0; i < existingItems.Count; i++)
            {
                var stagedModel = new TestStagingItemLocaleSupplierModelBuilder()
                    .WithRegion(this.region)
                    .WithBusinessUnit(this.bizUnitID)
                    .WithScanCode(existingItems[i].ScanCode)
                    .WithSupplierName("Updated")
                    .WithTimestamp(now)
                    .Build();
                stagedItemLocaleSuppliers.Add(stagedModel);
            }

            AddItemsToStaging(stagedItemLocaleSuppliers);

            // When
            AddOrUpdateItemLocaleSupplierCommand command = new AddOrUpdateItemLocaleSupplierCommand { Region = this.region, Timestamp = nonmatchingTimestamp };
            this.commandHandler.Execute(command);

            // Then
            var actualItemLocaleSuppliers = this.db.Connection
                .Query<ItemLocale_Supplier>(String.Format("SELECT * FROM ItemLocale_Supplier_{0} WHERE ItemID IN @ItemIDs", this.region),
                    new { ItemIDs = existingItems.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            for (int i = 0; i < stagedItemLocaleSuppliers.Count; i++)
            {
                AssertPropertiesMatch(this.bizUnitID, expectedItemLocaleSuppliers[i], actualItemLocaleSuppliers[i]);
                Assert.IsTrue(actualItemLocaleSuppliers[i].AddedDateUtc < DateTime.Now, "The AddedDate was not greater than the timestamp.");
            }
        }

        [TestMethod]
        public void AddOrUpdateItemLocaleSupplierCommand_MixedDataInStaging_ModifiesExpectedRowsInRegionalItemLocaleSupplierTable()
        {
            // Given
            DateTime now = DateTime.Now;
            DateTime lastWeek = now.Subtract(new TimeSpan(7, 0, 0, 0));
            Guid transactionId = Guid.NewGuid();
            var preexistingItemLocales = new List<ItemLocale_Supplier>();
            var stagedItemLocales = new List<StagingItemLocaleSupplierModel>();

            //re-query for items added during initialize so we have current itemIDs
            List<Item> existingItems = this.db.Connection
                .Query<Item>(@"SELECT * FROM Items WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = this.items.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            // make one item already set up for the locale (should get updated)
            preexistingItemLocales.Add(new TestItemLocale_SupplierBuilder()
                .WithRegion(this.region)
                .WithBusinessUnit(this.bizUnitID)
                .WithItemId(existingItems[1].ItemID)
                .WithAddedDateUtc(lastWeek)
                .Build());

            // add three items to the staged data: 2 to be inserted, 1 to be updated
            stagedItemLocales.Add(new TestStagingItemLocaleSupplierModelBuilder()
                .WithRegion(this.region)
                .WithBusinessUnit(this.bizUnitID)
                .WithScanCode(existingItems[0].ScanCode)
                .WithTimestamp(now)
                .WithTransactionId(transactionId)
                .Build());
            stagedItemLocales.Add(new TestStagingItemLocaleSupplierModelBuilder()
                .WithRegion(this.region)
                .WithBusinessUnit(this.bizUnitID)
                .WithScanCode(existingItems[1].ScanCode)
                .WithTimestamp(now)
                .WithTransactionId(transactionId)
                .Build());
            stagedItemLocales.Add(new TestStagingItemLocaleSupplierModelBuilder()
                .WithRegion(this.region)
                .WithBusinessUnit(this.bizUnitID)
                .WithScanCode(existingItems[2].ScanCode)
                .WithTimestamp(now)
                .WithTransactionId(transactionId)
                .Build());

            //write initial test data to db
            AddItemLocalesToDatabase(preexistingItemLocales, this.region);
            AddItemsToStaging(stagedItemLocales);

            // When
            AddOrUpdateItemLocaleSupplierCommand command = new AddOrUpdateItemLocaleSupplierCommand { Region = region, Timestamp = now, TransactionId = transactionId };
            this.commandHandler.Execute(command);

            // Then
            var actualItemLocaleSuppliers = this.db.Connection
                .Query<ItemLocale_Supplier>("SELECT * FROM ItemLocale_Supplier_SW WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = existingItems.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .OrderBy(a => a.ItemID)
                .ToList();

            for (int i = 0; i < stagedItemLocales.Count; i++)
            {
                AssertPropertiesMatchStaged(this.bizUnitID, stagedItemLocales[i], actualItemLocaleSuppliers[i]);
                if (i % 2 == 1)
                {
                    //should have been updated
                    Assert.AreEqual(new SqlDateTime(lastWeek).Value, actualItemLocaleSuppliers[i].AddedDateUtc, "The AddedDate should not have changed.");
                    Assert.IsNotNull(actualItemLocaleSuppliers[i].ModifiedDateUtc, "The ModifiedDate should not have been NULL.");
                    Assert.AreEqual(new SqlDateTime(now).Value, actualItemLocaleSuppliers[i].ModifiedDateUtc, "The ModifiedDate should match the Timestamp.");
                }
                else
                {
                    //should have been inserted
                    Assert.AreEqual(new SqlDateTime(now).Value, actualItemLocaleSuppliers[i].AddedDateUtc, "The AddedDate should match the Timestamp.");
                    Assert.IsNull(actualItemLocaleSuppliers[i].ModifiedDateUtc, "The ModifiedDate should have been NULL.");
                }
            }
        }

        [TestMethod]
        public void AddOrUpdateItemLocaleSupplierCommand_SupplierNonExistent_DoesNotAddRowsInRegionalItemLocaleSupplierTable()
        {
            // Given
            List<Item> existingItems = this.db.Connection
                .Query<Item>(@"SELECT * FROM Items WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = this.items.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            DateTime now = DateTime.Now;
            Guid transactionId = Guid.NewGuid();

            // When
            AddOrUpdateItemLocaleSupplierCommand command = new AddOrUpdateItemLocaleSupplierCommand { Region = region, Timestamp = now, TransactionId = transactionId };
            this.commandHandler.Execute(command);

            // Then
            var actual = this.db.Connection
                .Query<ItemLocale_Supplier>("SELECT * FROM ItemLocale_Supplier_SW WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = existingItems.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();
            Assert.AreEqual(0, actual.Count, "No ItemLocaleSupplier records should have been written");
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

        private void AddItemsToStaging(List<StagingItemLocaleSupplierModel> itemLocales)
        {
            string sql = @"INSERT INTO stage.ItemLocaleSupplier
                            (
	                            Region,
	                            BusinessUnitID,
	                            ScanCode,
                                SupplierName,
                                SupplierItemID,
                                SupplierCaseSize,
                                IrmaVendorKey,
	                            Timestamp,
                                TransactionId
                            )
                            VALUES
                            (
	                            @Region,
	                            @BusinessUnitID,
	                            @ScanCode,
                                @SupplierName,
                                @SupplierItemID,
                                @SupplierCaseSize,
                                @IrmaVendorKey,
	                            @Timestamp,
                                @TransactionId
                            )";

            this.db.Connection.Execute(sql, itemLocales, transaction: this.db.Transaction);
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

        private void AddItemLocalesToDatabase(List<ItemLocale_Supplier> itemLocales, string region)
        {
            string sql = String.Format(@"INSERT INTO ItemLocale_Supplier_{0}
                            (
	                            Region,
                                ItemID,
	                            BusinessUnitID,
                                SupplierName,    
                                SupplierItemID,  
                                SupplierCaseSize,
                                IrmaVendorKey,
                                AddedDateUtc    
                            )
                            VALUES
                            (
	                            @Region,
	                            @ItemID,
                                @BusinessUnitID,
	                            @SupplierName,
	                            @SupplierItemID,
	                            @SupplierCaseSize,
                                @IrmaVendorKey,
	                            @AddedDateUtc
                            )", region);

            this.db.Connection.Execute(sql, itemLocales, transaction: this.db.Transaction);
        }

        private void AssertPropertiesMatch<T>(int expectedBusinessUnitID, T expected, T actual)
            where T : ItemLocale_Supplier
        {
            Assert.AreEqual(expectedBusinessUnitID, actual.BusinessUnitID,
                 $"The actual BusinessUnitID did not match the expected value: {expectedBusinessUnitID}.");

            Assert.AreEqual(expected.IrmaVendorKey, actual.IrmaVendorKey, "IrmaVendorKey value did not match expected.");
            Assert.AreEqual(expected.SupplierCaseSize, actual.SupplierCaseSize, "SupplierCaseSize value did not match expected.");
            Assert.AreEqual(expected.SupplierItemID, actual.SupplierItemID, "SupplierItemID value did not match expected.");
            Assert.AreEqual(expected.SupplierName, actual.SupplierName, "SupplierName value did not match expected.");
            Assert.AreEqual(expected.Region, actual.Region, "Region value did not match expected.");
            Assert.AreEqual(expected.ItemID, actual.ItemID, $"The actual ItemID did not match the expected value: {expected.ItemID}.");
        }

        private void AssertPropertiesMatchStaged<T, U>(int expectedBusinessUnitID, T expected, U actual)
            where T : StagingItemLocaleSupplierModel
            where U : ItemLocale_Supplier
        {
            Assert.IsTrue(actual.BusinessUnitID == expectedBusinessUnitID,
                $"The actual BusinessUnitID did not match the expected value: {expectedBusinessUnitID}.");

            Assert.AreEqual(expected.IrmaVendorKey, actual.IrmaVendorKey, "IrmaVendorKey value did not match expected.");
            Assert.AreEqual(expected.SupplierCaseSize, actual.SupplierCaseSize, "SupplierCaseSize value did not match expected.");
            Assert.AreEqual(expected.SupplierItemId, actual.SupplierItemID, "SupplierItemID value did not match expected.");
            Assert.AreEqual(expected.SupplierName, actual.SupplierName, "SupplierName value did not match expected.");
            Assert.AreEqual(expected.Region, actual.Region, "Region value did not match expected.");
            Assert.IsNotNull(actual.AddedDateUtc, "The AddedDate is NULL.");
        }
    }
}
