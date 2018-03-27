using Dapper;
using Mammoth.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using WebSupport.DataAccess.Models;
using WebSupport.DataAccess.Queries;
using WebSupport.DataAccess.Test.TestData;

namespace WebSupport.DataAccess.Test.Queries
{
    [TestClass]
    public class GetGpmStatusQueryTests
    {
        private MammothContext mammothDbContext;
        private SqlConnection rawSqlConnection;
        private TransactionScope transaction;
        private GetGpmStatusQuery query;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            mammothDbContext = new MammothContext();

            query = new GetGpmStatusQuery(mammothDbContext);

            rawSqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MammothContext"].ConnectionString);
            rawSqlConnection.Execute(RegionGpmStatusTestData.SqlForInsertingTestRegions);
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void GetGpmStatus_WhenQueryingSingleRegionAndGpmNotEnabledForRegion_ReturnsFalse()
        {
            //Given
            var expectedStatus = false;
            var parameters = new GetGpmStatusParameters { Region = RegionGpmStatusTestData.ExistingRegionNotOnGpm };

            //When
            var result = query.Search(parameters).FirstOrDefault();

            //Then
            Assert.AreEqual(expectedStatus, result.IsGpmEnabled);
            Assert.AreEqual(parameters.Region, result.Region);
        }

        [TestMethod]
        public void GetGpmStatus_WhenQueryingSingleRegionAndGpmIsEnabledForRegion_ReturnsTrue()
        {
            //Given
            var expectedStatus = true;
            var parameters = new GetGpmStatusParameters { Region = RegionGpmStatusTestData.ExistingRegionOnGpm };

            //When
            var result = query.Search(parameters).FirstOrDefault();

            //Then
            Assert.AreEqual(expectedStatus, result.IsGpmEnabled);
            Assert.AreEqual(parameters.Region, result.Region);
        }

        [TestMethod]
        public void GetGpmStatus_WhenQueryingSingleRegionAndRegionDoesNotExist_ReturnsDefaultObject()
        {
            //Given
            var parameters = new GetGpmStatusParameters { Region = RegionGpmStatusTestData.NonExistentRegion };

            //When
            var result = query.Search(parameters).FirstOrDefault();

            //Then
            Assert.AreEqual(default(RegionGpmStatus), result);
        }

        [TestMethod]
        public void GetGpmStatus_WhenQueryingWithNoRegionParameter_ReturnsAllRegions()
        {
            //Given
            var parameters = new GetGpmStatusParameters();
            var expectedCount = rawSqlConnection
                .Query<int>(RegionGpmStatusTestData.SqlForCurrentRegionCount)
                .Single();

            //When
            var result = query.Search(parameters);

            //Then
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
        }

        [TestMethod]
        public void GetGpmStatus_WhenQueryingWithBlankRegionParameter_ReturnsAllRegions()
        {
            //Given
            var parameters = new GetGpmStatusParameters { Region = " "};
            var expectedCount = rawSqlConnection
                .Query<int>(RegionGpmStatusTestData.SqlForCurrentRegionCount)
                .Single();

            //When
            var result = query.Search(parameters);

            //Then
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
        }
    }
}