using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MammothWebApi.DataAccess.Queries;
using Mammoth.Common.DataAccess.DbProviders;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using System.Linq;

namespace MammothWebApi.Tests.DataAccess.Queries
{
    [TestClass]
    public class GetAllBusinessUnitsQueryHandlerTests
    {
        private GetAllBusinessUnitsQueryHandler queryHandler;
        private GetAllBusinessUnitsQuery query;
        private SqlDbProvider db;

        [TestInitialize]
        public void Initialize()
        {
            db = new SqlDbProvider();
            db.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            db.Connection.Open();
            db.Transaction = db.Connection.BeginTransaction();

            queryHandler = new GetAllBusinessUnitsQueryHandler(db);
            query = new GetAllBusinessUnitsQuery();
        }
        [TestMethod]
        public void GetAllBusinessUnits_LocalesExist_ShouldReturnAllBusinessUnits()
        {
            //Given
            var expectedBusinessUnits = db.Connection.Query<int>(
                @"SELECT BusinessUnitID from Locales_FL
                    UNION
                    SELECT BusinessUnitID from Locales_MA
                    UNION
                    SELECT BusinessUnitID from Locales_MW
                    UNION
                    SELECT BusinessUnitID from Locales_NA
                    UNION
                    SELECT BusinessUnitID from Locales_NC
                    UNION
                    SELECT BusinessUnitID from Locales_NE
                    UNION
                    SELECT BusinessUnitID from Locales_PN
                    UNION
                    SELECT BusinessUnitID from Locales_RM
                    UNION
                    SELECT BusinessUnitID from Locales_SO
                    UNION
                    SELECT BusinessUnitID from Locales_SP
                    UNION
                    SELECT BusinessUnitID from Locales_SW
                    UNION
                    SELECT BusinessUnitID from Locales_TS
                    UNION
                    SELECT BusinessUnitID from Locales_UK",
                null,
                db.Transaction).ToList();

            //When
            var actualBusinessUnits = queryHandler.Search(query);

            //Then
            Assert.IsTrue(expectedBusinessUnits.SequenceEqual(actualBusinessUnits));
        }
    }
}
