using Dapper;
using Mammoth.Common.DataAccess;
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
    public class StagingItemLocaleExtendedCommandHandlerTests
    {
        private ServiceSettings settings;
        private IDbProvider db;
        private StagingItemLocaleExtendedCommandHandler handler;
        private DateTime now;
        private Guid transactionId;
        private string region = "SW";

        [TestInitialize]
        public void InitializeTests()
        {
            this.now = DateTime.Now;
            this.transactionId = Guid.NewGuid();

            this.settings = new ServiceSettings();
            this.settings.ConnectionString = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;
            this.db = new SqlDbProvider();
            this.db.Connection = new SqlConnection(this.settings.ConnectionString);
            this.db.Connection.Open();

            this.handler = new StagingItemLocaleExtendedCommandHandler(this.db);
        }

        [TestCleanup]
        public void CleanupTests()
        {
            this.db.Connection.Execute("DELETE FROM Staging.dbo.ItemLocaleExtended WHERE Timestamp = @Timestamp",
                new { Timestamp = this.now });
            this.db.Connection.Dispose();
        }

        [TestMethod]
        public void ItemLocaleExtendedStaging_ValidItems_RowsAddedToItemLocaleExtendedStagingTable()
        {
            // Given
            var command = new StagingItemLocaleExtendedCommand();
            command.ItemLocalesExtended = BuildNewStagingItemLocaleExtendedModelList(numberOfItems: 1000, date: this.now);

            // When
            this.handler.Execute(command);

            // Then
            string sql = @"SELECT * FROM stage.ItemLocaleExtended il WHERE il.TransactionId = @TransactionId ORDER BY il.ScanCode";
            var actual = this.db.Connection
                .Query<StagingItemLocaleExtendedModel>(sql, new { TransactionId = this.transactionId }, transaction: this.db.Transaction)
                .ToList();
            var expected = command.ItemLocalesExtended.OrderBy(i => i.ScanCode).ToList();

            if (actual.Count == 0)
            {
                Assert.Fail("No results were returned.");
            }

            for (int i = 0; i < command.ItemLocalesExtended.Count; i++)
            {
                Assert.AreEqual(expected[i].Region, actual[i].Region, "Region did not match.");
                Assert.AreEqual(expected[i].BusinessUnitId, actual[i].BusinessUnitId, "BusinessUnitId did not match.");
                Assert.AreEqual(expected[i].ScanCode, actual[i].ScanCode, "ScanCode did not match.");
                Assert.AreEqual(expected[i].Timestamp.ToString(), actual[i].Timestamp.ToString(), "Timestamp did not match.");
                Assert.AreEqual(expected[i].AttributeId, actual[i].AttributeId, "AttributeId did not match.");
                Assert.AreEqual(expected[i].AttributeValue, actual[i].AttributeValue, "AttributeValue did not match.");
            }
        }

        private List<StagingItemLocaleExtendedModel> BuildNewStagingItemLocaleExtendedModelList(int numberOfItems, DateTime date)
        {
            var itemLocaleExtended = new List<StagingItemLocaleExtendedModel>();
            List<string> scanCodes = this.db.Connection.Query<string>("SELECT TOP (@Records) ScanCode FROM Items", new { Records = numberOfItems }, this.db.Transaction).ToList();
            int businessUnitId = this.db.Connection.Query<int>($"SELECT TOP 1 BusinessUnitID FROM Locales_{this.region}", transaction: this.db.Transaction).FirstOrDefault();
            bool scanCodesExist = scanCodes.Count == numberOfItems;

            for (int i = 0; i < numberOfItems; i++)
            {
                itemLocaleExtended.Add(new StagingItemLocaleExtendedModel
                {
                    AttributeId = Attributes.CountryOfProcessing,
                    AttributeValue = "USA",
                    BusinessUnitId = businessUnitId == 0 ? 11111 : businessUnitId,
                    ScanCode = scanCodesExist ? scanCodes[i] : String.Format("8888877{0}", i),
                    Region = this.region,
                    Timestamp = date,
                    TransactionId = this.transactionId
                });
            }
            return itemLocaleExtended;
        }
    }
}
