using Dapper;
using Icon.Infor.Listeners.Price.DataAccess.Commands;
using Icon.Infor.Listeners.Price.DataAccess.Models;
using Mammoth.Common.DataAccess.DbProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Newtonsoft.Json;

namespace Icon.Infor.Listeners.Price.DataAccess.Tests.Commands
{
    [TestClass]
    public class ArchivePricesCommandHandlerTests
    {
        private ArchivePricesCommandHandler commandHandler;
        private ArchivePricesCommand command;
        private SqlDbProvider db;
        private string region = "FL";
        private IEnumerable<ArchivePriceModel> prices;
        private object jsonObject = new { MyTest = "test", Price = "1.99" };

        [TestInitialize]
        public void Initialize()
        {
            command = new ArchivePricesCommand();
            db = new SqlDbProvider { Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString) };
            commandHandler = new ArchivePricesCommandHandler(db);

            db.Connection.Open();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.db.Connection.Execute($"DELETE FROM gpm.MessageArchivePrice WHERE GpmID IN @GpmIDs",
                new { GpmIDs = this.prices.Select(p => p.GpmID) },
                this.db.Transaction);
        }

        [TestMethod]
        public void ArchivePriceCommand_CollectionOfPrices_ArchivesPricesInDatabase()
        {
            // Given
            this.prices = CreateArchivePrices(10);
            this.command.Prices = this.prices;

            // When
            this.commandHandler.Execute(this.command);

            // Then
            foreach (var expectedArchive in this.prices)
            {
                var actualArchive = db.Connection.Query<ArchivePriceModel>("SELECT * FROM gpm.MessageArchivePrice WHERE GpmID = @GpmID",
                    new { GpmID = expectedArchive.GpmID }).FirstOrDefault();

                Assert.AreEqual(expectedArchive.InsertDate.ToString(), actualArchive.InsertDate.ToString(), "AddedDate incorrect");
                Assert.AreEqual(expectedArchive.BusinessUnitID, actualArchive.BusinessUnitID, "BuId incorrect");
                Assert.AreEqual(expectedArchive.ErrorCode, actualArchive.ErrorCode, "ErrorCode incorrect");
                Assert.AreEqual(expectedArchive.ErrorDetails, actualArchive.ErrorDetails, "ErrorDetails incorrect");
                Assert.AreEqual(expectedArchive.EsbMessageID, actualArchive.EsbMessageID, "EsbMessageID incorrect");
                Assert.AreEqual(expectedArchive.GpmID, actualArchive.GpmID, "GpmId incorrect");
                Assert.AreEqual(expectedArchive.ItemID, actualArchive.ItemID, "ItemId incorrect");
                Assert.AreEqual(expectedArchive.JsonObject, actualArchive.JsonObject, "JsonObject incorrect");
                Assert.AreEqual(expectedArchive.MessageAction, actualArchive.MessageAction, "MessageAction incorrect");
                Assert.IsTrue(actualArchive.MessageArchivePriceID != 0, "MessageArchivePriceID is zero");
                Assert.AreEqual(expectedArchive.PriceType, actualArchive.PriceType, "PriceType incorrect");
                Assert.AreEqual(expectedArchive.Region, actualArchive.Region, "Region incorrect");
                Assert.AreEqual(expectedArchive.StartDate.ToString(), actualArchive.StartDate.ToString(), "StartDate incorrect");
            }
        }

        private IEnumerable<ArchivePriceModel> CreateArchivePrices(int number)
        {
            List<ArchivePriceModel> prices = new List<ArchivePriceModel>();

            for (int i = 0; i < number; i++)
            {
                prices.Add(new ArchivePriceModel
                {
                    BusinessUnitID = 12345 + i,
                    GpmID = Guid.NewGuid(),
                    ItemID = 123456 + i,
                    PriceType = "REG",
                    Region = "FL",
                    StartDate = DateTime.Now.AddDays(1),
                    InsertDate = DateTime.Now,
                    EsbMessageID = Guid.NewGuid(),
                    MessageAction = "Add",
                    JsonObject = JsonConvert.SerializeObject(jsonObject)
                });
            }

            return prices;
        }
    }
}
