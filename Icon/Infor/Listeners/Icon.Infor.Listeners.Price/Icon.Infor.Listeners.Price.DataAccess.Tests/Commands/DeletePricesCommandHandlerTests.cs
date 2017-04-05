using Icon.Infor.Listeners.Price.DataAccess.Commands;
using Mammoth.Common.DataAccess.DbProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Icon.Infor.Listeners.Price.DataAccess.Models;
using Dapper;

namespace Icon.Infor.Listeners.Price.DataAccess.Tests.Commands
{
    [TestClass]
    public class DeletePricesCommandHandlerTests
    {
        private DeletePricesCommandHandler commandHandler;
        private DeletePricesCommand command;
        private SqlDbProvider dbProvider;
        private string region;

        [TestInitialize]
        public void Initialize()
        {
            this.region = "FL";
            command = new DeletePricesCommand();
            dbProvider = new SqlDbProvider { Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString) };
            commandHandler = new DeletePricesCommandHandler(dbProvider);

            dbProvider.Connection.Open();
            this.dbProvider.Transaction = this.dbProvider.Connection.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.dbProvider.Transaction.Rollback();
            this.dbProvider.Transaction.Dispose();
            this.dbProvider.Connection.Close();
        }

        [TestMethod]
        public void DeletePrices_PricesExist_DeletesPricesFromDatabase()
        {
            //Given
            List<DbPriceModel> prices = CreatePrices(10);
            command.Prices = prices;

            //When
            commandHandler.Execute(command);

            //Then
            foreach (var price in prices)
            {
                var dbPrice = dbProvider.Connection.Query<DbPriceModel>($"SELECT * FROM gpm.Price_{this.region} WHERE GpmID = @GpmID",
                    new { GpmID = price.GpmID },
                    this.dbProvider.Transaction).SingleOrDefault();
                Assert.IsNull(dbPrice);
            }
        }

        [TestMethod]
        public void DeletePrices_PricesExist_AddsPricesToDeletedPricesTable()
        {
            //Given
            DateTime beforeTestDate = DateTime.Now;
            List<DbPriceModel> prices = CreatePrices(10);
            command.Prices = prices;

            //When
            commandHandler.Execute(command);

            //Then
            var actualPrices = dbProvider.Connection.Query<DbPriceModel>($"SELECT * FROM gpm.Price_{this.region} WHERE GpmID IN @GpmID",
                new { GpmID = prices.Select(p => p.GpmID) },
                this.dbProvider.Transaction);

            foreach (var price in actualPrices)
            {
                var deletedPrice = dbProvider.Connection.Query<DeletedPriceModel>("SELECT * FROM gpm.DeletedPrices WHERE GpmID = @GpmID AND Region = @Region",
                    new { GpmID = price.GpmID, Region = this.region },
                    this.dbProvider.Transaction).First();
                Assert.AreEqual(price.AddedDate.ToString(), deletedPrice.AddedDate.ToString(), "AddedDate does not match");
                Assert.AreEqual(price.BusinessUnitID, deletedPrice.BusinessUnitID, "BusinessUnitID does not match");
                Assert.AreEqual(price.CurrencyID, deletedPrice.CurrencyID, "CurrencyID does not match");
                Assert.AreEqual(price.EndDate?.ToString(), deletedPrice.EndDate?.ToString(), "EndDate does not match");
                Assert.AreEqual(price.GpmID, deletedPrice.GpmID, "GpmID does not match");
                Assert.AreEqual(price.ItemID, deletedPrice.ItemID, "ItemID does not match");
                Assert.AreEqual(price.Multiple, deletedPrice.Multiple, "Multiple does not match");
                Assert.AreEqual(price.NewTagExpiration?.ToString(), deletedPrice.NewTagExpiration?.ToString(), "NewTagExpiration does not match");
                Assert.AreEqual(price.Price, deletedPrice.Price, "Price does not match");
                Assert.AreEqual(price.PriceID, deletedPrice.PriceID, "PriceID does not match");
                Assert.AreEqual(price.PriceType, deletedPrice.PriceType, "PriceType does not match");
                Assert.AreEqual(price.PriceTypeAttribute, deletedPrice.PriceTypeAttribute, "PriceTypeAttribute does not match");
                Assert.AreEqual(price.PriceUOM, deletedPrice.PriceUOM, "PriceUOM does not match");
                Assert.AreEqual(price.Region, deletedPrice.Region, "Region does not match");
                Assert.AreEqual(price.StartDate.ToString(), deletedPrice.StartDate.ToString(), "StartDate does not match");
                Assert.IsTrue(deletedPrice.DeleteDate > beforeTestDate);
            }
        }

        private List<DbPriceModel> CreatePrices(int numberOfPrices)
        {
            List<DbPriceModel> prices = new List<DbPriceModel>();

            for (int i = 0; i < numberOfPrices; i++)
            {
                var price = new DbPriceModel
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
                    Region = this.region,
                    StartDate = DateTime.Now.AddDays(1),
                    AddedDate = DateTime.Now
                };

                dbProvider.Connection.Execute($@"
                                INSERT INTO [gpm].[Price_{this.region}]
                                        ([Region]
                                        ,[GpmID]
                                        ,[ItemID]
                                        ,[BusinessUnitID]
                                        ,[StartDate]
                                        ,[EndDate]
                                        ,[Price]
                                        ,[PriceType]
                                        ,[PriceTypeAttribute]
                                        ,[PriceUOM]
                                        ,[CurrencyID]
                                        ,[Multiple]
                                        ,[NewTagExpiration])
                                    VALUES
                                        (@Region
                                        ,@GpmID
                                        ,@ItemID
                                        ,@BusinessUnitID
                                        ,@StartDate
                                        ,@EndDate
                                        ,@Price
                                        ,@PriceType
                                        ,@PriceTypeAttribute
                                        ,@PriceUOM
                                        ,@CurrencyID
                                        ,@Multiple
                                        ,@NewTagExpiration)",
                                   price,
                                   transaction: this.dbProvider.Transaction);
                prices.Add(price);
            }

            return prices;
        }
    }
}
