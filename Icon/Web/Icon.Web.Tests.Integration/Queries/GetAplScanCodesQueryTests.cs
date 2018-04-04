using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass] [Ignore]
    public class GetAplScanCodesQueryTests
    {
        private IconContext context;
        private GetAplScanCodesQuery query;
        private DbContextTransaction transaction;
        private List<AuthorizedProductList> testApl;
        private List<Agency> testAgencies;
        private List<string> testAgenciesId;
        private List<string> testWfmScanCodes;
        private List<string> testAplScanCodes;

        [TestInitialize]
        public void InitializeData()
        {
            context = new IconContext();
            query = new GetAplScanCodesQuery(this.context);

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

        private bool AllScanCodesContainItemDescription(List<EwicAplScanCodeModel> scanCodes)
        {
            return scanCodes.TrueForAll(sc => !String.IsNullOrEmpty(sc.ItemDescription));
        }

        [TestMethod]
        public void GetAplScanCodesQuery_NoScanCodesInTheApl_NoResultsShouldBeReturned()
        {
            // Given.

            // When.
            var scanCodes = query.Search(new GetAplScanCodesParameters()).Where(q => testWfmScanCodes.Contains(q.ScanCode)).ToList();

            // Then.
            Assert.AreEqual(0, scanCodes.Count);
        }

        [TestMethod]
        public void GetAplScanCodesQuery_ThreeDistinctScanCodesForThreeAgencies_ThreeScanCodesShouldBeReturned()
        {
            // Given.
            testApl = new List<AuthorizedProductList>
            {
                new TestAplBuilder().WithAgencyId(testAgenciesId[0]).WithScanCode(testWfmScanCodes[0]),
                new TestAplBuilder().WithAgencyId(testAgenciesId[1]).WithScanCode(testWfmScanCodes[1]),
                new TestAplBuilder().WithAgencyId(testAgenciesId[2]).WithScanCode(testWfmScanCodes[2])
            };

            context.AuthorizedProductList.AddRange(testApl);
            context.SaveChanges();

            // When.
            var scanCodes = query.Search(new GetAplScanCodesParameters()).Where(q => testWfmScanCodes.Contains(q.ScanCode)).ToList();

            // Then.
            Assert.AreEqual(3, scanCodes.Count);
            Assert.IsTrue(AllScanCodesContainItemDescription(scanCodes));
        }

        [TestMethod]
        public void GetAplScanCodesQuery_ThreeAgenciesWithTheSameScanCode_OneScanCodeShouldBeReturned()
        {
            // Given.
            testApl = new List<AuthorizedProductList>
            {
                new TestAplBuilder().WithAgencyId(testAgenciesId[0]).WithScanCode(testWfmScanCodes[0]),
                new TestAplBuilder().WithAgencyId(testAgenciesId[1]).WithScanCode(testWfmScanCodes[0]),
                new TestAplBuilder().WithAgencyId(testAgenciesId[2]).WithScanCode(testWfmScanCodes[0])
            };

            context.AuthorizedProductList.AddRange(testApl);
            context.SaveChanges();

            // When.
            var scanCodes = query.Search(new GetAplScanCodesParameters()).Where(q => testWfmScanCodes.Contains(q.ScanCode)).ToList();

            // Then.
            Assert.AreEqual(1, scanCodes.Count);
            Assert.IsTrue(AllScanCodesContainItemDescription(scanCodes));
        }
    }
}