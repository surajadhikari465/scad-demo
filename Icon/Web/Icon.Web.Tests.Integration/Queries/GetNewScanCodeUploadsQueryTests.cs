using Icon.Framework;
using Icon.Logging;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Tests.Integration.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetNewScanCodeUploadsQueryTests
    {
        private IconContext context;
        private Mock<ILogger> mockLogger;
        private GetNewScanCodeUploadsQuery getNewScanCodeUploadsQuery;
        private Item item;
        private ScanCode upc;
        private int addedItemId;
        private ItemTestHelper itemTestHelper;
        private TransactionScope transactionScope;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger>();
            
            transactionScope = new TransactionScope();
            context = new IconContext();
            itemTestHelper = new ItemTestHelper();
            itemTestHelper.Initialize(context.Database.Connection, false, false);

            itemTestHelper.TestScanCode = "424242424242";
            var tmpItem = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(tmpItem);

            item = context.Item.First(i => i.ItemId == tmpItem.ItemId);

            getNewScanCodeUploadsQuery = new GetNewScanCodeUploadsQuery(this.mockLogger.Object, this.context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            mockLogger = null;
            context.Dispose();
            transactionScope.Dispose();
        }

        [TestMethod]
        public void NewScanCodeSearch_NewScanCode_ShouldBeReturned()
        {
            // Given.
            var parameters = new GetNewScanCodeUploadsParameters
            {
                ScanCodes = new List<ScanCodeModel>
                {
                    new ScanCodeModel
                    {
                        ScanCode = "333333555555"
                    },

                    new ScanCodeModel
                    {
                        ScanCode = "424242424242"
                    }
                }
            };

            // When.
            var invalidItems = getNewScanCodeUploadsQuery.Search(parameters);
            var expectedCount = 1;
            var actualCount = invalidItems.Count;

            // Then.
            Assert.AreEqual(expectedCount, actualCount);
            Assert.AreEqual("333333555555", invalidItems.Single().ScanCode);
        }

        [ExpectedException(typeof(SqlException))]
        [TestMethod]
        public void GetExistingScanCodeQuery_ErrorWithUpload_ThrowsSqlException()
        {
            // Given.

            // Provide ScanCode longer than 13 characters so it doesn't match Db Table Type.
            var parameters = new GetNewScanCodeUploadsParameters
            {
                ScanCodes = new List<ScanCodeModel>
                {
                    new ScanCodeModel
                    {
                        ScanCode = "3333335555555555"
                    },

                    new ScanCodeModel
                    {
                        ScanCode = "424242424242"
                    }
                }
            };

            // When & Then: Sql Exception Expected
            var invalidItems = getNewScanCodeUploadsQuery.Search(parameters);
        }
    }
}
