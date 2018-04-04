using Icon.Framework;
using Icon.Logging;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass] [Ignore]
    public class GetNewScanCodeUploadsQueryTests
    {
        private IconContext context;
        private Mock<ILogger> mockLogger;
        private GetNewScanCodeUploadsQuery getNewScanCodeUploadsQuery;
        private Item item;
        private ScanCode upc;
        private int addedItemId;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger>();
            context = new IconContext();

            item = new Item { itemTypeID = 1 };
            context.Item.Add(item);
            context.SaveChanges();
            addedItemId = item.itemID;

            if (!context.ScanCode.Any(s => s.scanCode == "424242424242"))
            {
                upc = new ScanCode { itemID = item.itemID, scanCode = "424242424242", scanCodeTypeID = 1, localeID = 1 };
                context.ScanCode.Add(upc);
                context.SaveChanges();
            }
            getNewScanCodeUploadsQuery = new GetNewScanCodeUploadsQuery(this.mockLogger.Object, this.context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            mockLogger = null;

            item = context.Item.Where(i => i.itemID == addedItemId).FirstOrDefault();
            upc = context.ScanCode.Where(sc => sc.itemID == addedItemId).FirstOrDefault();
            context.Item.Remove(item);
            if (null != (upc))
            {
                context.ScanCode.Remove(upc);
            }
            context.Database.CommandTimeout = 180;
            context.SaveChanges();

            context.Dispose();
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
