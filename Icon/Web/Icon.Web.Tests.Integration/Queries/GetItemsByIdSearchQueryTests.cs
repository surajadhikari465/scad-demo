using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetItemsByIdSearchQueryTests
    {
        GetItemsByIdSearchQuery queryHandler;
        IDbProvider db;

        [TestInitialize]
        public void Initialize()
        {
            this.db = new SqlDbProvider();
            string connectionString = ConfigurationManager.ConnectionStrings["Icon"].ConnectionString;
            this.db.Connection = new SqlConnection(connectionString);
            this.db.Connection.Open();

            this.db.Transaction = this.db.Connection.BeginTransaction();

            queryHandler = new GetItemsByIdSearchQuery(db);
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.db.Transaction.Rollback();
            this.db.Transaction.Dispose();
        }

        [TestMethod]
        public void GetItemsByIdSearchQueryTests_GetZeroItems()
        {
            List<int> ids = new List<int>();

            var result = queryHandler.Search(new GetItemsByIdSearchParameters { ItemIds = ids });

            Assert.AreEqual(0, result.TotalRecordsCount);
        }

        [TestMethod]
        public void GetItemsByIdSearchQueryTests_GetSingleItem()
        {
            List<int> ids = new List<int>() { 15514 }; //banana

            var result = queryHandler.Search(new GetItemsByIdSearchParameters { ItemIds = ids });

            Assert.AreEqual(1, result.TotalRecordsCount);
            Assert.AreEqual("4011", result.Items.ToList()[0].ScanCode);
        }

        [TestMethod]
        public void GetItemsByIdSearchQueryTests_GetMultipleItem()
        {
            List<int> ids = new List<int>() { 15514, 15671 }; //banana, lemon

            var result = queryHandler.Search(new GetItemsByIdSearchParameters { ItemIds = ids });

            Assert.AreEqual(2, result.TotalRecordsCount);
            Assert.AreEqual("4011", result.Items.ToList()[0].ScanCode);
            Assert.AreEqual("4053", result.Items.ToList()[1].ScanCode);
        }
    }
}
