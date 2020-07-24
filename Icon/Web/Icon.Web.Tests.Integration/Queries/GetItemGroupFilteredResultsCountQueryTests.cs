using Dapper;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Tests.Integration.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetItemGroupFilteredResultsCountQueryTests
    {
        private IDbConnection connection;
        private TransactionScope transaction;
        private GetItemGroupFilteredResultsCountQueryParameters queryParameters;
        private GetItemGroupFilteredResultsCountQuery query;

        /// <summary>
        /// Initialize Data for each data.
        /// </summary>
        [TestInitialize]
        public void InitializeData()
        {
            this.transaction = new TransactionScope();
            this.connection = SqlConnectionBuilder.CreateIconConnection();
            this.queryParameters = new GetItemGroupFilteredResultsCountQueryParameters()
            {
                SearchTerm = null,
                ItemGroupTypeId = ItemGroupTypeId.Sku,                
            };

            this.query = new GetItemGroupFilteredResultsCountQuery(this.connection);
        }

        /// <summary>
        /// Cleanup Data for each test.
        /// </summary>
        [TestCleanup]
        public void CleanupData()
        {
            this.transaction.Dispose();
            this.connection.Dispose();
        }

        // <summary>
        /// Verify that GetFilteredResultsCountQuery with Filter returns values for Skus.
        /// </summary>
        [TestMethod]
        public void GetFilteredResultsCountQuery_Sku_with_filters__returns_values()
        {
            // Given
            // The database contains more that 1 tofu product

            // When
            this.queryParameters.ItemGroupTypeId = DataAccess.Models.ItemGroupTypeId.Sku;
            this.queryParameters.SearchTerm = "Tofu";
            var result = this.query.Search(this.queryParameters);

            // Then
            Assert.IsTrue(result > 1);
        }
    }
}
