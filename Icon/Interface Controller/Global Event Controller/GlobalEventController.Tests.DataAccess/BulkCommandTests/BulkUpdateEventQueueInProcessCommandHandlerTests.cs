using GlobalEventController.DataAccess.BulkCommands;
using GlobalEventController.DataAccess.Infrastructure;
using Icon.Framework;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;

namespace GlobalEventController.Tests.DataAccess.BulkTests
{
    [TestClass]
    public class BulkUpdateEventQueueInProcessCommandHandlerTests
    {
        private static List<int> ItemEventTypes = new List<int>
        {
            EventTypes.ItemUpdate,
            EventTypes.ItemValidation,
            EventTypes.NewIrmaItem
        };

        #region Fields

        private BulkUpdateEventQueueInProcessCommandHandler handler;
        private BulkUpdateEventQueueInProcessCommand command;
        private IconDbContextFactory contextFactory;
        private TransactionScope transaction;
        private IconContext context;
        private List<EventQueue> itemEvents;
        private List<EventQueue> taxEvents;
        private List<EventQueue> brandEvents;
        private List<EventQueue> testEventsAdded;
        private int previouslyExistingEvents = 0;

        #endregion

        [TestInitialize]
        public void Initialize()
        {
            this.transaction = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            });
            this.context = new IconContext();
            this.previouslyExistingEvents = this.context.EventQueue.Count();
            this.contextFactory = new IconDbContextFactory();
            this.handler = new BulkUpdateEventQueueInProcessCommandHandler(contextFactory);

            this.command = new BulkUpdateEventQueueInProcessCommand();
            this.command.RegisteredEventNames = new List<string>();
            this.command.Instance = "test";
            this.command.MaxRows = 30;
            this.testEventsAdded = new List<EventQueue>();

            this.itemEvents = new List<EventQueue>();

            if (this.itemEvents.Count == 0)
            {
                this.testEventsAdded.AddRange(BuildEventQueueWithItemEvents());
            }

            this.brandEvents = this.context.EventQueue.Where(e => e.EventId == EventTypes.BrandNameUpdate).ToList();
            if (this.brandEvents.Count == 0)
            {
                this.testEventsAdded.AddRange(BuildEventQueueWithBrandEvents());
            }

            this.taxEvents = this.context.EventQueue.Where(e => e.EventId == EventTypes.TaxNameUpdate).ToList();
            if (this.taxEvents.Count == 0)
            {
                this.testEventsAdded.AddRange(BuildEventQueueWithTaxEvents());
            }

            this.context.EventQueue.AddRange(this.testEventsAdded);
            this.context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.transaction.Dispose();
        }

        [TestMethod]
        public void BulkUpdateEventQueueInProcessCommand_OnlyBrandNameUpdateEventRegistered_ReturnsOnlyBrandEvents()
        {
            // Given
            this.command.RegisteredEventNames.Add("IconToIrmaBrandNameUpdate");
            int expectedCount = this.context.EventQueue
                .Where(e => e.EventId == EventTypes.BrandNameUpdate && e.ProcessFailedDate == null && String.IsNullOrEmpty(e.InProcessBy))
                .Take(this.command.MaxRows)
                .Count();

            // When
            List<EventQueueCustom> actual = this.handler.Handle(command);

            // Then
            Assert.AreEqual(expectedCount, actual.Count, "The expected brand event queue count did not match the actual brand event queue count");
        }

        [TestMethod]
        public void BulkUpdateEventQueueInProcessCommand_OnlyTaxEventsRegistered_ReturnOnlyTaxEvents()
        {
            // Given
            this.command.RegisteredEventNames.Add("IconToIrmaTaxClassUpdate");
            this.command.RegisteredEventNames.Add("IconToIrmaNewTaxClass");

            int expectedCount = this.context.EventQueue
                .Where(e => e.EventId == EventTypes.TaxNameUpdate || e.EventId == EventTypes.NewTaxHierarchy)
                .Take(this.command.MaxRows)
                .Count();

            // When
            List<EventQueueCustom> actual = this.handler.Handle(command);

            // Then
            Assert.AreEqual(expectedCount, actual.Count, "The expected tax event queue count did not match the actual tax event queue count");
        }

        [TestMethod]
        public void BulkUpdateEventQueueInProcessCommand_OnlyItemEventsRegistered_ReturnOnlyItemTypeEvents()
        {
            // Given
            this.command.RegisteredEventNames.Add("IrmaToIconNewItem");
            this.command.RegisteredEventNames.Add("IconToIrmaItemValidation");
            this.command.RegisteredEventNames.Add("IconToIrmaItemUpdates");

            int expectedCount = this.context.EventQueue.Where(e =>
                    ItemEventTypes.Contains(e.EventId)
                    && e.InProcessBy == null
                    && e.ProcessFailedDate == null)
                .ToList()
                .Take(this.command.MaxRows)
                .Count();

            // When
            List<EventQueueCustom> actual = this.handler.Handle(command);

            // Then
            Assert.AreEqual(expectedCount, actual.Count, "The expected item type event queue count did not match the actual item type event queue count");
        }

        //[TestMethod] //Ignoring test because it might have an indeterminate behavior. Records with specific EventID may already exist.
        public void BulkUpdateEventQueueInProcessCommand_AllTypesOfEvents_ReturnedNumberOfRowsEqualsMaxRows()
        {            
            // Given
            this.command.RegisteredEventNames.Add("IconToIrmaBrandNameUpdate");
            this.command.RegisteredEventNames.Add("IconToIrmaTaxClassUpdate");
            this.command.RegisteredEventNames.Add("IconToIrmaNewTaxClass");
            this.command.RegisteredEventNames.Add("IrmaToIconNewItem");
            this.command.RegisteredEventNames.Add("IconToIrmaItemValidation");
            this.command.RegisteredEventNames.Add("IconToIrmaItemUpdates");

            // When
            List<EventQueueCustom> actual = this.handler.Handle(command);

            // Then
            Assert.AreEqual(this.testEventsAdded.Count, actual.Count, "The returned number of event queue rows did not match the max rows provided by command class");
        }

        [TestMethod]
        public void BulkUpdateEventQueueInProcessCommand_AllTypesOfEvents_ReturnedRowsAreMarkedInProcessWithInstance()
        {
            // Given
            this.command.RegisteredEventNames.Add("IconToIrmaBrandNameUpdate");
            this.command.RegisteredEventNames.Add("IconToIrmaTaxClassUpdate");
            this.command.RegisteredEventNames.Add("IconToIrmaNewTaxClass");
            this.command.RegisteredEventNames.Add("IconToIrmaValidatedNewItems");
            this.command.RegisteredEventNames.Add("IconToIrmaItemValidation");
            this.command.RegisteredEventNames.Add("IconToIrmaItemUpdates");

            // When
            List<EventQueueCustom> actual = this.handler.Handle(command);

            // Then
            bool allEventsMarkedByInstance = actual.All(e => e.InProcessBy == command.Instance);

            Assert.IsTrue(allEventsMarkedByInstance);
        }

        [TestMethod]
        public void BulkUpdateEventQueueInProcessCommand_AllTypesOfEvents_ReturnedRowsDoNotHaveProcessFailedDate()
        {
            // Given
            this.command.RegisteredEventNames.Add("IconToIrmaBrandNameUpdate");
            this.command.RegisteredEventNames.Add("IconToIrmaTaxClassUpdate");
            this.command.RegisteredEventNames.Add("IconToIrmaNewTaxClass");
            this.command.RegisteredEventNames.Add("IconToIrmaValidatedNewItems");
            this.command.RegisteredEventNames.Add("IconToIrmaItemValidation");
            this.command.RegisteredEventNames.Add("IconToIrmaItemUpdates");

            // When
            List<EventQueueCustom> actual = this.handler.Handle(command);

            // Then
            Assert.IsFalse(actual.Any(a => a.ProcessFailedDate != null), "One or more of the returned rows had a ProcessFailedDate");
        }

        #region Private Methods

        private List<EventQueue> BuildEventQueueWithItemEvents()
        {
            List<EventQueue> queuedEvents = new List<EventQueue>();
            queuedEvents.Add(new TestEventQueueBuilder()
                .WithEventId(EventTypes.ItemUpdate).WithEventMessage("41341341341").WithRegionCode("FL").Build());
            queuedEvents.Add(new TestEventQueueBuilder()
                .WithEventId(EventTypes.ItemValidation).WithEventMessage("32165432165").WithRegionCode("FL").Build());
            queuedEvents.Add(new TestEventQueueBuilder()
                .WithEventId(EventTypes.NewIrmaItem).WithEventMessage("98765498765").WithRegionCode("MA").Build());
            queuedEvents.Add(new TestEventQueueBuilder()
                .WithEventId(EventTypes.NewIrmaItem).WithEventMessage("98765498765").WithRegionCode("PN").Build());
            queuedEvents.Add(new TestEventQueueBuilder()
                .WithEventId(EventTypes.ItemValidation).WithEventMessage("14725836914").WithRegionCode("MA").Build());
            queuedEvents.Add(new TestEventQueueBuilder()
                .WithEventId(EventTypes.ItemValidation).WithEventMessage("14725836914").WithRegionCode("PN").Build());
            queuedEvents.Add(new TestEventQueueBuilder()
                .WithEventId(EventTypes.ItemValidation).WithEventMessage("36925814725").WithRegionCode("FL").Build());

            return queuedEvents;
        }

        private List<EventQueue> BuildEventQueueWithBrandEvents()
        {
            List<EventQueue> eventQueue = new List<EventQueue>();
            eventQueue.Add(new TestEventQueueBuilder().WithEventId(EventTypes.BrandNameUpdate).WithEventReferenceId(1).WithEventMessage("TestBrand1").WithRegionCode("SW").Build());
            eventQueue.Add(new TestEventQueueBuilder().WithEventId(EventTypes.BrandNameUpdate).WithEventReferenceId(1).WithEventMessage("TestBrand1").WithRegionCode("FL").Build());
            eventQueue.Add(new TestEventQueueBuilder().WithEventId(EventTypes.BrandNameUpdate).WithEventReferenceId(2).WithEventMessage("TestBrand2").WithRegionCode("MA").Build());

            return eventQueue;
        }

        private List<EventQueue> BuildEventQueueWithTaxEvents()
        {
            List<EventQueue> eventQueue = new List<EventQueue>();
            eventQueue.Add(new TestEventQueueBuilder().WithEventId(EventTypes.TaxNameUpdate).WithEventMessage("5552222").WithEventMessage("TestTax1").WithRegionCode("SW").Build());
            eventQueue.Add(new TestEventQueueBuilder().WithEventId(EventTypes.TaxNameUpdate).WithEventMessage("5552222").WithEventMessage("TestTax1").WithRegionCode("MW").Build());
            eventQueue.Add(new TestEventQueueBuilder().WithEventId(EventTypes.NewTaxHierarchy).WithEventMessage("4444444").WithEventMessage("TestTax2").WithRegionCode("FL").Build());

            return eventQueue;
        }

        #endregion
    }
}
