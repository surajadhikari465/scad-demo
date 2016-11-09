using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Reflection;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Framework;
using Icon.Logging;
using Irma.Framework;
using TlogController.DataAccess.BulkCommands;
using TlogController.Common;
using TlogController.Controller.Processors;
using Moq;
using Icon.Testing.Builders;
using TlogController.DataAccess.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using TlogController.Controller.ProcessorModules;
using TlogController.DataAccess.Queries;
using System.Text;

namespace TlogController.Tests.Controller.Processor_Tests
{
    [TestClass]
    public class TlogProcessorIntegrationTest
    {
        private IconContext iconContext;
        private TlogProcessor tlogProcessor;

        private List<string> cleanupScripts = new List<string>();
        private string[] regionsToTest = new string[2] { "FL","SO" };
        private Dictionary<string, string> stagedRegionScanCodes = new Dictionary<string, string>();
        private Dictionary<string, List<string>> stagedRegionScanCodez = new Dictionary<string, List<string>>();
        private DateTime rightnow;
        private int lastMessageId;
        private const int RegisterNumber = 999;
        private const int NumberOfStagedItemMovements = 10; //200;
        private const int NumberOfStagedUniqueTransactions = 23;

        [TestInitialize]
        public void InitializeData()
        {
            StartupOptions.Instance = 123;
            StartupOptions.MaxTransactionsToProcess = 50;
            StartupOptions.MaxTlogTransactionsWhenSplit = 5;

            this.iconContext = new IconContext();

            var mockLogger = new Mock<ILogger<TlogProcessor>>();

            this.tlogProcessor = new TlogProcessor(
                mockLogger.Object, 
                this.BuildIconTlogProcessorModule());

            this.rightnow = DateTime.Parse(DateTime.Now.ToShortTimeString());

            string sql = @"delete app.ItemMovement";
            iconContext.Database.ExecuteSqlCommand(sql);

            sql = @"delete app.ItemMovementTransactionHistory where RegisterNumber = " + RegisterNumber;
            iconContext.Database.ExecuteSqlCommand(sql);
        }

        [TestCleanup]
        public void CleanupData()
        {
            foreach (string sql in cleanupScripts)
            {
                int returnCode = iconContext.Database.ExecuteSqlCommand(sql);
            }

            foreach (string irmaRegion in regionsToTest)
            {
                //Given
                IrmaContext db = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));

                string tempSql = @"delete sales_sumbyitem
                                   where Date_Key >= '" + rightnow.AddDays(-1).Date + "'";
                int returnCode = db.Database.ExecuteSqlCommand(tempSql);

                tempSql = @"delete TlogReprocessRequest
                                   where Date_Key = '" + rightnow.AddDays(-1).Date + "'";
                returnCode = db.Database.ExecuteSqlCommand(tempSql);
            }
        }

        [TestMethod]
        public void RunTlogProcessor_CurrentDateItemMovementDataProcessed_IrmaNewSales_SumByitemCreated_old()
        {
            foreach (string irmaRegion in regionsToTest)
            {
                //Given
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext db = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));

                StageItemMovementData_old(irmaRegion, db, true, rightnow);
            }

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //When
            tlogProcessor.Run();

            Console.WriteLine("Tlog Controller finished after running for " + stopWatch.Elapsed.ToString("h'h 'm'm 's's'"));
            stopWatch.Stop();

            //Then
            foreach (string irmaRegion in regionsToTest)
            {
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext db = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));

                string tempSql = @"select count (*) from sales_sumbyitem
                                               where Date_Key = '" + DateTime.Today + "'";
                var irmaNewSalesRecordCount = db.Database.SqlQuery<int>(tempSql).FirstOrDefault();

                var regionalScanCodes = stagedRegionScanCodes.Select(r => r).Where(r => r.Value == irmaRegion).ToDictionary(r => r.Key, r => r.Value);
                Assert.AreEqual(regionalScanCodes.Count(), irmaNewSalesRecordCount);
            }

            string sql = @"select count (*) from app.ItemMovementTransactionHistory
                                        where TransDate = '" + rightnow + "' and RegisterNumber = " + RegisterNumber +
                            " and InsertDate > '" + DateTime.Today + "'";
            var iconItemMovementTransactionHistoryRecordCount = iconContext.Database.SqlQuery<int>(sql).FirstOrDefault();

            Assert.AreEqual(NumberOfStagedUniqueTransactions * regionsToTest.Count(), iconItemMovementTransactionHistoryRecordCount);

            sql = @"select count (*) from app.ItemMovement
                                        where TransDate = '" + rightnow + "' and RegisterNumber = " + RegisterNumber +
                            " and InsertDate > '" + DateTime.Today + "'";
            var iconItemMovementRecordCount = iconContext.Database.SqlQuery<int>(sql).FirstOrDefault();

            Assert.AreEqual(0, iconItemMovementRecordCount);

        }

        [TestMethod]
        public void RunTlogProcessor_CurrentDateItemMovementDataProcessed_IrmaNewSales_SumByitemCreated()
        {
           // Dictionary<string,int>
            //Given
            foreach (string irmaRegion in regionsToTest)
            {
                using (IrmaContext db = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion)))
                {
                    //StageItemMovementData(irmaRegion, db, true, rightnow);
                    StageItemMovementData_new(irmaRegion, db, true, rightnow);
                }
            }

            //When
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            tlogProcessor.Run();
            stopWatch.Stop();
            Console.WriteLine("Tlog Controller finished after running for " + stopWatch.Elapsed.ToString("h'h 'm'm 's's'"));

            //Then
            foreach (string irmaRegion in regionsToTest)
            {
                //if (irmaRegion == "MW") continue;
                using (IrmaContext irmaContext = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion)))
                {
                    //string sqlToQuerySalesSumByItem = $"select count(*) from (select date_key, item_key from sales_sumbyitem where Date_Key = '{DateTime.Today}' group by date_key, item_key) as x";
                    string sqlToQuerySalesSumByItem = $"select count(*) from sales_sumbyitem where Date_Key = '{DateTime.Today}'";
                    var irmaNewSalesRecordCount = irmaContext.Database.SqlQuery<int>(sqlToQuerySalesSumByItem).FirstOrDefault();
                    List<string> temp = new List<string>();
                    stagedRegionScanCodez.TryGetValue(irmaRegion, out temp);
                    var regionalScanCodesCount = temp.Count;
                    //var regionalScanCodesCount = stagedRegionScanCodes.Where(r => r.Value == irmaRegion).Count();
                    Assert.AreEqual(regionalScanCodesCount, irmaNewSalesRecordCount);
                }
            }

            string sqlQuery = "select count (*) from app.ItemMovementTransactionHistory" +
                $" where TransDate = '{rightnow}' and RegisterNumber = {RegisterNumber} and InsertDate > '{DateTime.Today}'";
            var iconItemMovementTransactionHistoryRecordCount = iconContext.Database.SqlQuery<int>(sqlQuery).FirstOrDefault();
            int expectedTransactionHistoryCount = NumberOfStagedUniqueTransactions * regionsToTest.Length;
            Assert.AreEqual(expectedTransactionHistoryCount, iconItemMovementTransactionHistoryRecordCount);

            sqlQuery = "select count (*) from app.ItemMovement" +
                 $"where TransDate = '{rightnow}' and RegisterNumber = {RegisterNumber} and InsertDate > '{DateTime.Today}'";
            var iconItemMovementRecordCount = iconContext.Database.SqlQuery<int>(sqlQuery).FirstOrDefault();
            int expectedItemMovementCount = 0;
            Assert.AreEqual(expectedItemMovementCount, iconItemMovementRecordCount);
        }

        [TestMethod]
        public void RunTlogProcessor_PastDateItemMovementDataProcessed_IrmaTlogReprocessRequestCreated()
        {
            //Given
            string connectionString = ConnectionBuilder.GetConnection(regionsToTest[0]);
            IrmaContext db = new IrmaContext(ConnectionBuilder.GetConnection(regionsToTest[0].ToString()));

            int businessUnitCount = StageItemMovementData_old(regionsToTest[0], db, true, rightnow.AddDays(-1));

            //When
            tlogProcessor.Run();

            //Then

            string tempSql = @"select count (*) from TlogReprocessRequest
                                               where Date_Key = '" + rightnow.AddDays(-1).Date + "'";
            var tlogReprocessRequestCount = db.Database.SqlQuery<int>(tempSql).FirstOrDefault();

            Assert.AreEqual(businessUnitCount, tlogReprocessRequestCount);
        }

        [TestMethod]
        public void RunTlogProcessor_InvalidItemMovementDataProcessed_ItemMovementDataMarkedAsFailedToBeProcessed()
        {
            int invalideItemMovementCount = 3;
            //Given
            StageInvalidItemMovementData(invalideItemMovementCount);

            //When
            tlogProcessor.Run();

            //Then

            string tempSql = @"select count (*) from app.ItemMovement
                             where TransDate = '" + rightnow + "' and ProcessFailedDate > '" + rightnow + "'";
            var failedToProcesstCount = iconContext.Database.SqlQuery<int>(tempSql).FirstOrDefault();

            Assert.AreEqual(invalideItemMovementCount, failedToProcesstCount);
        }

        private IIconTlogProcessorModule BuildIconTlogProcessorModule()
        {
            return new IconTlogProcessorModule(
                new NLogLoggerInstance<IconTlogProcessorModule>(StartupOptions.Instance.ToString()),
                new GetBusinessUnitToRegionCodeMappingQueryHandler(iconContext),
                new BulkUpdateItemMovementInProcessCommandHandler(iconContext),
                new BulkUpdateItemMovementCommandHandler(new Mock<ILogger<BulkUpdateItemMovementCommandHandler>>().Object, iconContext));
        }

        private int? GetFirstBusinessUnitForIdentifier(IrmaContext db, string validatedIdentifier)
        {
            string tempSql = @"select top 1 BusinessUnit_Id from StoreItem si
                                join Store s on s.Store_No = si.Store_No and Authorized = 1
                                join ItemIdentifier ii on ii.Item_Key= si.Item_Key and ii.Deleted_Identifier = 0 and Remove_Identifier = 0 and ii.Default_Identifier = 1
                                join ValidatedScanCode vs on vs.ScanCode = ii.Identifier
                                where ii.Identifier = '" + validatedIdentifier + "'";
            var businessUnitIds = db.Database.SqlQuery<int>(tempSql).ToList();
            if (businessUnitIds != null && businessUnitIds.Count > 0)
            {
                return businessUnitIds.First();
            }
            return null;
        }

        private List<string> GetIconValidatedScanCodes(int numToTake, List<string> previouslyValidatedScanCodes = null)
        {
            if (previouslyValidatedScanCodes == null) previouslyValidatedScanCodes = new List<string>();

            List<string> newlyRetrievedValidatedScanCodes = 
                (from sc in iconContext.ScanCode.AsQueryable<ScanCode>()
                join it in iconContext.ItemTrait on sc.itemID equals it.itemID
                join t in iconContext.Trait on it.traitID equals t.traitID
                where t.traitCode == TraitCodes.ValidationDate && !previouslyValidatedScanCodes.Contains(sc.scanCode)
                select sc.scanCode)
                .Take(numToTake)
                .ToList();
            return newlyRetrievedValidatedScanCodes;
        }

        private int StageItemMovementData_old(string irmaRegion, IrmaContext db, bool allowDuplicates, DateTime tranDate)
        {
            List<string> validatedScanCode = new List<string>();
            List<string> newlyRetrievedValidatedScanCodes;
            string tempSql;

            Dictionary<string, int> toBeQueuedItems = new Dictionary<string, int>();

            int i = 0;
            do
            {
                newlyRetrievedValidatedScanCodes = new List<string>();

                newlyRetrievedValidatedScanCodes.AddRange((from sc in iconContext.ScanCode.AsQueryable<ScanCode>()
                                                           join it in iconContext.ItemTrait on sc.itemID equals it.itemID
                                                           join t in iconContext.Trait on it.traitID equals t.traitID
                                                           where t.traitCode == TraitCodes.ValidationDate
                                                              && !validatedScanCode.Contains(sc.scanCode)
                                                           select sc.scanCode).Take(NumberOfStagedItemMovements).ToList());

                validatedScanCode.AddRange(newlyRetrievedValidatedScanCodes);

                foreach (string validatedIdentifier in newlyRetrievedValidatedScanCodes)
                {
                    if (!stagedRegionScanCodes.ContainsKey(validatedIdentifier))
                    {
                        tempSql = @"select top 1 BusinessUnit_Id from StoreItem si
                                            join Store i on si.Store_No = si.Store_No and Authorized = 1
                                            join ItemIdentifier ii on si.Item_Key = ii.Item_Key and ii.Deleted_Identifier = 0 and Remove_Identifier = 0 and ii.Default_Identifier = 1
                                            join ValidatedScanCode vs on vs.ScanCode = ii.Identifier
                                            where ii.Identifier = '" + validatedIdentifier + "'";
                        var businessUnitId = db.Database.SqlQuery<int>(tempSql).ToList();

                        if (businessUnitId != null && businessUnitId.Count > 0)
                        {
                            stagedRegionScanCodes.Add(validatedIdentifier, irmaRegion);
                            toBeQueuedItems.Add(validatedIdentifier, businessUnitId[0]);
                            i++;
                        }
                        if (i > NumberOfStagedItemMovements - 1)
                            break;
                    }
                }
            } while (i < NumberOfStagedItemMovements);

            int firstMessageId = 1;

            lastMessageId = CreateQueuedItems(toBeQueuedItems, firstMessageId, tranDate);

            if (allowDuplicates)
            {
                lastMessageId = CreateQueuedItems(toBeQueuedItems, lastMessageId + 1, tranDate);
            }

            tempSql = @"delete app.ItemMovement where TransDate = '" + tranDate +
                     "' and RegisterNumber = " + RegisterNumber.ToString();
            cleanupScripts.Add(tempSql);

            tempSql = @"delete app.ItemMovementTransactionHistory where TransDate = '" + tranDate +
                      "' and RegisterNumber = " + RegisterNumber.ToString();
            cleanupScripts.Add(tempSql);

            //return number of dixtinct of business units added.
            return toBeQueuedItems.Values.Distinct().Count();
        }

        private class StoreItem
        {
            public string ScanCode { get; set; }
            public int BusinessUnit_ID { get; set; }

            public StoreItem()
            {

            }
            public StoreItem(string identifier, int businessUnit)
            {
                ScanCode = identifier;
                BusinessUnit_ID = businessUnit;
            }
        }

        private Dictionary<string, int> FindValidIdentifiersWithAuthorizedStore(IrmaContext irmaDb, int desiredItemCount)
        {
            ////query ICON for some valid identifiers
            //int numToTake = desiredItemCount * 10;
            //List<string> validIconScanCodes =
            //    (from sc in iconDb.ScanCode.AsQueryable<ScanCode>()
            //    join it in iconDb.ItemTrait on sc.itemID equals it.itemID
            //    join t in iconDb.Trait on it.traitID equals t.traitID
            //    where t.traitCode == TraitCodes.ValidationDate
            //    select sc.scanCode)
            //   .Take(numToTake)
            //   .ToList();

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(" select top ");
            sqlBuilder.Append(desiredItemCount);
            sqlBuilder.Append(" vsc.ScanCode, s.BusinessUnit_ID");
            sqlBuilder.Append(" from ValidatedScanCode vsc");
            sqlBuilder.Append(" cross apply (");
            sqlBuilder.Append("    select top 1 Item_Key from ItemIdentifier");
            sqlBuilder.Append("    where Identifier = vsc.ScanCode and Deleted_Identifier = 0 and Remove_Identifier = 0 and Default_Identifier = 1");
            sqlBuilder.Append(") as ii");
            sqlBuilder.Append(" cross apply (");
            sqlBuilder.Append("    select top 1 Store.BusinessUnit_ID");
            sqlBuilder.Append("    from StoreItem inner join Store on StoreItem.Store_No = Store.Store_No");
            sqlBuilder.Append("    where StoreItem.Item_Key = ii.Item_Key and StoreItem.Authorized = 1");
            sqlBuilder.Append(") as s");
            
            var codeStorePairs = irmaDb.Database.SqlQuery<StoreItem>(sqlBuilder.ToString()).ToList();

            Dictionary<string, int> testData = new Dictionary<string, int>();
            if (codeStorePairs != null)
            {
                foreach (var pair in codeStorePairs)
                {
                    testData.Add(pair.ScanCode, pair.BusinessUnit_ID);
                }
            }
            return testData;
        }


        private int StageItemMovementData_new(string irmaRegion, IrmaContext irmaContext, bool allowDuplicates, DateTime transactionDate)
        {
            //query irma for some valid identifiers with appropriate business units
            Dictionary<string, int> toBeQueuedItems = FindValidIdentifiersWithAuthorizedStore(irmaContext, NumberOfStagedItemMovements);

            //store all codes being used for the test in a class-wide structure
            stagedRegionScanCodez.Add(irmaRegion, toBeQueuedItems.Keys.Distinct().ToList());

            // queue items in ICON ItemMovement table
            lastMessageId = CreateQueuedItems(toBeQueuedItems, 1, transactionDate);
            if (allowDuplicates)
            {
                lastMessageId = CreateQueuedItems(toBeQueuedItems, lastMessageId + 1, transactionDate);
            }

            PopulateCleanupScriptsForItemMovement(transactionDate, RegisterNumber);

            //return number of distinct business units added.
            return toBeQueuedItems.Values.Distinct().Count();
        }

        private void PopulateCleanupScriptsForItemMovement(DateTime transactionDate, int registerNumber)
        {
            var sqlToCleanupItemMovement = $"delete app.ItemMovement where TransDate = '{transactionDate}' and RegisterNumber = {RegisterNumber}";
            cleanupScripts.Add(sqlToCleanupItemMovement);
            var sqlToCleanupItemMovementTransHistory = $"delete app.ItemMovementTransactionHistory where TransDate = '{transactionDate}' and RegisterNumber = {RegisterNumber}";
            cleanupScripts.Add(sqlToCleanupItemMovementTransHistory);
        }

        private int StageItemMovementData(string irmaRegion, IrmaContext irmaContext, bool allowDuplicates, DateTime transactionDate)
        {
            List<string> allValidatedScanCodes = new List<string>();
            int numberOfItemsToStage = NumberOfStagedItemMovements;
            List<string> newlyRetrievedValidatedScanCodes = new List<string>(numberOfItemsToStage);
            Dictionary <string, int> toBeQueuedItems = new Dictionary<string, int>();
            
            do
            {
                //get a batch of valid scan codes
                newlyRetrievedValidatedScanCodes = GetIconValidatedScanCodes(numberOfItemsToStage, allValidatedScanCodes);
                allValidatedScanCodes.AddRange(newlyRetrievedValidatedScanCodes);

                foreach (string validatedIdentifier in newlyRetrievedValidatedScanCodes)
                {
                    if (!stagedRegionScanCodes.ContainsKey(validatedIdentifier))
                    {
                        //find a store which uses the scan code
                        var businessUnitId = GetFirstBusinessUnitForIdentifier(irmaContext, validatedIdentifier);
                        if (businessUnitId.HasValue)
                        {
                            stagedRegionScanCodes.Add(validatedIdentifier, irmaRegion);
                            if (!toBeQueuedItems.Keys.Contains(validatedIdentifier))
                            {
                                toBeQueuedItems.Add(validatedIdentifier, businessUnitId.Value);
                                if (toBeQueuedItems.Count >= numberOfItemsToStage) break;
                            }
                        }
                    }
                }
            } while (toBeQueuedItems.Count < numberOfItemsToStage);

            // queue items in ItemMovement table
            lastMessageId = CreateQueuedItems(toBeQueuedItems, 1, transactionDate);
            if (allowDuplicates)
            {
                lastMessageId = CreateQueuedItems(toBeQueuedItems, lastMessageId + 1, transactionDate);
            }

            //populate cleanup scripts
            var sqlToCleanupItemMovement = $"delete app.ItemMovement where TransDate = '{transactionDate}' and RegisterNumber = {RegisterNumber}";
            cleanupScripts.Add(sqlToCleanupItemMovement);
            var sqlToCleanupItemMovementTransHistory = $"delete app.ItemMovementTransactionHistory where TransDate = '{transactionDate}' and RegisterNumber = {RegisterNumber}";
            cleanupScripts.Add(sqlToCleanupItemMovementTransHistory);

            //return number of distinct business units added.
            return toBeQueuedItems.Values.Distinct().Count();
        }

        private int CreateQueuedItems(Dictionary<string, int> queuedItems, int messageId, DateTime tranDate)
        {
            int transactionSequenceNumber = 1;
            int lineItemNumber = 1;
            bool isFirstTransaction = true;
            int lineitemsInEachTransaction = NumberOfStagedItemMovements / NumberOfStagedUniqueTransactions;
            int lineitemsInFirstTransaction = NumberOfStagedItemMovements - (NumberOfStagedUniqueTransactions * lineitemsInEachTransaction) + lineitemsInEachTransaction;

            foreach (KeyValuePair<string, int> queueItem in queuedItems)
            {
                if (( isFirstTransaction && lineItemNumber > lineitemsInFirstTransaction) ||
                    (!isFirstTransaction && lineItemNumber > lineitemsInEachTransaction))
                {
                    lineItemNumber = 1;
                    messageId++;
                    transactionSequenceNumber++;
                    isFirstTransaction = false;
                }

                iconContext.ItemMovement.Add(
                    new TestItemMovementBuilder()
                        .WithESBMessageID("Test" + messageId.ToString())
                        .WithIdentifier(queueItem.Key)
                        .WithBusinessUnitID(queueItem.Value)
                        .WithTransDate(tranDate)
                        .WithRegisterNumber(RegisterNumber)
                        .WithTransactionSequenceNumber(transactionSequenceNumber)
                        .WithLineItemNumber(lineItemNumber)
                );

                lineItemNumber++;
            }

            iconContext.SaveChanges();

            return messageId;
        }
        private void StageInvalidItemMovementData(int numberOfItemMovementEntriesToStage)
        {
            for (int i = 0; i < numberOfItemMovementEntriesToStage; i++)
            {
                iconContext.ItemMovement.Add(new TestItemMovementBuilder()
                        .WithIdentifier("00000")
                        .WithBusinessUnitID(1)
                        .WithTransDate(rightnow)
                        .WithRegisterNumber(RegisterNumber)
                        .WithTransactionSequenceNumber(1)
                        .WithLineItemNumber(i + 1));
            }
            iconContext.SaveChanges();

            string tempSql = @"delete app.ItemMovement where TransDate = '" + rightnow +
                         "' and RegisterNumber = " + RegisterNumber.ToString();
            cleanupScripts.Add(tempSql);
        }
    }
}
