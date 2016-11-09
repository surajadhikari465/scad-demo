using Dapper;
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

namespace MammothWebApi.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class DeleteStagingCommandHandlerTests
    {
        private DeleteStagingCommandHandler commandHandler;
        private DeleteStagingCommand commandParameters;
        private IDbProvider db;
        private Guid guid;

        [TestInitialize]
        public void InitializeTest()
        {

            string connectionString = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;
            this.db = new SqlDbProvider();
            this.db.Connection = new SqlConnection(connectionString);
            this.db.Connection.Open();
            this.db.Transaction = this.db.Connection.BeginTransaction();
            this.guid = Guid.NewGuid();

            this.commandHandler = new DeleteStagingCommandHandler(this.db);
            this.commandParameters = new DeleteStagingCommand();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            this.commandParameters = new DeleteStagingCommand();
            this.commandHandler = null;

            if (this.db.Transaction != null)
            {
                this.db.Transaction.Rollback();
                this.db.Transaction.Dispose();
            }
            this.db.Connection.Dispose();
        }
        
        [TestMethod]
        public void DeleteItemLocaleStagingCommand_ItemLocaleRowsInStagingWithCorrectTransactionId_DeletesRowsInStaging()
        {
            // Given
            DateTime utcNow = DateTime.UtcNow;
            Guid transactionId = Guid.NewGuid();

            List<StagingItemLocaleModel> expected = new List<StagingItemLocaleModel>();
            expected.Add(new TestStagingItemLocaleModelBuilder()
                .WithScanCode("123456").WithRegion("SW").WithBusinessUnit(11111).WithTimestamp(utcNow).WithTransactionId(transactionId).Build());
            expected.Add(new TestStagingItemLocaleModelBuilder()
                .WithScanCode("123456").WithRegion("FL").WithBusinessUnit(11112).WithTimestamp(utcNow).WithTransactionId(transactionId).Build());
            expected.Add(new TestStagingItemLocaleModelBuilder()
                .WithScanCode("123456").WithRegion("PN").WithBusinessUnit(11113).WithTimestamp(utcNow).WithTransactionId(transactionId).Build());

            AddRowsToItemLocaleStagingTable(expected);

            this.commandParameters.TransactionId = transactionId;
            this.commandParameters.StagingTableName = StagingTableNames.ItemLocale;

            // When
            this.commandHandler.Execute(commandParameters);

            // Then
            List<StagingItemLocaleModel> actual = this.db.Connection
                .Query<StagingItemLocaleModel>("SELECT * FROM stage.ItemLocale il WHERE il.TransactionId = @TransactionId",
                    new { TransactionId = transactionId },
                    transaction: this.db.Transaction)
                .ToList();

            Assert.IsTrue(actual.Count == 0, "The count of rows in the ItemLocale staging table were not equal to zero.");
        }

        [TestMethod]
        public void DeleteItemLocaleStagingCommand_ItemLocaleRowsInStagingIncorrectTransactionId_DoesNotDeleteRowsInStaging()
        {
            // Given
            DateTime utcNow = DateTime.UtcNow;
            Guid transactionId = Guid.NewGuid();

            List<StagingItemLocaleModel> staging = new List<StagingItemLocaleModel>();
            staging.Add(new TestStagingItemLocaleModelBuilder()
                .WithScanCode("123456").WithRegion("SW").WithBusinessUnit(11111).WithTimestamp(utcNow).WithTransactionId(transactionId).Build());
            staging.Add(new TestStagingItemLocaleModelBuilder()                                                          
                .WithScanCode("123456").WithRegion("FL").WithBusinessUnit(11112).WithTimestamp(utcNow).WithTransactionId(transactionId).Build());
            staging.Add(new TestStagingItemLocaleModelBuilder()                                                          
                .WithScanCode("123456").WithRegion("PN").WithBusinessUnit(11113).WithTimestamp(utcNow).WithTransactionId(transactionId).Build());

            AddRowsToItemLocaleStagingTable(staging);

            this.commandParameters.TransactionId = Guid.NewGuid();  // use different Guid value.
            this.commandParameters.StagingTableName = StagingTableNames.ItemLocale;

            // When
            this.commandHandler.Execute(commandParameters);

            // Then
            List<StagingItemLocaleModel> actual = this.db.Connection
                .Query<StagingItemLocaleModel>("SELECT * FROM stage.ItemLocale WHERE TransactionId = @TransactionId",
                    new { TransactionId = transactionId },
                    transaction: this.db.Transaction)
                .ToList();

            Assert.IsTrue(actual.Count == staging.Count, "The count of rows in the ItemLocale staging table was not equal to the expected value.");
        }

        [TestMethod]
        public void DeleteItemLocaleExtendedStagingCommand_ItemLocaleExtendedRowsInStagingCorrectTransactionId_DeletesRowsInStaging()
        {
            // Given
            DateTime utcNow = DateTime.UtcNow;
            Guid transactionId = Guid.NewGuid();

            List<StagingItemLocaleExtendedModel> staging = new List<StagingItemLocaleExtendedModel>();
            staging.Add(new StagingItemLocaleExtendedModel
            {
                AttributeId = Attributes.ColorAdded,
                AttributeValue = "1",
                Region = "FL",
                BusinessUnitId = 11112,
                Timestamp = utcNow,
                ScanCode = "123456",
                TransactionId = transactionId
            });

            staging.Add(new StagingItemLocaleExtendedModel
            {
                AttributeId = Attributes.ColorAdded,
                AttributeValue = "1",
                Region = "SW",
                BusinessUnitId = 11112,
                Timestamp = utcNow,
                ScanCode = "123457",
                TransactionId = transactionId
            });

            AddRowsToItemLocaleExtendedStagingTable(staging);

            this.commandParameters.TransactionId = transactionId;
            this.commandParameters.StagingTableName = StagingTableNames.ItemLocaleExtended;

            // When
            this.commandHandler.Execute(commandParameters);

            // Then
            List<StagingItemLocaleModel> actual = this.db.Connection
                .Query<StagingItemLocaleModel>("SELECT * FROM stage.ItemLocaleExtended il WHERE il.TransactionId = @TransactionId",
                    new { TransactionId = transactionId },
                    transaction: this.db.Transaction)
                .ToList();

            Assert.IsTrue(actual.Count == 0, "The count of rows in the ItemLocaleExtended staging table were not equal to zero.");
        }

        [TestMethod]
        public void DeleteItemLocaleExtendedStagingCommand_ItemLocaleExtendedRowsInStagingIncorrectTransactionId_DoesNotDeleteRowsInStaging()
        {
            // Given
            DateTime utcNow = DateTime.UtcNow;
            Guid transactionId = Guid.NewGuid();

            List<StagingItemLocaleExtendedModel> staging = new List<StagingItemLocaleExtendedModel>();
            staging.Add(new StagingItemLocaleExtendedModel
            {
                AttributeId = Attributes.ColorAdded,
                AttributeValue = "1",
                Region = "FL",
                BusinessUnitId = 11112,
                Timestamp = utcNow,
                ScanCode = "123456",
                TransactionId = transactionId
            });

            staging.Add(new StagingItemLocaleExtendedModel
            {
                AttributeId = Attributes.ColorAdded,
                AttributeValue = "1",
                Region = "FL",
                BusinessUnitId = 11112,
                Timestamp = utcNow,
                ScanCode = "123457",
                TransactionId = transactionId
            });

            AddRowsToItemLocaleExtendedStagingTable(staging);

            this.commandParameters.TransactionId = Guid.NewGuid();  // use different transactionId value.
            this.commandParameters.StagingTableName = StagingTableNames.ItemLocaleExtended;

            // When
            this.commandHandler.Execute(commandParameters);

            // Then
            List<StagingItemLocaleModel> actual = this.db.Connection
                .Query<StagingItemLocaleModel>("SELECT * FROM stage.ItemLocaleExtended WHERE TransactionId = @TransactionId",
                    new { TransactionId = transactionId },
                    transaction: this.db.Transaction)
                .ToList();

            Assert.IsTrue(actual.Count == staging.Count, "The count of rows in the ItemLocaleExtended staging table was not equal to the expected value.");
        }

        [TestMethod]
        public void DeletePriceStagingCommand_PriceRowsInStagingInWithIncorrectTransactionId_DoesNotDeleteRowsInStagingPriceTable()
        {
            // Given
            DateTime utcNow = DateTime.UtcNow;

            List<StagingPriceModel> prices = new List<StagingPriceModel>();
            prices.Add(new TestStagingPriceModelBuilder().WithScanCode("292929292929").WithTimestamp(utcNow).WithTransactionId(this.guid).Build());
            prices.Add(new TestStagingPriceModelBuilder().WithScanCode("292929292930").WithTimestamp(utcNow).WithTransactionId(this.guid).Build());
            prices.Add(new TestStagingPriceModelBuilder().WithScanCode("292929292931").WithTimestamp(utcNow).WithTransactionId(this.guid).Build());
            AddRowsToPriceStagingTable(prices);

            int expectedCount = prices.Count;

            // When
            this.commandParameters.TransactionId = Guid.NewGuid(); // input a new guid
            this.commandParameters.StagingTableName = StagingTableNames.Price;
            this.commandHandler.Execute(commandParameters);

            // Then
            List<StagingPriceModel> actual = this.db.Connection
                .Query<StagingPriceModel>("SELECT * FROM stage.Price WHERE TransactionId = @TransactionId",
                    new { TransactionId = this.guid },
                    transaction: this.db.Transaction)
                .ToList();

            Assert.AreEqual(expectedCount, actual.Count, String.Format("The count of rows in the price staging table were not equal to zero.", expectedCount));
        }

        [TestMethod]
        public void DeleteStagingCommand_PriceRowsInStagingCorrectTransactionId_DeletesRowsInStagingPriceTable()
        {
            // Given
            DateTime utcNow = DateTime.UtcNow;
            List<StagingPriceModel> prices = new List<StagingPriceModel>();
            prices.Add(new TestStagingPriceModelBuilder().WithScanCode("292929292929").WithTimestamp(utcNow).WithTransactionId(this.guid).Build());
            prices.Add(new TestStagingPriceModelBuilder().WithScanCode("292929292930").WithTimestamp(utcNow).WithTransactionId(this.guid).Build());
            prices.Add(new TestStagingPriceModelBuilder().WithScanCode("292929292931").WithTimestamp(utcNow).WithTransactionId(this.guid).Build());
            AddRowsToPriceStagingTable(prices);

            // When
            this.commandParameters.TransactionId = this.guid;
            this.commandParameters.StagingTableName = StagingTableNames.Price;
            this.commandHandler.Execute(commandParameters);

            // Then
            List<StagingPriceModel> actual = this.db.Connection
                .Query<StagingPriceModel>("SELECT * FROM stage.Price WHERE TransactionId = @TransactionId",
                    new { TransactionId = this.guid },
                    transaction: this.db.Transaction)
                .ToList();

            Assert.AreEqual(0, actual.Count, "The count of rows in the price staging table were not equal to zero.");
        }

        private void AddRowsToPriceStagingTable(List<StagingPriceModel> prices)
        {
            string sql = @" INSERT INTO stage.Price
                            (
	                            Region,
	                            BusinessUnitId,
	                            ScanCode,
	                            Multiple,
	                            Price,
	                            StartDate,
	                            EndDate,
	                            PriceType,
	                            PriceUom,
                                CurrencyCode,
	                            Timestamp,
                                TransactionId
                            )
                            VALUES
                            (
	                            @Region,
	                            @BusinessUnitId,
	                            @ScanCode,
	                            @Multiple,
	                            @Price,
	                            @StartDate,
	                            @EndDate,
	                            @PriceType,
	                            @PriceUom,
                                @CurrencyCode,
	                            @Timestamp,
                                @TransactionId
                            )";
            int affectedRows = this.db.Connection.Execute(sql, prices, transaction: this.db.Transaction);
        }

        private void AddRowsToItemLocaleStagingTable(List<StagingItemLocaleModel> itemLocales)
        {
            string sql = @" INSERT INTO stage.ItemLocale
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

            int affectedRows = this.db.Connection.Execute(sql, itemLocales, transaction: this.db.Transaction);
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
    }
}
