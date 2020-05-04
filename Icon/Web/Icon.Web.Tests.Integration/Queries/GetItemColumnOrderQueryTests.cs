using System.Collections.Generic;
using System.Data;
using System.Linq;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]

    public class GetItemColumnOrderQueryTests
    {
        private IconContext context;
        private IDbConnection connection;
        private GetItemColumnOrderQuery getItemColumnOrderQuery;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            connection = context.Database.Connection;
            
            getItemColumnOrderQuery = new GetItemColumnOrderQuery(connection);
        }

        [TestMethod]
        public void GetItemColumnOrderQuery_Search_ReturnsData()
        {
            var data = getItemColumnOrderQuery.Search(new EmptyQueryParameters<List<ItemColumnOrderModel>>());

            Assert.IsTrue(data.Any());
        }

        [TestCleanup]
        public void Cleanup()
        {
            connection.Dispose();
            context.Dispose();
        }

    }
}