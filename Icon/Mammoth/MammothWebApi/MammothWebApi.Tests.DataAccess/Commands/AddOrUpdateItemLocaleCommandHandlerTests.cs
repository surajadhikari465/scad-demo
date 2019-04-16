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

namespace MammothWebApi.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class AddOrUpdateItemLocaleCommandHandlerTests
    {
        private AddOrUpdateItemLocaleCommandHandler commandHandler;
        private SqlDbProvider db;
        private DateTime timestamp;
        private Locales locale;
        private string region;
        private List<Item> items;
        private int? maxItemId;

        private int bizUnitID
        {
            get { return locale == null ? 0 : this.locale.BusinessUnitID;  }
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

            this.commandHandler = new AddOrUpdateItemLocaleCommandHandler(this.db);

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
        public void AddOrUpdateItemLocaleCommand_NewItemLocaleDataInStaging_AddsRowsInRegionalItemLocaleTable()
        {
            // Given
            List<Item> existingItems = this.db.Connection
                .Query<Item>(@"SELECT * FROM Items WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = this.items.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            DateTime now = DateTime.Now;
            Guid transactionId = Guid.NewGuid();
            var expectedItems = new List<StagingItemLocaleModel>();
            for (int i = 0; i < existingItems.Count; i++)
            {
                expectedItems.Add(new TestStagingItemLocaleModelBuilder()
                    .WithRegion(this.region)
                    .WithBusinessUnit(this.bizUnitID)
                    .WithScanCode(existingItems[i].ScanCode)
                    .WithTimestamp(now)
                    .WithTransactionId(transactionId)
                    .WithScaleItem(true)
                    .Build());
            }

            AddItemsToStaging(expectedItems);

            // When
            AddOrUpdateItemLocaleCommand command = new AddOrUpdateItemLocaleCommand { Region = region, Timestamp = now, TransactionId = transactionId };
            this.commandHandler.Execute(command);

            // Then
            var actual = this.db.Connection
                .Query<ItemAttributes_Locale>("SELECT * FROM ItemAttributes_Locale_SW WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = existingItems.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            for (int i = 0; i < expectedItems.Count; i++)
            {
                AssertPropertiesMatchStaged(this.bizUnitID, expectedItems[i], actual[i]);
            }
        }

        [TestMethod]
        public void AddOrUpdateItemLocaleCommand_ExistingItemLocaleDataInStaging_UpdatesRowsInRespectiveRegionalItemLocaleTable()
        {
            // Given
            DateTime addedDate = DateTime.Now;

            List<Item> existingItems = this.db.Connection
                .Query<Item>(@"SELECT * FROM Items WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = this.items.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            var itemLocales = new List<ItemAttributes_Locale>();
            for (int i = 0; i < existingItems.Count; i++)
            {
                itemLocales.Add( new TestItemAttributeLocaleBuilder()
                    .WithRegion(this.region)
                    .WithBusinessUnit(this.bizUnitID)
                    .WithItemId(existingItems[i].ItemID)
                    .WithAddedDate(addedDate)
                    .Build());
            }

            AddItemLocalesToDatabase(itemLocales, this.region);

            DateTime now = DateTime.Now;
            Guid transactionId = Guid.NewGuid();
            var expectedItems = new List<StagingItemLocaleModel>();
            for (int i = 0; i < existingItems.Count; i++)
            {
                expectedItems.Add(new TestStagingItemLocaleModelBuilder()
                    .WithRegion(this.region)
                    .WithBusinessUnit(this.bizUnitID)
                    .WithScanCode(existingItems[i].ScanCode)
                    .WithTimestamp(now)
                    .WithTransactionId(transactionId)
                    .WithAuthorized(!itemLocales[i].Authorized)
                    .WithDiscontinued(!itemLocales[i].Discontinued)
                    .WithDiscountCase(!itemLocales[i].Discount_Case)
                    .WithDiscountTm(!itemLocales[i].Discount_TM)
                    .WithRestrictionAge(21)
                    .WithRomanceLong("TESTING THIS SIGN LONG ROMANCE TEXT FULL OF WONDER")
                    .WithRomanceShort("SHORT ROMANCE")
                    .WithLabelType("LG")
                    .Build());
            }

            AddItemsToStaging(expectedItems);

            // When
            AddOrUpdateItemLocaleCommand command = new AddOrUpdateItemLocaleCommand { Region = this.region, Timestamp = now, TransactionId = transactionId };
            this.commandHandler.Execute(command);

            // Then
            var actual = this.db.Connection
                .Query<ItemAttributes_Locale>(String.Format("SELECT * FROM ItemAttributes_Locale_{0} WHERE ItemID IN @ItemIDs", this.region),
                    new { ItemIDs = existingItems.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            for (int i = 0; i < expectedItems.Count; i++)
            {
                AssertPropertiesMatchStaged(this.bizUnitID, expectedItems[i], actual[i]);
                Assert.AreEqual(addedDate.ToShortTimeString(), actual[i].AddedDate.ToShortTimeString(), "The AddedDate does not equal the expected added date.");
            }
        }

        [TestMethod]
        public void AddOrUpdateItemLocaleCommand_NewItemLocaleDataInStagingTimestampNotMatching_DoesNotUpdatesRowsInRegionalItemLocaleTable()
        {
            // Given
            List<Item> existingItems = this.db.Connection
                .Query<Item>(@"SELECT * FROM Items WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = this.items.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            DateTime now = DateTime.Now;
            var expectedItems = new List<StagingItemLocaleModel>();
            for (int i = 0; i < existingItems.Count; i++)
            {
                expectedItems.Add(new TestStagingItemLocaleModelBuilder()
                    .WithRegion(this.region)
                    .WithBusinessUnit(this.bizUnitID)
                    .WithScanCode(existingItems[i].ScanCode)
                    .WithTimestamp(now)
                    //.WithTransactionId(transactionId)
                    .Build());
            }

            AddItemsToStaging(expectedItems);

            // When
            AddOrUpdateItemLocaleCommand command = new AddOrUpdateItemLocaleCommand { Region = region, Timestamp = DateTime.UtcNow };
            this.commandHandler.Execute(command);

            // Then
            var actual = this.db.Connection
                .Query<ItemAttributes_Locale>("SELECT * FROM ItemAttributes_Locale_SW WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = existingItems.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            Assert.IsTrue(actual.Count == 0, "The ItemLocale rows were added when they should not have been.");
        }

        [TestMethod]
        public void AddOrUpdateItemLocaleCommand_ExistingItemLocaleDataInStagingTimestampNotMatching_DoesNotUpdateRowsInRespectiveRegionalItemLocaleTable()
        {
            // Given
            var existingTimestamp = DateTime.Now;
            var nonmatchingTimestamp = DateTime.UtcNow;
            List<Item> existingItems = this.db.Connection
                .Query<Item>(@"SELECT * FROM Items WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = this.items.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            var expected = new List<ItemAttributes_Locale>();
            for (int i = 0; i < existingItems.Count; i++)
            {
                expected.Add(new TestItemAttributeLocaleBuilder()
                    .WithRegion(this.region)
                    .WithBusinessUnit(this.bizUnitID)
                    .WithItemId(existingItems[1].ItemID)
                    .WithAddedDate(existingTimestamp)
                    .Build());
            }

            AddItemLocalesToDatabase(expected, this.region);

            DateTime now = DateTime.Now;
            var stagedItems = new List<StagingItemLocaleModel>();
            for (int i = 0; i < existingItems.Count; i++)
            {
                var stagedModel = new TestStagingItemLocaleModelBuilder()
                    .WithRegion(this.region)
                    .WithBusinessUnit(this.bizUnitID)
                    .WithScanCode(existingItems[i].ScanCode)
                    .WithTimestamp(now)
                    .WithAuthorized(!expected[i].Authorized)
                    .WithDiscontinued(!expected[i].Discontinued)
                    .WithDiscountCase(!expected[i].Discount_Case)
                    .WithDiscountTm(!expected[i].Discount_TM)
                    .WithRestrictionAge(21)
                    .WithRomanceLong("TESTING THIS SIGN LONG ROMANCE TEXT FULL OF WONDER")
                    .WithRomanceShort("SHORT ROMANCE")
                    .WithLabelType("LG")
                    .Build();
                stagedItems.Add(stagedModel);
            }

            AddItemsToStaging(stagedItems);

            // When
            AddOrUpdateItemLocaleCommand command = new AddOrUpdateItemLocaleCommand { Region = this.region, Timestamp = nonmatchingTimestamp };
            this.commandHandler.Execute(command);

            // Then
            var actual = this.db.Connection
                .Query<ItemAttributes_Locale>(String.Format("SELECT * FROM ItemAttributes_Locale_{0} WHERE ItemID IN @ItemIDs", this.region),
                    new { ItemIDs = existingItems.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            for (int i = 0; i < stagedItems.Count; i++)
            {
                AssertPropertiesMatch(this.bizUnitID, expected[i], actual[i]);
                Assert.IsTrue(actual[i].AddedDate < DateTime.Now, "The AddedDate was not greater than the timestamp.");
            }
        }

        [TestMethod]
        public void AddOrUpdateItemLocaleCommand_MixedDataInStaging_ModifiesExpectedRowsInRegionalItemLocaleTable()
        {
            // Given
            DateTime now = DateTime.Now;
            DateTime lastWeek = now.Subtract(new TimeSpan(7, 0, 0, 0));
            Guid transactionId = Guid.NewGuid();
            var preexistingItemLocales = new List<ItemAttributes_Locale>();
            var stagedItemLocales = new List<StagingItemLocaleModel>();
            const string oldLocality = "Paris";
            const string newLocality = "Austin";
            bool oldScaleItem = false;
            bool newScaleItem = true;


            //re-query for items added during initialize so we have current itemIDs
            List<Item> existingItems = this.db.Connection
                .Query<Item>(@"SELECT * FROM Items WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = this.items.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            // make one item already set up for the locale (should get updated)
            preexistingItemLocales.Add(new TestItemAttributeLocaleBuilder()
                .WithRegion(this.region)
                .WithBusinessUnit(this.bizUnitID)
                .WithItemId(existingItems[1].ItemID)
                .WithAddedDate(lastWeek)
                .WithLocality(oldLocality)
                .WithScaleItem(oldScaleItem)
                .Build());

            // add three items to the staged data: 2 to be inserted, 1 to be updated
            stagedItemLocales.Add(new TestStagingItemLocaleModelBuilder()
                .WithRegion(this.region)
                .WithBusinessUnit(this.bizUnitID)
                .WithScanCode(existingItems[0].ScanCode)
                .WithTimestamp(now)
                .WithTransactionId(transactionId)
                .WithLocality(newLocality)
                .Build());
            stagedItemLocales.Add(new TestStagingItemLocaleModelBuilder()
                .WithRegion(this.region)
                .WithBusinessUnit(this.bizUnitID)
                .WithScanCode(existingItems[1].ScanCode)
                .WithTimestamp(now)
                .WithTransactionId(transactionId)
                .WithLocality(newLocality)
                .WithScaleItem(newScaleItem)
                .Build());
            stagedItemLocales.Add(new TestStagingItemLocaleModelBuilder()
                .WithRegion(this.region)
                .WithBusinessUnit(this.bizUnitID)
                .WithScanCode(existingItems[2].ScanCode)
                .WithTimestamp(now)
                .WithTransactionId(transactionId)
                .WithLocality(newLocality)
                .Build());

            //write initial test data to db
            AddItemLocalesToDatabase(preexistingItemLocales, this.region);
            AddItemsToStaging(stagedItemLocales);

            // When
            AddOrUpdateItemLocaleCommand command = new AddOrUpdateItemLocaleCommand { Region = region, Timestamp = now, TransactionId = transactionId };
            this.commandHandler.Execute(command);

            // Then
            var actualItemLocales = this.db.Connection
                .Query<ItemAttributes_Locale>("SELECT * FROM ItemAttributes_Locale_SW WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = existingItems.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .OrderBy(a=>a.ItemID)
                .ToList();

            for (int i = 0; i < stagedItemLocales.Count; i++)
            {
                AssertPropertiesMatchStaged(this.bizUnitID, stagedItemLocales[i], actualItemLocales[i] );
                if (i % 2 == 1)
                {
                    //should have been updated
                    Assert.AreEqual(new SqlDateTime(lastWeek).Value, actualItemLocales[i].AddedDate, "The AddedDate should not have changed.");
                    Assert.IsNotNull(actualItemLocales[i].ModifiedDate, "The ModifiedDate should not have been NULL.");
                    Assert.AreEqual(new SqlDateTime(now).Value, actualItemLocales[i].ModifiedDate, "The ModifiedDate should match the Timestamp.");
                }
                else
                {
                    //should have been inserted
                    Assert.AreEqual(new SqlDateTime(now).Value, actualItemLocales[i].AddedDate, "The AddedDate should match the Timestamp.");
                    Assert.IsNull(actualItemLocales[i].ModifiedDate, "The ModifiedDate should have been NULL.");
                }
            }
        }

        [TestMethod]
        public void AddOrUpdateItemLocaleCommand_NewItemLocaleWithNullDefaultScanCodeInStaging_AddsRowsInRegionalItemLocaleTable()
        {
            // Given
            List<Item> existingItems = this.db.Connection
                .Query<Item>(@"SELECT * FROM Items WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = this.items.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            DateTime now = DateTime.Now;
            Guid transactionId = Guid.NewGuid();
            var expectedItems = new List<StagingItemLocaleModel>();
            for (int i = 0; i < existingItems.Count; i++)
            {
                expectedItems.Add(new TestStagingItemLocaleModelBuilder()
                    .WithRegion(this.region)
                    .WithBusinessUnit(this.bizUnitID)
                    .WithScanCode(existingItems[i].ScanCode)
                    .WithTimestamp(now)
                    .WithTransactionId(transactionId)
                    .WithScaleItem(true)
                    .WithDefaultScanCode(null)
                    .Build());
            }

            AddItemsToStaging(expectedItems);

            // When
            AddOrUpdateItemLocaleCommand command = new AddOrUpdateItemLocaleCommand { Region = region, Timestamp = now, TransactionId = transactionId };
            this.commandHandler.Execute(command);

            // Then
            var actual = this.db.Connection
                .Query<ItemAttributes_Locale>("SELECT * FROM ItemAttributes_Locale_SW WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = existingItems.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            for (int i = 0; i < expectedItems.Count; i++)
            {
                AssertPropertiesMatchStaged(this.bizUnitID, expectedItems[i], actual[i]);
            }
        }

        [TestMethod]
        public void AddOrUpdateItemLocaleCommand_NewItemLocaleWithNullDiscontinuedInStaging_AddsRowsInRegionalItemLocaleTable()
        {
            // Given
            List<Item> existingItems = this.db.Connection
                .Query<Item>(@"SELECT * FROM Items WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = this.items.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            DateTime now = DateTime.Now;
            Guid transactionId = Guid.NewGuid();
            var expectedItems = new List<StagingItemLocaleModel>();
            for (int i = 0; i < existingItems.Count; i++)
            {
                expectedItems.Add(new TestStagingItemLocaleModelBuilder()
                    .WithRegion(this.region)
                    .WithBusinessUnit(this.bizUnitID)
                    .WithScanCode(existingItems[i].ScanCode)
                    .WithTimestamp(now)
                    .WithTransactionId(transactionId)
                    .WithScaleItem(true)
                    .WithDiscontinued(null)
                    .Build());
            }

            AddItemsToStaging(expectedItems);

            // When
            AddOrUpdateItemLocaleCommand command = new AddOrUpdateItemLocaleCommand { Region = region, Timestamp = now, TransactionId = transactionId };
            this.commandHandler.Execute(command);

            // Then
            var actual = this.db.Connection
                .Query<ItemAttributes_Locale>("SELECT * FROM ItemAttributes_Locale_SW WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = existingItems.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            for (int i = 0; i < expectedItems.Count; i++)
            {
                AssertPropertiesMatchStaged(this.bizUnitID, expectedItems[i], actual[i]);
            }
        }

        [TestMethod]
        public void AddOrUpdateItemLocaleCommand_NewItemLocaleWithNullLocalItemInStaging_AddsRowsInRegionalItemLocaleTable()
        {
            // Given
            List<Item> existingItems = this.db.Connection
                .Query<Item>(@"SELECT * FROM Items WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = this.items.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            DateTime now = DateTime.Now;
            Guid transactionId = Guid.NewGuid();
            var expectedItems = new List<StagingItemLocaleModel>();
            for (int i = 0; i < existingItems.Count; i++)
            {
                expectedItems.Add(new TestStagingItemLocaleModelBuilder()
                    .WithRegion(this.region)
                    .WithBusinessUnit(this.bizUnitID)
                    .WithScanCode(existingItems[i].ScanCode)
                    .WithTimestamp(now)
                    .WithTransactionId(transactionId)
                    .WithScaleItem(true)
                    .WithLocalItem(null)
                    .Build());
            }

            AddItemsToStaging(expectedItems);

            // When
            AddOrUpdateItemLocaleCommand command = new AddOrUpdateItemLocaleCommand { Region = region, Timestamp = now, TransactionId = transactionId };
            this.commandHandler.Execute(command);

            // Then
            var actual = this.db.Connection
                .Query<ItemAttributes_Locale>("SELECT * FROM ItemAttributes_Locale_SW WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = existingItems.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            for (int i = 0; i < expectedItems.Count; i++)
            {
                AssertPropertiesMatchStaged(this.bizUnitID, expectedItems[i], actual[i]);
            }
        }

        [TestMethod]
        public void AddOrUpdateItemLocaleCommand_NewItemLocaleWithNullOrderedByInforInStaging_AddsRowsInRegionalItemLocaleTable()
        {
            // Given
            List<Item> existingItems = this.db.Connection
                .Query<Item>(@"SELECT * FROM Items WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = this.items.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            DateTime now = DateTime.Now;
            Guid transactionId = Guid.NewGuid();
            var expectedItems = new List<StagingItemLocaleModel>();
            for (int i = 0; i < existingItems.Count; i++)
            {
                expectedItems.Add(new TestStagingItemLocaleModelBuilder()
                    .WithRegion(this.region)
                    .WithBusinessUnit(this.bizUnitID)
                    .WithScanCode(existingItems[i].ScanCode)
                    .WithTimestamp(now)
                    .WithTransactionId(transactionId)
                    .WithScaleItem(true)
                    .WithOrderedByInfor(null)
                    .Build());
            }

            AddItemsToStaging(expectedItems);

            // When
            AddOrUpdateItemLocaleCommand command = new AddOrUpdateItemLocaleCommand { Region = region, Timestamp = now, TransactionId = transactionId };
            this.commandHandler.Execute(command);

            // Then
            var actual = this.db.Connection
                .Query<ItemAttributes_Locale>("SELECT * FROM ItemAttributes_Locale_SW WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = existingItems.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            for (int i = 0; i < expectedItems.Count; i++)
            {
                AssertPropertiesMatchStaged(this.bizUnitID, expectedItems[i], actual[i]);
            }
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

        private void AddItemsToStaging(List<StagingItemLocaleModel> itemLocales)
        {
            string sql = @"INSERT INTO stage.ItemLocale
                            (
	                            Region,
	                            BusinessUnitID,
	                            ScanCode,
	                            Discount_Case,
	                            Discount_TM,
	                            Restriction_Age,
	                            Restriction_Hours,
	                            Authorized,
	                            Discontinued,
	                            LabelTypeDesc,
	                            LocalItem,
                                ScaleItem,
	                            Product_Code,
	                            RetailUnit,
	                            Sign_Desc,
	                            Locality,
	                            Sign_RomanceText_Long,
	                            Sign_RomanceText_Short,
                                Msrp,
                                OrderedByInfor,
                                AltRetailSize,
                                AltRetailUOM,
                                DefaultScanCode,
	                            Timestamp,
                                TransactionId
                            )
                            VALUES
                            (
	                            @Region,
	                            @BusinessUnitID,
	                            @ScanCode,
	                            @Discount_Case,
	                            @Discount_TM,
	                            @Restriction_Age,
	                            @Restriction_Hours,
	                            @Authorized,
	                            @Discontinued,
	                            @LabelTypeDesc,
	                            @LocalItem,
                                @ScaleItem,
	                            @Product_Code,
	                            @RetailUnit,
	                            @Sign_Desc,
	                            @Locality,
	                            @Sign_RomanceText_Long,
	                            @Sign_RomanceText_Short,
                                @Msrp,
                                @OrderedByInfor,
                                @AltRetailSize,
                                @AltRetailUOM,
                                @DefaultScanCode,
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

        private void AddItemLocalesToDatabase(List<ItemAttributes_Locale> itemLocales, string region)
        {
            string sql = String.Format(@"INSERT INTO ItemAttributes_Locale_{0}
                            (
	                            Region,
                                ItemID,
	                            BusinessUnitID,
	                            Discount_Case,
	                            Discount_TM,
	                            Restriction_Age,
	                            Restriction_Hours,
	                            Authorized,
	                            Discontinued,
	                            LabelTypeDesc,
	                            LocalItem,
                                ScaleItem,
	                            Product_Code,
	                            RetailUnit,
	                            Sign_Desc,
	                            Locality,
	                            Sign_RomanceText_Long,
	                            Sign_RomanceText_Short,
                                MSRP,
                                AltRetailSize,
                                AltRetailUOM,
                                DefaultScanCode,
	                            AddedDate
                            )
                            VALUES
                            (
	                            @Region,
	                            @ItemID,
                                @BusinessUnitID,
	                            @Discount_Case,
	                            @Discount_TM,
	                            @Restriction_Age,
	                            @Restriction_Hours,
	                            @Authorized,
	                            @Discontinued,
	                            @LabelTypeDesc,
	                            @LocalItem,
                                @ScaleItem,
	                            @Product_Code,
	                            @RetailUnit,
	                            @Sign_Desc,
	                            @Locality,
	                            @Sign_RomanceText_Long,
	                            @Sign_RomanceText_Short,
                                @MSRP,
                                @AltRetailSize,
                                @AltRetailUOM,
                                @DefaultScanCode,
	                            @AddedDate
                            )", region);

            this.db.Connection.Execute(sql, itemLocales, transaction: this.db.Transaction);
        }
        
        private void AssertPropertiesMatch<T>(int expectedBusinessUnitID, T expected, T actual) 
            where T : ItemAttributes_Locale
        {
            Assert.AreEqual(expectedBusinessUnitID, actual.BusinessUnitID,
                 $"The actual BusinessUnitID did not match the expected value: {expectedBusinessUnitID}.");

            Assert.AreEqual(expected.Authorized, actual.Authorized, "Authorized value did not match expected.");
            Assert.AreEqual(expected.Discontinued, actual.Discontinued, "Discontinued value did not match expected.");
            Assert.AreEqual(expected.Discount_Case, actual.Discount_Case, "Discount_Case value did not match expected.");
            Assert.AreEqual(expected.Discount_TM, actual.Discount_TM, "Discount_TM value did not match expected.");
            Assert.AreEqual(expected.LabelTypeDesc, actual.LabelTypeDesc, "LabelTypeDesc value did not match expected.");
            Assert.AreEqual(expected.LocalItem, actual.LocalItem, "LocalItem value did not match expected.");
            Assert.AreEqual(expected.Locality, actual.Locality, "Locality value did not match expected.");
            Assert.AreEqual(expected.Product_Code, actual.Product_Code, "Product_Code value did not match expected.");
            Assert.AreEqual(expected.Region, actual.Region, "Region value did not match expected.");
            Assert.AreEqual(expected.Restriction_Age, actual.Restriction_Age, "Restriction_Age value did not match expected.");
            Assert.AreEqual(expected.Restriction_Hours, actual.Restriction_Hours, "Restriction_Hours value did not match expected.");
            Assert.AreEqual(expected.RetailUnit, actual.RetailUnit, "RetailUnit value did not match expected.");
            Assert.AreEqual(expected.ScaleItem, actual.ScaleItem, "ScaleItem value did not match expected.");
            Assert.AreEqual(expected.Sign_Desc, actual.Sign_Desc, "Sign_Desc value did not match expected.");
            Assert.AreEqual(expected.Sign_RomanceText_Long, actual.Sign_RomanceText_Long, "Sign_RomanceText_Long value did not match expected.");
            Assert.AreEqual(expected.Sign_RomanceText_Short, actual.Sign_RomanceText_Short, "Sign_RomanceText_Short value did not match expected.");
            Assert.AreEqual(expected.Msrp, actual.Msrp, "Msrp value did not match expected.");
            Assert.AreEqual(expected.AltRetailSize, actual.AltRetailSize, "AltRetailSize did not match expected");
            Assert.AreEqual(expected.AltRetailUOM, actual.AltRetailUOM, "AltRetailUOM did not match expected");
            Assert.AreEqual(expected.DefaultScanCode, actual.DefaultScanCode, "DefaultScanCode did not match expected");
            Assert.AreEqual(expected.ItemID, actual.ItemID, $"The actual ItemID did not match the expected value: {expected.ItemID}.");
        }

        private void AssertPropertiesMatchStaged<T, U>(int expectedBusinessUnitID, T expected, U actual ) 
            where T : StagingItemLocaleModel
            where U : ItemAttributes_Locale
        {
            Assert.IsTrue(actual.BusinessUnitID == expectedBusinessUnitID,
                $"The actual BusinessUnitID did not match the expected value: {expectedBusinessUnitID}.");

            Assert.AreEqual(expected.Authorized, actual.Authorized, "Authorized value did not match expected.");
            Assert.AreEqual(expected.Discontinued, actual.Discontinued, "Discontinued value did not match expected.");
            Assert.AreEqual(expected.Discount_Case, actual.Discount_Case, "Discount_Case value did not match expected.");
            Assert.AreEqual(expected.Discount_TM, actual.Discount_TM, "Discount_TM value did not match expected.");
            Assert.AreEqual(expected.LabelTypeDesc, actual.LabelTypeDesc, "LabelTypeDesc value did not match expected.");
            Assert.AreEqual(expected.LocalItem, actual.LocalItem, "LocalItem value did not match expected.");
            Assert.AreEqual(expected.Locality, actual.Locality, "Locality value did not match expected.");
            Assert.AreEqual(expected.Product_Code, actual.Product_Code, "Product_Code value did not match expected.");
            Assert.AreEqual(expected.Region, actual.Region, "Region value did not match expected.");
            Assert.AreEqual(expected.Restriction_Age, actual.Restriction_Age, "Restriction_Age value did not match expected.");
            Assert.AreEqual(expected.Restriction_Hours, actual.Restriction_Hours, "Restriction_Hours value did not match expected.");
            Assert.AreEqual(expected.RetailUnit, actual.RetailUnit, "RetailUnit value did not match expected.");
            Assert.AreEqual(expected.ScaleItem, actual.ScaleItem, "ScaleItem value did not match expected.");
            Assert.AreEqual(expected.Sign_Desc, actual.Sign_Desc, "Sign_Desc value did not match expected.");
            Assert.AreEqual(expected.Sign_RomanceText_Long, actual.Sign_RomanceText_Long, "Sign_RomanceText_Long value did not match expected.");
            Assert.AreEqual(expected.Sign_RomanceText_Short, actual.Sign_RomanceText_Short, "Sign_RomanceText_Short value did not match expected.");
            Assert.AreEqual(expected.Msrp, actual.Msrp, "Msrp value did not match expected.");
            Assert.AreEqual(expected.OrderedByInfor, actual.OrderedByInfor, "OrderedByInfor did not match expected");
            Assert.AreEqual(expected.AltRetailSize, actual.AltRetailSize, "AltRetailSize did not match expected");
            Assert.AreEqual(expected.AltRetailUOM, actual.AltRetailUOM, "AltRetailUOM did not match expected");
            Assert.AreEqual(expected.DefaultScanCode.GetValueOrDefault(false), actual.DefaultScanCode, "DefaultScanCode did not match expected");
            Assert.IsNotNull(actual.AddedDate, "The AddedDate is NULL.");
        }
    }
}
