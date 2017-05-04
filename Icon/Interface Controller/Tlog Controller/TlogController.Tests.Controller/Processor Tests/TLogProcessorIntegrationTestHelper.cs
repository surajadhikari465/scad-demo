using Icon.Framework;
using Icon.Logging;
using Icon.Testing.Builders;
using Irma.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TlogController.Common;
using TlogController.Controller.ProcessorModules;
using TlogController.Controller.Processors;
using TlogController.DataAccess.BulkCommands;
using TlogController.DataAccess.Infrastructure;
using TlogController.DataAccess.Queries;

namespace TlogController.Tests.Controller.Processor_Tests
{
    public class TLogProcessorIntegrationTestHelper
    {
        public IconContext TestIconContext = null;
        public TlogProcessor TestTlogProcessor = null;
        public DateTime RightNow;

        public List<string> CleanupScripts = new List<string>();
        public List<string> RegionsToTest = new List<string>();
        public int Instance { get; set; }
        public int RegisterNumber { get; set; }
        public int NumberOfStorePerRegionsToGetItemsFor { get; set; }
        public int NumberOfTransactionsPerStore { get; set; }
        public int NumberOfItemsPerTransaction { get; set; }

        public int CountOfStagedUniqueTransactions { get; private set; }
        public int CountOfStagedItemMovementRecords { get; private set; }

        public List<TestStoreItem> AllItemsToQueue { get; private set; }

        
        public TLogProcessorIntegrationTestHelper(
            int instance,
            List<string> regionsToTest,
            int registerNumber,
            int numberOfStoresPerRegionToGetItemsFor,
            int numberOfTransactionsPerStore,
            int numberOfItemsPerTransaction)
        {
            Instance = instance;
            RegionsToTest = regionsToTest;
            RegisterNumber = registerNumber;
            NumberOfStorePerRegionsToGetItemsFor = numberOfStoresPerRegionToGetItemsFor;
            NumberOfTransactionsPerStore = numberOfTransactionsPerStore;
            NumberOfItemsPerTransaction = numberOfItemsPerTransaction;
            AllItemsToQueue = new List<TestStoreItem>(RegionsToTest.Count * NumberOfStorePerRegionsToGetItemsFor * NumberOfTransactionsPerStore * NumberOfItemsPerTransaction);
        }

        public void InitializeData()
        {
            StartupOptions.Instance = this.Instance;
            StartupOptions.MaxTransactionsToProcess = RegionsToTest.Count * NumberOfStorePerRegionsToGetItemsFor * NumberOfTransactionsPerStore;
            StartupOptions.MaxTlogTransactionsWhenSplit = NumberOfItemsPerTransaction;

            this.TestIconContext = new IconContext();

            var mockLogger = new Mock<ILogger<TlogProcessor>>();

            this.TestTlogProcessor = new TlogProcessor(mockLogger.Object, this.BuildIconTlogProcessorModule());

            this.RightNow = DateTime.Parse(DateTime.Now.ToShortTimeString());

            string sql = @"delete app.ItemMovement";
            TestIconContext.Database.ExecuteSqlCommand(sql);

            sql = @"delete app.ItemMovementTransactionHistory where RegisterNumber = " + RegisterNumber;
            TestIconContext.Database.ExecuteSqlCommand(sql);
        }

        public void CleanupData()
        {
            foreach (string sql in CleanupScripts)
            {
                int returnCode = this.TestIconContext.Database.ExecuteSqlCommand(sql);
            }

            foreach (string irmaRegion in RegionsToTest)
            {
                //Given
                IrmaContext db = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));

                string tempSql = @"delete sales_sumbyitem
                                   where Date_Key >= '" + RightNow.AddDays(-1).Date + "'";
                int returnCode = db.Database.ExecuteSqlCommand(tempSql);

                tempSql = @"delete TlogReprocessRequest
                                   where Date_Key = '" + RightNow.AddDays(-1).Date + "'";
                returnCode = db.Database.ExecuteSqlCommand(tempSql);
            }
        }

        protected IIconTlogProcessorModule BuildIconTlogProcessorModule()
        {
            return new IconTlogProcessorModule(
                new NLogLoggerInstance<IconTlogProcessorModule>(StartupOptions.Instance.ToString()),
                new GetBusinessUnitToRegionCodeMappingQueryHandler(this.TestIconContext),
                new BulkUpdateItemMovementInProcessCommandHandler(this.TestIconContext),
                new BulkUpdateItemMovementCommandHandler(new Mock<ILogger<BulkUpdateItemMovementCommandHandler>>().Object, this.TestIconContext));
        }

        public int StageValidItemMovementData(string irmaRegion, bool allowDuplicates, DateTime transactionDate)
        {
            int numberOfStoresWithEnoughValidItems = 0;
            List<int> businessUnitsAlreadyChecked = new List<int>();
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            const int numScanCodes = 1000;
            var irmaIdentifiers = new List<string>(numScanCodes);
            var storesInIrma = new List<int?>();
            using (IrmaContext irmaContext = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion)))
            {
                storesInIrma = irmaContext.Store.Select(s => s.BusinessUnit_ID).ToList();
            }
            int loopCount = 1;

            do
            {
                var businessUnit = this.TestIconContext.BusinessUnitRegionMapping
                    .Where(bum => bum.regionCode == irmaRegion && storesInIrma.Where(x=>x.HasValue).Contains(bum.businessUnit) && !businessUnitsAlreadyChecked.Contains(bum.businessUnit))
                    .OrderByDescending(bum=>bum.businessUnitRegionMappingID)
                    .FirstOrDefault().businessUnit;

                using (IrmaContext irmaContext = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion)))
                {
                    irmaIdentifiers = irmaContext.ValidatedScanCode.OrderByDescending(vsc => vsc.Id).Skip(numScanCodes * loopCount).Take(numScanCodes).Select(vsc=>vsc.ScanCode).ToList();
                }

                List<TestStoreItem> itemsToQueueForStore =
                    (from iis in this.TestIconContext.IRMAItemSubscription
                     join bum in this.TestIconContext.BusinessUnitRegionMapping on iis.regioncode equals bum.regionCode
                     where bum.businessUnit == businessUnit && !iis.identifier.StartsWith("-") && irmaIdentifiers.Contains(iis.identifier)
                     select new { iis.identifier, bum.businessUnit, bum.regionCode })
                    .Distinct()
                    .OrderBy(i => i.identifier)
                    .Take(NumberOfTransactionsPerStore * NumberOfItemsPerTransaction)
                    .ToList()
                    .Select(i => new TestStoreItem(i.regionCode, i.identifier, i.businessUnit))
                    .ToList();

                businessUnitsAlreadyChecked.Add(businessUnit);
                if (itemsToQueueForStore.Count == NumberOfTransactionsPerStore * NumberOfItemsPerTransaction)
                {
                    numberOfStoresWithEnoughValidItems++;
                    AllItemsToQueue.AddRange(itemsToQueueForStore);
                }
            } while (numberOfStoresWithEnoughValidItems < NumberOfStorePerRegionsToGetItemsFor && stopwatch.Elapsed < TimeSpan.FromSeconds(30));            
            
            //store all codes being used for the test in a class-wide structure
            //StagedRegionScanCodes.Add(irmaRegion, itemsToQueue.Select(each=>each.ScanCode).ToList());

            // queue items in ICON ItemMovement table
            int lastMessageId = CreateQueuedItemsForRegion(irmaRegion, AllItemsToQueue, transactionDate);
            if (allowDuplicates)
            {
                lastMessageId = CreateQueuedItemsForRegion(irmaRegion, AllItemsToQueue, transactionDate, lastMessageId );
            }

            PopulateCleanupScriptsForItemMovement(transactionDate, RegisterNumber);

            //return number of distinct business units added.
            var uniqueStoreCount =  AllItemsToQueue.Select(each=>each.BusinessUnit_ID).Distinct().Count();
            return uniqueStoreCount;
        }

        private int CreateQueuedItemsForRegion(string irmaRegion, List<TestStoreItem> testStoreItems, DateTime tranDate, int lastMessageId = 0)
        {
            int messageId = lastMessageId;
            int transactionSequenceNumber = 0;
            int lineItemNumber = 0;

            foreach (var businessUnit in testStoreItems.Select(each => each.BusinessUnit_ID).Distinct())
            {
                messageId++;
                transactionSequenceNumber = 1;

                for (int i=0; i< NumberOfTransactionsPerStore; i++)
                {
                    lineItemNumber = 1;
                    var identifiersForThisTransaction = testStoreItems
                        .Where(each => each.BusinessUnit_ID == businessUnit)
                        .Skip(i * NumberOfItemsPerTransaction)
                        .Take(NumberOfItemsPerTransaction)
                        .Select(each => each.ScanCode);

                    foreach (var itemIdentifier in identifiersForThisTransaction)
                    {
                        var itemMovementRecord =
                            new TestItemMovementBuilder()
                                .WithESBMessageID($"Test{messageId}")
                                .WithIdentifier(itemIdentifier)
                                .WithBusinessUnitID(businessUnit)
                                .WithTransDate(tranDate)
                                .WithItemType(0)
                                .WithRegisterNumber(RegisterNumber)
                                .WithTransactionSequenceNumber(transactionSequenceNumber)
                                .WithLineItemNumber(lineItemNumber)
                                .WithBasePrice(lineItemNumber*0.99m);

                        this.TestIconContext.ItemMovement.Add(itemMovementRecord);
                        lineItemNumber++;
                        this.CountOfStagedItemMovementRecords++;
                    }
                    transactionSequenceNumber++;
                    this.CountOfStagedUniqueTransactions++;
                }

            }

            var recordsSavedCount = this.TestIconContext.SaveChanges();

            return recordsSavedCount;
        }

        protected void PopulateCleanupScriptsForItemMovement(DateTime transactionDate, int registerNumber)
        {
            var sqlToCleanupItemMovement = $"delete app.ItemMovement where TransDate = '{transactionDate}' and RegisterNumber = {RegisterNumber}";
            CleanupScripts.Add(sqlToCleanupItemMovement);
            var sqlToCleanupItemMovementTransHistory = $"delete app.ItemMovementTransactionHistory where TransDate = '{transactionDate}' and RegisterNumber = {RegisterNumber}";
            CleanupScripts.Add(sqlToCleanupItemMovementTransHistory);
        }

        public void StageInvalidItemMovementData(int numberOfItemMovementEntriesToStage)
        {
            for (int i = 0; i < numberOfItemMovementEntriesToStage; i++)
            {
                this.TestIconContext.ItemMovement.Add(new TestItemMovementBuilder()
                        .WithIdentifier("00000")
                        .WithBusinessUnitID(1)
                        .WithTransDate(this.RightNow)
                        .WithRegisterNumber(RegisterNumber)
                        .WithTransactionSequenceNumber(1)
                        .WithLineItemNumber(i + 1));
            }
            this.TestIconContext.SaveChanges();

            string tempSql = @"delete app.ItemMovement where TransDate = '" + this.RightNow +
                         "' and RegisterNumber = " + RegisterNumber.ToString();
            CleanupScripts.Add(tempSql);
        }
    }

    public class TestStoreItem
    {
        public string Region { get; set; }
        public string ScanCode { get; set; }
        public int BusinessUnit_ID { get; set; }

        public TestStoreItem() { }

        public TestStoreItem(string region, string identifier, int businessUnit) : this()
        {
            Region = region;
            ScanCode = identifier;
            BusinessUnit_ID = businessUnit;
        }
    }
}
