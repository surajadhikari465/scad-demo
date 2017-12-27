using Dapper;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.Common;
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

namespace MammothWebApi.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class StagingItemLocaleCommandHandlerTests
    {
        private ServiceSettings settings;
        private IDbProvider db;
        private StagingItemLocaleCommandHandler handler;
        private DateTime now;
        private Guid transactionId;
        private TransactionScope transaction;

        [TestInitialize]
        public void InitializeTests()
        {
            transaction = new TransactionScope();

            this.now = new DateTime().AddYears(2016).AddMonths(7).AddDays(11).AddHours(3).AddMinutes(3).AddSeconds(3);
            this.transactionId = Guid.NewGuid();

            this.settings = new ServiceSettings();
            this.settings.ConnectionString = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;
            this.db = new SqlDbProvider();
            this.db.Connection = new SqlConnection(this.settings.ConnectionString);
            this.db.Connection.Open();

            this.handler = new StagingItemLocaleCommandHandler(this.db);
        }

        [TestCleanup]
        public void CleanupTests()
        {
            this.db.Connection.Dispose();
            transaction.Dispose();
        }

        [TestMethod]
        public void ItemLocaleStagingCommand_ValidItems_ItemLocaleRowsAddedToStagingTable()
        {
            // Given
            var command = new StagingItemLocaleCommand();
            command.ItemLocales = BuildNewStagingItemLocaleModelList(numberOfItems: 5, date: this.now);

            // When
            this.handler.Execute(command);

            // Then
            string sql = @"SELECT * FROM stage.ItemLocale il WHERE il.TransactionId = @TransactionId ORDER BY il.ScanCode";
            var actual = this.db.Connection.Query<StagingItemLocaleModel>(sql, new { TransactionId = this.transactionId }, transaction: this.db.Transaction).ToList();
            var expected = command.ItemLocales.OrderBy(i => i.ScanCode).ToList();

            if (actual.Count == 0)
            {
                Assert.Fail("No results were returned.");
            }

            for (int i = 0; i < command.ItemLocales.Count; i++)
            {
                Assert.AreEqual(expected[i].Region, actual[i].Region);
                Assert.AreEqual(expected[i].BusinessUnitID, actual[i].BusinessUnitID);
                Assert.AreEqual(expected[i].ScanCode, actual[i].ScanCode);
                Assert.AreEqual(expected[i].Discount_Case, actual[i].Discount_Case);
                Assert.AreEqual(expected[i].Discount_TM, actual[i].Discount_TM);
                Assert.AreEqual(expected[i].Restriction_Age, actual[i].Restriction_Age);
                Assert.AreEqual(expected[i].Restriction_Hours, actual[i].Restriction_Hours);
                Assert.AreEqual(expected[i].Authorized, actual[i].Authorized);
                Assert.AreEqual(expected[i].Discontinued, actual[i].Discontinued);
                Assert.AreEqual(expected[i].LabelTypeDesc, actual[i].LabelTypeDesc);
                Assert.AreEqual(expected[i].LocalItem, actual[i].LocalItem);
                Assert.AreEqual(expected[i].Product_Code, actual[i].Product_Code);
                Assert.AreEqual(expected[i].RetailUnit, actual[i].RetailUnit);
                Assert.AreEqual(expected[i].Sign_Desc, actual[i].Sign_Desc);
                Assert.AreEqual(expected[i].Sign_RomanceText_Long, actual[i].Sign_RomanceText_Long);
                Assert.AreEqual(expected[i].Sign_RomanceText_Short, actual[i].Sign_RomanceText_Short);
                Assert.AreEqual(expected[i].Msrp, actual[i].Msrp);
                Assert.AreEqual(expected[i].OrderedByInfor, actual[i].OrderedByInfor);
                Assert.AreEqual(expected[i].AltRetailSize, actual[i].AltRetailSize);
                Assert.AreEqual(expected[i].AltRetailUOM, actual[i].AltRetailUOM);
                Assert.AreEqual(expected[i].DefaultScanCode, actual[i].DefaultScanCode);
                Assert.AreEqual(expected[i].Timestamp.ToString("dd-MM-yyyy hh:mm:ss"), actual[i].Timestamp.ToString("dd-MM-yyyy hh:mm:ss"),
                    String.Format("actual: {0}, expected: {1}", actual[i].Timestamp.ToString("dd-MM-yyyy hh:mm:ss"),
                        expected[i].Timestamp.ToString("dd-MM-yyyy hh:mm:ss")));
            }
        }

        private List<StagingItemLocaleModel> BuildNewStagingItemLocaleModelList(int numberOfItems, DateTime date)
        {
            var items = new List<StagingItemLocaleModel>();
            int j;
            for (int i = 0; i < numberOfItems; i++)
            {
                j = i + 1;
                items.Add(new TestStagingItemLocaleModelBuilder()
                    .WithScanCode(String.Format("8888877{0}", i))
                    .WithTimestamp(date)
                    .WithBusinessUnit(11111)
                    .WithTransactionId(this.transactionId)
                    .Build());
            }

            return items;
        }
    }
}
