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
    public class TlogProcessorPerformanceTest
    {
        private TLogProcessorIntegrationTestHelper helper;

        [TestInitialize]
        public void InitializeData()
        {
            helper = new TLogProcessorIntegrationTestHelper(
                instance: 123,
                regionsToTest: new List<string>() { "FL" },
                registerNumber: 888,
                numberOfStoresPerRegionToGetItemsFor: 10,
                numberOfTransactionsPerStore: 10,
                numberOfItemsPerTransaction: 10);
            helper.InitializeData();
        }

        [TestCleanup]
        public void CleanupData()
        {
            helper.CleanupData();
        }

        [TestMethod]
        public void TlogProcessorPerformanceTest_CurrentDateItemMovementDataProcessed_ItemMovementRemovedAndItemMovementTransactionHistoryInserted()
        {
            using (IrmaContext irmaContext = new IrmaContext(ConnectionBuilder.GetConnection(helper.RegionsToTest.First())))
            {
                helper.StageValidItemMovementData(irmaRegion: helper.RegionsToTest.First(), allowDuplicates: false, transactionDate: helper.RightNow);
            }

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //When
            helper.TestTlogProcessor.Run();

            Console.WriteLine("Tlog Controller finished after running for " + stopWatch.Elapsed.ToString("h'h 'm'm 's's'"));
            stopWatch.Stop();

            //Then
            string sql = $"select count (*) from app.ItemMovementTransactionHistory where RegisterNumber = {helper.RegisterNumber} " +
                         $" and TransDate between '{helper.RightNow.Date.AddDays(-1)}' and '{helper.RightNow.Date.AddDays(1)}' and InsertDate >= '{ DateTime.Today}'";
            var iconItemMovementTransactionHistoryRecordCount = helper.TestIconContext.Database.SqlQuery<int>(sql).FirstOrDefault();

            Assert.AreEqual(helper.CountOfStagedUniqueTransactions, iconItemMovementTransactionHistoryRecordCount);
            
            sql = $"select count (*) from app.ItemMovement where RegisterNumber = {helper.RegisterNumber} " +
                         $" and TransDate between '{helper.RightNow.Date.AddDays(-1)}' and '{helper.RightNow.Date.AddDays(1)}' and InsertDate >= '{ DateTime.Today}'";
            var iconItemMovementRecordCount = helper.TestIconContext.Database.SqlQuery<int>(sql).FirstOrDefault();
            
            Assert.AreEqual(0, iconItemMovementRecordCount);
        }
    }
}
