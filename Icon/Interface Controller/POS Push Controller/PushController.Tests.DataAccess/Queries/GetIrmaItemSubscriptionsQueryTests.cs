using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PushController.DataAccess.Queries;
using System.Collections.Generic;
using System.Data.Entity;

namespace PushController.Tests.DataAccess.Queries
{
    [TestClass]
    public class GetIrmaItemSubscriptionsQueryTests
    {
        private GetIrmaItemSubscriptionsQueryHandler queryHandler;
        private GlobalIconContext globalContext;
        private DbContextTransaction transaction;
        private List<string> testScanCodes;

        [TestInitialize]
        public void Initialize()
        {
            globalContext = new GlobalIconContext(new IconContext());
            queryHandler = new GetIrmaItemSubscriptionsQueryHandler(globalContext);

            testScanCodes = new List<string>
            {
                "22222220",
                "22222221",
                "22222222",
            };

            transaction = globalContext.Context.Database.BeginTransaction();
        }

        private void StageIrmaItemSubscriptions(List<IRMAItemSubscription> subscriptions)
        {
            globalContext.Context.IRMAItemSubscription.AddRange(subscriptions);
            globalContext.Context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void GetIrmaItemSubscriptions_ThreeSubscriptionsMatchTheQueryParameters_ThreeSubscriptionsShouldBeReturned()
        {
            // Given.
            var subscriptions = new List<IRMAItemSubscription>
            {
                new TestIrmaItemSubscriptionBuilder().WithRegionCode("SP").WithIdentifier(testScanCodes[0]),
                new TestIrmaItemSubscriptionBuilder().WithRegionCode("NC").WithIdentifier(testScanCodes[1]),
                new TestIrmaItemSubscriptionBuilder().WithRegionCode("SW").WithIdentifier(testScanCodes[2])
            };

            StageIrmaItemSubscriptions(subscriptions);

            var testPosData = new List<IRMAPush>
            {
                new TestIrmaPushBuilder().WithRegionCode("SP").WithIdentifier(testScanCodes[0]),
                new TestIrmaPushBuilder().WithRegionCode("NC").WithIdentifier(testScanCodes[1]),
                new TestIrmaPushBuilder().WithRegionCode("SW").WithIdentifier(testScanCodes[2])
            };

            var query = new GetIrmaItemSubscriptionsQuery
            {
                IrmaPushData = testPosData
            };

            // When.
            var queryResults = queryHandler.Execute(query);

            // Then.
            Assert.AreEqual(subscriptions.Count, queryResults.Count);
        }

        [TestMethod]
        public void GetIrmaItemSubscriptions_NoSubscriptionMatchesTheQueryParameters_NoResultsShouldBeReturned()
        {
            // Given.
            var subscriptions = new List<IRMAItemSubscription>
            {
                new TestIrmaItemSubscriptionBuilder().WithRegionCode("SP").WithIdentifier(testScanCodes[0]),
                new TestIrmaItemSubscriptionBuilder().WithRegionCode("NC").WithIdentifier(testScanCodes[1]),
                new TestIrmaItemSubscriptionBuilder().WithRegionCode("SW").WithIdentifier(testScanCodes[2])
            };

            StageIrmaItemSubscriptions(subscriptions);

            var testPosData = new List<IRMAPush>
            {
                new TestIrmaPushBuilder().WithRegionCode("RM").WithIdentifier(testScanCodes[0]),
                new TestIrmaPushBuilder().WithRegionCode("NA").WithIdentifier(testScanCodes[1]),
                new TestIrmaPushBuilder().WithRegionCode("MW").WithIdentifier(testScanCodes[2])
            };

            var query = new GetIrmaItemSubscriptionsQuery
            {
                IrmaPushData = testPosData
            };

            // When.
            var queryResults = queryHandler.Execute(query);

            // Then.
            Assert.AreEqual(0, queryResults.Count);
        }
    }
}
