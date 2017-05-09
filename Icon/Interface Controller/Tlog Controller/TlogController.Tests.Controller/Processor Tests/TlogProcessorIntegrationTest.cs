using Irma.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TlogController.DataAccess.Infrastructure;

namespace TlogController.Tests.Controller.Processor_Tests
{
    [TestClass]
    [Ignore] //Ignoring TLog tests because they have an indeterminate behavior and should be fixed separately
    public class TlogProcessorIntegrationTest
    {
        private TLogProcessorIntegrationTestHelper helper;

        [TestInitialize]
        public void InitializeData()
        {
            helper = new TLogProcessorIntegrationTestHelper(
                instance: 123,
                regionsToTest: new List<string>() { "FL" },
                registerNumber: 999,
                numberOfStoresPerRegionToGetItemsFor: 1,
                numberOfTransactionsPerStore: 1,
                numberOfItemsPerTransaction: 3);
            helper.InitializeData();
        }

        [TestCleanup]
        public void CleanupData()
        {
            helper.CleanupData();
        }

        [TestMethod]
        public void RunTlogProcessor_CurrentDateItemMovementDataProcessed_IrmaNewSales_SumByitemCreated()
        {
            //Given
            foreach (string irmaRegion in helper.RegionsToTest)
            {
                helper.StageValidItemMovementData(irmaRegion: irmaRegion, allowDuplicates: false,  transactionDate:helper.RightNow);
            }

            //When
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            helper.TestTlogProcessor.Run();
            stopWatch.Stop();
            Console.WriteLine("Tlog Controller finished after running for " + stopWatch.Elapsed.ToString("h'h 'm'm 's's'"));

            //Then
            foreach (string irmaRegion in helper.RegionsToTest)
            {
                using (IrmaContext irmaContext = new IrmaContext(ConnectionBuilder.GetConnection(irmaRegion)))
                {
                    string sqlToQuerySalesSumByItem = $"select count(*) from sales_sumbyitem where Date_Key = '{DateTime.Today}'";
                    var irmaNewSalesRecordCount = irmaContext.Database.SqlQuery<int>(sqlToQuerySalesSumByItem).FirstOrDefault();
                    Assert.AreEqual(helper.CountOfStagedItemMovementRecords, irmaNewSalesRecordCount);
                    var regionalScanCodesCount = helper.AllItemsToQueue.Where(each => each.Region == irmaRegion).Count();
                    Assert.AreEqual(regionalScanCodesCount, irmaNewSalesRecordCount);
                }
            }

            string sqlQuery = "select count (*) from app.ItemMovementTransactionHistory" +
                $" where TransDate = '{helper.RightNow}' and RegisterNumber = {helper.RegisterNumber} and InsertDate > '{DateTime.Today}'";
            var iconItemMovementTransactionHistoryRecordCount = helper.TestIconContext.Database.SqlQuery<int>(sqlQuery).FirstOrDefault();
            int expectedTransactionHistoryCount = helper.CountOfStagedUniqueTransactions;
            Assert.AreEqual(expectedTransactionHistoryCount, iconItemMovementTransactionHistoryRecordCount);

            sqlQuery = "select count (*) from app.ItemMovement" +
                 $" where TransDate = '{helper.RightNow}' and RegisterNumber = {helper.RegisterNumber} and InsertDate > '{DateTime.Today}'";
            var iconItemMovementRecordCount = helper.TestIconContext.Database.SqlQuery<int>(sqlQuery).FirstOrDefault();
            int expectedItemMovementCount = 0;
            Assert.AreEqual(expectedItemMovementCount, iconItemMovementRecordCount);
        }

        [TestMethod]
        public void RunTlogProcessor_InvalidItemMovementDataProcessed_ItemMovementDataMarkedAsFailedToBeProcessed()
        {
            int invalideItemMovementCount = 3;
            //Given
            helper.StageInvalidItemMovementData(invalideItemMovementCount);

            //When
            helper.TestTlogProcessor.Run();

            //Then

            string tempSql = @"select count (*) from app.ItemMovement
                             where TransDate = '" + helper.RightNow + "' and ProcessFailedDate > '" + helper.RightNow + "'";
            var failedToProcesstCount = helper.TestIconContext.Database.SqlQuery<int>(tempSql).FirstOrDefault();

            Assert.AreEqual(invalideItemMovementCount, failedToProcesstCount);
        }
    }
}
