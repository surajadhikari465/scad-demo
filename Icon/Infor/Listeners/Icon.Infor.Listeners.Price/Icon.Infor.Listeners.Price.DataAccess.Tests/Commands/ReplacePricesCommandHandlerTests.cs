using Dapper;
using FastMember;
using Icon.Infor.Listeners.Price.DataAccess.Commands;
using Icon.Infor.Listeners.Price.DataAccess.Models;
using Mammoth.Common.DataAccess.DbProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Infor.Listeners.Price.DataAccess.Tests.Commands
{
    [TestClass]
    public class ReplacePricesCommandHandlerTests
    {
        private ReplacePricesCommandHandler commandHandler;
        private ReplacePricesCommand command;
        private SqlDbProvider dbProvider;
        private string region = "FL";
        private IEnumerable<DbPriceModel> existingPrices;
        private IEnumerable<DbPriceModel> newPrices;

        [TestInitialize]
        public void Initialize()
        {
            command = new ReplacePricesCommand();
            dbProvider = new SqlDbProvider { Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString) };
            commandHandler = new ReplacePricesCommandHandler(dbProvider);

            dbProvider.Connection.Open();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.dbProvider.Connection.Execute($"DELETE FROM gpm.Price_{this.region} WHERE GpmID IN @Prices",
                new { Prices = this.existingPrices.Select(p => p.GpmID) },
                this.dbProvider.Transaction);
        }

        [TestMethod]
        public void ReplacePricesCommand_NewPriceReplacingExistingPrice_DeletesOldPriceAndAddsNewPrice()
        {
            // Given
            DateTime beginningOfTest = DateTime.Now;
            existingPrices = CreateExistingPrices(5);
            InsertPricesIntoDatabase(this.existingPrices);

            this.newPrices = CreateNewPrices(existingPrices);
            command.Prices = this.newPrices;

            // When
            this.commandHandler.Execute(this.command);

            // Then
            foreach (var existingPrice in existingPrices)
            {
                var actualExistingPrice = dbProvider.Connection.Query<DbPriceModel>($"SELECT * FROM gpm.Price_{this.region} WHERE GpmID = @GpmID",
                    new { GpmID = existingPrice.GpmID },
                    this.dbProvider.Transaction).SingleOrDefault();
                Assert.IsNull(actualExistingPrice);
            }

            foreach (var newPrice in newPrices)
            {
                var actualNewPrice = dbProvider.Connection.Query<DbPriceModel>($"SELECT * FROM gpm.Price_{this.region} WHERE GpmID = @GpmID",
                    new { GpmID = newPrice.GpmID },
                    this.dbProvider.Transaction).SingleOrDefault();
                Assert.IsTrue(newPrice.AddedDate > beginningOfTest, "AddedDate is not correct");
                Assert.AreEqual(newPrice.BusinessUnitID, actualNewPrice.BusinessUnitID, "BusinessUnitId does not match");
                Assert.AreEqual(newPrice.CurrencyID, actualNewPrice.CurrencyID, "CurrencyID does not match");
                Assert.AreEqual(newPrice.EndDate.ToString(), actualNewPrice.EndDate.ToString(), "EndDate does not match");
                Assert.AreEqual(newPrice.GpmID, actualNewPrice.GpmID, "GpmID does not match");
                Assert.AreEqual(newPrice.ItemID, actualNewPrice.ItemID, "ItemID does not match");
                Assert.AreEqual(newPrice.Multiple, actualNewPrice.Multiple, "Multiple does not match");
                Assert.AreEqual(newPrice.NewTagExpiration?.ToString(), actualNewPrice.NewTagExpiration?.ToString(), "NewTagExpiration does not match");
                Assert.AreEqual(newPrice.Price, actualNewPrice.Price, "Price does not match");
                Assert.AreEqual(newPrice.PriceType, actualNewPrice.PriceType, "PriceType does not match");
                Assert.AreEqual(newPrice.PriceTypeAttribute, actualNewPrice.PriceTypeAttribute, "PriceTypeAttribute does not match");
                Assert.AreEqual(newPrice.PriceUOM, actualNewPrice.PriceUOM, "PriceUOM does not match");
                Assert.AreEqual(newPrice.Region, actualNewPrice.Region, "Region does not match");
                Assert.AreEqual(newPrice.StartDate.ToString(), actualNewPrice.StartDate.ToString(), "StartDate does not match");
            }
        }

        [TestMethod]
        public void ReplacePricesCommand_NewPriceReplacingExistingPrice_OldPricesAddedToDeletedPricesTable()
        {
            // Given
            DateTime beginningOfTest = DateTime.Now;
            existingPrices = CreateExistingPrices(5);
            InsertPricesIntoDatabase(this.existingPrices);

            this.newPrices = CreateNewPrices(existingPrices);
            command.Prices = this.newPrices;

            // When
            this.commandHandler.Execute(this.command);

            // Then
            foreach (var existingPrice in existingPrices)
            {
                var actualNewPrice = dbProvider.Connection.Query<DbPriceModel>($"SELECT * FROM gpm.DeletedPrices WHERE GpmID = @GpmID",
                    new { GpmID = existingPrice.GpmID },
                    this.dbProvider.Transaction).SingleOrDefault();

                //Assert.AreEqual(existingPrice.AddedDate.ToString(), actualNewPrice.AddedDate.ToString(), "AddedDate is not correct");
                Assert.AreEqual(existingPrice.BusinessUnitID, actualNewPrice.BusinessUnitID, "BusinessUnitId does not match");
                Assert.AreEqual(existingPrice.CurrencyID, actualNewPrice.CurrencyID, "CurrencyID does not match");
                Assert.AreEqual(existingPrice.EndDate.ToString(), actualNewPrice.EndDate.ToString(), "EndDate does not match");
                Assert.AreEqual(existingPrice.GpmID, actualNewPrice.GpmID, "GpmID does not match");
                Assert.AreEqual(existingPrice.ItemID, actualNewPrice.ItemID, "ItemID does not match");
                Assert.AreEqual(existingPrice.Multiple, actualNewPrice.Multiple, "Multiple does not match");
                Assert.AreEqual(existingPrice.NewTagExpiration?.ToString(), actualNewPrice.NewTagExpiration?.ToString(), "NewTagExpiration does not match");
                Assert.AreEqual(existingPrice.Price, actualNewPrice.Price, "Price does not match");
                Assert.AreEqual(existingPrice.PriceType, actualNewPrice.PriceType, "PriceType does not match");
                Assert.AreEqual(existingPrice.PriceTypeAttribute, actualNewPrice.PriceTypeAttribute, "PriceTypeAttribute does not match");
                Assert.AreEqual(existingPrice.PriceUOM, actualNewPrice.PriceUOM, "PriceUOM does not match");
                Assert.AreEqual(existingPrice.Region, actualNewPrice.Region, "Region does not match");
                Assert.AreEqual(existingPrice.StartDate.ToString(), actualNewPrice.StartDate.ToString(), "StartDate does not match");
            }
        }

        private IEnumerable<DbPriceModel> CreateExistingPrices(int numberOfPrices)
        {
            List<DbPriceModel> prices = new List<DbPriceModel>();

            for (int i = 0; i < numberOfPrices; i++)
            {
                prices.Add(new DbPriceModel
                {
                    BusinessUnitID = 12345 + i,
                    CurrencyID = 123 + i,
                    EndDate = DateTime.Now.AddDays(7),
                    GpmID = Guid.NewGuid(),
                    ItemID = 123456 + i,
                    Multiple = 1,
                    NewTagExpiration = DateTime.Now,
                    Price = 9.99m + i,
                    PriceType = "REG",
                    PriceTypeAttribute = "EDV",
                    PriceUOM = "EA",
                    Region = "FL",
                    StartDate = DateTime.Now.AddDays(1),
                    AddedDate = DateTime.Now
                });
            }

            return prices;
        }

        private IEnumerable<DbPriceModel> CreateNewPrices(IEnumerable<DbPriceModel> existingPrices)
        {
            List<DbPriceModel> prices = new List<DbPriceModel>();

            foreach (var price in existingPrices)
            {
                prices.Add(new DbPriceModel
                {
                    BusinessUnitID = price.BusinessUnitID,
                    CurrencyID = price.CurrencyID,
                    EndDate = price.EndDate.GetValueOrDefault().AddDays(3),
                    GpmID = Guid.NewGuid(), // The new GpmIDs to be added
                    ItemID = price.ItemID,
                    Multiple = price.Multiple,
                    NewTagExpiration = price.NewTagExpiration,
                    Price = price.Price + 1,
                    PriceType = price.PriceType,
                    PriceTypeAttribute = price.PriceTypeAttribute,
                    PriceUOM = price.PriceUOM,
                    Region = price.Region,
                    StartDate = price.StartDate,
                    ReplaceGpmId = price.GpmID, // These are the GpmIDs to be deleted
                    AddedDate = DateTime.Now
                });
            }

            return prices;
        }

        private void InsertPricesIntoDatabase(IEnumerable<DbPriceModel> prices)
        {
            using (var reader = ObjectReader.Create(
                prices,
                nameof(DbPriceModel.Region),
                nameof(DbPriceModel.PriceID),
                nameof(DbPriceModel.GpmID),
                nameof(DbPriceModel.ItemID),
                nameof(DbPriceModel.BusinessUnitID),
                nameof(DbPriceModel.StartDate),
                nameof(DbPriceModel.EndDate),
                nameof(DbPriceModel.Price),
                nameof(DbPriceModel.PriceType),
                nameof(DbPriceModel.PriceTypeAttribute),
                nameof(DbPriceModel.PriceUOM),
                nameof(DbPriceModel.CurrencyID),
                nameof(DbPriceModel.Multiple),
                nameof(DbPriceModel.NewTagExpiration)))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(dbProvider.Connection as SqlConnection))
                {
                    sqlBulkCopy.DestinationTableName = $"gpm.Price_{this.region}";
                    sqlBulkCopy.WriteToServer(reader);
                }
            }
        }
    }
}
