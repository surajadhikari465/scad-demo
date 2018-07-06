using Dapper;
using FastMember;
using Mammoth.Common.DataAccess;
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
using System.Transactions;

namespace MammothWebApi.Tests.DataAccess.Commands
{
    [TestClass]
    public class CancelAllSalesCommandHandlerTests
    {
        private CancelAllSalesCommandHandler commandHandler;
        private CancelAllSalesCommand cancelAllSalesCommand;
        private IDbProvider db;
        private TransactionScope transaction;
        private int? maxItemId;
        private List<Item> items;
        private string region = "FL";
        private int insertedBusinessUnitID;
        private List<CancelAllSalesModel> cancelAllSalesModelList;

        [TestInitialize]
        public void InitializeTests()
        {
            transaction = new TransactionScope();
            string connectionString = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;
            this.db = new SqlDbProvider();
            this.db.Connection = new SqlConnection(connectionString);
            this.db.Connection.Open();
            this.commandHandler = new CancelAllSalesCommandHandler(this.db);
            cancelAllSalesModelList = new List<CancelAllSalesModel>();
            cancelAllSalesCommand = new CancelAllSalesCommand();

            this.db.Connection
                .Execute(String.Format("INSERT INTO Locales_{0} (BusinessUnitID, StoreName, StoreAbbrev, AddedDate) VALUES (1, 'TEST STORE', 'TES', GETDATE())",
                    this.region),
                    transaction: this.db.Transaction);
            insertedBusinessUnitID = 1;
            this.items = BuildItems(5);
            AddToItemsTable(this.items);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void CancelAllSalesCommand_ReceivedCancelAllSalesRequest_ShouldUpdateEndDateAndCreateQueuePriceRecord()
        {   
            // Given
            DateTime existingStartDate = new DateTime(2015, 10, 1);
            DateTime endDate = new DateTime(2079, 10, 1);
            DateTime addedDate = DateTime.Today;
            List<Prices> existingPrices = BuildExistingPrices(this.items, this.insertedBusinessUnitID, 2.99M, "TPR", addedDate, existingStartDate, endDate);
            AddPriceToPriceTable(existingPrices);
            BuildCommand(cancelAllSalesModelList);

            // When
            this.commandHandler.Execute(cancelAllSalesCommand);

            //Then
            List<int> itemIds = items.Select(i => i.ItemID).OrderBy(i => i).ToList();
            string getActualSql = @"SELECT * FROM Price_{0} WHERE ItemID IN @ItemIDs AND BusinessUnitID = @BusinessUnitID AND StartDate = @StartDate ORDER BY ItemID";
            List<Prices> actualPrices = this.db.Connection
                .Query<Prices>(String.Format(getActualSql, this.region),
                    new { ItemIDs = itemIds, BusinessUnitID = this.insertedBusinessUnitID, StartDate = existingStartDate },
                    transaction: this.db.Transaction).ToList();

            string getMessageQueuePriceInfo = @"SELECT * FROM esb.MessageQueuePrice WHERE ItemID IN @ItemIDs AND BusinessUnitID = @BusinessUnitID ORDER BY ItemID";
            List<MessageQueuePrice> messageQueuePrices = this.db.Connection
               .Query<MessageQueuePrice>(String.Format(getMessageQueuePriceInfo, this.region),
                   new { ItemIDs = itemIds, BusinessUnitID = this.insertedBusinessUnitID},
                   transaction: this.db.Transaction).ToList();

            for (int i = 0; i < cancelAllSalesModelList.Count; i++)
            {
                Assert.AreEqual(DateTime.Today, actualPrices[i].EndDate);
                Assert.AreEqual(messageQueuePrices.Where(msq=>msq.ItemId == actualPrices[i].ItemID).Count(),1);
            }
        }

        [TestMethod]
        public void CancelAllSalesCommand_PriceAddedDateIsLessThanEventCreatedDate_ShouldSetPriceEndDateToEndDate()
        {
            // Given
            DateTime existingStartDate = new DateTime(2015, 10, 1);
            DateTime endDate = new DateTime(2079, 10, 1);
            DateTime addedDate = DateTime.Now.AddDays(-1);
            List<Prices> existingPrices = BuildExistingPrices(this.items, this.insertedBusinessUnitID, 2.99M, "TPR", addedDate, existingStartDate, endDate);
            AddPriceToPriceTable(existingPrices);
            BuildCommand(cancelAllSalesModelList);

            // When
            this.commandHandler.Execute(cancelAllSalesCommand);

            //Then
            List<int> itemIds = items.Select(i => i.ItemID).OrderBy(i => i).ToList();
            string getActualSql = @"SELECT * FROM Price_{0} WHERE ItemID IN @ItemIDs AND BusinessUnitID = @BusinessUnitID AND StartDate = @StartDate ORDER BY ItemID";
            List<Prices> actualPrices = this.db.Connection
                .Query<Prices>(String.Format(getActualSql, this.region),
                    new { ItemIDs = itemIds, BusinessUnitID = this.insertedBusinessUnitID, StartDate = existingStartDate },
                    transaction: this.db.Transaction).ToList();

            string getMessageQueuePriceInfo = @"SELECT * FROM esb.MessageQueuePrice WHERE ItemID IN @ItemIDs AND BusinessUnitID = @BusinessUnitID ORDER BY ItemID";
            List<MessageQueuePrice> messageQueuePrices = this.db.Connection
               .Query<MessageQueuePrice>(String.Format(getMessageQueuePriceInfo, this.region),
                   new { ItemIDs = itemIds, BusinessUnitID = this.insertedBusinessUnitID },
                   transaction: this.db.Transaction).ToList();

            for (int i = 0; i < cancelAllSalesModelList.Count; i++)
            {
                Assert.AreEqual(DateTime.Today, actualPrices[i].EndDate);
                Assert.AreEqual(messageQueuePrices.Where(msq => msq.ItemId == actualPrices[i].ItemID).Count(), 1);
            }
        }

        [TestMethod]
        public void CancelAllSalesCommand_PriceAddedDateIsGreaterThanEventCreatedDate_ShouldNotUpdatePriceEndDate()
        {
            // Given
            DateTime existingStartDate = new DateTime(2015, 10, 1);
            DateTime endDate = new DateTime(2079, 10, 1);
            DateTime addedDate = DateTime.Now.AddDays(1);
            List<Prices> existingPrices = BuildExistingPrices(this.items, this.insertedBusinessUnitID, 2.99M, "TPR", addedDate, existingStartDate, endDate);
            AddPriceToPriceTable(existingPrices);
            BuildCommand(cancelAllSalesModelList);

            // When
            this.commandHandler.Execute(cancelAllSalesCommand);

            //Then
            List<int> itemIds = items.Select(i => i.ItemID).OrderBy(i => i).ToList();
            string getActualSql = @"SELECT * FROM Price_{0} WHERE ItemID IN @ItemIDs AND BusinessUnitID = @BusinessUnitID AND StartDate = @StartDate ORDER BY ItemID";
            List<Prices> actualPrices = this.db.Connection
                .Query<Prices>(String.Format(getActualSql, this.region),
                    new { ItemIDs = itemIds, BusinessUnitID = this.insertedBusinessUnitID, StartDate = existingStartDate },
                    transaction: this.db.Transaction).ToList();

            string getMessageQueuePriceInfo = @"SELECT * FROM esb.MessageQueuePrice WHERE ItemID IN @ItemIDs AND BusinessUnitID = @BusinessUnitID ORDER BY ItemID";
            List<MessageQueuePrice> messageQueuePrices = this.db.Connection
               .Query<MessageQueuePrice>(String.Format(getMessageQueuePriceInfo, this.region),
                   new { ItemIDs = itemIds, BusinessUnitID = this.insertedBusinessUnitID },
                   transaction: this.db.Transaction).ToList();

            for (int i = 0; i < cancelAllSalesModelList.Count; i++)
            {
                Assert.AreEqual(endDate, actualPrices[i].EndDate);
                Assert.AreEqual(0, messageQueuePrices.Where(msq => msq.ItemId == actualPrices[i].ItemID).Count());
            }
        }

        private List<Item> BuildItems(int numberOfItems)
        {
            this.maxItemId = this.db.Connection.Query<int?>("SELECT MAX(ItemID) FROM Items", transaction: this.db.Transaction).FirstOrDefault();
            var newItems = new List<Item>();

            for (int i = 1; i < numberOfItems - 1; i++)
            {
                newItems.Add(new TestItemBuilder().WithScanCode($"11111177{i.ToString()}").WithItemId((maxItemId ?? default(int)) + (i)).Build());
                cancelAllSalesModelList.Add(new CancelAllSalesModel { BusinessUnitID = insertedBusinessUnitID, ScanCode = $"11111177{i.ToString()}", EndDate = DateTime.Today, EventCreatedDate = DateTime.Now });
            }           
            return newItems;
        }

        private CancelAllSalesCommand BuildCommand(List<CancelAllSalesModel> cancelAllSalesModelList)
        {
            cancelAllSalesCommand.Region = region;
            cancelAllSalesCommand.MessageActionId = MessageActions.AddOrUpdate;
            cancelAllSalesCommand.Timestamp = DateTime.Now;
            cancelAllSalesCommand.CancelAllSalesModelList = cancelAllSalesModelList;

            return cancelAllSalesCommand;
        }

        private void AddToItemsTable(List<Item> items)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(this.db.Connection as SqlConnection))
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

        private void AddPriceToPriceTable(List<Prices> prices)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(this.db.Connection as SqlConnection))
            {
                bulkCopy.ColumnMappings.Add("Region", "Region");
                bulkCopy.ColumnMappings.Add("ItemID", "ItemID");
                bulkCopy.ColumnMappings.Add("BusinessUnitID", "BusinessUnitID");
                bulkCopy.ColumnMappings.Add("StartDate", "StartDate");
                bulkCopy.ColumnMappings.Add("EndDate", "EndDate");
                bulkCopy.ColumnMappings.Add("Price", "Price");
                bulkCopy.ColumnMappings.Add("PriceType", "PriceType");
                bulkCopy.ColumnMappings.Add("PriceUOM", "PriceUOM");
                bulkCopy.ColumnMappings.Add("CurrencyID", "CurrencyID");
                bulkCopy.ColumnMappings.Add("Multiple", "Multiple");
                bulkCopy.ColumnMappings.Add("AddedDate", "AddedDate");
                bulkCopy.ColumnMappings.Add("ModifiedDate", "ModifiedDate");

                using (var reader = ObjectReader.Create(
                    prices))
                {
                    bulkCopy.DestinationTableName = $"[dbo].[Price_{this.region}]";
                    bulkCopy.WriteToServer(reader);
                }
            }
        }

        private List<Prices> BuildExistingPrices(IEnumerable<Item> items, int businessUnit, decimal price, string priceType, DateTime addedDate, DateTime startDate, DateTime? endDate = null)
        {
            // new prices for staging
            List<Prices> existingPrices = new List<Prices>();
            foreach (var item in items)
            {
                // Existing prices
                Prices existing = new Prices()
                {
                    Region = this.region,
                    ItemID = item.ItemID,
                    BusinessUnitID = this.insertedBusinessUnitID,
                    Price = price,
                    PriceUOM = "EA",
                    StartDate = startDate,
                    EndDate = endDate,
                    AddedDate = addedDate,
                    CurrencyID = 1,
                    Multiple = 1,
                    ModifiedDate = null,
                    PriceType = priceType
                };
                existingPrices.Add(existing);
            }

            return existingPrices;
        }

    }
}