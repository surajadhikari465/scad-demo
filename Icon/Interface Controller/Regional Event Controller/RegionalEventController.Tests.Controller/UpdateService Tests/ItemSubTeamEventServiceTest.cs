using Icon.Framework;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RegionalEventController.Controller.ProcessorModules;
using RegionalEventController.Controller.UpdateServices;
using RegionalEventController.DataAccess.Commands;
using RegionalEventController.DataAccess.Interfaces;
using RegionalEventController.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegionalEventController.Tests.Controller.UpdateService_Tests
{
    [TestClass]
    public class ItemSubTeamEventServiceTest
    {
        private IconContext context;
        private ItemSubTeamEventService itemSubTeamEventService;
        private Mock<ILogger<ItemSubTeamEventService>> mockServiceLogger;
        private InsertEventQueuesToIconBulkCommandHandler insertEventQueuesToIconBulkCommandHandler;
        private Mock<IQueryHandler<GetIconIrmaItemsBulkQuery, Dictionary<string, int>>> getIconIrmaItemsBulkQueryHandler; 
        private NewItemProcessingModule newItemProcessingModule;
        private List<RegionalEventController.DataAccess.Models.IrmaNewItem> testIrmaNewItems;
        private List<string> cleanupScripts;
        private string regionCode = "FL";

        [TestInitialize]
        public void Initialize()
        {
            this.context = new IconContext();

            this.mockServiceLogger = new Mock<ILogger<ItemSubTeamEventService>>();
            this.insertEventQueuesToIconBulkCommandHandler = new InsertEventQueuesToIconBulkCommandHandler(new Mock<ILogger<InsertEventQueuesToIconBulkCommandHandler>>().Object, context);
            getIconIrmaItemsBulkQueryHandler = new Mock<IQueryHandler<GetIconIrmaItemsBulkQuery, Dictionary<string, int>>>();
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
            }.ConvertAll(n => new DataAccess.Models.IrmaNewItem
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
        public void UpdateBulk_ItemsInIcon_ItemSubTeamEventsAreCreated()
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

                string sql = @"delete app.EventQueue 
                           where EventMessage in " + testIdentifiersToString + " and RegionCode = '" + testIrmaNewItems[0].RegionCode + "'";
                int returnCode = context.Database.ExecuteSqlCommand(sql);

                cleanupScripts.Add(sql);
                sql = @"delete app.IRMAItemSubscription
                           where identifier in " + testIdentifiersToString + " and RegionCode = '" + testIrmaNewItems[0].RegionCode + "'";
                returnCode = context.Database.ExecuteSqlCommand(sql);

                cleanupScripts.Add(sql);
            }
            Dictionary<string, int> iconItems = testIrmaNewItems.ToDictionary(i => i.Identifier, i => i.IconItemId);
            getIconIrmaItemsBulkQueryHandler.Setup(e => e.Execute(It.IsAny<GetIconIrmaItemsBulkQuery>())).Returns(iconItems);

            this.itemSubTeamEventService = new ItemSubTeamEventService(
                mockServiceLogger.Object,
                context,
                testIrmaNewItems,
                getIconIrmaItemsBulkQueryHandler.Object,
                insertEventQueuesToIconBulkCommandHandler,
                newItemProcessingModule,
                regionCode);

            // When
            this.itemSubTeamEventService.UpdateBulk();

            // Then
            var eventQueueCtr = context.EventQueue.Where(eq => testIdentifiers.Contains(eq.EventMessage) && eq.EventId == EventTypes.ItemSubTeamUpdate).ToList().Count();
            
            Assert.AreEqual(testIrmaNewItems.Count(), eventQueueCtr);
        }

        [TestMethod]
        public void UpdateRowByRow_ItemsInIcon_ItemSubTeamEventsAreCreated()
        {
            // Given.
            StageValidatedItems();
            var testIdentifiers = testIrmaNewItems.Select(i => i.Identifier).ToList();

            var testIdentifiersStringBuilder = new StringBuilder();
            foreach (string testIdentifier in testIdentifiers)
            {
                testIdentifiersStringBuilder.Append("'" + testIdentifier + "',");
            }

            foreach (var irmaItem in testIrmaNewItems)
            {
                var item = context.Item.Add(new Item
                {
                    itemTypeID = ItemTypes.RetailSale,
                    ScanCode = new List<ScanCode> { new ScanCode { scanCode = irmaItem.Identifier, scanCodeTypeID = ScanCodeTypes.Upc } }
                });
                context.SaveChanges();

                cleanupScripts.Add(@"delete dbo.ScanCode where itemID = " + item.itemID);
                cleanupScripts.Add(@"delete dbo.Item where itemID = " + item.itemID);
            }

            // --Delete EventQueue and IRMAItemSubscription records that may match the randomly generated records.
            string testIdentifiersToString = testIdentifiersStringBuilder.ToString();
            if (testIdentifiersToString.Length > 0)
            {
                testIdentifiersToString = "(" + testIdentifiersToString.Remove(testIdentifiersStringBuilder.Length - 1, 1) + ")";

                string sql = @"delete app.EventQueue 
                           where EventMessage in " + testIdentifiersToString + " and RegionCode = '" + testIrmaNewItems[0].RegionCode + "'";
                int returnCode = context.Database.ExecuteSqlCommand(sql);

                cleanupScripts.Add(sql);
                sql = @"delete app.IRMAItemSubscription
                           where identifier in " + testIdentifiersToString + " and RegionCode = '" + testIrmaNewItems[0].RegionCode + "'";
                returnCode = context.Database.ExecuteSqlCommand(sql);

                cleanupScripts.Add(sql);               
            }

            this.itemSubTeamEventService = new ItemSubTeamEventService(
                mockServiceLogger.Object,
                context,
                testIrmaNewItems,
                getIconIrmaItemsBulkQueryHandler.Object,
                insertEventQueuesToIconBulkCommandHandler,
                newItemProcessingModule,
                regionCode);

            // When
            this.itemSubTeamEventService.UpdateRowByRow();

            // Then
            var eventQueueCtr = context.EventQueue
                .AsNoTracking()
                .Where(eq => testIdentifiers.Contains(eq.EventMessage) && eq.EventId == EventTypes.ItemSubTeamUpdate)
                .Count();            
            Assert.AreEqual(testIrmaNewItems.Count(), eventQueueCtr);            
        }

        [TestMethod]
        public void UpdateRowByRow_ItemNotInIcon_NoItemSubTeamEventsShouldBeCreated()
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

                string sql = @"delete app.EventQueue 
                           where EventMessage in " + testIdentifiersToString + " and RegionCode = '" + testIrmaNewItems[0].RegionCode + "'";
                int returnCode = context.Database.ExecuteSqlCommand(sql);

                cleanupScripts.Add(sql);

                sql = @"delete app.IRMAItemSubscription
                           where identifier in " + testIdentifiersToString + " and RegionCode = '" + testIrmaNewItems[0].RegionCode + "'";
                returnCode = context.Database.ExecuteSqlCommand(sql);

                cleanupScripts.Add(sql);
            }
            Dictionary<string, int> iconItems = new Dictionary<string, int>();
            getIconIrmaItemsBulkQueryHandler.Setup(e => e.Execute(It.IsAny<GetIconIrmaItemsBulkQuery>())).Returns(iconItems);

           
            this.itemSubTeamEventService = new ItemSubTeamEventService(
                mockServiceLogger.Object,
                context,
                testIrmaNewItems,
                getIconIrmaItemsBulkQueryHandler.Object,
                insertEventQueuesToIconBulkCommandHandler,
                newItemProcessingModule,
                regionCode);

            // When
            this.itemSubTeamEventService.UpdateRowByRow();

            // Then
            var eventQueueCtr = context.EventQueue.Where(eq => testIdentifiers.Contains(eq.EventMessage) && eq.EventId == EventTypes.ItemSubTeamUpdate).ToList().Count();
            
            Assert.AreEqual(0, eventQueueCtr);
        }
               
    }
}
