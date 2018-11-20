using Dapper;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using KitBuilder.Esb.LocaleListener.Commands;
using KitBuilder.Esb.LocaleListener.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace KitBuilder.Esb.LocaleListener.Tests.Commands
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
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["KitBuilder"].ConnectionString);
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
					LocaleID = 163,
					LocaleName = "New Venue",
					LocaleTypeID = 5,
                    BusinessUnitID = 12345,
                    RegionCode = "FL",
					ChainID = 1,
					RegionID = 2,
					MetroID = 77,
					StoreID = 672,
					LocaleCloseDate = null,
                    LocaleOpenDate = DateTime.Today,
					Hospitality = true,
					CurrencyCode = "USD",
					StoreAbbreviation = null 
                }
            };

            //When
            commandHandler.Execute(command);

            //Then
            var locale = dbProvider.Connection.Query<dynamic>(
				@"select * from [dbo].[Locale] where LocaleId = @LocaleID", 
                new { @LocaleID = "163" }, 
                dbProvider.Transaction)
                .Single();
            Assert.AreEqual(command.Locales[0].LocaleID, locale.LocaleId);
			Assert.AreEqual(command.Locales[0].LocaleName, locale.LocaleName);
			Assert.AreEqual(command.Locales[0].LocaleTypeID, locale.LocaleTypeId);
			Assert.AreEqual(command.Locales[0].RegionCode, locale.RegionCode);
			Assert.AreEqual(command.Locales[0].ChainID, locale.ChainId);
			Assert.AreEqual(command.Locales[0].RegionID, locale.RegionId);
			Assert.AreEqual(command.Locales[0].StoreID,locale.StoreId);
			Assert.AreEqual(command.Locales[0].MetroID,locale.MetroId);
			Assert.AreEqual(command.Locales[0].LocaleCloseDate,locale.LocaleCloseDate);
			Assert.AreEqual(command.Locales[0].LocaleOpenDate.Value, locale.LocaleOpenDate);
			Assert.AreEqual(command.Locales[0].Hospitality, locale.Hospitality);
			Assert.AreEqual(command.Locales[0].CurrencyCode,locale.CurrencyCode);
			Assert.AreEqual(command.Locales[0].StoreAbbreviation,locale.StoreAbbreviation);
            Assert.IsNull(locale.LocaleCloseDate);
        }

        [TestMethod]
        public void AddOrUpdateLocales_LocaleExists_ShouldUpdateLocale()
        {
            //Given
            command.Locales = new List<LocaleModel>
            {
                new LocaleModel
				{
					LocaleID = 163,
					LocaleName = "New Venue",
					LocaleTypeID = 5,
					BusinessUnitID = 123,
					RegionCode = "FL",
					ChainID = 1,
					RegionID = 2,
					MetroID = 77,
					StoreID = 672,
					LocaleCloseDate = null,
					LocaleOpenDate = DateTime.Today,
					Hospitality = true,
					CurrencyCode = "USD",
					StoreAbbreviation = null
				}
            };

            string sql = @"INSERT [dbo].[Locale](
                        LocaleId,
                        LocaleName,
                        LocaleTypeId,
                        StoreId,
                        MetroId,
                        RegionId,
                        ChainId,
                        LocaleOpenDate,
                        LocaleCloseDate,
                        RegionCode,
                        BusinessUnitId,
                        StoreAbbreviation,
						CurrencyCode,
						Hospitality)
                    VALUES (
                        @LocaleID,
                        @LocaleName,
                        @LocaleTypeID,
                        @StoreID,
                        @MetroID,
                        @RegionID,
                        @ChainID,
                        @LocaleOpenDate,
                        @LocaleCloseDate,
                        @RegionCode,
						@BusinessUnitID,
                        @StoreAbbreviation,
						@CurrencyCode,
						@Hospitality)";
            dbProvider.Connection.Execute(sql, command.Locales, dbProvider.Transaction);

            command.Locales = new List<LocaleModel>
            {
                new LocaleModel
                {
					LocaleID = 163,
					LocaleName = "New Venue",
					LocaleTypeID = 5,
					BusinessUnitID = 12345,
					RegionCode = "FL",
					ChainID = 1,
					RegionID = 2,
					MetroID = 77,
					StoreID = 672,
					LocaleCloseDate = null,
					LocaleOpenDate = DateTime.Today,
					Hospitality = true,
					CurrencyCode = "USD",
					StoreAbbreviation = null
				}
            };

            //When
            commandHandler.Execute(command);

			//Then
			var locale = dbProvider.Connection.Query<dynamic>(
				 @"select * from [dbo].[Locale] where LocaleId = @localeID",
				 new { @localeID = "163" },
				 dbProvider.Transaction)
				 .Single();

			Assert.AreEqual(command.Locales[0].LocaleID, locale.LocaleId);
			Assert.AreEqual(command.Locales[0].LocaleName, locale.LocaleName);
			Assert.AreEqual(command.Locales[0].LocaleTypeID, locale.LocaleTypeId);
			Assert.AreEqual(command.Locales[0].RegionCode, locale.RegionCode);
			Assert.AreEqual(command.Locales[0].ChainID, locale.ChainId);
			Assert.AreEqual(command.Locales[0].RegionID, locale.RegionId);
			Assert.AreEqual(command.Locales[0].StoreID, locale.StoreId);
			Assert.AreEqual(command.Locales[0].MetroID, locale.MetroId);
			Assert.AreEqual(command.Locales[0].LocaleCloseDate, locale.LocaleCloseDate);
			Assert.AreEqual(command.Locales[0].LocaleOpenDate.Value, locale.LocaleOpenDate);
			Assert.AreEqual(command.Locales[0].Hospitality, locale.Hospitality);
			Assert.AreEqual(command.Locales[0].CurrencyCode, locale.CurrencyCode);
			Assert.AreEqual(command.Locales[0].StoreAbbreviation, locale.StoreAbbreviation);
			Assert.IsNull(locale.LocaleCloseDate);
		}
    }
}
