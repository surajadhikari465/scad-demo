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
    public class AddOrUpdateItemLocaleCommandHandlerTests
    {
        private AddOrUpdateItemLocaleCommandHandler commandHandler;
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
            string region = "SW";
            List<Item> existingItems = this.db.Connection
                .Query<Item>(@"SELECT * FROM Items WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = this.items.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();
            var locale = this.db.Connection
                .Query<Locales>(String.Format("SELECT * FROM dbo.Locales_{0}", region), 
                    transaction: this.db.Transaction)
                .First();

            DateTime now = DateTime.Now;
            Guid transactionId = Guid.NewGuid();
            var expectedItems = new List<StagingItemLocaleModel>();
            for (int i = 0; i < existingItems.Count; i++)
            {
                StagingItemLocaleModel model = new TestStagingItemLocaleModelBuilder()
                    .WithBusinessUnit(locale.BusinessUnitID)
                    .WithScanCode(existingItems[i].ScanCode)
                    .WithTimestamp(now)
                    .WithTransactionId(transactionId)
                    .Build();
                expectedItems.Add(model);
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
                Assert.AreEqual(expectedItems[i].Authorized, actual[i].Authorized, "Authorized value did not match expected.");
                Assert.AreEqual(expectedItems[i].Discontinued, actual[i].Discontinued, "Discontinued value did not match expected.");
                Assert.AreEqual(expectedItems[i].Discount_Case, actual[i].Discount_Case, "Discount_Case value did not match expected.");
                Assert.AreEqual(expectedItems[i].Discount_TM, actual[i].Discount_TM, "Discount_TM value did not match expected.");
                Assert.AreEqual(expectedItems[i].LabelTypeDesc, actual[i].LabelTypeDesc, "LabelTypeDesc value did not match expected.");
                Assert.AreEqual(expectedItems[i].LocalItem, actual[i].LocalItem, "LocalItem value did not match expected.");
                Assert.AreEqual(expectedItems[i].Locality, actual[i].Locality, "Locality value did not match expected.");
                Assert.AreEqual(expectedItems[i].Product_Code, actual[i].Product_Code, "Product_Code value did not match expected.");
                Assert.AreEqual(expectedItems[i].Region, actual[i].Region, "Region value did not match expected.");
                Assert.AreEqual(expectedItems[i].Restriction_Age, actual[i].Restriction_Age, "Restriction_Age value did not match expected.");
                Assert.AreEqual(expectedItems[i].Restriction_Hours, actual[i].Restriction_Hours, "Restriction_Hours value did not match expected.");
                Assert.AreEqual(expectedItems[i].RetailUnit, actual[i].RetailUnit, "RetailUnit value did not match expected.");
                Assert.AreEqual(expectedItems[i].Sign_Desc, actual[i].Sign_Desc, "Sign_Desc value did not match expected.");
                Assert.AreEqual(expectedItems[i].Sign_RomanceText_Long, actual[i].Sign_RomanceText_Long, "Sign_RomanceText_Long value did not match expected.");
                Assert.AreEqual(expectedItems[i].Sign_RomanceText_Short, actual[i].Sign_RomanceText_Short, "Sign_RomanceText_Short value did not match expected.");
                Assert.AreEqual(expectedItems[i].Msrp, actual[i].Msrp, "Msrp value did not match expected.");
                Assert.IsNotNull(actual[i].AddedDate, "The AddedDate is NULL.");
                Assert.IsTrue(actual[i].BusinessUnitID == locale.BusinessUnitID, String.Format("The actual BusinessUnitID did not match the expected value: {0}.", locale.BusinessUnitID));
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
                itemLocales.Add(new TestItemAttributeLocaleBuilder()
                    .WithBusinessUnit(this.locale.BusinessUnitID)
                    .WithItemId(existingItems[i].ItemID)
                    .WithRegion(this.region)
                    .WithAddedDate(addedDate)
                    .Build());
            }

            AddItemLocalesToDatabase(itemLocales, this.region);

            DateTime now = DateTime.Now;
            Guid transactionId = Guid.NewGuid();
            var expectedItems = new List<StagingItemLocaleModel>();
            for (int i = 0; i < existingItems.Count; i++)
            {
                StagingItemLocaleModel model = new TestStagingItemLocaleModelBuilder()
                    .WithBusinessUnit(locale.BusinessUnitID)
                    .WithScanCode(existingItems[i].ScanCode)
                    .WithAuthorized(!itemLocales[i].Authorized)
                    .WithDiscontinued(!itemLocales[i].Discontinued)
                    .WithDiscountCase(!itemLocales[i].Discount_Case)
                    .WithDiscountTm(!itemLocales[i].Discount_TM)
                    .WithRestrictionAge(21)
                    .WithRomanceLong("TESTING THIS SIGN LONG ROMANCE TEXT FULL OF WONDER")
                    .WithRomanceShort("SHORT ROMANCE")
                    .WithLabelType("LG")
                    .WithTimestamp(now)
                    .WithTransactionId(transactionId)
                    .Build();
                expectedItems.Add(model);
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
                Assert.AreEqual(expectedItems[i].Authorized, actual[i].Authorized, "Authorized value did not match expected.");
                Assert.AreEqual(expectedItems[i].Discontinued, actual[i].Discontinued, "Discontinued value did not match expected.");
                Assert.AreEqual(expectedItems[i].Discount_Case, actual[i].Discount_Case, "Discount_Case value did not match expected.");
                Assert.AreEqual(expectedItems[i].Discount_TM, actual[i].Discount_TM, "Discount_TM value did not match expected.");
                Assert.AreEqual(expectedItems[i].LabelTypeDesc, actual[i].LabelTypeDesc, "LabelTypeDesc value did not match expected.");
                Assert.AreEqual(expectedItems[i].LocalItem, actual[i].LocalItem, "LocalItem value did not match expected.");
                Assert.AreEqual(expectedItems[i].Locality, actual[i].Locality, "Locality value did not match expected.");
                Assert.AreEqual(expectedItems[i].Product_Code, actual[i].Product_Code, "Product_Code value did not match expected.");
                Assert.AreEqual(expectedItems[i].Region, actual[i].Region, "Region value did not match expected.");
                Assert.AreEqual(expectedItems[i].Restriction_Age, actual[i].Restriction_Age, "Restriction_Age value did not match expected.");
                Assert.AreEqual(expectedItems[i].Restriction_Hours, actual[i].Restriction_Hours, "Restriction_Hours value did not match expected.");
                Assert.AreEqual(expectedItems[i].RetailUnit, actual[i].RetailUnit, "RetailUnit value did not match expected.");
                Assert.AreEqual(expectedItems[i].Sign_Desc, actual[i].Sign_Desc, "Sign_Desc value did not match expected.");
                Assert.AreEqual(expectedItems[i].Sign_RomanceText_Long, actual[i].Sign_RomanceText_Long, "Sign_RomanceText_Long value did not match expected.");
                Assert.AreEqual(expectedItems[i].Sign_RomanceText_Short, actual[i].Sign_RomanceText_Short, "Sign_RomanceText_Short value did not match expected.");
                Assert.AreEqual(expectedItems[i].Msrp, actual[i].Msrp, "Msrp value did not match expected.");
                Assert.AreEqual(addedDate.ToShortTimeString(), actual[i].AddedDate.ToShortTimeString(), "The AddedDate does not equal the expected added date.");
                Assert.IsTrue(actual[i].ItemID == existingItems[i].ItemID, String.Format("The actual ItemID did not match the expected value: {0}.", existingItems[i].ItemID));
                Assert.IsTrue(actual[i].BusinessUnitID == locale.BusinessUnitID, String.Format("The actual LocaleID did not match the expected value: {0}.", locale.BusinessUnitID));
            }
        }

        [TestMethod]
        public void AddOrUpdateItemLocaleCommand_NewItemLocaleDataInStagingTimestampNotMatching_DoesNotUpdatesRowsInRegionalItemLocaleTable()
        {
            // Given
            string region = "SW";
            List<Item> existingItems = this.db.Connection
                .Query<Item>(@"SELECT * FROM Items WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = this.items.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();
            var locale = this.db.Connection
                .Query<Locales>(String.Format("SELECT * FROM dbo.Locales_{0}", region),
                    transaction: this.db.Transaction)
                .First();

            DateTime now = DateTime.Now;
            var expectedItems = new List<StagingItemLocaleModel>();
            for (int i = 0; i < existingItems.Count; i++)
            {
                StagingItemLocaleModel model = new TestStagingItemLocaleModelBuilder()
                    .WithBusinessUnit(locale.BusinessUnitID).WithScanCode(existingItems[i].ScanCode)
                    .WithTimestamp(now)
                    .Build();
                expectedItems.Add(model);
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
            List<Item> existingItems = this.db.Connection
                .Query<Item>(@"SELECT * FROM Items WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = this.items.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            var expected = new List<ItemAttributes_Locale>();
            for (int i = 0; i < existingItems.Count; i++)
            {
                expected.Add(new TestItemAttributeLocaleBuilder()
                    .WithBusinessUnit(this.locale.BusinessUnitID)
                    .WithItemId(existingItems[i].ItemID)
                    .WithRegion(this.region)
                    .WithAddedDate(DateTime.Now)
                    .Build());
            }

            AddItemLocalesToDatabase(expected, this.region);

            DateTime now = DateTime.Now;
            var stagedItems = new List<StagingItemLocaleModel>();
            for (int i = 0; i < existingItems.Count; i++)
            {
                StagingItemLocaleModel model = new TestStagingItemLocaleModelBuilder()
                    .WithBusinessUnit(locale.BusinessUnitID)
                    .WithScanCode(existingItems[i].ScanCode)
                    .WithAuthorized(!expected[i].Authorized)
                    .WithDiscontinued(!expected[i].Discontinued)
                    .WithDiscountCase(!expected[i].Discount_Case)
                    .WithDiscountTm(!expected[i].Discount_TM)
                    .WithRestrictionAge(21)
                    .WithRomanceLong("TESTING THIS SIGN LONG ROMANCE TEXT FULL OF WONDER")
                    .WithRomanceShort("SHORT ROMANCE")
                    .WithLabelType("LG")
                    .WithTimestamp(now)
                    .Build();
                stagedItems.Add(model);
            }

            AddItemsToStaging(stagedItems);

            // When
            AddOrUpdateItemLocaleCommand command = new AddOrUpdateItemLocaleCommand { Region = this.region, Timestamp = DateTime.UtcNow };
            this.commandHandler.Execute(command);

            // Then
            var actual = this.db.Connection
                .Query<ItemAttributes_Locale>(String.Format("SELECT * FROM ItemAttributes_Locale_{0} WHERE ItemID IN @ItemIDs", this.region),
                    new { ItemIDs = existingItems.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            for (int i = 0; i < stagedItems.Count; i++)
            {
                Assert.AreEqual(expected[i].Authorized, actual[i].Authorized, "Authorized value did not match expected.");
                Assert.AreEqual(expected[i].Discontinued, actual[i].Discontinued, "Discontinued value did not match expected.");
                Assert.AreEqual(expected[i].Discount_Case, actual[i].Discount_Case, "Discount_Case value did not match expected.");
                Assert.AreEqual(expected[i].Discount_TM, actual[i].Discount_TM, "Discount_TM value did not match expected.");
                Assert.AreEqual(expected[i].LabelTypeDesc, actual[i].LabelTypeDesc, "LabelTypeDesc value did not match expected.");
                Assert.AreEqual(expected[i].LocalItem, actual[i].LocalItem, "LocalItem value did not match expected.");
                Assert.AreEqual(expected[i].Locality, actual[i].Locality, "Locality value did not match expected.");
                Assert.AreEqual(expected[i].Product_Code, actual[i].Product_Code, "Product_Code value did not match expected.");
                Assert.AreEqual(expected[i].Region, actual[i].Region, "Region value did not match expected.");
                Assert.AreEqual(expected[i].Restriction_Age, actual[i].Restriction_Age, "Restriction_Age value did not match expected.");
                Assert.AreEqual(expected[i].Restriction_Hours, actual[i].Restriction_Hours, "Restriction_Hours value did not match expected.");
                Assert.AreEqual(expected[i].RetailUnit, actual[i].RetailUnit, "RetailUnit value did not match expected.");
                Assert.AreEqual(expected[i].Sign_Desc, actual[i].Sign_Desc, "Sign_Desc value did not match expected.");
                Assert.AreEqual(expected[i].Sign_RomanceText_Long, actual[i].Sign_RomanceText_Long, "Sign_RomanceText_Long value did not match expected.");
                Assert.AreEqual(expected[i].Sign_RomanceText_Short, actual[i].Sign_RomanceText_Short, "Sign_RomanceText_Short value did not match expected.");
                Assert.AreEqual(expected[i].Msrp, actual[i].Msrp, "Msrp value did not match expected.");
                Assert.IsTrue(actual[i].AddedDate < DateTime.Now, "The AddedDate was not greater than the timestamp.");
                Assert.IsTrue(actual[i].ItemID == expected[i].ItemID, String.Format("The actual ItemID did not match the expected value: {0}.", expected[i].ItemID));
                Assert.IsTrue(actual[i].BusinessUnitID == locale.BusinessUnitID, String.Format("The actual LocaleID did not match the expected value: {0}.", locale.BusinessUnitID));
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
	                            Product_Code,
	                            RetailUnit,
	                            Sign_Desc,
	                            Locality,
	                            Sign_RomanceText_Long,
	                            Sign_RomanceText_Short,
                                Msrp,
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
	                            @Product_Code,
	                            @RetailUnit,
	                            @Sign_Desc,
	                            @Locality,
	                            @Sign_RomanceText_Long,
	                            @Sign_RomanceText_Short,
                                @Msrp,
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
	                            Product_Code,
	                            RetailUnit,
	                            Sign_Desc,
	                            Locality,
	                            Sign_RomanceText_Long,
	                            Sign_RomanceText_Short,
                                MSRP,
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
	                            @Product_Code,
	                            @RetailUnit,
	                            @Sign_Desc,
	                            @Locality,
	                            @Sign_RomanceText_Long,
	                            @Sign_RomanceText_Short,
                                @MSRP,
	                            @AddedDate
                            )", region);

            this.db.Connection.Execute(sql, itemLocales, transaction: this.db.Transaction);
        }
    }
}
