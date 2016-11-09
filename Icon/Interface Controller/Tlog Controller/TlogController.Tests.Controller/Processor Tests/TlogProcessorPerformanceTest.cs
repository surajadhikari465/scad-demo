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
using System.Text;
using TlogController.Controller.ProcessorModules;
using TlogController.DataAccess.Queries;

namespace TlogController.Tests.Controller.Processor_Tests
{
    [TestClass]
    public class TlogProcessorPerformanceTest
    {
        private IconContext iconContext;
        private TlogProcessor tlogProcessor;

        private List<string> cleanupScripts = new List<string>();
        private List<string> regionsToTest = new List<string>();
        private DateTime rightnow;
        private int uniqueTransactionCreated = 0; 

        private const int RegisterNumber = 888;
        private const int NumberOfStagedItemMovements = 10000; //split into more than 1000 unique transcations and it took 1.5 minutes to process.
        private const int NumberOfStagedUniqueTransactionsPerStore = 10;

        [TestInitialize]
        public void InitializeData()
        {
            StartupOptions.Instance = 123;
            StartupOptions.MaxTransactionsToProcess = 300;
            StartupOptions.MaxTlogTransactionsWhenSplit = 50;

            iconContext = new IconContext();

            var mockLogger = new Mock<ILogger<TlogProcessor>>();
            
            tlogProcessor = new TlogProcessor(mockLogger.Object, BuildIconTlogProcessorModule());

            rightnow = DateTime.Parse(DateTime.Now.ToShortTimeString());

            string sql = @"delete app.ItemMovement";
            int returnCode = iconContext.Database.ExecuteSqlCommand(sql);

            sql = @"delete app.ItemMovementTransactionHistory where RegisterNumber = " + RegisterNumber;
            returnCode = iconContext.Database.ExecuteSqlCommand(sql);
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
                string connectionString = ConnectionBuilder.GetConnection(irmaRegion);
                IrmaContext db = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion.ToString()));

                string tempSql = @"delete sales_sumbyitem
                                   where Date_Key >= '" + rightnow.AddDays(-1).Date + "'";
                int returnCode = db.Database.ExecuteSqlCommand(tempSql);

                tempSql = @"delete TlogReprocessRequest
                                   where Date_Key = '" + rightnow.AddDays(-1).Date + "'";
                returnCode = db.Database.ExecuteSqlCommand(tempSql);
            }
        }

        private IIconTlogProcessorModule BuildIconTlogProcessorModule()
        {
            return new IconTlogProcessorModule(
                new NLogLoggerInstance<IconTlogProcessorModule>(StartupOptions.Instance.ToString()),
                new GetBusinessUnitToRegionCodeMappingQueryHandler(iconContext),
                new BulkUpdateItemMovementInProcessCommandHandler(iconContext),
                new BulkUpdateItemMovementCommandHandler(new Mock<ILogger<BulkUpdateItemMovementCommandHandler>>().Object, iconContext));
        }

        private int StageItemMovementData(bool allowDuplicates, DateTime tranDate)
        {
            string tempSql;

            Dictionary<string, string> toBeQueuedItems = new Dictionary<string, string>();

            int index = 0;
            var recordsToBeQueued = new List<Tuple<string, string, string>>();
            while(recordsToBeQueued.Count < NumberOfStagedItemMovements)
            {
                var nextRecordSet = (from iis in this.iconContext.IRMAItemSubscription
                                     join bum in this.iconContext.BusinessUnitRegionMapping on iis.regioncode equals bum.regionCode
                                     select new { iis.identifier, bum.businessUnit, bum.regionCode })
                                     .OrderBy(r => r.identifier)
                                     .Skip(index * NumberOfStagedItemMovements)
                                     .Take(NumberOfStagedItemMovements)
                                     .Distinct()
                                     .ToList()
                                     .Select(r => Tuple.Create(r.identifier, r.businessUnit.ToString(), r.regionCode));

                recordsToBeQueued.AddRange(nextRecordSet);
                recordsToBeQueued = recordsToBeQueued.Distinct().ToList();

                index++;
            }

            var toBeQueued = recordsToBeQueued.Select(r => new
            {
                key = r.Item1 + "-" + r.Item2,
                value = r.Item2 + "-" + r.Item3
            }).ToList();

            //var toBeQueued = (from iis in iconContext.IRMAItemSubscription
            //                  join bum in iconContext.BusinessUnitRegionMapping on iis.regioncode equals bum.regionCode
            //                  select new {
            //                      key = iis.identifier + "-" + bum.businessUnit,
            //                    value = bum.businessUnit + "-" + bum.regionCode
            //                  }).Distinct().Take(NumberOfStagedItemMovements).ToList();

            //Sort toBeQueued list by businessUnit because the same transaction comes from the same businessUnit (store)
            foreach (var toBeQueuedItem in toBeQueued.OrderBy(value => value.value))
            {
                string region = toBeQueuedItem.value.Substring(toBeQueuedItem.value.IndexOf('-') + 1, toBeQueuedItem.value.Length - toBeQueuedItem.value.IndexOf('-') - 1);
                if (!regionsToTest.Contains(region))
                {
                    regionsToTest.Add(region);
                }

                toBeQueuedItems.Add(toBeQueuedItem.key, toBeQueuedItem.value.Substring(0, toBeQueuedItem.value.IndexOf('-')));
            }
            
            int firstMessageId = 1;

            int LastMessageId = CreateQueuedItems(toBeQueuedItems, firstMessageId, tranDate);

            if (allowDuplicates)
            {
                LastMessageId = CreateQueuedItems(toBeQueuedItems, LastMessageId, tranDate);
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

        private int CreateQueuedItems(Dictionary<string, string> queuedItems, int messageId, DateTime tranDate)
        {
            int counter = 1;
            string insertSql;
            string sql;
            int transactionSequenceNumber = 1;
            int lineItemNumber = 1;
            bool firstTransacation = true;
            string preBusinessUnit = String.Empty;
            int lineitemCountInStore = 0;
            int lineitemsInEachTransaction = 0;
            int lineitemsInFirstTransaction = 0;
            
            StringBuilder insertedValues = new StringBuilder();

            foreach (KeyValuePair<string, string> queueItem in queuedItems)
            {
                if (queueItem.Value != preBusinessUnit) //when business unit changes, it'll start a new transaction.
                {
                    firstTransacation = true;
                    preBusinessUnit = queueItem.Value;
                    lineitemCountInStore = queuedItems.Count(q => q.Value == queueItem.Value);
                    lineitemsInEachTransaction = lineitemCountInStore < NumberOfStagedUniqueTransactionsPerStore ? 1 : (lineitemCountInStore / NumberOfStagedUniqueTransactionsPerStore);
                    lineitemsInFirstTransaction = lineitemCountInStore < NumberOfStagedUniqueTransactionsPerStore ? lineitemCountInStore : lineitemCountInStore - (NumberOfStagedUniqueTransactionsPerStore * lineitemsInEachTransaction) + lineitemsInEachTransaction;
                    transactionSequenceNumber = 1;
                    lineItemNumber = 1;
                    messageId++;
                    uniqueTransactionCreated++;
                }              

                //Split the entries in the same business unit into multiple transactions
                if ((firstTransacation && lineItemNumber > lineitemsInFirstTransaction) || (!firstTransacation && lineItemNumber > lineitemsInEachTransaction))
                {
                    lineItemNumber = 1;
                    messageId++;
                    transactionSequenceNumber++;
                    firstTransacation = false;
                    uniqueTransactionCreated++;
                }

                insertedValues.Append("('TestMessage" + messageId.ToString() + "',");
                insertedValues.Append(queueItem.Value + ",");
                insertedValues.Append(RegisterNumber + ",");
                insertedValues.Append(transactionSequenceNumber + ",");
                insertedValues.Append(lineItemNumber + ",");
                insertedValues.Append("'" + queueItem.Key.Substring(0, queueItem.Key.IndexOf('-')) + "',");
                insertedValues.Append("'" + tranDate + "',");
                insertedValues.Append(lineItemNumber + ",");
                insertedValues.Append(0 + ",");
                insertedValues.Append(0 + ",");
                insertedValues.Append(3.99 + ",");
                insertedValues.Append(0 + ",");
                insertedValues.Append(0 + ",");
                insertedValues.Append("'" + DateTime.Now + "'),");

                if (counter % 50 == 0)
                {
                    insertSql = insertedValues.ToString();
                    sql = @"INSERT INTO [app].[ItemMovement]([ESBMessageID],[BusinessUnitID],[RegisterNumber],[TransactionSequenceNumber],[LineItemNumber]
                                ,[Identifier],[TransDate],[Quantity],[ItemVoid],[ItemType],[BasePrice],[Weight],[MarkDownAmount],[InsertDate])
                                VALUES" + insertSql.Substring(0, insertSql.Length - 1);
                    iconContext.Database.ExecuteSqlCommand(sql);
                    insertedValues = new StringBuilder();
                }

                lineItemNumber++;
                counter++;
            }

            return messageId;
        }

        [TestMethod]
        public void TlogProcessorPerformanceTest_CurrentDateItemMovementDataProcessed_ItemMovementRemovedAndItemMovementTransactionHistoryInserted()
        {
            StageItemMovementData(false, rightnow);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //When
            tlogProcessor.Run();

            Console.WriteLine("Tlog Controller finished after running for " + stopWatch.Elapsed.ToString("h'h 'm'm 's's'"));
            stopWatch.Stop();

            //Then
            //string sql = @"select count (*) from app.ItemMovementTransactionHistory
            //                            where TransDate = '" + rightnow + "' and RegisterNumber = " + RegisterNumber +
            //                " and InsertDate > '" + DateTime.Today + "'";
            string sql = $"select count (*) from app.ItemMovementTransactionHistory where RegisterNumber = {RegisterNumber} " +
                         $" and TransDate between '{rightnow.Date.AddDays(-1)}' and '{rightnow.Date.AddDays(1)}' and InsertDate >= '{ DateTime.Today}'";
            var iconItemMovementTransactionHistoryRecordCount = iconContext.Database.SqlQuery<int>(sql).FirstOrDefault();

            Assert.AreEqual(uniqueTransactionCreated, iconItemMovementTransactionHistoryRecordCount);

            //sql = @"select count (*) from app.ItemMovement
            //                            where TransDate = '" + rightnow + "' and RegisterNumber = " + RegisterNumber +
            //                " and InsertDate > '" + DateTime.Today + "'";
            sql = $"select count (*) from app.ItemMovement where RegisterNumber = {RegisterNumber} " +
                         $" and TransDate between '{rightnow.Date.AddDays(-1)}' and '{rightnow.Date.AddDays(1)}' and InsertDate >= '{ DateTime.Today}'";
            var iconItemMovementRecordCount = iconContext.Database.SqlQuery<int>(sql).FirstOrDefault();
            
            Assert.AreEqual(0, iconItemMovementRecordCount);
        }
    }
}
