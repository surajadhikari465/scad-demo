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
    public class DoesScanCodeExistQueryTests
    {
        private SqlConnection connection;
        private DoesScanCodeExistQuery query;
        private TransactionScope transaction;
        private DoesScanCodeExistParameters parameters;
        private int itemId;
        private int itemTypeId;
        private int scanCodeTypeId;
        private string testScanCode = "testScanCode";

        [TestInitialize]
        public void InitializeData()
        {
            transaction = new TransactionScope();
            connection = SqlConnectionBuilder.CreateIconConnection();
            query = new DoesScanCodeExistQuery(connection);
            parameters = new DoesScanCodeExistParameters();
            StageData();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void DoesScanCodeExistQuery_ScanCodeExists_ReturnTrue()
        {
            //Given
            parameters.ScanCode = testScanCode;

            //When
            var result = query.Search(parameters);

            //Then
            Assert.AreEqual(true, result);
        }

        private void StageData()
        {
            var itemattributes = "'{\"TestProp\":\"TestValue\"}'";

            scanCodeTypeId = connection.Query<int>($@"
                             INSERT INTO dbo.ScancodeType(scanCodeTypeDesc) 
                             VALUES ('tst')
                             SELECT SCOPE_IDENTITY()").Single();

            itemTypeId = connection.Query<int>($@"
                         INSERT INTO dbo.ItemType(itemTypeCode,itemTypeDesc)
                         VALUES ('tst','testItemType')
                         SELECT SCOPE_IDENTITY()").Single();

            itemId = connection.Query<int>($@"
                    INSERT INTO dbo.Item(itemTypeID,ItemAttributesJson) 
                    VALUES ({itemTypeId},{itemattributes})
                    SELECT SCOPE_IDENTITY()", new
            {
                itemTypeId = itemTypeId,
                itemattributes = itemattributes
            }).Single();

            int scanCodeId = connection.Query<int>($@"
                            INSERT INTO[dbo].[ScanCode] ([itemID],[scanCode],[scanCodeTypeID],[localeID]) 
                            VALUES ({itemId},'{testScanCode}',{scanCodeTypeId},1)  
                            SELECT SCOPE_IDENTITY()", new
            {
                itemTypeId = itemTypeId,
                testScanCode = testScanCode,
                scanCodeTypeId = scanCodeTypeId
            }).Single();
        }
    }
}