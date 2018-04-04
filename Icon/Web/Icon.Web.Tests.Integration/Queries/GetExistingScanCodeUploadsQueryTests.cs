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
    public class GetExistingScanCodeUploadsQueryTests
    {
        private IconContext context;
        private Mock<ILogger> mockLogger;
        private GetExistingScanCodeUploadsQuery getExistingScanCodeUploadsQuery;
        private Item item;
        private ScanCode upc;
        private int addedItemId;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger>();
            context = new IconContext();
            context.Database.CommandTimeout = 180;

            context.ScanCode.RemoveRange(context.ScanCode.Where(sc => sc.scanCode == "424242424242"));
            context.SaveChanges();

            item = new Item { itemTypeID = 1 };
            context.Item.Add(item);
            context.SaveChanges();
            addedItemId = item.itemID;

            upc = new ScanCode { itemID = item.itemID, scanCode = "424242424242", scanCodeTypeID = 1, localeID = 1 };
            context.ScanCode.Add(upc);
            context.SaveChanges();

            getExistingScanCodeUploadsQuery = new GetExistingScanCodeUploadsQuery(this.mockLogger.Object, this.context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            item = context.Item.Where(i => i.itemID == addedItemId).FirstOrDefault();
            upc = context.ScanCode.Where(sc => sc.itemID == addedItemId).FirstOrDefault();
            context.Item.Remove(item);
            context.ScanCode.Remove(upc);
            context.SaveChanges();

            context.Dispose();
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
