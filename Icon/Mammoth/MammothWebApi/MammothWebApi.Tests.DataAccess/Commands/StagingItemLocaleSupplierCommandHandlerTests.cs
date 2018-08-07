using Dapper;
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
using System.Transactions;

namespace MammothWebApi.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class StagingItemLocaleSupplierCommandHandlerTests
    {
        private ServiceSettings settings;
        private IDbProvider db;
        private StagingItemLocaleSupplierCommandHandler handler;
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

            this.handler = new StagingItemLocaleSupplierCommandHandler(this.db);
        }

        [TestCleanup]
        public void CleanupTests()
        {
            this.db.Connection.Dispose();
            transaction.Dispose();
        }

        [TestMethod]
        public void StagingItemLocaleSupplierCommand_ValidItems_ItemLocaleRowsAddedToStagingTable()
        {
            // Given
            var command = new StagingItemLocaleSupplierCommand();
            command.ItemLocaleSuppliers = BuildNewStagingItemLocaleSuppliersModelList(numberOfItems: 5, date: this.now);

            // When
            this.handler.Execute(command);

            // Then
            string sql = @"SELECT * FROM stage.ItemLocaleSupplier il WHERE il.TransactionId = @TransactionId ORDER BY il.ScanCode";
            var actual = this.db.Connection.Query<StagingItemLocaleSupplierModel>(sql, new { TransactionId = this.transactionId }, transaction: this.db.Transaction).ToList();
            var expected = command.ItemLocaleSuppliers.OrderBy(i => i.ScanCode).ToList();

            if (actual.Count == 0)
            {
                Assert.Fail("No results were returned.");
            }

            for (int i = 0; i < command.ItemLocaleSuppliers.Count; i++)
            {
                Assert.AreEqual(expected[i].Region, actual[i].Region);
                Assert.AreEqual(expected[i].BusinessUnitID, actual[i].BusinessUnitID);
                Assert.AreEqual(expected[i].ScanCode, actual[i].ScanCode);
                Assert.AreEqual(expected[i].IrmaVendorKey, actual[i].IrmaVendorKey);
                Assert.AreEqual(expected[i].SupplierCaseSize, actual[i].SupplierCaseSize);
                Assert.AreEqual(expected[i].SupplierItemId, actual[i].SupplierItemId);
                Assert.AreEqual(expected[i].SupplierName, actual[i].SupplierName);
                Assert.AreEqual(expected[i].Timestamp.ToString("dd-MM-yyyy hh:mm:ss"), actual[i].Timestamp.ToString("dd-MM-yyyy hh:mm:ss"),
                    String.Format("actual: {0}, expected: {1}", actual[i].Timestamp.ToString("dd-MM-yyyy hh:mm:ss"),
                        expected[i].Timestamp.ToString("dd-MM-yyyy hh:mm:ss")));
            }
        }

        [TestMethod]
        public void StagingItemLocaleSupplierCommand_ValidItemsWithNoSupplier_ItemLocaleRowsNotAddedToStagingTable()
        {
            // Given
            var command = new StagingItemLocaleSupplierCommand();
            command.ItemLocaleSuppliers = new List<StagingItemLocaleSupplierModel>();

            // When
            this.handler.Execute(command);

            // Then
            string sql = @"SELECT * FROM stage.ItemLocaleSupplier il WHERE il.TransactionId = @TransactionId ORDER BY il.ScanCode";
            var actual = this.db.Connection.Query<StagingItemLocaleSupplierModel>(sql, new { TransactionId = this.transactionId }, transaction: this.db.Transaction).ToList();
            Assert.AreEqual(0, actual.Count, "No ItemLocaleSupplier records should have been written");
        }

        private List<StagingItemLocaleSupplierModel> BuildNewStagingItemLocaleSuppliersModelList(int numberOfItems, DateTime date)
        {
            var items = new List<StagingItemLocaleSupplierModel>();
            int j;
            for (int i = 0; i < numberOfItems; i++)
            {
                j = i + 1;
                items.Add(new StagingItemLocaleSupplierModel
                { 
                    BusinessUnitID = 12345,
                    IrmaVendorKey = i + "Key",
                    Region = "FL",
                    ScanCode = String.Format("8888877{0}", i),
                    SupplierCaseSize = 1 + i,
                    SupplierItemId = i + "ItemId",
                    SupplierName = i + "SupplierName",
                    Timestamp = date,
                    TransactionId = transactionId
                });
            }

            return items;
        }
    }
}
