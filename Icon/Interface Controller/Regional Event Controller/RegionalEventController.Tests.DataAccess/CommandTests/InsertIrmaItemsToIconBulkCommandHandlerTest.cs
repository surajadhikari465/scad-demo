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
    public class InsertIrmaItemsToIconBulkCommandHandlerTest
    {
        private InsertIrmaItemsToIconBulkCommandHandler handler;
        private InsertIrmaItemsToIconBulkCommand command;
        private Mock<ILogger<InsertIrmaItemsToIconBulkCommandHandler>> mockLogger;
        private IconContext context;
        private DbContextTransaction transaction;
        private List<IRMAItem> irmaItemEntries;

        [TestInitialize]
        public void InitializeData()
        {
            this.mockLogger = new Mock<ILogger<InsertIrmaItemsToIconBulkCommandHandler>>();
            this.context = new IconContext(); 
            this.command = new InsertIrmaItemsToIconBulkCommand();
            this.handler = new InsertIrmaItemsToIconBulkCommandHandler(this.mockLogger.Object, this.context);
            this.irmaItemEntries = new List<IRMAItem>();

            StartupOptions.Instance = 123;
            this.transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void CleanupData()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        private void BuildIrmaItemEntries()
        {
            var random = new Random();
            irmaItemEntries.Add(new TestIrmaItemBuilder()
                .WithIdentifier("1112223334".ToString())
                .WithRegionCode("MW").Build());
            irmaItemEntries.Add(new TestIrmaItemBuilder()
                .WithIdentifier("1112223335".ToString())
                .WithRegionCode("MW").Build());
            irmaItemEntries.Add(new TestIrmaItemBuilder()
                .WithIdentifier("1112223336".ToString())
                .WithRegionCode("MW").WithOrganicAgencyId(11127).Build());
        }

        [TestMethod]
        public void InsertIrmaItemsToIcon_IrmaItemsAreReady_AllIrmaItemsAreInserted()
        {
            // Given
            BuildIrmaItemEntries();
            command.irmaNewItems = irmaItemEntries;

            // When
            this.handler.Execute(this.command);

            // Then
            foreach (IRMAItem irmaItemEntry in irmaItemEntries)
            {
                var actualIrmaItem = context.IRMAItem.Single(ii => ii.identifier == irmaItemEntry.identifier && ii.regioncode == irmaItemEntry.regioncode);

                Assert.AreEqual(irmaItemEntry.defaultIdentifier, actualIrmaItem.defaultIdentifier);
                Assert.AreEqual(irmaItemEntry.brandName, actualIrmaItem.brandName);
                Assert.AreEqual(irmaItemEntry.itemDescription, actualIrmaItem.itemDescription);
                Assert.AreEqual(irmaItemEntry.posDescription, actualIrmaItem.posDescription);
                Assert.AreEqual(irmaItemEntry.packageUnit, actualIrmaItem.packageUnit);
                Assert.AreEqual(irmaItemEntry.retailSize, actualIrmaItem.retailSize);
                Assert.AreEqual(irmaItemEntry.retailUom, actualIrmaItem.retailUom);
                Assert.AreEqual(irmaItemEntry.foodStamp, actualIrmaItem.foodStamp);
                Assert.AreEqual(Math.Round(irmaItemEntry.posScaleTare), actualIrmaItem.posScaleTare); //PosScaleTare is stored as a decimal(18, 0) so it will not allow decimals
                Assert.AreEqual(irmaItemEntry.departmentSale, actualIrmaItem.departmentSale);
                Assert.AreEqual(irmaItemEntry.giftCard, actualIrmaItem.giftCard);
                Assert.AreEqual(irmaItemEntry.taxClassID, actualIrmaItem.taxClassID);
                Assert.AreEqual(irmaItemEntry.merchandiseClassID, actualIrmaItem.merchandiseClassID);
                Assert.IsTrue(irmaItemEntry.insertDate <= actualIrmaItem.insertDate);
                Assert.AreEqual(irmaItemEntry.irmaSubTeamName, actualIrmaItem.irmaSubTeamName);
                Assert.AreEqual(irmaItemEntry.nationalClassID, actualIrmaItem.nationalClassID);
                Assert.AreEqual(irmaItemEntry.OrganicAgencyId, actualIrmaItem.OrganicAgencyId);
            }
        }
    }
}
