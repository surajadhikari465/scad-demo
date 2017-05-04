using Icon.ApiController.DataAccess.Queries;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Icon.ApiController.Tests.Queries
{
    [TestClass]
    public class GetFinancialHierarchyClassesQueryTests
    {
        private GetFinancialHierarchyClassesQuery query;
        private IconContext context;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            query = new GetFinancialHierarchyClassesQuery(new IconDbContextFactory());
        }

        [TestMethod]
        public void GetFinancialHierarchyClasses_SuccessfulExecution_ShouldReturnAllFinancialHierarchyClasses()
        {
            // Given.
            var parameters = new GetFinancialHierarchyClassesParameters();

            // When.
            var financialClasses = query.Search(parameters);

            // Then.
            int actualFinancialClassesCount = context.HierarchyClass.Where(hc => hc.Hierarchy.hierarchyName == HierarchyNames.Financial).ToList().Count;

            Assert.AreEqual(actualFinancialClassesCount, financialClasses.Count);
        }
    }
}
