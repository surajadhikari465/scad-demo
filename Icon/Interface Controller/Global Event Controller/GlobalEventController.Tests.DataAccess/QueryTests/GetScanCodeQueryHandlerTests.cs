using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Framework;
using System.Data.Entity;
using Icon.Testing.Builders;
using GlobalEventController.DataAccess.Queries;
using System.Collections.Generic;
using System.Linq;
using Irma.Framework;


namespace GlobalEventController.Tests.DataAccess.QueryTests
{
    [TestClass]
    public class GetScanCodeQueryHandlerTests
    {
        private IconContext context;
        private DbContextTransaction transaction;
        List<string> expectedScanCodeList;
        List<Icon.Framework.Item> itemList;
        private GetScanCodeQueryHandler queryHandler;

        [TestInitialize]
        public void InitializeData()
        {
            // Fake list of scancodes to build items.
            this.expectedScanCodeList = new List<string>();
            this.expectedScanCodeList.Add("5558675309111");
            this.expectedScanCodeList.Add("5558675309222");
            this.expectedScanCodeList.Add("5558675309333");
            this.expectedScanCodeList.Add("5558675309444");
            this.expectedScanCodeList.Add("5558675309555");
            this.itemList = new List<Icon.Framework.Item>();
            TestItemBuilder builder = new TestItemBuilder();

            this.context = new IconContext();
            this.queryHandler = new GetScanCodeQueryHandler(this.context);
            this.transaction = this.context.Database.BeginTransaction();
            // build scancode list...
            foreach (var scancode in expectedScanCodeList)
            {
                itemList.Add(builder.WithScanCode(scancode));
            }
            this.context.Item.AddRange(itemList);
            this.context.SaveChanges();
        }

        [TestCleanup]
        public void CleanupData()
        {
            this.transaction.Rollback();
            this.context.Dispose();
        }

        [TestMethod]
        public void GetScanCodeQuery_ValidScanCodeList_ReturnsCorrectListOfScanCodeObjects()
        {
            // Given
            GetScanCodeQuery query = new GetScanCodeQuery { ScanCodes = this.expectedScanCodeList };

            // When
            List<ScanCode> actual = queryHandler.Handle(query);

            // Then
            Assert.IsNotNull(actual);
            // Check if the scancode count from intersection of the lists matches our target list (both lists contain same scancodes).
            Assert.AreEqual(
                actual.Select(sc => sc.scanCode).Intersect(this.expectedScanCodeList).Count(), 
                this.expectedScanCodeList.Count, 
                string.Format("Our expected scancode list did not match the retrieved list: expected count={0}, actual count={1}.", this.expectedScanCodeList.Count(), actual.Count())
            );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetScanCodeQuery_AllInvalidScancodes_ThrowsArgumentException()
        {
            // Given
            GetScanCodeQuery query = new GetScanCodeQuery { ScanCodes = new List<string> { "abc", "def" } };

            // When
            queryHandler.Handle(query);

            // Then
            // Expected ArgumentException
        }
    }
}
