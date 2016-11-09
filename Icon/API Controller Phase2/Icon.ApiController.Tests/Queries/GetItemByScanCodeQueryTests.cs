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

namespace Icon.ApiController.Tests.Queries
{
    [TestClass]
    public class GetItemByScanCodeQueryTests
    {
        private IconContext context;
        private GlobalIconContext globalContext;
        private DbContextTransaction transaction;
        private Item testItem;
        private string testScanCode;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            globalContext = new GlobalIconContext(context);
            testScanCode = "1112223334445";

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
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
            var getItemByScanCodeQuery = new GetItemByScanCodeQuery(new Mock<ILogger<GetItemByScanCodeQuery>>().Object, globalContext);

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

            var getItemByScanCodeQuery = new GetItemByScanCodeQuery(new Mock<ILogger<GetItemByScanCodeQuery>>().Object, globalContext);

            var parameters = new GetItemByScanCodeParameters
            {
                ScanCode = testScanCode
            };

            // When.
            var item = getItemByScanCodeQuery.Search(parameters);

            // Then.
            Assert.IsNotNull(item);
            Assert.AreEqual(item.ScanCode.Single().scanCode, testScanCode);
        }
    }
}
