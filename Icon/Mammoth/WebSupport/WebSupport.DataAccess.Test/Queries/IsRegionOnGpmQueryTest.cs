using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dapper;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;
using WebSupport.DataAccess.Queries;

namespace WebSupport.DataAccess.Test.Queries
{
    [TestClass]
    public class IsRegionOnGpmQueryTest
    {
        private IsRegionOnGpmQuery isRegionOnGpmQuery;
        private IsRegionOnGpmParameters parameters;
        private TransactionScope transaction;
        private SqlConnection connection;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MammothContext"].ConnectionString);
            isRegionOnGpmQuery = new IsRegionOnGpmQuery(connection);
            parameters = new IsRegionOnGpmParameters();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }


        [TestMethod]
        public void IsRegionOnGpmQuery_RegionOnGpm_ReturnsTrue()
        {
            //Given          
            parameters.Region = "FL";
            InsertOrUpdateRegionGpmStatus(parameters.Region, true);

            //When
            var isRegionOnGpm = isRegionOnGpmQuery.Search(parameters);

            //Then
            Assert.AreEqual(isRegionOnGpm, true);

        }

        [TestMethod]
        public void IsRegionOnGpmQuery_RegionNotOnGpm_ReturnsFalse()
        { 
            //Given
            parameters.Region = "FL";
            InsertOrUpdateRegionGpmStatus(parameters.Region, false);

            //When
            var isRegionOnGpm = isRegionOnGpmQuery.Search(parameters);

            //Then
            Assert.AreEqual(isRegionOnGpm, false);
        }

        [TestMethod]
        public void IsRegionOnGpmQuery_RegionNotOnGpmAndRecordoesNotExistInRegionGpmStatus_ReturnsFalse()
        {
            //Given
            parameters.Region = "FL";

            //When
            var isRegionOnGpm = isRegionOnGpmQuery.Search(parameters);

            //Then
            Assert.AreEqual(isRegionOnGpm, false);
        }
        private void InsertOrUpdateRegionGpmStatus(string region, bool regionGpmStatus)
        {
            connection.Execute(
                 $@" IF NOT EXISTS(SELECT 1 FROM RegionGpmStatus WHERE Region = '{region}')
                    INSERT INTO RegionGpmStatus(
                        [Region], 
                        [IsGpmEnabled])
                    VALUES
                        ('{region}',
                        '{regionGpmStatus}')
                    ELSE
                        UPDATE RegionGpmStatus
                        SET IsGpmEnabled = '{regionGpmStatus}'
                        WHERE Region = '{region}'
                "  
                );
        }
    }
}