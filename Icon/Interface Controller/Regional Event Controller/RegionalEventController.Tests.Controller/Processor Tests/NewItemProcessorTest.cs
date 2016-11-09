using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionalEventController.DataAccess.Queries;
using RegionalEventController.Common;
using RegionalEventController.Controller.Processors;
using RegionalEventController.DataAccess.Models;
using RegionalEventController.DataAccess.Infrastructure;
using RegionalEventController.DataAccess.Commands;
using RegionalEventController.Controller.ProcessorModules;
using Icon.Framework;
using Icon.Logging;
using Icon.Testing.Builders;
using Irma.Framework;
using Moq;
using System.Text;
using RegionalEventController.DataAccess.Interfaces;
using Icon.Common.Email;
using Irma.Testing.Builders;


namespace RegionalEventController.Tests.Controller.Processor_Tests
{
    [TestClass]
    public class NewItemProcessorTest
    {
        private IconContext context;
        private Dictionary<string, string> validatedRegionUpcs = new Dictionary<string, string>();
        private Dictionary<string, string> validatedRegionPlus = new Dictionary<string, string>();
        private Dictionary<string, string> brandNewRegionUpcs = new Dictionary<string, string>();
        private Dictionary<string, string> brandNewRegionPlus = new Dictionary<string, string>();
        private Dictionary<string, string> loadedRegionUpcs = new Dictionary<string, string>();
        private Dictionary<string, string> loadedRegionPlus = new Dictionary<string, string>();
        private Dictionary<string, string> invalidIrmaNewItems = new Dictionary<string, string>();

        private string[] regionsToTest = new string[1] { "PN" };
        private List<string> cleanupScripts;

        [TestInitialize]
        public void Initialize()
        {
            Cache.brandNameToBrandAbbreviationMap?.Clear();
            Cache.defaultCertificationAgencies?.Clear();
            Cache.itemSbTeamEventEnabledRegions?.Clear();
            Cache.nationalClassCodeToNationalClassId?.Clear();
            Cache.taxCodeToTaxId?.Clear();

            this.context = new IconContext();

            StartupOptions.RegionsToProcess = regionsToTest;
            StartupOptions.Instance = 123;

            cleanupScripts = new List<string>();

            var iconReferenceProcessor = BuildIconReferenceProcessor();
            iconReferenceProcessor.Run();
            Console.WriteLine("Finished preparing the RegionalEventController.");

            //IrmaItem will be wiped out to make brandnew scancodes to be identified in a timely manner.
            string sql = @"delete app.IRMAItem";
            int returnCode = context.Database.ExecuteSqlCommand(sql);
        }

        [TestCleanup]
        public void Cleanup()
        {
            foreach (string sql in cleanupScripts)
            {
                int returnCode = context.Database.ExecuteSqlCommand(sql);
            }
        }

        protected void StageNewIdentifiers(string irmaRegion,
            int numberOfIdentifiersToAdd,
            Dictionary<string, string> dictionaryOfAddedIdentifiers,
            IrmaContext irmaContext,
            IQueryable<ItemIdentifier> queryForIrmaItemIdentifiers,
            IconContext iconContext)
        {
            StageIdentifiers(irmaRegion, numberOfIdentifiersToAdd, dictionaryOfAddedIdentifiers, irmaContext, iconContext, queryForIrmaItemIdentifiers, null, true);
        }

        protected void StageIdentifiers(string irmaRegion,
            int numberOfIdentifiersToAdd,
            Dictionary<string, string> dictionaryOfAddedIdentifiers,
            IrmaContext irmaContext,
            IQueryable<ItemIdentifier> queryForIrmaItemIdentifiers,
            IQueryable<string> queryForIconIdentifiers)
        {
            StageIdentifiers(irmaRegion, numberOfIdentifiersToAdd, dictionaryOfAddedIdentifiers, irmaContext, null, queryForIrmaItemIdentifiers, queryForIconIdentifiers, false);
        }

        protected void StageIdentifiers(string irmaRegion,
            int numberOfIdentifiersToAdd,
            Dictionary<string, string> dictionaryOfAddedIdentifiers,
            IrmaContext irmaContext,
            IconContext iconContext,
            IQueryable<ItemIdentifier> queryForIrmaItemIdentifiers,
            IQueryable<string> queryForIconIdentifiers = null,
            bool addingNewItems = true)
        {
           // List<string> allIconIdentifiers = new List<string>();
            List<string> newlyRetrievedIconIdentifiers = new List<string>(); ;
            Dictionary<int, string> toBeQueuedItems = new Dictionary<int, string>();

            const int howManyToTake = 10000;
            if (null != queryForIconIdentifiers)
            {
                //read some identifiers from ICON
                var ids = queryForIconIdentifiers.Take(howManyToTake).ToList();
                //temp 
                if (null == ids || ids.Count < numberOfIdentifiersToAdd)
                {
                    throw new ArgumentOutOfRangeException("insufficient ICON data found to run test");
                }
                newlyRetrievedIconIdentifiers.AddRange(ids);
                //allIconIdentifiers.AddRange(newlyRetrievedIconIdentifiers);
            }
            //get the item identifiers from IRMA
            var existingItemIdentifiers = queryForIrmaItemIdentifiers.Take(howManyToTake).ToList();
            //temp 
            if (null == existingItemIdentifiers || existingItemIdentifiers.Count < numberOfIdentifiersToAdd)
            {
                throw new ArgumentOutOfRangeException("insufficient IRMA data found to run test");
            }

            // build collection of identifiers to be staged
            int numberAddedSoFar = 0;
            for (int i = 0; i < existingItemIdentifiers.Count; i++)
            {
                var existingIdentifier = existingItemIdentifiers.ElementAt(i);
                if (!toBeQueuedItems.ContainsKey(existingIdentifier.Item_Key))
                {
                    if (addingNewItems 
                        && iconContext.ScanCode.Any(sc=>sc.scanCode==existingIdentifier.Identifier)
                        && iconContext.IRMAItem.Any(ii => ii.identifier == existingIdentifier.Identifier))
                    {
                        continue;
                    }
                    //keep track of identifiers in class-wide dictionary
                    dictionaryOfAddedIdentifiers.Add(existingIdentifier.Identifier, irmaRegion);
                    //keep track of identifiers we are going to queue
                    toBeQueuedItems.Add(existingIdentifier.Item_Key, existingIdentifier.Identifier);
                    numberAddedSoFar++;
                }
                if (numberAddedSoFar >= numberOfIdentifiersToAdd) break;
            }

            //add the identifiers to the queue
            foreach (KeyValuePair<int, string> queueItem in toBeQueuedItems)
            {
                irmaContext.IconItemChangeQueue.Add(
                    new TestIconItemChangeQueueBuilder()
                        .WithItemKey(queueItem.Key)
                        .WithIdentifier(queueItem.Value)
                        .WithInProcessBy(StartupOptions.Instance.ToString()));
            }

            irmaContext.SaveChanges();
        }

        private void StageValidatedUpcs(string irmaRegion, IrmaContext db)
        {
            Dictionary<string, string> identifierDictionaryForTest = validatedRegionUpcs;

            IQueryable<string> iconQueryForIdentifiers = from sc in context.ScanCode.AsQueryable<ScanCode>()
                                                         join it in context.ItemTrait on sc.itemID equals it.itemID
                                                         join t in context.Trait on it.traitID equals t.traitID
                                                         where t.traitCode == TraitCodes.ValidationDate
                                                            && sc.scanCode.Substring(0, 1) != "2"
                                                            && sc.scanCode.Length > 10
                                                            && !identifierDictionaryForTest.Values.ToList().Contains(sc.scanCode)
                                                         select sc.scanCode;

            IQueryable<ItemIdentifier> irmaQueryForIdentifiers = from ii in db.ItemIdentifier
                                                                 where ii.Deleted_Identifier == 0 
                                                                     && ii.Remove_Identifier == 0
                                                                     //&& !identifierDictionaryForTest.Values.ToList().Contains(ii.Identifier)
                                                                 select ii;

            StageIdentifiers(irmaRegion, 3, identifierDictionaryForTest, db, irmaQueryForIdentifiers, iconQueryForIdentifiers);
        }

        private void StageBrandNewUpcs(string irmaRegion, IrmaContext db)
        {
            Dictionary<string, string> identifierDictionaryForTest = brandNewRegionUpcs;
            // IQueryable<string> iconQueryForIdentifiers = null;

            IQueryable<ItemIdentifier> irmaQueryForIdentifiers = from ii in db.ItemIdentifier
                                                                 where ii.Deleted_Identifier == 0
                                                                     && ii.Remove_Identifier == 0 
                                                                     && ii.Identifier.Substring(0, 1) != "2"
                                                                     && ii.Identifier.Length > 10
                                                                     //&& !identifierDictionaryForTest.Values.ToList().Contains(ii.Identifier)
                                                                 select ii;

            StageNewIdentifiers(irmaRegion, 3, identifierDictionaryForTest, db, irmaQueryForIdentifiers, context);
        }

        private void StageLoadedNotValidatedUpcs(string irmaRegion, IrmaContext db)
        {
            var iconContext = context;
            Dictionary<string, string> identifierDictionaryForTest = loadedRegionUpcs;

            IQueryable<string> iconQueryForIdentifiers = from sc in iconContext.ScanCode.AsQueryable<ScanCode>()
                                                         join it in iconContext.ItemTrait on sc.itemID equals it.itemID
                                                         join t in iconContext.Trait on it.traitID equals t.traitID
                                                         where !iconContext.ItemTrait.Any(itt => itt.itemID == sc.itemID && itt.traitID == Traits.ValidationDate)
                                                              //t.traitID != Traits.ValidationDate
                                                              && !sc.scanCode.StartsWith("2")
                                                              && sc.scanCode.Length > 10 
                                                              && sc.scanCode.StartsWith("9")
                                                              && !identifierDictionaryForTest.Values.ToList().Contains(sc.scanCode)
                                                         orderby sc.scanCode
                                                         select sc.scanCode;

            IQueryable<ItemIdentifier> irmaQueryForIdentifiers = from ii in db.ItemIdentifier
                                                                 where ii.Deleted_Identifier == 0
                                                                     && ii.Remove_Identifier == 0
                                                                    // && identifierDictionaryForTest.Values.ToList().Contains(ii.Identifier)
                                                                     && ii.Identifier.StartsWith("9")
                                                                 select ii;

            StageIdentifiers(irmaRegion, 3, identifierDictionaryForTest, db, irmaQueryForIdentifiers, iconQueryForIdentifiers);
        }

        private void StageValidatedPlus(string irmaRegion, IrmaContext db)
        {
            var iconContext = context;
            Dictionary<string, string> identifierDictionaryForTest = validatedRegionPlus;

            IQueryable<string> iconQueryForIdentifiers = from sc in iconContext.ScanCode.AsQueryable<ScanCode>()
                                                         join it in iconContext.ItemTrait on sc.itemID equals it.itemID
                                                         join t in iconContext.Trait on it.traitID equals t.traitID
                                                         where t.traitCode == TraitCodes.ValidationDate
                                                            && ((sc.scanCode.StartsWith("2") && sc.scanCode.EndsWith("00000") && sc.scanCode.Length == 11)
                                                                || sc.scanCode.Length < 7)
                                                            && !identifierDictionaryForTest.Values.ToList().Contains(sc.scanCode)
                                                         select sc.scanCode;

            IQueryable<ItemIdentifier> irmaQueryForIdentifiers = from ii in db.ItemIdentifier
                                                                 join it in db.Item on ii.Item_Key equals it.Item_Key
                                                                 where ii.Deleted_Identifier == 0 
                                                                     && ii.Remove_Identifier == 0
                                                                     //&& identifierDictionaryForTest.Values.ToList().Contains(ii.Identifier)
                                                                     && it.Retail_Sale == true 
                                                                     && it.Deleted_Item == false 
                                                                     && it.Remove_Item == 0
                                                                 select ii;

            StageIdentifiers(irmaRegion, 3, identifierDictionaryForTest, db, irmaQueryForIdentifiers, iconQueryForIdentifiers);
        }

        private void StageBrandNewPlus(string irmaRegion, IrmaContext db)
        {
            Dictionary<string, string> identifierDictionaryForTest = brandNewRegionPlus;
            // IQueryable<string> iconQueryForIdentifiers = null;

            //Add 3 brand new scancodes (scancodes don't exist in Icon)
            IQueryable<ItemIdentifier> irmaQueryForIdentifiers = from ii in db.ItemIdentifier
                                                                 join it in db.Item on ii.Item_Key equals it.Item_Key
                                                                 where ii.Deleted_Identifier == 0 
                                                                 && ii.Remove_Identifier == 0 
                                                                 && it.Remove_Item == 0 
                                                                 && it.Deleted_Item == false
                                                                 && ((ii.Identifier.StartsWith("2") && ii.Identifier.EndsWith("00000") && ii.Identifier.Length == 11)
                                                                    || ii.Identifier.Length < 7)
                                                                 //&& identifierDictionaryForTest.Values.ToList().Contains(ii.Identifier)
                                                                 //&& !brandNewPlusList.Contains(ii.Identifier)
                                                                 select ii;

            StageNewIdentifiers(irmaRegion, 3, identifierDictionaryForTest, db, irmaQueryForIdentifiers, context);
        }

        private void StageLoadedNotValidatedPlus(string irmaRegion, IrmaContext db)
        {
            var iconContext = context;
            Dictionary<string, string> identifierDictionaryForTest = loadedRegionPlus;

            IQueryable<string> iconQueryForIdentifiers = from sc in iconContext.ScanCode.AsQueryable<ScanCode>()
                                                         join it in iconContext.ItemTrait on sc.itemID equals it.itemID
                                                         where !iconContext.ItemTrait.Any(itt => itt.itemID == sc.itemID && itt.traitID == Traits.ValidationDate)
                                                            && ((sc.scanCode.StartsWith("2") && sc.scanCode.EndsWith("00000") && sc.scanCode.Length == 11)
                                                               || sc.scanCode.Length < 7)
                                                            && !identifierDictionaryForTest.Values.ToList().Contains(sc.scanCode)
                                                         select sc.scanCode;

            IQueryable<ItemIdentifier> irmaQueryForIdentifiers = from ii in db.ItemIdentifier
                                                                 join it in db.Item on ii.Item_Key equals it.Item_Key
                                                                 where ii.Deleted_Identifier == 0 
                                                                     && ii.Remove_Identifier == 0
                                                                     //&& identifierDictionaryForTest.Values.ToList().Contains(ii.Identifier)
                                                                     && it.Retail_Sale == true 
                                                                     && it.Deleted_Item == false 
                                                                     && it.Remove_Item == 0
                                                                 select ii;

            StageIdentifiers(irmaRegion, 3, identifierDictionaryForTest, db, irmaQueryForIdentifiers, iconQueryForIdentifiers);
        }

        private void StageInvalidQueueItems(string irmaRegion, IrmaContext db)
        {
            int itemKey = (from i in db.Item
                           select i.Item_Key).FirstOrDefault();

            var random = new Random();

            var invalidIrmaQueueItems = new List<Icon.Testing.CustomModels.IrmaNewItem>
            {
                new TestIrmaNewItemBuilder().WithIdentifier(random.Next(1000000, 1000000000).ToString()).WithItemKey(itemKey).Build()
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

            foreach (RegionalEventController.DataAccess.Models.IrmaNewItem invalidIrmaQueueItem in invalidIrmaQueueItems)
            {
                //invalidIrmaNewItems.Add(invalidIrmaQueueItem.Identifier, irmaRegion);
                db.IconItemChangeQueue.Add(new TestIconItemChangeQueueBuilder().WithItemKey(invalidIrmaQueueItem.IrmaItemKey).WithIdentifier(invalidIrmaQueueItem.Identifier).WithInProcessBy(StartupOptions.Instance.ToString()));
            }

            db.SaveChanges();
        }
        private void ComposeCleanUpScripts()
        {
            string sql;
            //Compose the cleanup scripts to remove the possible regioncode and scancode combination from the EventQueue and IrmaItemSubscription tables
            var testIRMAItemSubscriptionCleanUpBuilder = new StringBuilder();
            var testEventQueueCleanUpBuilder = new StringBuilder();
            var testIrmaItemCleanUpBuilder = new StringBuilder();

            foreach (KeyValuePair<string, string> regionScanCode in validatedRegionUpcs)
            {
                testIRMAItemSubscriptionCleanUpBuilder.Append(" or (regioncode = '" + regionScanCode.Value + "' and identifier = '" + regionScanCode.Key + "')");
                testEventQueueCleanUpBuilder.Append(" or (RegionCode = '" + regionScanCode.Value + "' and EventMessage = '" + regionScanCode.Key + "')");
            }

            foreach (KeyValuePair<string, string> regionScanCode in brandNewRegionUpcs)
            {
                testIRMAItemSubscriptionCleanUpBuilder.Append(" or (regioncode = '" + regionScanCode.Value + "' and identifier = '" + regionScanCode.Key + "')");
                testIrmaItemCleanUpBuilder.Append(" or (identifier = '" + regionScanCode.Key + "')");
            }

            foreach (KeyValuePair<string, string> regionScanCode in loadedRegionUpcs)
            {
                testIRMAItemSubscriptionCleanUpBuilder.Append(" or (regioncode = '" + regionScanCode.Value + "' and identifier = '" + regionScanCode.Key + "')");
            }

            foreach (KeyValuePair<string, string> regionScanCode in validatedRegionPlus)
            {
                testIRMAItemSubscriptionCleanUpBuilder.Append(" or (regioncode = '" + regionScanCode.Value + "' and identifier = '" + regionScanCode.Key + "')");
                testEventQueueCleanUpBuilder.Append(" or (RegionCode = '" + regionScanCode.Value + "' and EventMessage = '" + regionScanCode.Key + "')");
            }

            foreach (KeyValuePair<string, string> regionScanCode in brandNewRegionPlus)
            {
                testIRMAItemSubscriptionCleanUpBuilder.Append(" or (regioncode = '" + regionScanCode.Value + "' and identifier = '" + regionScanCode.Key + "')");
            }

            foreach (KeyValuePair<string, string> regionScanCode in loadedRegionPlus)
            {
                testIRMAItemSubscriptionCleanUpBuilder.Append(" or (regioncode = '" + regionScanCode.Value + "' and identifier = '" + regionScanCode.Key + "')");
            }

            string testIRMAItemSubscriptionToString = testIRMAItemSubscriptionCleanUpBuilder.ToString();
            string testEventQueueToString = testEventQueueCleanUpBuilder.ToString();
            string testIrmaItemToString = testIrmaItemCleanUpBuilder.ToString();

            if (testIRMAItemSubscriptionToString.Length > 0)
            {
                sql = @"delete [app].[IRMAItemSubscription] where " + testIRMAItemSubscriptionToString.Remove(0, 3);
                int returnCode = context.Database.ExecuteSqlCommand(sql);

                cleanupScripts.Add(sql);
            }

            if (testEventQueueToString.Length > 0)
            {
                sql = @"delete [app].[EventQueue] where " + testEventQueueToString.Remove(0, 3);
                int returnCode = context.Database.ExecuteSqlCommand(sql);

                cleanupScripts.Add(sql);
            }

            if (testIrmaItemToString.Length > 0)
            {
                sql = @"delete [app].[IRMAItem] where " + testIrmaItemToString.Remove(0, 3);
                int returnCode = context.Database.ExecuteSqlCommand(sql);

                cleanupScripts.Add(sql);
            }
        }


        private void TurnOnEnableUPCIConToIRMAFlowInIrma(IrmaContext db)
        {
            string sql = @"update AppConfigValue
                                set value = 1
                               from AppConfigValue V
                         inner join AppConfigApp A on V.ApplicationID = A.ApplicationID
                         inner join AppConfigEnv E on V.EnvironmentID = E.EnvironmentID 
                         inner join AppConfigKey K on V.KeyID = K.KeyID
                              where Left(E.ShortName, 1) = Left((select top 1 Environment from Version),1)
                                and V.Deleted = 0
                                and A.Name = 'IRMA CLIENT'
                                and K.Name = 'EnableUPCIConToIRMAFlow'";

            int returnCode = db.Database.ExecuteSqlCommand(sql);
        }

        private void TurnOffEnableUPCIConToIRMAFlowInIrma(IrmaContext db)
        {
            string sql = @"update AppConfigValue
                                set value = 0
                               from AppConfigValue V
                         inner join AppConfigApp A on V.ApplicationID = A.ApplicationID
                         inner join AppConfigEnv E on V.EnvironmentID = E.EnvironmentID 
                         inner join AppConfigKey K on V.KeyID = K.KeyID
                              where Left(E.ShortName, 1) = Left((select top 1 Environment from Version),1)
                                and V.Deleted = 0
                                and A.Name = 'IRMA CLIENT'
                                and K.Name = 'EnableUPCIConToIRMAFlow'";

            int returnCode = db.Database.ExecuteSqlCommand(sql);
        }

        [TestMethod]
        [Ignore] // Ignoring these tests from this class due to the tests stalling the build
        public void RunNewItemProcessor_PLUsValidatedInIcon_EventQueueAndIrmaItemSubscriptionAreCreated()
        {
            // Given.
            string sql;
            int returnCode;

            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext db = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));

                sql = @"delete dbo.IconItemChangeQueue";
                returnCode = db.Database.ExecuteSqlCommand(sql);

                StageValidatedPlus(irmaRegion, db);
                StageBrandNewPlus(irmaRegion, db);
                StageLoadedNotValidatedPlus(irmaRegion, db);
            }
            //Housekeeping steps to remove pre-existing EventQueue, IrmaItemSubscription entries for the selected ScanCodes for testing.
            ComposeCleanUpScripts();

            //When
            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext irmaContext = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));

                var newItemProcessor = BuildNewItemProcessor(irmaContext, irmaRegion);

                newItemProcessor.Run();

                Console.WriteLine("newItemProcessor finished running for " + irmaRegion);
            }

            // Then
            int eventQueueCtr = GetEventQueueCount(context, validatedRegionPlus);
            int subscriptionCtr = GetIrmaItemSubscriptionCount(context, validatedRegionPlus);
            int irmaItemCtr = GetIrmaItemCount(context, validatedRegionPlus); 

            Assert.AreEqual(validatedRegionPlus.Count(), eventQueueCtr);
            Assert.AreEqual(validatedRegionPlus.Count(), subscriptionCtr);
            Assert.AreEqual(0, irmaItemCtr);
        }

        protected int GetEventQueueCount(IconContext iconContext, Dictionary<string,string> identifierDictionary, int? eventType = null)
        {
            int eventQueueCtr = 0;

            if (null == eventType)
            {
                eventQueueCtr = iconContext.EventQueue.Where(eq =>
                   identifierDictionary.Keys.Contains(eq.EventMessage) &&
                   identifierDictionary.Values.Contains(eq.RegionCode))
                .Count();
            }
            else
            {
                eventQueueCtr = iconContext.EventQueue.Where(eq =>
                   identifierDictionary.Keys.Contains(eq.EventMessage) &&
                   identifierDictionary.Values.Contains(eq.RegionCode) &&
                   eq.EventId == eventType)
                .Count();
            }
            return eventQueueCtr;
        }
        protected int GetIrmaItemSubscriptionCount(IconContext iconContext, Dictionary<string, string> identifierDictionary)
        {
            int subscriptionCtr = iconContext.IRMAItemSubscription.Where(iis =>
                      identifierDictionary.Keys.Contains(iis.identifier) &&
                      identifierDictionary.Values.Contains(iis.regioncode)
                      && iis.deleteDate == null)
                  .Count();
            return subscriptionCtr;
        }
        protected int GetIrmaItemCount(IconContext iconContext, Dictionary<string, string> identifierDictionary)
        {
            int irmaItemCtr = iconContext.IRMAItem.Where(ii =>
                     identifierDictionary.Keys.Contains(ii.identifier))
                .Count();
            return irmaItemCtr;
        }

        [TestMethod]
        [Ignore] // Ignoring these tests from this class due to the tests stalling the build
        public void RunNewItemProcessor_PLUsBrandNewToIcon_OnlyIrmaItemSubscriptionIsCreated()
        {
            // Given.
            string sql;
            int returnCode;

            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext db = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));

                sql = @"delete dbo.IconItemChangeQueue";
                returnCode = db.Database.ExecuteSqlCommand(sql);

                StageValidatedPlus(irmaRegion, db);
                StageBrandNewPlus(irmaRegion, db);
                StageLoadedNotValidatedPlus(irmaRegion, db);
            }
            //Housekeeping steps to remove pre-existing EventQueue, IrmaItemSubscription entries for the selected ScanCodes for testing.
            ComposeCleanUpScripts();

            //When
            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext irmaContext = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));
                var newItemProcessor = BuildNewItemProcessor(irmaContext, irmaRegion);
                newItemProcessor.Run();
                Console.WriteLine("newItemProcessor finished running for " + irmaRegion);
            }

            // Then
            int eventQueueCtr = GetEventQueueCount(context, brandNewRegionPlus);
            int subscriptionCtr = GetIrmaItemSubscriptionCount(context, brandNewRegionPlus);
            int irmaItemCtr = GetIrmaItemCount(context, brandNewRegionPlus);

            Assert.AreEqual(brandNewRegionPlus.Count(), subscriptionCtr);
            Assert.AreEqual(0, eventQueueCtr);
            Assert.AreEqual(0, irmaItemCtr);
        }

        [TestMethod]
        [Ignore] // Ignoring these tests from this class due to the tests stalling the build
        public void RunNewItemProcessor_PLUsOnlyLoadedInIcon_OnlyIrmaItemSubscriptionIsCreated()
        {
            // Given
            string sql;
            int returnCode;

            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext db = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));

                sql = @"delete dbo.IconItemChangeQueue";
                returnCode = db.Database.ExecuteSqlCommand(sql);

                StageValidatedPlus(irmaRegion, db);
                StageBrandNewPlus(irmaRegion, db);
                StageLoadedNotValidatedPlus(irmaRegion, db);
            }
            //Housekeeping steps to remove pre-existing EventQueue, IrmaItemSubscription entries for the selected ScanCodes for testing.
            ComposeCleanUpScripts();

            // When
            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext irmaContext = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));
                var newItemProcessor = BuildNewItemProcessor(irmaContext, irmaRegion);
                newItemProcessor.Run();
                Console.WriteLine("newItemProcessor finished running for " + irmaRegion);
            }

            // Then
            int eventQueueCtr = GetEventQueueCount(context, loadedRegionPlus, (int)EventTypes.NewIrmaItem);
            int subscriptionCtr = GetIrmaItemSubscriptionCount(context, loadedRegionPlus);
            int irmaItemCtr = GetIrmaItemCount(context, loadedRegionPlus);

            Assert.AreEqual(loadedRegionPlus.Count(), subscriptionCtr);
            Assert.AreEqual(0, eventQueueCtr);
            Assert.AreEqual(0, irmaItemCtr);

        }

        [TestMethod]
        [Ignore] // Ignoring these tests from this class due to the tests stalling the build
        public void RunNewItemProcessor_EnableUPCIConToIRMAFlowIsOnAndUPCsValidatedInIcon_EventQueueAndIrmaItemSubscriptionAreCreated()
        {
            // Given
            string sql;
            int returnCode;

            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext db = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));

                sql = @"delete dbo.IconItemChangeQueue";
                returnCode = db.Database.ExecuteSqlCommand(sql);

                StageValidatedUpcs(irmaRegion, db);
                StageBrandNewUpcs(irmaRegion, db);
                StageLoadedNotValidatedUpcs(irmaRegion, db);
                StageValidatedPlus(irmaRegion, db);
            }
            //Housekeeping steps to remove pre-existing EventQueue, IrmaItemSubscription entries for the selected ScanCodes for testing.
            ComposeCleanUpScripts();

            // When
            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext irmaContext = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));
                //update EnableUPCIConToIRMAFlow config in IRMA to true
                TurnOnEnableUPCIConToIRMAFlowInIrma(irmaContext);
                var newItemProcessor = BuildNewItemProcessor(irmaContext, irmaRegion);
                newItemProcessor.Run();
                Console.WriteLine("newItemProcessor finished running for " + irmaRegion);
            }

            // Then
            int eventQueueCtr = GetEventQueueCount(context, validatedRegionUpcs);
            int subscriptionCtr = GetIrmaItemSubscriptionCount(context, validatedRegionUpcs);
            int irmaItemCtr = GetIrmaItemCount(context, validatedRegionUpcs);

            Assert.AreEqual(validatedRegionUpcs.Count(), eventQueueCtr);
            Assert.AreEqual(validatedRegionUpcs.Count(), subscriptionCtr);
            Assert.AreEqual(0, irmaItemCtr);
        }

        [TestMethod]
        [Ignore] // Ignoring these tests from this class due to the tests stalling the build
        public void RunNewItemProcessor_EnableUPCIConToIRMAFlowIsOffAndUPCsValidatedInIcon_NoIrmaItemSubscriptionOrEventQueueEntriesAreCreated()
        {
            // Given
            string sql;
            int returnCode;

            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext db = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));

                sql = @"delete dbo.IconItemChangeQueue";
                returnCode = db.Database.ExecuteSqlCommand(sql);

                StageValidatedUpcs(irmaRegion, db);
                StageBrandNewUpcs(irmaRegion, db);
                StageLoadedNotValidatedUpcs(irmaRegion, db);
                StageValidatedPlus(irmaRegion, db);
            }
            //Housekeeping steps to remove pre-existing EventQueue, IrmaItemSubscription entries for the selected ScanCodes for testing.
            ComposeCleanUpScripts();

            // When
            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext irmaContext = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));
                //update EnableUPCIConToIRMAFlow config in IRMA to false
                TurnOffEnableUPCIConToIRMAFlowInIrma(irmaContext);
                var newItemProcessor = BuildNewItemProcessor(irmaContext, irmaRegion);
                newItemProcessor.Run();
                Console.WriteLine("newItemProcessor finished running for " + irmaRegion);
            }

            // Then
            int eventQueueCtr = GetEventQueueCount(context, validatedRegionUpcs, (int)EventTypes.NewIrmaItem);
            int subscriptionCtr = GetIrmaItemSubscriptionCount(context, validatedRegionUpcs);
            int irmaItemCtr = GetIrmaItemCount(context, validatedRegionUpcs);

            Assert.AreEqual(0, eventQueueCtr);
            Assert.AreEqual(0, subscriptionCtr);
            Assert.AreEqual(0, irmaItemCtr);
        }

        [TestMethod]
        [Ignore] // Ignoring these tests from this class due to the tests stalling the build
        public void RunNewItemProcessor_EnableUPCIConToIRMAFlowIsOnAndUPCsBrandNewToIcon_IrmaItemAndIrmaItemSubscriptionAreCreated()
        {
            // Given
            string sql;
            int returnCode;

            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext db = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));

                sql = @"delete dbo.IconItemChangeQueue";
                returnCode = db.Database.ExecuteSqlCommand(sql);

                StageValidatedUpcs(irmaRegion, db);
                StageBrandNewUpcs(irmaRegion, db);
                StageLoadedNotValidatedUpcs(irmaRegion, db);             
                StageBrandNewPlus(irmaRegion, db);
            }
            //Housekeeping steps to remove pre-existing EventQueue, IrmaItemSubscription entries for the selected ScanCodes for testing.
            ComposeCleanUpScripts();

            // When
            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext irmaContext = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));
                //update EnableUPCIConToIRMAFlow config in IRMA to true
                TurnOnEnableUPCIConToIRMAFlowInIrma(irmaContext);
                var newItemProcessor = BuildNewItemProcessor(irmaContext, irmaRegion);
                newItemProcessor.Run();
                Console.WriteLine("newItemProcessor finished running for " + irmaRegion);
            }

            // Then
            int eventQueueCtr = GetEventQueueCount(context, brandNewRegionUpcs);
            int subscriptionCtr = GetIrmaItemSubscriptionCount(context, brandNewRegionUpcs);
            int irmaItemCtr = GetIrmaItemCount(context, brandNewRegionUpcs);

            Assert.AreEqual(brandNewRegionUpcs.Count(), irmaItemCtr);
            Assert.AreEqual(brandNewRegionUpcs.Count(), subscriptionCtr);
            Assert.AreEqual(0, eventQueueCtr);
        }

        [TestMethod]
        [Ignore] // Ignoring these tests from this class due to the tests stalling the build
        public void RunNewItemProcessor_EnableUPCIConToIRMAFlowIsOffAndUPCsBrandNewToIcon_OnlyIrmaItemIsCreated()
        {
            // Given
            string sql;
            int returnCode;

            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext db = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));

                sql = @"delete dbo.IconItemChangeQueue";
                returnCode = db.Database.ExecuteSqlCommand(sql);

                StageValidatedUpcs(irmaRegion, db);
                StageBrandNewUpcs(irmaRegion, db);
                StageLoadedNotValidatedUpcs(irmaRegion, db);
                StageBrandNewPlus(irmaRegion, db);
            }
            //Housekeeping steps to remove pre-existing EventQueue, IrmaItemSubscription entries for the selected ScanCodes for testing.
            ComposeCleanUpScripts();

            // When
            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext irmaContext = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));
                //update EnableUPCIConToIRMAFlow config in IRMA to false
                TurnOffEnableUPCIConToIRMAFlowInIrma(irmaContext);
                var newItemProcessor = BuildNewItemProcessor(irmaContext, irmaRegion);
                newItemProcessor.Run();
                Console.WriteLine("newItemProcessor finished running for " + irmaRegion);
            }

            // Then
            int eventQueueCtr = GetEventQueueCount(context, brandNewRegionUpcs);
            int subscriptionCtr = GetIrmaItemSubscriptionCount(context, brandNewRegionUpcs);
            int irmaItemCtr = GetIrmaItemCount(context, brandNewRegionUpcs);

            Assert.AreEqual(brandNewRegionUpcs.Count(), irmaItemCtr);
            Assert.AreEqual(0, subscriptionCtr);
            Assert.AreEqual(0, eventQueueCtr);
        }

        [TestMethod]
        [Ignore] // Ignoring these tests from this class due to the tests stalling the build
        public void RunNewItemProcessor_EnableUPCIConToIRMAFlowIsOnAndUPCsOnlyLoadedInIcon_IrmaItemSubscriptionIsCreated()
        {
            // Given
            string sql;
            int returnCode;

            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext db = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));

                sql = @"delete dbo.IconItemChangeQueue";
                returnCode = db.Database.ExecuteSqlCommand(sql);

                StageValidatedUpcs(irmaRegion, db);
                StageBrandNewUpcs(irmaRegion, db);
                StageLoadedNotValidatedUpcs(irmaRegion, db);
                StageLoadedNotValidatedPlus(irmaRegion, db);
            }
            //Housekeeping steps to remove pre-existing EventQueue, IrmaItemSubscription entries for the selected ScanCodes for testing.
            ComposeCleanUpScripts();

            // When
            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext irmaContext = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));
                //update EnableUPCIConToIRMAFlow config in IRMA to true
                TurnOnEnableUPCIConToIRMAFlowInIrma(irmaContext);
                var newItemProcessor = BuildNewItemProcessor(irmaContext, irmaRegion);
                newItemProcessor.Run();
                Console.WriteLine("newItemProcessor finished running for " + irmaRegion);
            }

            // Then
            int eventQueueCtr = GetEventQueueCount(context, loadedRegionPlus, (int)EventTypes.NewIrmaItem);
            int subscriptionCtr = GetIrmaItemSubscriptionCount(context, loadedRegionPlus);
            int irmaItemCtr = GetIrmaItemCount(context, loadedRegionPlus);

            Assert.AreEqual(loadedRegionUpcs.Count(), subscriptionCtr);
            Assert.AreEqual(0, irmaItemCtr);
            Assert.AreEqual(0, eventQueueCtr);
        }

        [TestMethod]
        [Ignore] // Ignoring these tests from this class due to the tests stalling the build
        public void RunNewItemProcessor_EnableUPCIConToIRMAFlowIsOffAndUPCsOnlyLoadedInIcon_NoIrmaItemSubscriptionIsCreated()
        {
            // Given
            string sql;
            int returnCode;

            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext db = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));

                sql = @"delete dbo.IconItemChangeQueue";
                returnCode = db.Database.ExecuteSqlCommand(sql);

                StageValidatedUpcs(irmaRegion, db);
                StageBrandNewUpcs(irmaRegion, db);
                StageLoadedNotValidatedUpcs(irmaRegion, db);
                StageLoadedNotValidatedPlus(irmaRegion, db);
            }
            //Housekeeping steps to remove pre-existing EventQueue, IrmaItemSubscription entries for the selected ScanCodes for testing.
            ComposeCleanUpScripts();

            // When
            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext irmaContext = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));
                //update EnableUPCIConToIRMAFlow config in IRMA to false
                TurnOffEnableUPCIConToIRMAFlowInIrma(irmaContext);
                var newItemProcessor = BuildNewItemProcessor(irmaContext, irmaRegion);
                newItemProcessor.Run();
                Console.WriteLine("newItemProcessor finished running for " + irmaRegion);
            }

            // Then
            int eventQueueCtr = GetEventQueueCount(context, loadedRegionUpcs, (int)EventTypes.NewIrmaItem);
            int subscriptionCtr = GetIrmaItemSubscriptionCount(context, loadedRegionUpcs);
            int irmaItemCtr = GetIrmaItemCount(context, loadedRegionUpcs);

            Assert.AreEqual(0, subscriptionCtr);
            Assert.AreEqual(0, irmaItemCtr);
            Assert.AreEqual(0, eventQueueCtr);
        }

        [TestMethod]
        [Ignore] // Ignoring these tests from this class due to the tests stalling the build
        public void RunNewItemProcessor_AllIconItemChangeQueueEntriesSuccessfullyProcessed_IconItemChangeQueueEntriesAreDeleted()
        {
            // Given
            string sql;
            int returnCode;

            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext db = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));
                sql = @"delete dbo.IconItemChangeQueue";
                returnCode = db.Database.ExecuteSqlCommand(sql);
                StageValidatedUpcs(irmaRegion, db);
                StageLoadedNotValidatedUpcs(irmaRegion, db);
                StageLoadedNotValidatedPlus(irmaRegion, db);
            }
            //Housekeeping steps to remove pre-existing EventQueue, IrmaItemSubscription entries for the selected ScanCodes for testing.
            ComposeCleanUpScripts();

            // When
            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext irmaContext = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));
                var newItemProcessor = BuildNewItemProcessor(irmaContext, irmaRegion);
                newItemProcessor.Run();
                Console.WriteLine("newItemProcessor finished running for " + irmaRegion);

                // Then
                Assert.IsTrue(!irmaContext.IconItemChangeQueue.Any());
            }
        }

        [TestMethod]
        [Ignore] // Ignoring these tests from this class due to the tests stalling the build
        public void RunNewItemProcessor_OneIconItemChangeQueueEntryFailedToBeProcessed_IconItemChangeQueueEntryIsMarkedFailed()
        {
            // Given
            string sql;
            int returnCode;

            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext db = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));

                sql = @"delete dbo.IconItemChangeQueue";
                returnCode = db.Database.ExecuteSqlCommand(sql);

                StageValidatedUpcs(irmaRegion, db);
                StageInvalidQueueItems(irmaRegion, db);
                StageLoadedNotValidatedPlus(irmaRegion, db);
            }
            //Housekeeping steps to remove pre-existing EventQueue, IrmaItemSubscription entries for the selected ScanCodes for testing.
            ComposeCleanUpScripts();

            // When
            int invalidQueueEntriesCtr = 0;

            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext irmaContext = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));
                var newItemProcessor = BuildNewItemProcessor(irmaContext, irmaRegion);
                newItemProcessor.Run();
                Console.WriteLine("newItemProcessor finished running for " + irmaRegion);

                // Then
                foreach (KeyValuePair<string, string> invalidIrmaNewItem in invalidIrmaNewItems)
                {
                    if (irmaContext.IconItemChangeQueue.Where(iq => iq.Identifier == invalidIrmaNewItem.Key && irmaRegion == invalidIrmaNewItem.Value).Any())
                        invalidQueueEntriesCtr++;
                }
            }

            Assert.AreEqual(invalidIrmaNewItems.Count(), invalidQueueEntriesCtr);
        }

        private IconReferenceProcessor BuildIconReferenceProcessor()
        {
            var mockLogger = new Mock<ILogger<IconReferenceProcessor>>().Object;

            var getTaxCodeToTaxIdMappingQueryHandler = new GetTaxCodeToTaxIdMappingQueryHandler(context);
            var getNationalClassCodeToIdMappingHandler = new GetNationalClassCodeToClassIdMappingQueryHandler(context);
            var getBrandAbbreviationQueryQueryHandler = new GetBrandAbbreviationQueryHandler(context);
            var getDefaultCertificationAgenciesQueryHandler = new GetDefaultCertificationAgenciesQueryHandler(context);
            return new IconReferenceProcessor(
                mockLogger,
                EmailClient.CreateFromConfig(),
                context,
                getTaxCodeToTaxIdMappingQueryHandler,
                new GetRegionalSettingsBySettingsKeyNameQuery(context),
                getNationalClassCodeToIdMappingHandler,
                getBrandAbbreviationQueryQueryHandler,
                getDefaultCertificationAgenciesQueryHandler);
        }
        private NewItemProcessor BuildNewItemProcessor(IrmaContext irmaContext, string currentRegion)
        {
            var mockLogger = new Mock<ILogger<NewItemProcessor>>().Object;

            var getIrmaNewItemsQueryHandler = new GetIrmaNewItemsQueryHandler(irmaContext);

            var getAppConfigValueQueryHandler = new GetAppConfigValueQueryHandler(irmaContext);

            var getInvalidInProcessedQueueEntriesQueryHandler = new GetInvalidInProcessedQueueEntriesQueryHandler(irmaContext);

            var deleteNewItemsFromIrmaQueueCommandHandler = new DeleteNewItemsFromIrmaQueueCommandHandler(new Mock<ILogger<DeleteNewItemsFromIrmaQueueCommandHandler>>().Object,
               irmaContext);

            var markIconItemChangeQueueEntriesInProcessByCommandHandler = new MarkIconItemChangeQueueEntriesInProcessByCommandHandler(new Mock<ILogger<MarkIconItemChangeQueueEntriesInProcessByCommandHandler>>().Object,
                irmaContext);

            var getValidatedItemsQueryHandler = new GetValidatedItemsQueryHandler(context);

            var getBrandNewScanCodesQueryHandler = new GetBrandNewScanCodesQueryHandler(context);

            var getScanCodesNeedSubscriptionQueryHandler = new GetScanCodesNeedSubscriptionQueryHandler(context);

            return new NewItemProcessor(
                mockLogger,
                context,
                currentRegion,
                getIrmaNewItemsQueryHandler,
                getAppConfigValueQueryHandler,
                getInvalidInProcessedQueueEntriesQueryHandler,
                deleteNewItemsFromIrmaQueueCommandHandler,
                markIconItemChangeQueueEntriesInProcessByCommandHandler,
                getValidatedItemsQueryHandler,
                getBrandNewScanCodesQueryHandler,
                getScanCodesNeedSubscriptionQueryHandler);
        }

        [TestMethod]
        [Ignore] // Ignoring these tests from this class due to the tests stalling the build
        public void RunNewItemProcessor_ItemSubteamEventsAreGenerated()
        {
            // Given
            string sql;
            int returnCode;

            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext db = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));

                sql = @"delete dbo.IconItemChangeQueue";
                returnCode = db.Database.ExecuteSqlCommand(sql);

                StageValidatedUpcs(irmaRegion, db);
                StageLoadedNotValidatedUpcs(irmaRegion, db);
            }
            //Housekeeping steps to remove pre-existing EventQueue, IrmaItemSubscription entries for the selected ScanCodes for testing.
            ComposeCleanUpScripts();
            foreach(string region in StartupOptions.RegionsToProcess)
            {
                Cache.itemSbTeamEventEnabledRegions.Add(region);
            }

            // When
            foreach (string irmaRegion in StartupOptions.RegionsToProcess)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext irmaContext = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));

                //update EnableUPCIConToIRMAFlow config in IRMA to false
                TurnOffEnableUPCIConToIRMAFlowInIrma(irmaContext);

                var newItemProcessor = BuildNewItemProcessor(irmaContext, irmaRegion);

                newItemProcessor.Run();

                Console.WriteLine("newItemProcessor finished running for " + irmaRegion);
            }

            // Then
            int eventQueueCtr = 0;

            foreach (KeyValuePair<string, string> validatedRegionUpc in validatedRegionUpcs)
            {
                if (context.EventQueue.Where(eq => eq.EventMessage == validatedRegionUpc.Key && eq.RegionCode == validatedRegionUpc.Value && eq.EventId == EventTypes.ItemSubTeamUpdate).Any())
                {
                    eventQueueCtr++;
                }
            }

            int expectedEventCount = validatedRegionUpcs.Count;
            Assert.AreEqual(eventQueueCtr, expectedEventCount);
        }

    }
}