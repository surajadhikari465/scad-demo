using System.Linq;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Web.DataAccess.Queries;
using Icon.Framework;
using System.Data.Entity;
using Icon.Testing.Builders;
using System.Collections.Generic;
using Icon.Web.Tests.Integration.TestHelpers;
using System.Transactions;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetDefaultTaxClassMismatchesQueryTests
    {
        private GetDefaultTaxClassMismatchesQuery query;
        private GetDefaultTaxClassMismatchesParameters parameters;
        private IconContext context;
        private DbContextTransaction transaction;
        private HierarchyClassTrait merchDefaultTaxTrait;
        private HierarchyClass testMerchandiseClass;
        private HierarchyClass testParentMerchandiseClass;
        private HierarchyClass testBrand;
        private HierarchyClass defaultTaxClass;
        private HierarchyClass updatedTaxClass;
        private HierarchyClass overridenTaxClass;


        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();

            testMerchandiseClass = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Merchandise).WithHierarchyLevel(HierarchyLevels.SubBrick);
            testParentMerchandiseClass = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Merchandise).WithHierarchyLevel(HierarchyLevels.Gs1Brick);
            testBrand = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Brands).WithHierarchyLevel(HierarchyLevels.Brand);
            defaultTaxClass = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Tax).WithHierarchyLevel(HierarchyLevels.Tax);
            updatedTaxClass = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Tax).WithHierarchyLevel(HierarchyLevels.Tax);
            overridenTaxClass = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Tax).WithHierarchyLevel(HierarchyLevels.Tax);

            query = new GetDefaultTaxClassMismatchesQuery(context);
            parameters = new GetDefaultTaxClassMismatchesParameters();

            transaction = context.Database.BeginTransaction();

            context.HierarchyClass.Add(testMerchandiseClass);
            context.HierarchyClass.Add(testParentMerchandiseClass);
            context.HierarchyClass.Add(testBrand);
            context.HierarchyClass.Add(defaultTaxClass);
            context.HierarchyClass.Add(updatedTaxClass);
            context.HierarchyClass.Add(overridenTaxClass);
            context.SaveChanges();

            testMerchandiseClass.hierarchyParentClassID = testParentMerchandiseClass.hierarchyClassID;
            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void CreateDefaultMapping()
        {
            merchDefaultTaxTrait = new HierarchyClassTrait
            {
                traitID = Traits.MerchDefaultTaxAssociatation,
                traitValue = defaultTaxClass.hierarchyClassID.ToString(),
                hierarchyClassID = testMerchandiseClass.hierarchyClassID
            };

            context.HierarchyClassTrait.Add(merchDefaultTaxTrait);
            context.SaveChanges();
        }

        private void CreateSecondaryMapping(HierarchyClass merchandiseClass, HierarchyClass taxClass)
        {
            var merchDefaultTaxTrait = new HierarchyClassTrait
            {
                traitID = Traits.MerchDefaultTaxAssociatation,
                traitValue = taxClass.hierarchyClassID.ToString(),
                hierarchyClassID = merchandiseClass.hierarchyClassID
            };

            context.HierarchyClassTrait.Add(merchDefaultTaxTrait);
            context.SaveChanges();
        }

        [TestMethod]
        public void GetDefaultTaxClassMismatchesQuery_NoDefaultMappingsExist_NoResultsShouldBeReturned()
        {
            // Given.
            var merchDefaultTaxTraits = context.HierarchyClassTrait.Where(hct => hct.traitID == Traits.MerchDefaultTaxAssociatation).ToList();
            context.HierarchyClassTrait.RemoveRange(merchDefaultTaxTraits);
            context.SaveChanges();

            // When.
            var results = query.Search(parameters);

            // Then.
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void GetDefaultTaxClassMismatchesQuery_MappingExistsWithNoOverrides_NoResultsShouldBeReturned()
        {
            // Given.
            CreateDefaultMapping();

            // When.
            var results = query.Search(parameters).Where(q => q.MerchandiseLineage.Split('|')[1] == testMerchandiseClass.hierarchyClassName.ToString()).ToList();

            // Then.
            Assert.AreEqual(0, results.Count);
        }
    }
}
