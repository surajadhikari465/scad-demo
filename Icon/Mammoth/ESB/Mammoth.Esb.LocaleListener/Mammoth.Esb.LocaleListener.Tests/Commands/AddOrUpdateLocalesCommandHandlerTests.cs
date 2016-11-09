using Dapper;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Mammoth.Esb.LocaleListener.Commands;
using Mammoth.Esb.LocaleListener.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
                    StoreName = "Test Store"
                }
            };

            //When
            commandHandler.Execute(command);

            //Then
            var locale = dbProvider.Connection.Query<dynamic>("select * from Locales_FL where BusinessUnitID = @BusinessUnitId", new { BusinessUnitId = 12345 }, dbProvider.Transaction).Single();

            Assert.AreEqual(command.Locales[0].BusinessUnitID, locale.BusinessUnitID);
            Assert.AreEqual(command.Locales[0].StoreAbbrev, locale.StoreAbbrev);
            Assert.AreEqual(command.Locales[0].StoreName, locale.StoreName);
            Assert.AreEqual(command.Locales[0].Region, locale.Region);
            Assert.IsNotNull(locale.AddedDate);
            Assert.IsNull(locale.ModifiedDate);
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
                    StoreName = "Test Store"
                }
            };

            string sql = "INSERT into Locales_FL (BusinessUnitID, StoreName, StoreAbbrev) " +
                                  "VALUES (@BusinessUnitID, @StoreName, @StoreAbbrev);";
            dbProvider.Connection.Execute(sql, command.Locales, dbProvider.Transaction);

            command.Locales = new List<LocaleModel>
            {
                new LocaleModel
                {
                    BusinessUnitID = 12345,
                    Region = "FL",
                    StoreAbbrev = "TSTUP",
                    StoreName = "Test Store Updated"
                }
            };

            //When
            commandHandler.Execute(command);

            //Then
            var locale = dbProvider.Connection.Query<dynamic>("select * from Locales_FL where BusinessUnitID = @BusinessUnitId", new { BusinessUnitId = 12345 }, dbProvider.Transaction).Single();

            Assert.AreEqual(command.Locales[0].BusinessUnitID, locale.BusinessUnitID);
            Assert.AreEqual(command.Locales[0].StoreAbbrev, locale.StoreAbbrev);
            Assert.AreEqual(command.Locales[0].StoreName, locale.StoreName);
            Assert.AreEqual(command.Locales[0].Region, locale.Region);
            Assert.IsNotNull(locale.AddedDate);
            Assert.IsNotNull(locale.ModifiedDate);
        }
    }
}
