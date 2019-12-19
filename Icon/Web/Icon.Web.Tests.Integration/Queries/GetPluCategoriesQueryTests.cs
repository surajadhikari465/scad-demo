using Icon.Framework;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass] [Ignore]
    public class GetPluCategoriesQueryTests
    {
        private GetPluCategoriesQuery query;
        private GetPluCategoriesParameters parameters;
        private IconContext context;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            query = new GetPluCategoriesQuery(context);
            parameters = new GetPluCategoriesParameters();
        }

        [TestMethod]
        public void GetPluCategories_PluCategoriesExist_ShouldReturnPluCategoriesOrderedByBeginRange()
        {
            //When
            var pluCategories = query.Search(parameters);

            //Then
            var sortedPluCategories = pluCategories.OrderBy(p => Int64.Parse(p.BeginRange)).ToList();

            for (int i = 0; i < pluCategories.Count; i++)
            {
                Assert.AreEqual(sortedPluCategories[i].BeginRange, pluCategories[i].BeginRange);
                Assert.AreEqual(sortedPluCategories[i].EndRange, pluCategories[i].EndRange);
                Assert.AreEqual(sortedPluCategories[i].PluCategoryID, pluCategories[i].PluCategoryID);
                Assert.AreEqual(sortedPluCategories[i].PluCategoryName, pluCategories[i].PluCategoryName);
            }
        }
    }
}
