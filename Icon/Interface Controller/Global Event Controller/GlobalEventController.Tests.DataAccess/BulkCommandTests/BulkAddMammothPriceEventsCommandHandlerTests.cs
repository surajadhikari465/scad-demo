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
    public class BulkAddMammothPriceEventsCommandHandlerTests
    {
        private BulkAddMammothPriceEventsCommandHandler commandHandler;
        private BulkAddMammothPriceEventsCommand command;
        private IrmaContext context;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void Initialize()
        {
            this.context = new IrmaContext();
            this.transaction = context.Database.BeginTransaction();
            this.commandHandler = new BulkAddMammothPriceEventsCommandHandler(context);
            this.command = new BulkAddMammothPriceEventsCommand { ValidatedItems = new List<ValidatedItemModel>() };
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void BulkAddMammothPriceEvents_ScanCodeExistsInIrmaAsAlternateIdentifier_ShouldAddMammothEventsForEveryValidStoreInIrma()
        {
            //Given
            //Exclude labs and closed stores defined in the app config in IRMA
            var invalidStoreNo = (from v in context.AppConfigValue
                                  join a in context.AppConfigApp on v.ApplicationID equals a.ApplicationID
                                  join e in context.AppConfigEnv on v.EnvironmentID equals e.EnvironmentID
                                  join k in context.AppConfigKey on v.KeyID equals k.KeyID
                                  where (!v.Deleted
                                     && a.Name == "IRMA Client"
                                     && k.Name == "LabAndClosedStoreNo"
                                     && e.ShortName.Substring(0, 1) == "T")
                                  select v.Value)
                    .FirstOrDefault()
                    .ToString().Split('|').ToList(); 
            
            //Randomly select 3 items/identifiers with price records 
            var storeItems = (from p in context.Price
                               join t1 in
                                   (
                                       ((from ii in context.ItemIdentifier
                                         where ii.Deleted_Identifier == 0 && ii.Remove_Identifier == 0 && ii.Default_Identifier == 0
                                         select new
                                         {
                                             ii.Identifier,
                                             ii.Item_Key
                                         }).Take(3))) on new { Item_Key = p.Item_Key } equals new { Item_Key = (int)t1.Item_Key }
                               where (p.Store.WFM_Store || p.Store.Mega_Store) && (p.Store.Internal && p.Store.BusinessUnit_ID != null)
                               select new
                               {
                                   Store_No = p.Store_No,
                                   Identifier = t1.Identifier,
                                   Item_Key = (int?)t1.Item_Key
                               }).ToList();

            var items = (from si in storeItems
                         select new
                             {
                                 Item_Key = si.Item_Key,
                                 Identifier = si.Identifier
                             }).Distinct().ToList();

            var invalidStoreItems = storeItems.FindAll(si => invalidStoreNo.Contains(si.Store_No.ToString())).ToList();
            var validStoreItems = storeItems.Except(invalidStoreItems);
            
            //stage 3 items for processing
            var validatedItems = items.Select(sc => new ValidatedItemModel { ScanCode = sc.Identifier, EventTypeId = EventTypes.ItemValidation }).ToList();
            command.ValidatedItems.AddRange(validatedItems);

            //When
            commandHandler.Handle(command);

            //Then
            var expectedNumberOfEvents = validStoreItems.Count();
            var identifiers = items.Select(i => i.Identifier);
            var itemKeys = items.Select(i => i.Item_Key);
            var events = context.PriceChangeQueue.Where(e => identifiers.Any(i => i == e.Identifier)).ToList();
            Assert.AreEqual(expectedNumberOfEvents, events.Count);

            foreach (var e in events)
            {
                Assert.IsTrue(validStoreItems.Any(s => s.Store_No == e.Store_No));
                Assert.IsTrue(identifiers.Any(i => i == e.Identifier));
                Assert.IsTrue(itemKeys.Any(i => i == e.Item_Key));
                Assert.AreEqual(3, e.EventTypeID);
            }
        }

        [TestMethod]
        public void BulkAddMammothPriceEvents_ScanCodeExistsInIrmaAsDefaultIdentifier_ShouldAddMammothEventsForEveryValidStoreInIrma()
        {
            //Given
            //Randomly select 3 items/identifiers with price records 
            var storeItems = (from p in context.Price
                              join t1 in
                                  (
                                      ((from ii in context.ItemIdentifier
                                        where ii.Deleted_Identifier == 0 && ii.Remove_Identifier == 0 && ii.Default_Identifier == 1
                                        select new
                                        {
                                            ii.Identifier,
                                            ii.Item_Key
                                        }).Take(3))) on new { Item_Key = p.Item_Key } equals new { Item_Key = (int)t1.Item_Key }
                              where (p.Store.WFM_Store || p.Store.Mega_Store) && (p.Store.Internal && p.Store.BusinessUnit_ID != null)
                              select new
                              {
                                  Store_No = p.Store_No,
                                  Identifier = t1.Identifier,
                                  Item_Key = (int?)t1.Item_Key
                              }).ToList();

            var items = (from si in storeItems
                         select new
                         {
                             Item_Key = si.Item_Key,
                             Identifier = si.Identifier
                         }).Distinct().ToList();

            var invalidStoreNo = (from v in context.AppConfigValue
                                  join a in context.AppConfigApp on v.ApplicationID equals a.ApplicationID
                                  join e in context.AppConfigEnv on v.EnvironmentID equals e.EnvironmentID
                                  join k in context.AppConfigKey on v.KeyID equals k.KeyID
                                  where (!v.Deleted
                                     && a.Name == "IRMA Client"
                                     && k.Name == "LabAndClosedStoreNo"
                                     && e.ShortName.Substring(0, 1) == "T")
                                  select v.Value)
                    .FirstOrDefault()
                    .ToString().Split('|').ToList();

            var invalidStoreItems = storeItems.FindAll(si => invalidStoreNo.Contains(si.Store_No.ToString())).ToList();
            var validStoreItems = storeItems.Except(invalidStoreItems);

            //stage 3 items for processing
            var validatedItems = items.Select(sc => new ValidatedItemModel { ScanCode = sc.Identifier, EventTypeId = EventTypes.ItemValidation }).ToList();
            command.ValidatedItems.AddRange(validatedItems);

            var identifiers = items.Select(i => i.Identifier);
            var existingEvents = context.PriceChangeQueue.Where(e => identifiers.Any(i => i == e.Identifier)).Count();

            //When
            commandHandler.Handle(command);

            //Then
            var expectedNumberOfEvents = validStoreItems.Count();
            
            var events = context.PriceChangeQueue.Where(e => identifiers.Any(i => i == e.Identifier)).ToList();
            Assert.AreEqual(expectedNumberOfEvents, events.Count - existingEvents);
        }

        [TestMethod]
        public void BulkAddMammothPriceEvents_EventTypesOtherThanValidatedItemAndNewIrmaItem_ShouldOnlyGenerateEventsForValidatedItemAndNewIrmaItem()
        {
            //var items = context.ItemIdentifier.Where(ii => ii.Deleted_Identifier == 0 && ii.Remove_Identifier == 0)
            //    .Select(ii => new { ii.Identifier, ii.Item_Key })
            //    .Take(3)
            //    .ToList();
            //var validStores = context.Store.Where(s => (s.WFM_Store || s.Mega_Store) && (s.Internal && s.BusinessUnit_ID != null)).ToList();

            //var validatedItems = items.Select(sc => new ValidatedItemModel { ScanCode = sc.Identifier }).ToList();
            
            //Exclude labs and closed stores defined in the app config in IRMA
            var invalidStoreNo = (from v in context.AppConfigValue
                                  join a in context.AppConfigApp on v.ApplicationID equals a.ApplicationID
                                  join e in context.AppConfigEnv on v.EnvironmentID equals e.EnvironmentID
                                  join k in context.AppConfigKey on v.KeyID equals k.KeyID
                                  where (!v.Deleted
                                     && a.Name == "IRMA Client"
                                     && k.Name == "LabAndClosedStoreNo"
                                     && e.ShortName.Substring(0, 1) == "T")
                                  select v.Value)
                    .FirstOrDefault()
                    .ToString().Split('|').ToList();

            //Randomly select 3 items/identifiers with price records 
            var storeItems = (from p in context.Price
                              join t1 in
                                  (
                                      ((from ii in context.ItemIdentifier
                                        where ii.Deleted_Identifier == 0 && ii.Remove_Identifier == 0 && ii.Default_Identifier == 0
                                        select new
                                        {
                                            ii.Identifier,
                                            ii.Item_Key
                                        }).Take(3))) on new { Item_Key = p.Item_Key } equals new { Item_Key = (int)t1.Item_Key }
                              where (p.Store.WFM_Store || p.Store.Mega_Store) && (p.Store.Internal && p.Store.BusinessUnit_ID != null)
                              select new
                              {
                                  Store_No = p.Store_No,
                                  Identifier = t1.Identifier,
                                  Item_Key = (int?)t1.Item_Key
                              }).ToList();

            var items = (from si in storeItems
                         select new
                         {
                             Item_Key = si.Item_Key,
                             Identifier = si.Identifier
                         }).Distinct().ToList();

            //stage 3 items for processing
            var stagedItems = items.Select(sc => new ValidatedItemModel { ScanCode = sc.Identifier }).ToList();
            stagedItems[0].EventTypeId = EventTypes.ItemValidation;
            stagedItems[1].EventTypeId = EventTypes.NewIrmaItem;
            stagedItems[2].EventTypeId = EventTypes.ItemUpdate;
            command.ValidatedItems.AddRange(stagedItems);

            var invalidStoreItems = storeItems.FindAll(si => invalidStoreNo.Contains(si.Store_No.ToString()) || si.Identifier == stagedItems[2].ScanCode).ToList();
            var validStoreItems = storeItems.Except(invalidStoreItems);

            //When
            commandHandler.Handle(command);

            //Then
            var expectedNumberOfEvents = validStoreItems.Count(); ;
            var identifiers = validStoreItems.Select(i => i.Identifier);
            var itemKeys = validStoreItems.Select(i => i.Item_Key);
            var events = context.PriceChangeQueue.Where(e => identifiers.Any(i => i == e.Identifier)).ToList();
            Assert.AreEqual(expectedNumberOfEvents, events.Count);

            foreach (var e in events)
            {
                Assert.IsTrue(validStoreItems.Any(s => s.Store_No == e.Store_No));
                Assert.IsTrue(identifiers.Any(i => i == e.Identifier));
                Assert.IsTrue(itemKeys.Any(i => i == e.Item_Key));
                Assert.IsTrue(e.EventTypeID == EventTypes.ItemValidation || e.EventTypeID == EventTypes.NewIrmaItem);
            }
        }
    }
}