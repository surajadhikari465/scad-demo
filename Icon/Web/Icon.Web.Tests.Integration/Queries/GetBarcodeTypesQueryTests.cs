using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Data.SqlClient;
using System.Transactions;
using Dapper;
using Icon.Web.Tests.Integration.TestHelpers;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetBarcodeTypesQueryTests
    {
        private SqlConnection connection;
        private GetBarcodeTypesQuery query;
        private TransactionScope transaction;
        private GetBarcodeTypeParameters parameters;
        private int barcodeTypeId;

        [TestInitialize]
        public void InitializeData()
        {
            transaction = new TransactionScope();
            connection = SqlConnectionBuilder.CreateIconConnection();
            query = new GetBarcodeTypesQuery(connection);
            parameters = new GetBarcodeTypeParameters();
            StageData();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void GetBarcodeTypesQuery_BarcodeTypeExists_ReturnsBarcodeType()
        {
            //When
            var result = query.Search(parameters);
            //Then
            Assert.AreEqual(barcodeTypeId, result.Where(r => r.BarcodeType == "Test1").Select(s => s.BarcodeTypeId).FirstOrDefault());
        }

        [TestMethod]
        public void GetBarcodeTypesQuery_BarcodeTypesExists_ReturnsMoreThanOneBarcodeType()
        {
            StageMoreBarcodeTypeData();
            //When
            var result = query.Search(parameters);
            //Then
            Assert.IsTrue(result.Count > 1);
        }

        private void StageData()
        {
            barcodeTypeId = connection.Query<int>($@"
                            INSERT INTO[dbo].[BarcodeType]([BarCodeType],[BeginRange],[EndRange]) 
                            VALUES ('Test1',100,200)  
                            SELECT SCOPE_IDENTITY()"
                            ).Single();
        }

        private void StageMoreBarcodeTypeData()
        {
            barcodeTypeId = connection.Query<int>($@"
                            INSERT INTO[dbo].[BarcodeType] ([BarCodeType],[BeginRange],[EndRange]) 
                            VALUES ('Test2',300,400)  
                            SELECT SCOPE_IDENTITY()"
                            ).Single();
        }
    }
}