using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Unit.Extensions
{
    [TestClass] [Ignore]
    public class DataAccessExtensionsTests
    {
        private IconContext context;
        private DbContextTransaction transaction;

        [TestCleanup]
        public void Cleanup()
        {
            if (transaction != null)
            {
                transaction.Rollback();
            }
        }

        [TestMethod]
        public void AddHierarchyClassTrait_SuccessfulExecution_HierarchyClassTraitShouldBeAddedWithCorrectTraitValue()
        {
            // Given.
            context = new IconContext();

            HierarchyClass testHierarchyClass = new TestHierarchyClassBuilder();
            string testTraitValue = "TEST";

            // When.
            transaction = context.Database.BeginTransaction();

            context.HierarchyClass.Add(testHierarchyClass);
            context.SaveChanges();

            testHierarchyClass.AddHierarchyClassTrait(context, TraitCodes.BrandAbbreviation, testTraitValue);

            // Then.
            var newHierarchyClassTrait = context.HierarchyClassTrait
                .Single(hct => hct.hierarchyClassID == testHierarchyClass.hierarchyClassID && hct.traitID == Traits.BrandAbbreviation);

            Assert.AreEqual(testTraitValue, newHierarchyClassTrait.traitValue);
            Assert.IsNull(newHierarchyClassTrait.uomID);
        }
    }
}
