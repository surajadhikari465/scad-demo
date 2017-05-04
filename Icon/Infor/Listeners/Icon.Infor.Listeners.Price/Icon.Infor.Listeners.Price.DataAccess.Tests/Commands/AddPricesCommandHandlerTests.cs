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
    public class AddPricesCommandHandlerTests
    {
        private AddPricesCommandHandler commandHandler;
        private AddPricesCommand command;
        private SqlDbProvider dbProvider;
        private string region = "FL";
        private IEnumerable<DbPriceModel> prices;

        [TestInitialize]
        public void Initialize()
        {
            command = new AddPricesCommand();
            dbProvider = new SqlDbProvider { Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString) };
            commandHandler = new AddPricesCommandHandler(dbProvider);

            dbProvider.Connection.Open();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.dbProvider.Connection.Execute($"DELETE FROM gpm.Price_{this.region} WHERE GpmID IN @GpmIDs",
                new { GpmIDs = this.prices.Select(p => p.GpmID) },
                this.dbProvider.Transaction);
        }

        [TestMethod]
        public void AddPrices_NewPrices_AddPricesToDatabase()
        {
            //Given
            this.prices = CreatePrices(3);
            command.Prices = prices;

            //When
            commandHandler.Execute(command);

            //Then
            foreach (var price in prices)
            {
                var dbPrice = dbProvider.Connection.Query<DbPriceModel>("SELECT * FROM gpm.Price_FL WHERE GpmID = @GpmID", new { GpmID = price.GpmID }).Single();

                Assert.AreNotEqual(0, dbPrice.PriceID, "PriceID is equal to zero");
                Assert.IsNotNull(dbPrice.AddedDate, "AddedDate is incorrect");
                Assert.AreEqual(dbPrice.BusinessUnitID, price.BusinessUnitID, "BusinessUnitID does not match");
                Assert.AreEqual(dbPrice.CurrencyID, price.CurrencyID, "CurrencyID does not match");
                Assert.AreEqual(dbPrice.EndDate.ToString(), price.EndDate.ToString(), "EndDate does not match");
                Assert.AreEqual(dbPrice.GpmID, price.GpmID, "GpmID does not match");
                Assert.AreEqual(dbPrice.ItemID, price.ItemID, "ItemID does not match");
                Assert.AreEqual(dbPrice.Multiple, price.Multiple, "Multiple does not match");
                Assert.AreEqual(dbPrice.NewTagExpiration.ToString(), price.NewTagExpiration.ToString(), "NewTabeExpiration does not match");
                Assert.AreEqual(dbPrice.Price, price.Price, "Price does not match");
                Assert.AreEqual(dbPrice.PriceType, price.PriceType, "PriceType does not match");
                Assert.AreEqual(dbPrice.PriceTypeAttribute, price.PriceTypeAttribute, "PriceTypeAttribute does not match");
                Assert.AreEqual(dbPrice.PriceUOM, price.PriceUOM, "PriceUOM does not match");
                Assert.AreEqual(dbPrice.Region, price.Region, "Region does not match");
                Assert.AreEqual(dbPrice.StartDate.ToString(), price.StartDate.ToString(), "StartDate does not match");
            }
        }

        [TestMethod]
        public void AddPrices_GpmIdAlreadyExists_ThrowsPkViolationException()
        {
            // Given
            this.prices = CreatePrices(1);
            command.Prices = prices;

            InsertPricesIntoDatabase(this.prices);

            try
            {
                // When
                this.commandHandler.Execute(command);
            }
            catch (SqlException ex)
            {
                // Then
                Assert.IsTrue(ex.Message.Contains("Violation of PRIMARY KEY constraint"));                
            }
        }

        private IEnumerable<DbPriceModel> CreatePrices(int numberOfPrices)
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
                    Multiple = 1 + i,
                    NewTagExpiration = DateTime.Now,
                    Price = 9.99m + i,
                    PriceType = "REG",
                    PriceTypeAttribute = "REGREG",
                    PriceUOM = "EA",
                    Region = "FL",
                    StartDate = DateTime.Now.AddDays(1),
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
