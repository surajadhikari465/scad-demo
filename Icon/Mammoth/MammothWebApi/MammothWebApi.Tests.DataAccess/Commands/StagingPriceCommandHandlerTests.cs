﻿using Dapper;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.Common;
using MammothWebApi.DataAccess.Commands;
using MammothWebApi.DataAccess.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace MammothWebApi.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class StagingPriceCommandHandlerTests
    {
        private ServiceSettings settings;
        private IDbProvider db;
        private StagingPriceCommandHandler handler;
        private DateTime now;
        private Guid guid;

        [TestInitialize]
        public void InitializeTests()
        {
            this.now = DateTime.Now;
            this.guid = Guid.NewGuid();

            this.settings = new ServiceSettings();
            this.settings.ConnectionString = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;
            this.db = new SqlDbProvider();
            this.db.Connection = new SqlConnection(this.settings.ConnectionString);
            this.db.Connection.Open();

            this.handler = new StagingPriceCommandHandler(this.db);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            this.db.Connection.Execute("DELETE FROM stage.Price WHERE TransactionId = @TransactionId",
                new { TransactionId = this.guid });
            this.db.Connection.Dispose();
        }

        [TestMethod]
        public void PriceStagingCommand_RegularPricesWithNoEndDate_AddedToPriceStagingTable()
        {
            // Given
            var command = new StagingPriceCommand();
            command.Prices = BuildPriceStagingModel(numberOfItems: 3, priceType: "REG", date: this.now, guid: this.guid);

            // When
            this.handler.Execute(command);

            // Then
            string sql = @"SELECT * FROM stage.Price WHERE TransactionId = @TransactionId ORDER BY ScanCode";
            var actual = this.db.Connection.Query<StagingPriceModel>(sql, new { TransactionId = this.guid }, transaction: this.db.Transaction).ToList();
            var expected = command.Prices.OrderBy(p => p.ScanCode).ToList();

            if (actual.Count == 0)
            {
                Assert.Fail("No results were returned.");
            }

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].BusinessUnitId, actual[i].BusinessUnitId, "BusinessUnitId does not match.");
                Assert.AreEqual(expected[i].ScanCode, actual[i].ScanCode, "ScanCode does not match.");
                Assert.AreEqual(expected[i].Price, actual[i].Price, "Price does not match.");
                Assert.AreEqual(expected[i].PriceType, actual[i].PriceType, "PriceType does not match.");
                Assert.AreEqual(expected[i].StartDate.ToString(), actual[i].StartDate.ToString(), "StartDate does not match.");
                Assert.AreEqual(expected[i].EndDate?.ToString(), actual[i].EndDate?.ToString(), "EndDate does not match.");
                Assert.AreEqual(expected[i].Multiple, actual[i].Multiple, "Multiple does not match.");
                Assert.AreEqual(expected[i].TransactionId, actual[i].TransactionId, "TransactionId does not match.");
            }
        }

        [TestMethod]
        public void PriceStagingCommand_SalePricesWithEndDate_AddedToPriceStagingTable()
        {
            // Given
            var command = new StagingPriceCommand();
            command.Prices = BuildPriceStagingModel(numberOfItems: 3, priceType: "SAL", date: this.now, guid: this.guid, endDate: DateTime.MaxValue);

            // When
            this.handler.Execute(command);

            // Then
            string sql = @"SELECT * FROM stage.Price WHERE TransactionId = @TransactionId ORDER BY ScanCode";
            var actual = this.db.Connection.Query<StagingPriceModel>(sql, new { TransactionId = this.guid }, transaction: this.db.Transaction).ToList();
            var expected = command.Prices.OrderBy(p => p.ScanCode).ToList();

            if (actual.Count == 0)
            {
                Assert.Fail("No results were returned.");
            }

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].BusinessUnitId, actual[i].BusinessUnitId, "BusinessUnitId does not match.");
                Assert.AreEqual(expected[i].ScanCode, actual[i].ScanCode, "ScanCode does not match.");
                Assert.AreEqual(expected[i].Price, actual[i].Price, "Price does not match.");
                Assert.AreEqual(expected[i].PriceType, actual[i].PriceType, "PriceType does not match.");
                Assert.AreEqual(expected[i].StartDate.ToString(), actual[i].StartDate.ToString(), "StartDate does not match.");
                Assert.AreEqual(expected[i].EndDate?.ToString(), actual[i].EndDate?.ToString(), "EndDate does not match.");
                Assert.AreEqual(expected[i].Multiple, actual[i].Multiple, "Multiple does not match.");
                Assert.AreEqual(expected[i].TransactionId, actual[i].TransactionId, "TransactionId does not match.");
            }
        }

        private List<StagingPriceModel> BuildPriceStagingModel(int numberOfItems, string priceType, DateTime date, Guid guid, DateTime? endDate = null)
        {
            var prices = new List<StagingPriceModel>(numberOfItems);
            for (int i = 0; i < numberOfItems; i++)
            {
                StagingPriceModel price = new StagingPriceModel
                {
                    ScanCode = String.Format("55555555555{0}", i.ToString()),
                    BusinessUnitId = 1,
                    Price = (decimal)1.99 + i,
                    PriceType = priceType,
                    StartDate = new DateTime(1990, 1, 25),
                    EndDate = endDate,
                    Multiple = 1,
                    PriceUom = "EA",
                    CurrencyCode = "USD",
                    Region = "SW",
                    Timestamp = date,
                    TransactionId = guid
                };

                prices.Add(price);
            }

            return prices;
        }
    }
}
