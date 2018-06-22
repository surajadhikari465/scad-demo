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

                Assert.AreEqual(irmaItemEntry.defaultIdentifier, actualIrmaItem.defaultIdentifier,
                    $"actual {nameof(actualIrmaItem.defaultIdentifier)} ({actualIrmaItem.defaultIdentifier}) did not match expected ({irmaItemEntry.defaultIdentifier})");
                Assert.AreEqual(irmaItemEntry.brandName, actualIrmaItem.brandName,
                    $"actual {nameof(actualIrmaItem.brandName)} ({actualIrmaItem.brandName}) did not match expected ({irmaItemEntry.brandName})");
                Assert.AreEqual(irmaItemEntry.itemDescription, actualIrmaItem.itemDescription,
                    $"actual {nameof(actualIrmaItem.itemDescription)} ({actualIrmaItem.itemDescription}) did not match expected ({irmaItemEntry.itemDescription})");
                Assert.AreEqual(irmaItemEntry.posDescription, actualIrmaItem.posDescription,
                    $"actual {nameof(actualIrmaItem.posDescription)} ({actualIrmaItem.posDescription}) did not match expected ({irmaItemEntry.posDescription})");
                Assert.AreEqual(irmaItemEntry.packageUnit, actualIrmaItem.packageUnit,
                    $"actual {nameof(actualIrmaItem.packageUnit)} ({actualIrmaItem.packageUnit}) did not match expected ({irmaItemEntry.packageUnit})");
                Assert.AreEqual(irmaItemEntry.retailSize, actualIrmaItem.retailSize,
                    $"actual {nameof(actualIrmaItem.retailSize)} ({actualIrmaItem.retailSize}) did not match expected ({irmaItemEntry.retailSize})");
                Assert.AreEqual(irmaItemEntry.retailUom, actualIrmaItem.retailUom,
                    $"actual {nameof(actualIrmaItem.retailUom)} ({actualIrmaItem.retailUom}) did not match expected ({irmaItemEntry.retailUom})");
                Assert.AreEqual(irmaItemEntry.foodStamp, actualIrmaItem.foodStamp,
                    $"actual {nameof(actualIrmaItem.foodStamp)} ({actualIrmaItem.foodStamp}) did not match expected ({irmaItemEntry.foodStamp})");
                Assert.AreEqual(Math.Round(irmaItemEntry.posScaleTare), actualIrmaItem.posScaleTare,
                    $"actual {nameof(actualIrmaItem.posScaleTare)} ({actualIrmaItem.posScaleTare}) did not match expected ({irmaItemEntry.posScaleTare})"); //PosScaleTare is stored as a decimal(18, 0) so it will not allow decimals
                Assert.AreEqual(irmaItemEntry.departmentSale, actualIrmaItem.departmentSale,
                    $"actual {nameof(actualIrmaItem.departmentSale)} ({actualIrmaItem.departmentSale}) did not match expected ({irmaItemEntry.departmentSale})");
                Assert.AreEqual(irmaItemEntry.giftCard, actualIrmaItem.giftCard,
                    $"actual {nameof(actualIrmaItem.giftCard)} ({actualIrmaItem.giftCard}) did not match expected ({irmaItemEntry.giftCard})");
                Assert.AreEqual(irmaItemEntry.taxClassID, actualIrmaItem.taxClassID,
                    $"actual {nameof(actualIrmaItem.taxClassID)} ({actualIrmaItem.taxClassID}) did not match expected ({irmaItemEntry.taxClassID})");
                Assert.AreEqual(irmaItemEntry.merchandiseClassID, actualIrmaItem.merchandiseClassID,
                    $"actual {nameof(actualIrmaItem.merchandiseClassID)} ({actualIrmaItem.merchandiseClassID}) did not match expected ({irmaItemEntry.merchandiseClassID})");
                Assert.IsTrue(irmaItemEntry.insertDate <= actualIrmaItem.insertDate,
                    $"actual {nameof(actualIrmaItem.insertDate)} ({actualIrmaItem.insertDate}) was not less than or equal to ({irmaItemEntry.insertDate})");
                Assert.AreEqual(irmaItemEntry.irmaSubTeamName, actualIrmaItem.irmaSubTeamName,
                    $"actual {nameof(actualIrmaItem.irmaSubTeamName)} ({actualIrmaItem.irmaSubTeamName}) did not match expected ({irmaItemEntry.irmaSubTeamName})");
                Assert.AreEqual(irmaItemEntry.nationalClassID, actualIrmaItem.nationalClassID,
                    $"actual {nameof(actualIrmaItem.nationalClassID)} ({actualIrmaItem.nationalClassID}) did not match expected ({irmaItemEntry.nationalClassID})");              
            }
        }
    }
}
