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
    public class GetExistingScanCodeUploadsQueryTests
    {
        private IconContext context;
        private Mock<ILogger> mockLogger;
        private GetExistingScanCodeUploadsQuery getExistingScanCodeUploadsQuery;
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
            
            context.Database.CommandTimeout = 180;

            itemTestHelper.TestScanCode = "424242424242";
            var tmpItem = itemTestHelper.CreateDefaultTestItem();
            itemTestHelper.SaveItem(tmpItem);

            item = context.Item.First(i => i.ItemId == tmpItem.ItemId);

            getExistingScanCodeUploadsQuery = new GetExistingScanCodeUploadsQuery(this.mockLogger.Object, this.context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
            transactionScope.Dispose();

        }

        [TestMethod]
        public void ExistingScanCodeSearch_ExistingScanCode_ShouldBeReturned()
        {
            // Given.
            var parameters = new GetExistingScanCodeUploadsParameters
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
            var invalidItems = getExistingScanCodeUploadsQuery.Search(parameters);
            var expectedCount = 1;
            var actualCount = invalidItems.Count;

            // Then.
            Assert.AreEqual(expectedCount, actualCount);
            Assert.AreEqual("424242424242", invalidItems.Single().ScanCode);
        }

        [ExpectedException(typeof(SqlException))]
        [TestMethod]
        public void GetExistingScanCodeQuery_ErrorWithUpload_ThrowsSqlException()
        {
            // Given.

            // Provide ScanCode longer than 13 characters so it doesn't match Db Table Type.
            var parameters = new GetExistingScanCodeUploadsParameters
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
            var invalidItems = getExistingScanCodeUploadsQuery.Search(parameters);
        }
    }
}
