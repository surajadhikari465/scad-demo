using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebSupport.DataAccess.Queries;
using System.Data.SqlClient;
using System.Transactions;
using System.Configuration;
using Dapper;

namespace WebSupport.DataAccess.Test.Queries
{
    [TestClass]
    public class DoesStoreExistQueryTests
    {
        private DoesStoreExistQuery query;
        private SqlConnection connection;
        private TransactionScope transaction;
        private DoesStoreExistParameters parameters;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MammothContext"].ConnectionString);
            query = new DoesStoreExistQuery(connection);
            parameters = new DoesStoreExistParameters();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void DoesStoreExist_ScanCodeExists_ReturnTrue()
        {
            //Given
            var businessUnitId = "99999";
            connection.Execute(
                $@"INSERT INTO dbo.Locales_FL (Region, BusinessUnitID, StoreName, StoreAbbrev)
                VALUES ('FL', {businessUnitId}, 'Test', 'Test')");
            parameters.BusinessUnitId = businessUnitId;

            //When
            var result = query.Search(parameters);

            //Then
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DoesStoreExist_ScanCodeDoesNotExist_ReturnFalse()
        {
            //Given
            var businessUnitId = "99999";
            parameters.BusinessUnitId = businessUnitId;

            //When
            var result = query.Search(parameters);

            //Then
            Assert.IsFalse(result);
        }
    }
}
