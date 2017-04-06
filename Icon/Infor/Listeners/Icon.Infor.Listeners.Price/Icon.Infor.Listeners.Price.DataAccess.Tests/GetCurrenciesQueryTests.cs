using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mammoth.Common.DataAccess.DbProviders;
using System.Configuration;
using System.Data.SqlClient;
using Icon.Infor.Listeners.Price.DataAccess.Models;
using Dapper;
using System.Collections.Generic;
using Icon.Infor.Listeners.Price.DataAccess.Queries;
using System.Linq;

namespace Icon.Infor.Listeners.Price.DataAccess.Tests
{
    [TestClass]
    public class GetCurrenciesQueryTests
    {
        private IDbProvider db;
        private IEnumerable<Currency> existingCurrencies;
        private GetCurrenciesQuery getCurrenciesQuery;
        private GetCurrenciesParameters getCurrenciesParameters;

        [TestInitialize]
        public void InitializeTest()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;
            this.db = new SqlDbProvider();
            this.db.Connection = new SqlConnection(connectionString);
            this.db.Connection.Open();
            this.db.Transaction = this.db.Connection.BeginTransaction();

            // insert fake currency for test
            this.existingCurrencies = new List<Currency>
            {
                new Currency { CurrencyCode = "FCD", CurrencyDesc = "FAKE CURRENCY D" },
                new Currency { CurrencyCode = "FCX", CurrencyDesc = "FAKE CURRENCY X" }
            };
            string addCurrencySql = "INSERT INTO Currency (CurrencyCode, CurrencyDesc) VALUES (@CurrencyCode, @CurrencyDesc)";
            this.db.Connection.Execute(addCurrencySql, this.existingCurrencies, this.db.Transaction);

            this.getCurrenciesParameters = new GetCurrenciesParameters();
            this.getCurrenciesQuery = new GetCurrenciesQuery(this.db);
        }

        [TestMethod]
        public void GetCurrency_CurrenciesExists_ReturnsCurrency()
        {
            // When
            IEnumerable<Currency> actualCurrencies = this.getCurrenciesQuery.Search(this.getCurrenciesParameters);

            // Then
            Assert.IsTrue(actualCurrencies.Any(c => this.existingCurrencies.Select(ec => ec.CurrencyCode).Contains(c.CurrencyCode)),
                "The expected currencies were not returned in the query.");
        }
    }
}
