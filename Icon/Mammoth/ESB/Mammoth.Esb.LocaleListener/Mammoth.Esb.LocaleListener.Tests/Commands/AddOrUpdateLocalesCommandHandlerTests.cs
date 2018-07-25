using Dapper;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Mammoth.Esb.LocaleListener.Commands;
using Mammoth.Esb.LocaleListener.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Mammoth.Esb.LocaleListener.Tests.Commands
{
    [TestClass]
    public class AddOrUpdateLocalesCommandHandlerTests
    {
        private AddOrUpdateLocalesCommandHandler commandHandler;
        private AddOrUpdateLocalesCommand command;
        private SqlDbProvider dbProvider;

        [TestInitialize]
        public void Initialize()
        {
            dbProvider = new SqlDbProvider();
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            dbProvider.Connection.Open();
            dbProvider.Transaction = dbProvider.Connection.BeginTransaction();

            commandHandler = new AddOrUpdateLocalesCommandHandler(dbProvider);
            command = new AddOrUpdateLocalesCommand();
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbProvider.Transaction.Rollback();
            dbProvider.Transaction.Dispose();
            dbProvider.Connection.Dispose();
        }

        [TestMethod]
        public void AddOrUpdateLocales_NewLocale_ShouldAddLocale()
        {
            //Given
            command.Locales = new List<LocaleModel>
            {
                new LocaleModel
                {
                    BusinessUnitID = 12345,
                    Region = "FL",
                    StoreAbbrev = "TST",
                    StoreName = "Test Store",
                    PhoneNumber = "Test PhoneNumber",
                    Address1 = "Test Address1",
                    Address2 = "Test Address2",
                    Address3 = "Test Address3",
                    City = "Test City",
                    Country = "Test Country",
                    CountryAbbrev = "CAbb",
                    PostalCode = "Test PostalCode",
                    Territory = "Test Territory",
                    TerritoryAbbrev = "TAbb",
                    Timezone = "Test Timezone",
                    LocaleOpenDate = DateTime.Today
                }
            };

            //When
            commandHandler.Execute(command);

            //Then
            var locale = dbProvider.Connection.Query<dynamic>(
                @"select * from Locales_FL where BusinessUnitID = @BusinessUnitId", 
                new { BusinessUnitId = 12345 }, 
                dbProvider.Transaction)
                .Single();
            Assert.AreEqual(command.Locales[0].BusinessUnitID, locale.BusinessUnitID);
            Assert.AreEqual(command.Locales[0].StoreAbbrev, locale.StoreAbbrev);
            Assert.AreEqual(command.Locales[0].StoreName, locale.StoreName);
            Assert.AreEqual(command.Locales[0].Region, locale.Region);
            Assert.AreEqual(command.Locales[0].PhoneNumber, locale.PhoneNumber);
            Assert.AreEqual(command.Locales[0].LocaleOpenDate.Value, locale.LocaleOpenDate);
            Assert.IsNull(locale.LocaleCloseDate);
            Assert.IsNotNull(locale.AddedDate);
            Assert.IsNull(locale.ModifiedDate);

            var storeAddress = dbProvider.Connection.Query<dynamic>(
                @"select * from StoreAddress where BusinessUnitID = @BusinessUnitId",
                new { BusinessUnitId = 12345 },
                dbProvider.Transaction)
                .Single();
            Assert.AreEqual(command.Locales[0].BusinessUnitID, storeAddress.BusinessUnitID);
            Assert.AreEqual(command.Locales[0].Address1, storeAddress.Address1);
            Assert.AreEqual(command.Locales[0].Address2, storeAddress.Address2);
            Assert.AreEqual(command.Locales[0].Address3, storeAddress.Address3);
            Assert.AreEqual(command.Locales[0].City, storeAddress.City);
            Assert.AreEqual(command.Locales[0].Country, storeAddress.Country);
            Assert.AreEqual(command.Locales[0].CountryAbbrev, storeAddress.CountryAbbrev);
            Assert.AreEqual(command.Locales[0].PostalCode, storeAddress.PostalCode);
            Assert.AreEqual(command.Locales[0].Territory, storeAddress.Territory);
            Assert.AreEqual(command.Locales[0].TerritoryAbbrev, storeAddress.TerritoryAbbrev);
            Assert.AreEqual(command.Locales[0].Timezone, storeAddress.Timezone);
            Assert.IsNotNull(storeAddress.AddedDate);
            Assert.IsNull(storeAddress.ModifiedDate);
        }

        [TestMethod]
        public void AddOrUpdateLocales_LocaleExists_ShouldUpdateLocale()
        {
            //Given
            command.Locales = new List<LocaleModel>
            {
                new LocaleModel
                {
                    BusinessUnitID = 12345,
                    Region = "FL",
                    StoreAbbrev = "TST",
                    StoreName = "Test Store",
                    PhoneNumber = "Test PhoneNumber",
                    Address1 = "Test Address1",
                    Address2 = "Test Address2",
                    Address3 = "Test Address3",
                    City = "Test City",
                    Country = "Test Country",
                    CountryAbbrev = "CAbb",
                    PostalCode = "Test PostalCode",
                    Territory = "Test Territory",
                    TerritoryAbbrev = "TAbb",
                    Timezone = "Test Timezone"
                }
            };

            string sql = "INSERT into Locales_FL (BusinessUnitID, StoreName, StoreAbbrev, PhoneNumber) " +
                                  "VALUES (@BusinessUnitID, @StoreName, @StoreAbbrev, @PhoneNumber);";
            dbProvider.Connection.Execute(sql, command.Locales, dbProvider.Transaction);
            sql = @"INSERT dbo.StoreAddress(
                        BusinessUnitID,
                        Address1,
                        Address2,
                        Address3,
                        City,
                        Territory,
                        TerritoryAbbrev,
                        PostalCode,
                        Country,
                        CountryAbbrev,
                        Timezone,
                        AddedDate)
                    VALUES (
                        @BusinessUnitID,
                        @Address1,
                        @Address2,
                        @Address3,
                        @City,
                        @Territory,
                        @TerritoryAbbrev,
                        @PostalCode,
                        @Country,
                        @CountryAbbrev,
                        @Timezone,
                        GETDATE())";
            dbProvider.Connection.Execute(sql, command.Locales, dbProvider.Transaction);

            command.Locales = new List<LocaleModel>
            {
                new LocaleModel
                {
                    BusinessUnitID = 12345,
                    Region = "FL",
                    StoreAbbrev = "TSTUP",
                    StoreName = "Test Store Updated",
                    PhoneNumber = "Test PhoneNumber Updated",
                    Address1 = "Test Address1 Updated",
                    Address2 = "Test Address2 Updated",
                    Address3 = "Test Address3 Updated",
                    City = "Test City Updated",
                    Country = "Test Country Updated",
                    CountryAbbrev = "CAbbU",
                    PostalCode = "Test PostalCode Up",
                    Territory = "Test Territory Updated",
                    TerritoryAbbrev = "TAbbU",
                    Timezone = "Test Timezone Updated",
                    LocaleOpenDate = DateTime.Today,
                    LocaleCloseDate = DateTime.Today.AddDays(5)
                }
            };

            //When
            commandHandler.Execute(command);

            //Then
            var locale = dbProvider.Connection.Query<dynamic>(
                "select * from Locales_FL where BusinessUnitID = @BusinessUnitId", 
                new { BusinessUnitId = 12345 }, 
                dbProvider.Transaction)
                .Single();

            Assert.AreEqual(command.Locales[0].BusinessUnitID, locale.BusinessUnitID);
            Assert.AreEqual(command.Locales[0].StoreAbbrev, locale.StoreAbbrev);
            Assert.AreEqual(command.Locales[0].StoreName, locale.StoreName);
            Assert.AreEqual(command.Locales[0].Region, locale.Region);
            Assert.AreEqual(command.Locales[0].LocaleOpenDate.Value, locale.LocaleOpenDate);
            Assert.AreEqual(command.Locales[0].LocaleCloseDate.Value, locale.LocaleCloseDate);
            Assert.IsNotNull(locale.AddedDate);
            Assert.IsNotNull(locale.ModifiedDate);

            var storeAddress = dbProvider.Connection.Query<dynamic>(
                @"select * from StoreAddress where BusinessUnitID = @BusinessUnitId",
                new { BusinessUnitId = 12345 },
                dbProvider.Transaction)
                .Single();

            Assert.AreEqual(command.Locales[0].BusinessUnitID, storeAddress.BusinessUnitID);
            Assert.AreEqual(command.Locales[0].Address1, storeAddress.Address1);
            Assert.AreEqual(command.Locales[0].Address2, storeAddress.Address2);
            Assert.AreEqual(command.Locales[0].Address3, storeAddress.Address3);
            Assert.AreEqual(command.Locales[0].City, storeAddress.City);
            Assert.AreEqual(command.Locales[0].Country, storeAddress.Country);
            Assert.AreEqual(command.Locales[0].CountryAbbrev, storeAddress.CountryAbbrev);
            Assert.AreEqual(command.Locales[0].PostalCode, storeAddress.PostalCode);
            Assert.AreEqual(command.Locales[0].Territory, storeAddress.Territory);
            Assert.AreEqual(command.Locales[0].TerritoryAbbrev, storeAddress.TerritoryAbbrev);
            Assert.AreEqual(command.Locales[0].Timezone, storeAddress.Timezone);
            Assert.IsNotNull(storeAddress.AddedDate);
            Assert.IsNotNull(storeAddress.ModifiedDate);
        }
    }
}
