using Icon.ApiController.DataAccess.Queries;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data.Entity;
using System.Linq;
using System.Transactions;

namespace Icon.ApiController.Tests.Queries
{
    [TestClass]
    public class GetItemByScanCodeQueryTests
    {
        private GetItemByScanCodeQuery getItemByScanCodeQuery;
        private Mock<ILogger<GetItemByScanCodeQuery>> mockLogger;
        private IconContext context;
        private TransactionScope transaction;
        private Item testItem;
        private string testScanCode;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();

            context = new IconContext();
            testScanCode = "1112223334445";

            mockLogger = new Mock<ILogger<GetItemByScanCodeQuery>>();
            getItemByScanCodeQuery = new GetItemByScanCodeQuery(mockLogger.Object, new IconDbContextFactory());
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        private void StageTestScanCode()
        {
            testItem = new TestItemBuilder().WithScanCode(testScanCode);

            context.Item.Add(testItem);
            context.SaveChanges();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetItemByScanCodeQuery_ScanCodeDoesNotExist_ReturnsNull()
        {
            // Given.
            var parameters = new GetItemByScanCodeParameters
            {
                ScanCode = "-1"
            };

            // When.
            getItemByScanCodeQuery.Search(parameters);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        public void GetItemsByScanCodeQuery_ScanCodeExists_ShouldReturnItemEntity()
        {
            // Given.
            StageTestScanCode();

            var parameters = new GetItemByScanCodeParameters
            {
                ScanCode = testScanCode
            };

            // When.
            var item = getItemByScanCodeQuery.Search(parameters);

            // Then.
            Assert.IsNotNull(item);
            Assert.AreEqual(item.itemID, testItem.itemID);
        }
    }
}
