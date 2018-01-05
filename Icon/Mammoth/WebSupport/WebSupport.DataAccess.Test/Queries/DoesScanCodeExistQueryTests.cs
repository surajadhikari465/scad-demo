using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;
using WebSupport.DataAccess.Queries;

namespace WebSupport.DataAccess.Test.Queries
{
    [TestClass]
    public class DoesScanCodeExistQueryTests
    {
        private DoesScanCodeExistQuery query;
        private SqlConnection connection;
        private TransactionScope transaction;
        private DoesScanCodeExistParameters parameters;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MammothContext"].ConnectionString);
            query = new DoesScanCodeExistQuery(connection);
            parameters = new DoesScanCodeExistParameters();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void DoesScanCodeExist_ScanCodeExists_ReturnTrue()
        {
            //Given
            var scanCode = "999999999999";
            connection.Execute($"insert into dbo.Items(ItemID, ScanCode) values (900000000, '{scanCode}')");
            parameters.ScanCode = scanCode;

            //When
            var result = query.Search(parameters);

            //Then
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DoesScanCodeExist_ScanCodeDoesNotExist_ReturnFalse()
        {
            //Given
            var scanCode = "999999999999";
            parameters.ScanCode = scanCode;

            //When
            var result = query.Search(parameters);

            //Then
            Assert.IsFalse(result);
        }
    }
}
