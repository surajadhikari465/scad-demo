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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MammothWebApi.Tests.DataAccess.Commands
{
    [TestClass]
    public class DeleteItemLocaleSupplierCommandHandlerTests
    {
        private DeleteItemLocaleSupplierCommandHandler commandHandler;
        private int? maxItemId;
        private List<Item> items;
        private TransactionScope transaction;
        private SqlDbProvider db;
        private DateTime timestamp;
        private Guid transactionId;
        private string region;
        private Locales locale;
        private List<StagingItemLocaleSupplierDeleteModel> stagingItemLocaleSupplierDeletes;

        [TestInitialize]
        public void InitializeTests()
        {
            transaction = new TransactionScope();
            this.timestamp = DateTime.Now;
            this.transactionId = Guid.NewGuid();
            this.region = "FL";
            this.db = new SqlDbProvider();
            this.db.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            this.db.Connection.Open();

            this.commandHandler = new DeleteItemLocaleSupplierCommandHandler(this.db);

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
            this.db.Connection.Dispose();
            transaction.Dispose();
        }

        [TestMethod]
        public void DeleteItemLocaleSupplierCommand_StagingRecordsExist_DeleteItemLocaleSupplierRecords()
        {
            //Given
            DateTime addedDate = DateTime.Now;

            List<Item> existingItems = this.db.Connection
                .Query<Item>(@"SELECT * FROM Items WHERE ItemID IN @ItemIDs",
                    new { ItemIDs = this.items.Select(i => i.ItemID) },
                    transaction: this.db.Transaction)
                .ToList();

            var itemLocales = new List<ItemLocale_Supplier>();
            for (int i = 0; i < existingItems.Count; i++)
            {
                itemLocales.Add(new TestItemLocale_SupplierBuilder()
                    .WithRegion(this.region)
                    .WithBusinessUnit(locale.BusinessUnitID)
                    .WithItemId(existingItems[i].ItemID)
                    .WithAddedDateUtc(addedDate)
                    .Build());
            }
            AddItemLocalesToDatabase(itemLocales, region);

            DateTime now = DateTime.Now;
            Guid transactionId = Guid.NewGuid();
            stagingItemLocaleSupplierDeletes = new List<StagingItemLocaleSupplierDeleteModel>();
            for (int i = 0; i < existingItems.Count; i++)
            {
                stagingItemLocaleSupplierDeletes.Add(new StagingItemLocaleSupplierDeleteModel
                {
                    Region = region,
                    BusinessUnitID = locale.BusinessUnitID,
                    Timestamp = now,
                    TransactionId = transactionId,
                    ScanCode = existingItems[i].ScanCode
                });
            }
            AddItemsToStaging(stagingItemLocaleSupplierDeletes);
            //Assert ItemLocaleSuppliers exist before trying to delete
            var records = db.Connection.Query<dynamic>(
                string.Format("SELECT * FROM ItemLocale_Supplier_{0} WHERE ItemID IN @ItemIDs", this.region),
                new { ItemIDs = existingItems.Select(i => i.ItemID) });
            Assert.AreEqual(3, records.Count());

            //When
            this.commandHandler.Execute(new DeleteItemLocaleSupplierCommand
            {
                Region = region,
                Timestamp = timestamp,
                TransactionId = transactionId
            });

            //Then
            records = db.Connection.Query<dynamic>(
                string.Format("SELECT * FROM ItemLocale_Supplier_{0} WHERE ItemID IN @ItemIDs", this.region),
                new { ItemIDs = existingItems.Select(i => i.ItemID) });

            Assert.AreEqual(0, records.Count());
        }

        private void AddItemsToStaging(List<StagingItemLocaleSupplierDeleteModel> itemLocales)
        {
            string sql = @"INSERT INTO stage.ItemLocaleSupplierDelete
                            (
	                            Region,
	                            BusinessUnitID,
	                            ScanCode,
	                            Timestamp,
                                TransactionId
                            )
                            VALUES
                            (
	                            @Region,
	                            @BusinessUnitID,
	                            @ScanCode,
	                            @Timestamp,
                                @TransactionId
                            )";

            this.db.Connection.Execute(sql, itemLocales, transaction: this.db.Transaction);
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
    }
}
