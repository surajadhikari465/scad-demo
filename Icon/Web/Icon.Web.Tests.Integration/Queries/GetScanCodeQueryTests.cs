using Icon.Web.DataAccess.Queries;
using Icon.Web.Tests.Integration.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Dapper;
using System.Data;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetScanCodeQueryTests
    {
        private GetScanCodesQuery queryHandler;
        private GetScanCodesParameters parameters;       
        private TransactionScope transaction;       
        private List<string> testScanCode;
        private IDbConnection dbConnection;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            dbConnection = SqlConnectionBuilder.CreateIconConnection();
            queryHandler = new GetScanCodesQuery(dbConnection);
            parameters = new GetScanCodesParameters ();            
        }

        [TestCleanup]
        public void Cleanup()
        {            
            transaction.Dispose();
        }

        [TestMethod]
        public void GetScanCodeExistQuery_ScanCodeExists_ReturnCount()
        {
            //Given
            CreateTestScancodes();
            testScanCode = new List<string>
            {
                "11"
            };
            parameters.ListOfScanCodes = testScanCode;            

            //When
            var result = queryHandler.Search(parameters);

            //Then
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());            
        }

        [TestMethod]
        public void GetMissingScanCodeQuery_ScanCodeNotExists_ReturnZeroCount()
        {
            //Given
            CreateTestScancodes();
            testScanCode = new List<string>
            {
                "10"                
            };
            parameters.ListOfScanCodes = testScanCode;

            //When
            var result = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(0, result.Count());
            Assert.AreNotSame(1, result);
        }       

        [TestMethod]
        public void GetMissingScanCodeQuery_ScanCodeExistsAndNotExists_ReturnCount()
        {
            //Given
            CreateTestScancodes();            
            testScanCode = new List<string>
            {
                "10",
                "11",                

            };
            parameters.ListOfScanCodes = testScanCode;

            //When
            var result = queryHandler.Search(parameters);

            //Then
            Assert.AreEqual(1, result.Count());
            Assert.AreNotSame(2, result);
        }

        public void CreateTestScancodes()
        {
            dbConnection.Execute($@"INSERT INTO dbo.ScanCode(scanCode, scanCodeTypeId, itemId, barcodeTypeId)
                VALUES ('11', '1', '1', '1');");
        }
    }
}