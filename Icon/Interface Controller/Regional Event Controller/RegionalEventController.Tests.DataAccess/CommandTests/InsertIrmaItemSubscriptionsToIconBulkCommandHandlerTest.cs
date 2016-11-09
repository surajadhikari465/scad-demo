using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Framework;
using RegionalEventController.DataAccess.Commands;
using RegionalEventController.DataAccess.Models;
using Moq;
using Icon.Logging;
using Icon.Testing.Builders;
using RegionalEventController.Common;

namespace RegionalEventController.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class InsertIrmaItemSubscriptionsToIconBulkCommandHandlerTest
    {
        private Mock<ILogger<InsertIrmaItemSubscriptionsToIconBulkCommandHandler>> mockLogger;
        private IconContext context;
        private InsertIrmaItemSubscriptionsToIconBulkCommand command;
        private InsertIrmaItemSubscriptionsToIconBulkCommandHandler handler;
        private List<IRMAItemSubscription> irmaItemSubscriptionEntries;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void InitializeData()
        {
            this.mockLogger = new Mock<ILogger<InsertIrmaItemSubscriptionsToIconBulkCommandHandler>>();
            this.context = new IconContext(); 
            this.command = new InsertIrmaItemSubscriptionsToIconBulkCommand();
            this.handler = new InsertIrmaItemSubscriptionsToIconBulkCommandHandler(this.mockLogger.Object, this.context);
            this.irmaItemSubscriptionEntries = new List<IRMAItemSubscription>();

            this.transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void CleanupData()
        {
            if (transaction != null)
            {
                this.transaction.Rollback();
            }
        }

        private void BuildIActivermaItemSubscriptionEntries()
        {
            var random = new Random();
            irmaItemSubscriptionEntries.Add(new TestIrmaItemSubscriptionBuilder()
                .WithIdentifier(random.Next(1000000, 1000000000).ToString())
                .WithRegionCode("MW").Build());
            irmaItemSubscriptionEntries.Add(new TestIrmaItemSubscriptionBuilder()
                .WithIdentifier(random.Next(1000000, 1000000000).ToString())
                .WithRegionCode("MW").Build());
            irmaItemSubscriptionEntries.Add(new TestIrmaItemSubscriptionBuilder()
                .WithIdentifier(random.Next(1000000, 1000000000).ToString())
                .WithRegionCode("MW").Build());
        }
        private void BuildInactiveIrmaItemSubscriptionEntries()
        {
            var random = new Random();
            irmaItemSubscriptionEntries.Add(new TestIrmaItemSubscriptionBuilder()
                .WithIdentifier(random.Next(1000000, 1000000000).ToString())
                .WithRegionCode("MW")
                .WithDeleteDate(DateTime.Today)
                .Build());
            irmaItemSubscriptionEntries.Add(new TestIrmaItemSubscriptionBuilder()
                .WithIdentifier(random.Next(1000000, 1000000000).ToString())
                .WithRegionCode("MW")
                .WithDeleteDate(DateTime.Today)
                .Build());
            irmaItemSubscriptionEntries.Add(new TestIrmaItemSubscriptionBuilder()
                .WithIdentifier(random.Next(1000000, 1000000000).ToString())
                .WithRegionCode("MW")
                .WithDeleteDate(DateTime.Today)
                .Build());
        }
        [TestMethod]
        public void InsertIrmaItemSubscriptionsToIcon_IrmaItemSubscriptionsDoNotExist_AllIrmaItemSubscriptionsAreInserted()
        {
            // Given
            BuildIActivermaItemSubscriptionEntries();
            command.IrmaNewItemSubscriptions = irmaItemSubscriptionEntries;

            foreach (IRMAItemSubscription irmaItemSubscriptionEntry in irmaItemSubscriptionEntries)
            {
                IRMAItemSubscription toBeDeletedSubscription = context.IRMAItemSubscription.Where(s => s.identifier == irmaItemSubscriptionEntry.identifier && s.regioncode == irmaItemSubscriptionEntry.regioncode && s.deleteDate == null).FirstOrDefault();
                if (toBeDeletedSubscription != null)
                {
                    context.IRMAItemSubscription.Remove(toBeDeletedSubscription);
                    context.SaveChanges();
                }
            }
            // When
            this.handler.Execute(this.command);

            // Then
            foreach (IRMAItemSubscription irmaItemSubscriptionEntry in irmaItemSubscriptionEntries)
            {
                Assert.IsTrue(context.IRMAItemSubscription.Any(ii => ii.identifier == irmaItemSubscriptionEntry.identifier && ii.regioncode == irmaItemSubscriptionEntry.regioncode && ii.deleteDate == null));
            }
        }

        [TestMethod]
        public void InsertIrmaItemSubscriptionsToIcon_InactiveIrmaItemSubscriptionsAlreadyExist_AllActiveIrmaItemSubscriptionAreInserted()
        {
            // Given
            BuildInactiveIrmaItemSubscriptionEntries();
            command.IrmaNewItemSubscriptions = irmaItemSubscriptionEntries;

            context.IRMAItemSubscription.AddRange(irmaItemSubscriptionEntries);
            context.SaveChanges();

            // When
            this.handler.Execute(this.command);

            // Then
            foreach (IRMAItemSubscription irmaItemSubscriptionEntry in irmaItemSubscriptionEntries)
            {
                Assert.IsTrue(context.IRMAItemSubscription.Any(ii => ii.identifier == irmaItemSubscriptionEntry.identifier && ii.regioncode == irmaItemSubscriptionEntry.regioncode && ii.deleteDate == null));
            }
        }
    }
}
