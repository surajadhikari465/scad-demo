using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetEwicMappingseQueryTests
    {
        private IconContext context;
        private GetEwicMappingsQuery query;
        private DbContextTransaction transaction;
        private List<AuthorizedProductList> testApl;
        private List<Agency> testAgencies;
        private List<Item> testItems;
        private List<string> testAgenciesId;
        private List<string> testWfmScanCodes;
        private List<string> testAplScanCodes;
        private Dictionary<int, Trait> traitReferences;

        [TestInitialize]
        public void InitializeData()
        {
            context = new IconContext();
            query = new GetEwicMappingsQuery(this.context);

            BuildTraitReferences();

            testAgenciesId = new List<string>
            {
                "ZZ",
                "XX",
                "YY"
            };

            testWfmScanCodes = new List<string>
            {
                "2222222",
                "2222223",
                "2222224"
            };

            testAplScanCodes = new List<string>
            {
                "2222",
                "2223",
                "2224"
            };

            transaction = context.Database.BeginTransaction();

            StageTestAgencies();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void BuildTraitReferences()
        {
            traitReferences = new Dictionary<int, Trait>();

            foreach (var trait in context.Trait)
            {
                traitReferences.Add(trait.traitID, trait);
            }
        }

        private void StageTestAgencies()
        {
            testAgencies = new List<Agency>
            {
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[0]),
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[1]),
                new TestAgencyBuilder().WithAgencyId(testAgenciesId[2])
            };

            context.Agency.AddRange(testAgencies);
            context.SaveChanges();
        }

        private void StageTestApl()
        {
            testApl = new List<AuthorizedProductList>();

            for (int agency = 0; agency < testAgenciesId.Count; agency++)
            {
                for (int scanCode = 0; scanCode < testAplScanCodes.Count; scanCode++)
                {
                    testApl.Add(new TestAplBuilder().WithAgencyId(testAgenciesId[agency]).WithScanCode(testAplScanCodes[scanCode]).WithItemDescription("Test " + agency.ToString() + scanCode.ToString()));
                }
            }

            context.AuthorizedProductList.AddRange(testApl);
            context.SaveChanges();
        }

        private bool ResultsIncludeAplDescription(List<EwicMappingModel> mappings)
        {
            return mappings.TrueForAll(m => !String.IsNullOrEmpty(m.ProductDescription));
        }

        [TestMethod]
        public void GetEwicMappingsQuery_NoScanCodesMapped_NoResultsShouldBeReturned()
        {
            // Given.
            var parameters = new GetEwicMappingsParameters
            {
                AplScanCode = testWfmScanCodes[0]
            };

            // When.
            var mappings = query.Search(parameters);

            // Then.
            Assert.AreEqual(0, mappings.Count);
        }

        [TestMethod]
        public void GetEwicMappingsQuery_OneScanCodeMappedForOneAgency_OneMappingShouldBeReturned()
        {
            // Given.
            StageTestApl();

            testItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(testWfmScanCodes[0]).WithTraitReferences(traitReferences)
            };

            Mapping mapping = new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[0]).WithAplScanCode(testAplScanCodes[0]).WithWfmScanCode(testItems[0].ScanCode.Single());

            context.Item.AddRange(testItems);
            context.Mapping.Add(mapping);
            context.SaveChanges();

            var parameters = new GetEwicMappingsParameters
            {
                AplScanCode = testAplScanCodes[0]
            };

            // When.
            var mappings = query.Search(parameters);

            // Then.
            Assert.AreEqual(1, mappings.Count);
            Assert.IsTrue(ResultsIncludeAplDescription(mappings));
        }

        [TestMethod]
        public void GetEwicMappingsQuery_OneScanCodeMappedForTwoAgencies_OneMappingShouldBeReturned()
        {
            // Given.
            StageTestApl();

            testItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(testWfmScanCodes[0]).WithTraitReferences(traitReferences)
            };

            var testMappings = new List<Mapping>
            {
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[0]).WithAplScanCode(testAplScanCodes[0]).WithWfmScanCode(testItems[0].ScanCode.Single()),
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[1]).WithAplScanCode(testAplScanCodes[0]).WithWfmScanCode(testItems[0].ScanCode.Single())
            };

            context.Item.AddRange(testItems);
            context.Mapping.AddRange(testMappings);
            context.SaveChanges();

            var parameters = new GetEwicMappingsParameters
            {
                AplScanCode = testAplScanCodes[0]
            };

            // When.
            var mappings = query.Search(parameters);

            // Then.
            Assert.AreEqual(1, mappings.Count);
            Assert.IsTrue(ResultsIncludeAplDescription(mappings));
        }

        [TestMethod]
        public void GetEwicMappingsQuery_TwoScanCodesMappedForOneAgency_TwoMappingsShouldBeReturned()
        {
            // Given.
            StageTestApl();

            testItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(testWfmScanCodes[0]).WithTraitReferences(traitReferences),
                new TestItemBuilder().WithScanCode(testWfmScanCodes[1]).WithTraitReferences(traitReferences)
            };

            var testMappings = new List<Mapping>
            {
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[0]).WithAplScanCode(testAplScanCodes[0]).WithWfmScanCode(testItems[0].ScanCode.Single()),
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[0]).WithAplScanCode(testAplScanCodes[0]).WithWfmScanCode(testItems[1].ScanCode.Single())
            };

            context.Item.AddRange(testItems);
            context.Mapping.AddRange(testMappings);
            context.SaveChanges();

            var parameters = new GetEwicMappingsParameters
            {
                AplScanCode = testAplScanCodes[0]
            };

            // When.
            var mappings = query.Search(parameters);

            // Then.
            Assert.AreEqual(2, mappings.Count);
            Assert.IsTrue(ResultsIncludeAplDescription(mappings));
        }

        [TestMethod]
        public void GetEwicMappingsQuery_TwoScanCodesMappedForTwoAgencies_TwoMappingsShouldBeReturned()
        {
            // Given.
            StageTestApl();

            testItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(testWfmScanCodes[0]).WithTraitReferences(traitReferences),
                new TestItemBuilder().WithScanCode(testWfmScanCodes[1]).WithTraitReferences(traitReferences)
            };

            var testMappings = new List<Mapping>
            {
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[0]).WithAplScanCode(testAplScanCodes[0]).WithWfmScanCode(testItems[0].ScanCode.Single()),
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[0]).WithAplScanCode(testAplScanCodes[0]).WithWfmScanCode(testItems[1].ScanCode.Single()),
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[1]).WithAplScanCode(testAplScanCodes[0]).WithWfmScanCode(testItems[0].ScanCode.Single()),
                new TestEwicMappingBuilder().WithAgencyId(testAgenciesId[1]).WithAplScanCode(testAplScanCodes[0]).WithWfmScanCode(testItems[1].ScanCode.Single())
            };

            context.Item.AddRange(testItems);
            context.Mapping.AddRange(testMappings);
            context.SaveChanges();

            var parameters = new GetEwicMappingsParameters
            {
                AplScanCode = testAplScanCodes[0]
            };

            // When.
            var mappings = query.Search(parameters);

            // Then.
            Assert.AreEqual(2, mappings.Count);
            Assert.IsTrue(ResultsIncludeAplDescription(mappings));
        }
    }
}
