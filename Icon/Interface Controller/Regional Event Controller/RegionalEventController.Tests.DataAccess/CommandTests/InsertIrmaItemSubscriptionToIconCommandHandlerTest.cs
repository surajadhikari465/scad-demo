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
    public class InsertIrmaItemSubscriptionToIconCommandHandlerTest
    {
        private Mock<ILogger<InsertIrmaItemSubscriptionToIconCommandHandler>> mockLogger;
        private IconContext context;
        private InsertIrmaItemSubscriptionToIconCommand command;
        private InsertIrmaItemSubscriptionToIconCommandHandler handler;
        private IRMAItemSubscription irmaItemSubscriptionEntry;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void InitializeData()
        {
            this.mockLogger = new Mock<ILogger<InsertIrmaItemSubscriptionToIconCommandHandler>>();
            this.context = new IconContext();
            this.command = new InsertIrmaItemSubscriptionToIconCommand();
            this.handler = new InsertIrmaItemSubscriptionToIconCommandHandler(this.mockLogger.Object, this.context);
            this.irmaItemSubscriptionEntry = new IRMAItemSubscription();

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

        private void BuildActiveIrmaItemSubscriptionEntry()
        {
            var random = new Random();
            irmaItemSubscriptionEntry = new TestIrmaItemSubscriptionBuilder()
                .WithIdentifier(random.Next(1000000, 1000000000).ToString())
                .WithRegionCode("MW")
                .Build();
        }

        private void BuildInactiveIrmaItemSubscriptionEntry()
        {
            var random = new Random();
            irmaItemSubscriptionEntry = new TestIrmaItemSubscriptionBuilder()
                .WithIdentifier(random.Next(1000000, 1000000000).ToString())
                .WithRegionCode("MW")
                .WithDeleteDate(DateTime.Today)
                .Build();
        }

        [TestMethod]
        public void InsertIrmaItemSubscriptionToIcon_IrmaItemSubscriptionDoesNotExist_IrmaItemSubscriptionIsInsertedAsActive()
        {
            // Given
            BuildActiveIrmaItemSubscriptionEntry();
            command.irmaNewItemSubscription = irmaItemSubscriptionEntry;

            IRMAItemSubscription toBeDeletedSubscription = context.IRMAItemSubscription.Where(s => s.identifier == irmaItemSubscriptionEntry.identifier && s.regioncode == irmaItemSubscriptionEntry.regioncode).FirstOrDefault();
            if (toBeDeletedSubscription != null)
            {
                context.IRMAItemSubscription.Remove(toBeDeletedSubscription);
                context.SaveChanges();
            }
            // When
            this.handler.Execute(this.command);

            // Then
            Assert.IsTrue(context.IRMAItemSubscription.Any(ii => ii.identifier == irmaItemSubscriptionEntry.identifier && ii.regioncode == irmaItemSubscriptionEntry.regioncode && ii.deleteDate == null));
        }

        [TestMethod]
        public void InsertIrmaItemSubscriptionToIcon_InactiveIrmaItemSubscriptionAlreadyExists_AnActiveIrmaItemSubscriptionIsInserted()
        {
            // Given
            BuildInactiveIrmaItemSubscriptionEntry();
            command.irmaNewItemSubscription = irmaItemSubscriptionEntry;

            context.IRMAItemSubscription.Add(irmaItemSubscriptionEntry);
            context.SaveChanges();

            // When
            this.handler.Execute(this.command);

            // Then
            Assert.IsTrue(context.IRMAItemSubscription.Any(ii => ii.identifier == irmaItemSubscriptionEntry.identifier && ii.regioncode == irmaItemSubscriptionEntry.regioncode && ii.deleteDate == null));
        }
    }
}
