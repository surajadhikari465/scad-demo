using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GlobalEventController.DataAccess.BulkCommands;
using Irma.Framework;
using GlobalEventController.Common;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Icon.Framework;

namespace GlobalEventController.Tests.DataAccess.BulkCommandTests
{
    [TestClass]
    public class BulkAddMammothItemLocaleEventsCommandHandlerTests
    {
        private BulkAddMammothItemLocaleEventsCommandHandler commandHandler;
        private BulkAddMammothItemLocaleEventsCommand command;
        private IrmaContext context;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void Initialize()
        {
            this.context = new IrmaContext();
            this.transaction = context.Database.BeginTransaction();
            this.commandHandler = new BulkAddMammothItemLocaleEventsCommandHandler(context);
            this.command = new BulkAddMammothItemLocaleEventsCommand { ValidatedItems = new List<ValidatedItemModel>() };
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void BulkAddMammothItemLocaleEvents_ScanCodeExistsInIrma_ShouldAddMammothEventsForEveryItem()
        {
            //Given
            var items = context.ItemIdentifier.Where(ii => ii.Deleted_Identifier == 0 && ii.Remove_Identifier == 0)
                .Select(ii => new { ii.Identifier, ii.Item_Key })
                .Take(3)
                .ToList();

            var validatedItems = items.Select(sc => new ValidatedItemModel { ScanCode = sc.Identifier, EventTypeId = EventTypes.ItemValidation }).ToList();
            command.ValidatedItems.AddRange(validatedItems);

            //When
            commandHandler.Handle(command);

            //Then
            var expectedNumberOfEvents = items.Count;
            var identifiers = items.Select(i => i.Identifier);
            var itemKeys = items.Select(i => i.Item_Key);
            var events = context.MammothItemLocaleChangeQueue.Where(e => identifiers.Any(i => i == e.Identifier)).ToList();
            Assert.AreEqual(expectedNumberOfEvents, events.Count);

            foreach (var e in events)
            {
                Assert.IsNull(e.Store_No);
                Assert.IsTrue(identifiers.Any(i => i == e.Identifier));
                Assert.IsTrue(itemKeys.Any(i => i == e.Item_Key));
                Assert.AreEqual(1, e.EventTypeID);
            }
        }

        [TestMethod]
        public void BulkAddMammothItemLocaleEvents_EventTypesOtherThanValidatedItemAndNewIrmaItem_ShouldOnlyGenerateEventsForValidatedItemAndNewIrmaItem()
        {
            var items = context.ItemIdentifier.Where(ii => ii.Deleted_Identifier == 0 && ii.Remove_Identifier == 0)
                .Select(ii => new { ii.Identifier, ii.Item_Key })
                .Take(3)
                .ToList();

            var validatedItems = items.Select(sc => new ValidatedItemModel { ScanCode = sc.Identifier }).ToList();
            validatedItems[0].EventTypeId = EventTypes.ItemValidation;
            validatedItems[1].EventTypeId = EventTypes.NewIrmaItem;
            validatedItems[2].EventTypeId = EventTypes.ItemUpdate;
            command.ValidatedItems.AddRange(validatedItems);

            //When
            commandHandler.Handle(command);

            //Then
            var expectedNumberOfEvents = 2; // the itemlocalecontroller will make them per store -> esb.messagequeueitemlocale
            var identifiers = items.Select(i => i.Identifier);
            var itemKeys = items.Select(i => i.Item_Key);
            var events = context.MammothItemLocaleChangeQueue.Where(e => identifiers.Any(i => i == e.Identifier)).ToList();
            Assert.AreEqual(expectedNumberOfEvents, events.Count);

            foreach (var e in events)
            {
                Assert.IsNull(e.Store_No); // store should be null.
                Assert.IsTrue(identifiers.Any(i => i == e.Identifier));
                Assert.IsTrue(itemKeys.Any(i => i == e.Item_Key));
                Assert.IsTrue(e.EventTypeID == EventTypes.ItemValidation || e.EventTypeID == EventTypes.NewIrmaItem);
            }
        }
    }
}
