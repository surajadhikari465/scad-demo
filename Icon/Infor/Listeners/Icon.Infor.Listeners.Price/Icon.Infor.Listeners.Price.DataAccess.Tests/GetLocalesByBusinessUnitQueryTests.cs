using Dapper;
using Icon.Infor.Listeners.Price.DataAccess.Models;
using Icon.Infor.Listeners.Price.DataAccess.Queries;
using Mammoth.Common.DataAccess.DbProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Infor.Listeners.Price.DataAccess.Tests
{
    [TestClass]
    public class GetLocalesByBusinessUnitQueryTests
    {
        private IDbProvider db;
        private string region = "FL";
        private DateTime now = DateTime.Now;
        private List<Locale> existingLocales;
        private GetLocalesByBusinessUnitsParameters getLocalesQueryParameters;
        private GetLocalesByBusinessUnitsQuery getLocalesQuery;

        [TestInitialize]
        public void InitializeTest()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;
            this.db = new SqlDbProvider();
            this.db.Connection = new SqlConnection(connectionString);
            this.db.Connection.Open();
            this.db.Transaction = this.db.Connection.BeginTransaction();

            existingLocales = new List<Locale>
            {
                new Locale { Region = this.region, BusinessUnitID = 1, StoreName = "Test Store 1", StoreAbbrev = "TS1", AddedDate = now },
                new Locale { Region = this.region, BusinessUnitID = 2, StoreName = "Test Store 2", StoreAbbrev = "TS2", AddedDate = now },
                new Locale { Region = this.region, BusinessUnitID = 3, StoreName = "Test Store 3", StoreAbbrev = "TS3", AddedDate = now },
            };

            this.db.Connection
                .Execute($@"INSERT INTO Locales_{this.region} (BusinessUnitID, StoreName, StoreAbbrev, AddedDate)
                            VALUES (@BusinessUnitID, @StoreName, @StoreAbbrev, @AddedDAte)",
                    existingLocales,
                    transaction: this.db.Transaction);

            this.getLocalesQueryParameters = new GetLocalesByBusinessUnitsParameters { BusinessUnitIDs = existingLocales.Select(el => el.BusinessUnitID) };
            this.getLocalesQuery = new GetLocalesByBusinessUnitsQuery(this.db);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            if (this.db.Transaction != null)
            {
                this.db.Transaction.Rollback();
                this.db.Transaction.Dispose();
            }
            this.db.Connection.Close();
            this.db.Connection.Dispose();
        }

        [TestMethod]
        public void GetLocalesByBusinessUnits_ListOfExistingBusinessUnits()
        {
            // When
            List<Locale> actualLocales = this.getLocalesQuery.Search(this.getLocalesQueryParameters).ToList();

            // Then
            for (int i = 0; i < actualLocales.Count; i++)
            {
                Assert.AreEqual(existingLocales[i].Region, actualLocales[i].Region, "Region did not match.");
                Assert.AreEqual(existingLocales[i].BusinessUnitID, actualLocales[i].BusinessUnitID, "BusinessUnit did not match.");
                Assert.AreEqual(existingLocales[i].StoreAbbrev, actualLocales[i].StoreAbbrev, "StoreAbbrev did not match.");
                Assert.AreEqual(existingLocales[i].StoreName, actualLocales[i].StoreName, "StoreName did not match.");
            }
        }
    }
}
