using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PushController.DataAccess.Commands;
using System.Collections.Generic;
using System.Data.Entity;

namespace PushController.Tests.DataAccess.Commands
{
    [TestClass]
    public class DeleteItemSubscriptionCommandTests
    {
        private DeleteItemSubscriptionCommandHandler commandHandler;
        private GlobalIconContext globalContext;
        private DbContextTransaction transaction;
        private List<string> testScanCodes;

        [TestInitialize]
        public void Initialize()
        {
            globalContext = new GlobalIconContext(new IconContext());
            commandHandler = new DeleteItemSubscriptionCommandHandler(new Mock<ILogger<DeleteItemSubscriptionCommandHandler>>().Object, globalContext);

            testScanCodes = new List<string>
            {
                "22222220",
                "22222221",
                "22222222",
            };

            transaction = globalContext.Context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void StageIrmaItemSubscriptions(List<IRMAItemSubscription> subscriptions)
        {
            globalContext.Context.IRMAItemSubscription.AddRange(subscriptions);
            globalContext.Context.SaveChanges();
        }

        [TestMethod]
        public void DeleteItemSubscription_ThreeSubscriptionsToBeDeleted_ThreeSubscriptionsShouldBeDeleted()
        {
            // Given.
            var subscriptions = new List<IRMAItemSubscription>
            {
                new TestIrmaItemSubscriptionBuilder().WithRegionCode("SP").WithIdentifier(testScanCodes[0]),
                new TestIrmaItemSubscriptionBuilder().WithRegionCode("NC").WithIdentifier(testScanCodes[1]),
                new TestIrmaItemSubscriptionBuilder().WithRegionCode("SW").WithIdentifier(testScanCodes[2])
            };

            StageIrmaItemSubscriptions(subscriptions);

            var command = new DeleteItemSubscriptionCommand
            {
                Subscriptions = subscriptions
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            foreach (var subscription in subscriptions)
            {
                globalContext.Context.Entry(subscription).Reload();
            }

            bool allSubscriptionsAreDeleted = subscriptions.TrueForAll(s => s.deleteDate != null);

            Assert.IsTrue(allSubscriptionsAreDeleted);
        }
    }
}
