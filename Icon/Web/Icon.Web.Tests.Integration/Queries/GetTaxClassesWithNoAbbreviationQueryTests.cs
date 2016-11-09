using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetTaxClassesWithNoAbbreviationQueryTests
    {
        private GetTaxClassesWithNoAbbreviationQuery getTaxClassWithNoAbbreviationQuery;
        
        private IconContext context;
        private HierarchyClass taxClassWithNoAbbreviation;
        private HierarchyClass taxClassWithAbbreviation;
        private string testTaxName;
        private string testTaxAbbreviation;
        private List<string> testTaxClassId;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            getTaxClassWithNoAbbreviationQuery = new GetTaxClassesWithNoAbbreviationQuery(this.context);
            testTaxName = "Test Tax";
            testTaxAbbreviation = "Test Tax Abbr";
            
            RemoveTestData();

            taxClassWithNoAbbreviation = new HierarchyClass
            {
                hierarchyID = Hierarchies.Tax,
                hierarchyClassName = testTaxName,
                hierarchyLevel = 1,
                hierarchyParentClassID = null
            };

            taxClassWithAbbreviation = new HierarchyClass
            {
                hierarchyID = Hierarchies.Tax,
                hierarchyClassName = testTaxName,
                hierarchyLevel = 1,
                hierarchyParentClassID = null
            };

            context.HierarchyClass.AddRange(new List<HierarchyClass> { taxClassWithAbbreviation, taxClassWithNoAbbreviation });
            context.SaveChanges();

            testTaxClassId = new List<string> { taxClassWithAbbreviation.hierarchyClassID.ToString(), taxClassWithNoAbbreviation.hierarchyClassID.ToString() };

            var taxClassTrait = new List<HierarchyClassTrait>
            {
                new HierarchyClassTrait
                {
                    hierarchyClassID = taxClassWithAbbreviation.hierarchyClassID,
                    traitID = Traits.TaxAbbreviation,
                    traitValue = testTaxAbbreviation
                }
            };

            context.HierarchyClassTrait.Add(taxClassTrait[0]);
            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            RemoveTestData();
        }

        private void RemoveTestData()
        {
            context.ItemHierarchyClass.RemoveRange(context.ItemHierarchyClass.Where(ihc => ihc.HierarchyClass.hierarchyClassName == testTaxName));
            context.HierarchyClassTrait.RemoveRange(context.HierarchyClassTrait.Where(hct => hct.HierarchyClass.hierarchyClassName == testTaxName));
            context.HierarchyClass.RemoveRange(context.HierarchyClass.Where(hc => hc.hierarchyClassName == testTaxName));
            context.SaveChanges();
        }

        [TestMethod]
        public void GetTaxWithNoAbbreviationSearch_TaxClassWithNoAbbreviation_ShouldBeReturned()
        {
            // Given.
            var parameters = new GetTaxClassesWithNoAbbreviationParameters
            {
                TaxClasses = testTaxClassId
            };

            // When.
            var taxClass = getTaxClassWithNoAbbreviationQuery.Search(parameters).Single();

            // Then.
            Assert.AreEqual(taxClassWithNoAbbreviation.hierarchyClassID.ToString(), taxClass);
        }

        [TestMethod]
        public void GetTaxWithNoAbbreviationSearch_TaxClassWithAbbreviation_ShouldNotBeReturned()
        {
            // Given.
            var parameters = new GetTaxClassesWithNoAbbreviationParameters
            {
                TaxClasses = testTaxClassId
            };

            // When.
            var taxClass = getTaxClassWithNoAbbreviationQuery.Search(parameters);

            // Then.
            Assert.IsFalse(taxClass.Contains(taxClassWithAbbreviation.hierarchyClassID.ToString()));
        }
    }
}
