using Dapper;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.DataAccess.Commands;
using MammothWebApi.DataAccess.Models;
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
    public class StagingItemLocaleSupplierDeleteCommandHandlerTests
    {
        private IDbProvider db;
        private StagingItemLocaleSupplierDeleteCommandHandler handler;
        private DateTime now;
        private Guid transactionId;
        private TransactionScope transaction;

        [TestInitialize]
        public void InitializeTests()
        {
            transaction = new TransactionScope();
            this.now = new DateTime().AddYears(2016).AddMonths(7).AddDays(11).AddHours(3).AddMinutes(3).AddSeconds(3);
            this.transactionId = Guid.NewGuid();

            this.db = new SqlDbProvider();
            this.db.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            this.db.Connection.Open();

            this.handler = new StagingItemLocaleSupplierDeleteCommandHandler(this.db);
        }

        [TestCleanup]
        public void CleanupTests()
        {
            this.db.Connection.Dispose();
            transaction.Dispose();
        }

        [TestMethod]
        public void StagingItemLocaleSupplierDelete_ValidItems_ItemLocaleRowsAddedToStagingTable()
        {
            // Given
            var command = new StagingItemLocaleSupplierDeleteCommand();
            command.ItemLocaleSupplierDeletes = BuildNewStagingItemLocaleSupplierDeleteModelsList(numberOfItems: 5, date: this.now);

            // When
            this.handler.Execute(command);

            // Then
            string sql = @"SELECT * FROM stage.ItemLocaleSupplierDelete il WHERE il.TransactionId = @TransactionId ORDER BY il.ScanCode";
            var actual = this.db.Connection.Query<StagingItemLocaleSupplierDeleteModel>(sql, new { TransactionId = this.transactionId }, transaction: this.db.Transaction).ToList();
            var expected = command.ItemLocaleSupplierDeletes.OrderBy(i => i.ScanCode).ToList();

            if (actual.Count == 0)
            {
                Assert.Fail("No results were returned.");
            }

            for (int i = 0; i < command.ItemLocaleSupplierDeletes.Count; i++)
            {
                Assert.AreEqual(expected[i].Region, actual[i].Region);
                Assert.AreEqual(expected[i].BusinessUnitID, actual[i].BusinessUnitID);
                Assert.AreEqual(expected[i].ScanCode, actual[i].ScanCode);
                Assert.AreEqual(expected[i].Timestamp.ToString("dd-MM-yyyy hh:mm:ss"), actual[i].Timestamp.ToString("dd-MM-yyyy hh:mm:ss"),
                    String.Format("actual: {0}, expected: {1}", actual[i].Timestamp.ToString("dd-MM-yyyy hh:mm:ss"),
                        expected[i].Timestamp.ToString("dd-MM-yyyy hh:mm:ss")));
            }
        }

        private List<StagingItemLocaleSupplierDeleteModel> BuildNewStagingItemLocaleSupplierDeleteModelsList(int numberOfItems, DateTime date)
        {
            var items = new List<StagingItemLocaleSupplierDeleteModel>();
            int j;
            for (int i = 0; i < numberOfItems; i++)
            {
                j = i + 1;
                items.Add(new StagingItemLocaleSupplierDeleteModel
                {
                    BusinessUnitID = 12345,
                    Region = "FL",
                    ScanCode = String.Format("8888877{0}", i),
                    Timestamp = date,
                    TransactionId = transactionId
                });
            }

            return items;
        }
    }
}

