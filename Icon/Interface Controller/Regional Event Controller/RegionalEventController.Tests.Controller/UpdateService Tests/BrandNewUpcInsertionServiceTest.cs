using Icon.Framework;
using Icon.Logging;
using Icon.Testing.Builders;
using Icon.Testing.CustomModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RegionalEventController.Controller.ProcessorModules;
using RegionalEventController.Controller.UpdateServices;
using RegionalEventController.DataAccess.Commands;
using RegionalEventController.DataAccess.Interfaces;
using RegionalEventController.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace RegionalEventController.Tests.Controller.UpdateService_Tests
{
    [TestClass]
    public class BrandNewUpcInsertionServiceTest
    {
        private IconContext context;
        private BrandNewUpcInsertionService brandNewUpcInsertionService;
        private Mock<ILogger<BrandNewUpcInsertionService>> mockServiceLogger;
        private InsertIrmaItemsToIconBulkCommandHandler insertIrmaItemsToIconBulkCommandHandler;
        private InsertIrmaItemSubscriptionsToIconBulkCommandHandler insertIrmaItemSubscriptionsToIconBulkCommandHandler;
        private NewItemProcessingModule newItemProcessingModule;
        private List<RegionalEventController.DataAccess.Models.IrmaNewItem> testIrmaNewItems;
        private List<string> cleanupScripts;

        [TestInitialize]
        public void Initialize()
        {
            this.context = new IconContext();

            this.mockServiceLogger = new Mock<ILogger<BrandNewUpcInsertionService>>();
            this.insertIrmaItemsToIconBulkCommandHandler = new InsertIrmaItemsToIconBulkCommandHandler(new Mock<ILogger<InsertIrmaItemsToIconBulkCommandHandler>>().Object, context);
            this.insertIrmaItemSubscriptionsToIconBulkCommandHandler = new InsertIrmaItemSubscriptionsToIconBulkCommandHandler(new Mock<ILogger<InsertIrmaItemSubscriptionsToIconBulkCommandHandler>>().Object, context);
            this.newItemProcessingModule = new NewItemProcessingModule(
                   new Mock<ILogger<NewItemProcessingModule>>().Object,
                   new InsertEventQueueToIconCommandHandler(new Mock<ILogger<InsertEventQueueToIconCommandHandler>>().Object, context),
                   new InsertIrmaItemSubscriptionToIconCommandHandler(new Mock<ILogger<InsertIrmaItemSubscriptionToIconCommandHandler>>().Object, context),
                   new InsertIrmaItemToIconCommandHandler(new Mock<ILogger<InsertIrmaItemToIconCommandHandler>>().Object, context));

            cleanupScripts = new List<string>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            foreach (string sql in cleanupScripts)
            {
                int returnCode = context.Database.ExecuteSqlCommand(sql);
            }
        }

        private void StageValidatedItems()
        {
            var random = new Random();

            this.testIrmaNewItems = new List<Icon.Testing.CustomModels.IrmaNewItem>
            {
                new TestIrmaNewItemBuilder().WithIdentifier(random.Next(1000000, 1000000000).ToString()).Build(),
                new TestIrmaNewItemBuilder().WithIdentifier(random.Next(1000000, 1000000000).ToString()).Build(),
                new TestIrmaNewItemBuilder().WithIdentifier(random.Next(1000000, 1000000000).ToString()).Build()
            }.ConvertAll(n => new RegionalEventController.DataAccess.Models.IrmaNewItem
            {
                QueueId = n.QueueId,
                RegionCode = n.RegionCode,
                ProcessedByController = n.ProcessedByController,
                IrmaTaxClass = n.IrmaTaxClass,
                IrmaItemKey = n.IrmaItemKey,
                Identifier = n.Identifier,
                IconItemId = n.IconItemId,
                IrmaItem = n.IrmaItem
            });
        }

        [TestMethod]
        public void UpdateBulk_BrandNewUPCsUpdateIsSuccessful_IrmaItemAndSubscriptionShouldBeCreated()
        {
            // Given.
            StageValidatedItems();
            var testIdentifiers = testIrmaNewItems.Select(i => i.Identifier).ToList();

            var testIdentifiersStringBuilder = new StringBuilder();
            foreach (string testIdentifier in testIdentifiers)
            {
                testIdentifiersStringBuilder.Append("'" + testIdentifier + "',");
            }

            // --Delete EventQueue and IRMAItemSubscription records that may match the randomly generated records.
            string testIdentifiersToString = testIdentifiersStringBuilder.ToString();
            if (testIdentifiersToString.Length > 0)
            {
                testIdentifiersToString = "(" + testIdentifiersToString.Remove(testIdentifiersStringBuilder.Length - 1, 1) + ")";

                string sql = @"delete app.IRMAItem
                           where identifier in " + testIdentifiersToString;
                int returnCode = context.Database.ExecuteSqlCommand(sql);

                cleanupScripts.Add(sql);

                sql = @"delete app.IRMAItemSubscription
                           where identifier in " + testIdentifiersToString + " and RegionCode = '" + testIrmaNewItems[0].RegionCode + "'";
                returnCode = context.Database.ExecuteSqlCommand(sql);

                cleanupScripts.Add(sql);
            }

            this.brandNewUpcInsertionService = new BrandNewUpcInsertionService(
                mockServiceLogger.Object,
                context,
                testIrmaNewItems,
                insertIrmaItemSubscriptionsToIconBulkCommandHandler,
                insertIrmaItemsToIconBulkCommandHandler,
                newItemProcessingModule);

            // When
            this.brandNewUpcInsertionService.UpdateBulk();

            // Then
            var irmaItemCtr = context.IRMAItem.Where(i => testIdentifiers.Contains(i.identifier)).ToList().Count();
            var subscriptionCtr = context.IRMAItemSubscription.Where(sb => testIdentifiers.Contains(sb.identifier) && sb.deleteDate == null).ToList().Count();

            Assert.AreEqual(testIrmaNewItems.Count(), irmaItemCtr);
            Assert.AreEqual(testIrmaNewItems.Count(), subscriptionCtr);
        }

        [TestMethod]
        public void UpdateRowByRow_BrandNewUPCsUpdateIsSuccessful_IrmaItemAndSubscriptionShouldBeCreated()
        {
            // Given.
            StageValidatedItems();
            var testIdentifiers = testIrmaNewItems.Select(i => i.Identifier).ToList();

            var testIdentifiersStringBuilder = new StringBuilder();
            foreach (string testIdentifier in testIdentifiers)
            {
                testIdentifiersStringBuilder.Append("'" + testIdentifier + "',");
            }

            // --Delete EventQueue and IRMAItemSubscription records that may match the randomly generated records.
            string testIdentifiersToString = testIdentifiersStringBuilder.ToString();
            if (testIdentifiersToString.Length > 0)
            {
                testIdentifiersToString = "(" + testIdentifiersToString.Remove(testIdentifiersStringBuilder.Length - 1, 1) + ")";

                string sql = @"delete app.IRMAItem
                           where identifier in " + testIdentifiersToString;
                int returnCode = context.Database.ExecuteSqlCommand(sql);

                cleanupScripts.Add(sql);

                sql = @"delete app.IRMAItemSubscription
                           where identifier in " + testIdentifiersToString + " and RegionCode = '" + testIrmaNewItems[0].RegionCode + "'";
                returnCode = context.Database.ExecuteSqlCommand(sql);

                cleanupScripts.Add(sql);
            }

            this.brandNewUpcInsertionService = new BrandNewUpcInsertionService(
                mockServiceLogger.Object,
                context,
                testIrmaNewItems,
                insertIrmaItemSubscriptionsToIconBulkCommandHandler,
                insertIrmaItemsToIconBulkCommandHandler,
                newItemProcessingModule);

            // When
            this.brandNewUpcInsertionService.UpdateRowByRow();

            // Then
            var irmaItemCtr = context.IRMAItem.Where(i => testIdentifiers.Contains(i.identifier)).ToList().Count();
            var subscriptionCtr = context.IRMAItemSubscription.Where(sb => testIdentifiers.Contains(sb.identifier) && sb.deleteDate == null).ToList().Count();

            Assert.AreEqual(testIrmaNewItems.Count(), irmaItemCtr);
            Assert.AreEqual(testIrmaNewItems.Count(), subscriptionCtr);
        }

        [TestMethod]
        public void UpdateRowByRow_BrandNewUPCsUpdateFailedForOneEntry_IrmaItemAndSubscriptionShouldBeCreatedForOtherEntries()
        {
            // Given.
            StageValidatedItems();

            var testIdentifiers = testIrmaNewItems.Select(i => i.Identifier).ToList();

            var testIdentifiersStringBuilder = new StringBuilder();
            foreach (string testIdentifier in testIdentifiers)
            {
                testIdentifiersStringBuilder.Append("'" + testIdentifier + "',");
            }

            // --Delete EventQueue and IRMAItemSubscription records that may match the randomly generated records.
            string testIdentifiersToString = testIdentifiersStringBuilder.ToString();
            if (testIdentifiersToString.Length > 0)
            {
                testIdentifiersToString = "(" + testIdentifiersToString.Remove(testIdentifiersStringBuilder.Length - 1, 1) + ")";

                string sql = @"delete app.IRMAItem
                           where identifier in " + testIdentifiersToString;
                int returnCode = context.Database.ExecuteSqlCommand(sql);

                cleanupScripts.Add(sql);

                sql = @"delete app.IRMAItemSubscription
                           where identifier in " + testIdentifiersToString + " and RegionCode = '" + testIrmaNewItems[0].RegionCode + "'";
                returnCode = context.Database.ExecuteSqlCommand(sql);

                cleanupScripts.Add(sql);
            }

            testIrmaNewItems[testIrmaNewItems.Count() - 1].IrmaItem.regioncode = null;

            this.brandNewUpcInsertionService = new BrandNewUpcInsertionService(
                mockServiceLogger.Object,
                context,
                testIrmaNewItems,
                insertIrmaItemSubscriptionsToIconBulkCommandHandler,
                insertIrmaItemsToIconBulkCommandHandler,
                newItemProcessingModule);

            // When
            this.brandNewUpcInsertionService.UpdateRowByRow();

            // Then
            var irmaItemCtr = context.IRMAItem.Where(i => testIdentifiers.Contains(i.identifier)).ToList().Count();
            var subscriptionCtr = context.IRMAItemSubscription.Where(sb => testIdentifiers.Contains(sb.identifier) && sb.deleteDate == null).ToList().Count();

            Assert.AreEqual(irmaItemCtr, testIrmaNewItems.Count() - 1);
            Assert.AreEqual(subscriptionCtr, testIrmaNewItems.Count() - 1);
        }

        [TestMethod]
        public void UpdateRowByRow_BrandNewUPCsUpdateFailedForOneEntry_ErrorShouldBeLogged()
        {
            // Given.
            StageValidatedItems();

            var testIdentifiers = testIrmaNewItems.Select(i => i.Identifier).ToList();

            var testIdentifiersStringBuilder = new StringBuilder();
            foreach (string testIdentifier in testIdentifiers)
            {
                testIdentifiersStringBuilder.Append("'" + testIdentifier + "',");
            }

            // --Delete EventQueue and IRMAItemSubscription records that may match the randomly generated records.
            string testIdentifiersToString = testIdentifiersStringBuilder.ToString();
            if (testIdentifiersToString.Length > 0)
            {
                testIdentifiersToString = "(" + testIdentifiersToString.Remove(testIdentifiersStringBuilder.Length - 1, 1) + ")";

                string sql = @"delete app.IRMAItem
                           where identifier in " + testIdentifiersToString;
                int returnCode = context.Database.ExecuteSqlCommand(sql);

                cleanupScripts.Add(sql);

                sql = @"delete app.IRMAItemSubscription
                           where identifier in " + testIdentifiersToString + " and RegionCode = '" + testIrmaNewItems[0].RegionCode + "'";
                returnCode = context.Database.ExecuteSqlCommand(sql);

                cleanupScripts.Add(sql);
            }

            testIrmaNewItems[testIrmaNewItems.Count() - 1].IrmaItem.regioncode = null;

            this.brandNewUpcInsertionService = new BrandNewUpcInsertionService(
                mockServiceLogger.Object,
                context,
                testIrmaNewItems,
                insertIrmaItemSubscriptionsToIconBulkCommandHandler,
                insertIrmaItemsToIconBulkCommandHandler,
                newItemProcessingModule);

            // When
            this.brandNewUpcInsertionService.UpdateRowByRow();

            // Then
            mockServiceLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
        }
    }
}