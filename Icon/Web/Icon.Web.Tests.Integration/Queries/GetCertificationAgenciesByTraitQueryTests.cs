using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass] [Ignore]
    public class GetCertificationAgenciesByTraitQueryTests
    {
        private IconContext context;
        private DbContextTransaction transaction;
        private GetCertificationAgenciesByTraitQuery query;
        private List<string> testOrganicAgencyNames;
        private List<string> testHierarchyClassNames;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            query = new GetCertificationAgenciesByTraitQuery(this.context);

            testOrganicAgencyNames = new List<string>
            {
                "Organic Agency1",
                "Organic Agency2",
                "Organic Agency3"
            };

            testHierarchyClassNames = new List<string>
            {
                "Test HierarchyClass1",
                "Test HierarchyClass2",
                "Test HierarchyClass3"
            };

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void GetCertificationAgencies_NoMatchingResults_NoResultsShouldBeReturned()
        {
            // Given.
            var parameters = new GetCertificationAgenciesByTraitParameters { AgencyTypeTraitCode = String.Empty };

            // When.
            var results = query.Search(parameters);

            // Then.
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void GetCertificationAgencies_OneMatchingResult_OneResultShouldBeReturned()
        {
            // Given.
            HierarchyClass testHierarchyClass = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.CertificationAgencyManagement).WithHierarchyClassName(testHierarchyClassNames[0]);

            context.HierarchyClass.Add(testHierarchyClass);

            testHierarchyClass.HierarchyClassTrait = new List<HierarchyClassTrait>
            {
                new HierarchyClassTrait
                {
                    hierarchyClassID = testHierarchyClass.hierarchyClassID,
                    traitID = Traits.Organic,
                    Trait = context.Trait.Single(t => t.traitCode == TraitCodes.Organic),
                    traitValue = "Organic Agency"
                }
            };

            context.SaveChanges();

            var parameters = new GetCertificationAgenciesByTraitParameters { AgencyTypeTraitCode = TraitCodes.Organic };

            // When.
            string testHierarchyClassName = testHierarchyClassNames[0];
            var results = query.Search(parameters).Where(r => r.hierarchyClassName == testHierarchyClassName).ToList();

            // Then.
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void GetCertificationAgencies_ThreeMatchingResult_ThreeResultsShouldBeReturned()
        {
            // Given.
            var testHierarchyClasses = new List<HierarchyClass>
            {
                new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.CertificationAgencyManagement).WithHierarchyClassName(testHierarchyClassNames[0]),
                new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.CertificationAgencyManagement).WithHierarchyClassName(testHierarchyClassNames[1]),
                new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.CertificationAgencyManagement).WithHierarchyClassName(testHierarchyClassNames[2])
            };

            context.HierarchyClass.AddRange(testHierarchyClasses);

            var organicTrait = context.Trait.Single(t => t.traitCode == TraitCodes.Organic);
            foreach (var hierarchyClass in testHierarchyClasses)
            {
                hierarchyClass.HierarchyClassTrait = new List<HierarchyClassTrait>
                {
                    new HierarchyClassTrait
                    {
                        hierarchyClassID = hierarchyClass.hierarchyClassID,
                        traitID = Traits.Organic,
                        Trait = organicTrait,
                        traitValue = "Organic Agency"
                    }
                };
            }

            context.SaveChanges();

            var parameters = new GetCertificationAgenciesByTraitParameters { AgencyTypeTraitCode = TraitCodes.Organic };

            // When.
            string testHierarchyClassName = testHierarchyClassNames[0];
            var results = query.Search(parameters).Where(r => testHierarchyClassNames.Contains(r.hierarchyClassName)).ToList();

            // Then.
            Assert.AreEqual(3, results.Count);
        }
    }
}
