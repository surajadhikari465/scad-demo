using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetHierarchyLineageQueryTests
    {
        private IconContext context;
        
        private GetHierarchyLineageParameters queryParameters;
        private GetHierarchyLineageQuery query;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IconContext();
            this.queryParameters = new GetHierarchyLineageParameters();
            this.query = new GetHierarchyLineageQuery(context);
        }

        [TestCleanup]
        public void CleanupData()
        {
            this.context.Dispose();
            this.query = null;
        }

        [TestMethod]
        public void GetHierarchyLineageQuery_RunSearch_ReturnsResults()
        {
            // When
            var result = query.Search(new GetHierarchyLineageParameters());

            // Then
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetHierarchyLineageQuery_RunSearch_ReturnsNoDuplicates()
        {
            // When
            HierarchyClassListModel result = query.Search(new GetHierarchyLineageParameters());

            var brandList = result.BrandHierarchyList.GroupBy(x => x.HierarchyClassId)
              .Where(g => g.Count() > 1)
              .Select(y => new { Element = y.Key, Counter = y.Count() })
              .ToList();

            var taxList = result.TaxHierarchyList.GroupBy(x => x.HierarchyClassId)
             .Where(g => g.Count() > 1)
             .Select(y => new { Element = y.Key, Counter = y.Count() })
             .ToList();

            var merchList = result.MerchandiseHierarchyList.GroupBy(x => x.HierarchyClassId)
             .Where(g => g.Count() > 1)
             .Select(y => new { Element = y.Key, Counter = y.Count() })
             .ToList();

            var browsingList = result.BrowsingHierarchyList.GroupBy(x => x.HierarchyClassId)
             .Where(g => g.Count() > 1)
             .Select(y => new { Element = y.Key, Counter = y.Count() })
             .ToList();

            // Then
            Assert.IsTrue(brandList.Count == 0);
            Assert.IsTrue(taxList.Count == 0);
            Assert.IsTrue(merchList.Count == 0);
            Assert.IsTrue(browsingList.Count == 0);
        }

        [TestMethod]
        public void GetHierarchyLineageQuery_Search_ReturnsHierarchyClassListModel()
        {
            //When
            var results = query.Search(new GetHierarchyLineageParameters());

            //Then
            Assert.IsInstanceOfType(results, typeof(HierarchyClassListModel));
        }


        [TestMethod]
        public void GetHierarchyLineageQuery_SearchWithBrowsing_ReturnsBrowsingList()
        {
            //When
            var results = query.Search(new GetHierarchyLineageParameters());
            //Then
            Assert.IsNotNull(results.BrowsingHierarchyList);
        }
    }
}
