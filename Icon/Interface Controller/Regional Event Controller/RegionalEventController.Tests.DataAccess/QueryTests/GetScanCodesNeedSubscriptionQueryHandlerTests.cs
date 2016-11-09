using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionalEventController.DataAccess.Queries;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using Icon.Framework;

namespace RegionalEventController.Tests.DataAccess.QueryTests
{
    [TestClass]
    public class GetScanCodesNeedSubscriptionQueryHandlerTests
    {
        private IconContext context;
        private GetScanCodesNeedSubscriptionQuery query;
        private GetScanCodesNeedSubscriptionQueryHandler handler;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IconContext();
            this.query = new GetScanCodesNeedSubscriptionQuery();
            this.handler = new GetScanCodesNeedSubscriptionQueryHandler(this.context);

            this.transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void CleanupData()
        {
            if (this.transaction != null)
            {
                this.transaction.Rollback();
            }

            if (this.context != null)
            {
                this.context.Dispose();
            }
        }

        [TestMethod]
        public void GetScanCodesNeedSubscription_ScanCodesHaveSubscriptions_NoScanCodesReturned()
        {
            // Given
            List<IRMAItemSubscription> subscriptions = AddRegionSubscription();
            this.context.IRMAItemSubscription.AddRange(subscriptions);
            this.context.SaveChanges();

            this.query.regionCode = "FL";
            this.query.scanCodes = subscriptions.Select(s => s.identifier).ToList();
            
            List<string> irmaItemSubscriptions = this.context.IRMAItemSubscription
                .Where(s => this.query.scanCodes.Contains(s.identifier) && s.regioncode == this.query.regionCode && s.deleteDate == null)
                .Select(s => s.identifier)
                .ToList();
            List<string> expected = this.query.scanCodes.Except(irmaItemSubscriptions).ToList();

            // When
            List<string> actual = this.handler.Execute(this.query);

            // Then
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void GetScanCodesNeedSubscription_ScanCodesDoNotHaveSubscription_ScanCodesReturned()
        {
            // Given
            List<IRMAItemSubscription> subscriptions = AddRegionSubscription();
            this.query.regionCode = "FL";
            this.query.scanCodes = subscriptions.Select(s => s.identifier).ToList();

            List<string> irmaItemSubscriptions = this.context.IRMAItemSubscription
                .Where(s => this.query.scanCodes.Contains(s.identifier) && s.regioncode == this.query.regionCode && s.deleteDate == null)
                .Select(s => s.identifier)
                .ToList();

            if (irmaItemSubscriptions.Count > 0)
            {
                this.query.scanCodes.Add("test");
            }

            List<string> expected = this.query.scanCodes.Except(irmaItemSubscriptions).ToList();

            // When
            List<string> actual = this.handler.Execute(this.query);

            // Then
            Assert.AreEqual(expected.Count, actual.Count);
        }

        private List<IRMAItemSubscription> AddRegionSubscription()
        {
            List<IRMAItemSubscription> subscriptions = new List<IRMAItemSubscription>();

            for (int i = 0; i < 3; i++)
            {
                IRMAItemSubscription subscription = new IRMAItemSubscription
                {
                    identifier = "77777777777" + i.ToString(),
                    regioncode = "FL",
                    insertDate = DateTime.Now
                };

                subscriptions.Add(subscription);
            }

            return subscriptions;
        }
    }
}
