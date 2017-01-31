using Icon.Testing.Builders;
using InterfaceController.Common;
using Irma.Framework;
using Irma.Framework.RenewableContext;
using Irma.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PushController.DataAccess.Commands;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PushController.Tests.DataAccess.Commands
{
    [TestClass]
    public class MarkPublishedRecordsAsInProcessCommandTests
    {
        private MarkPublishedRecordsAsInProcessCommandHandler markPublishedRecordsCommandHandler;
        private IrmaContext irmaContext;
        private GlobalIrmaContext globalContext;
        private IrmaContextProvider contextProvider;
        private DbContextTransaction transaction;
        private List<IConPOSPushPublish> testPublishedRecords;
        private int testStoreNumber;

        [TestInitialize]
        public void Initialize()
        {
            this.contextProvider = new IrmaContextProvider();
            this.irmaContext = contextProvider.GetRegionalContext(ConnectionBuilder.GetConnection("FL"));
            this.globalContext = new GlobalIrmaContext(irmaContext, ConnectionBuilder.GetConnection("FL"));
            this.markPublishedRecordsCommandHandler = new MarkPublishedRecordsAsInProcessCommandHandler();

            this.transaction = irmaContext.Database.BeginTransaction();

            this.testStoreNumber = irmaContext.Store.First(s => s.WFM_Store && s.Internal && s.BusinessUnit_ID.HasValue).Store_No;
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
        }
        
        [TestMethod]
        public void MarkPublishedRecordsAsInProcess_OneRecordIsReady_OneRecordShouldBeMarkedAsInProcess()
        {
            // Given.
            testPublishedRecords = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder().WithStoreNumber(testStoreNumber)
            };

            StageTestPublishedRecords();

            var command = new MarkPublishedRecordsAsInProcessCommand
            {
                Context = irmaContext,
                Instance = 99,
                MaxRecordsToProcess = 10000
            };

            // When.
            this.markPublishedRecordsCommandHandler.Execute(command);

            // Then.
            int markedIconPosPushPublishId = testPublishedRecords[0].IConPOSPushPublishID;
            var markedIconPosPushPublishRecord = irmaContext.IConPOSPushPublish.Single(publish => publish.IConPOSPushPublishID == markedIconPosPushPublishId);

            // Have to reload the entity since the update was done via stored procedure.
            irmaContext.Entry(markedIconPosPushPublishRecord).Reload();

            Assert.AreEqual(command.Instance, markedIconPosPushPublishRecord.InProcessBy);
        }

        [TestMethod]
        public void MarkPublishedRecordsAsInProcess_FiveRecordsAreReady_FiveRecordsShouldBeMarkedAsInProcess()
        {
            // Given.
            var random = new Random();

            testPublishedRecords = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder().WithStoreNumber(testStoreNumber).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))),
                new TestIconPosPushPublishBuilder().WithStoreNumber(testStoreNumber).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))),
                new TestIconPosPushPublishBuilder().WithStoreNumber(testStoreNumber).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))),
                new TestIconPosPushPublishBuilder().WithStoreNumber(testStoreNumber).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))),
                new TestIconPosPushPublishBuilder().WithStoreNumber(testStoreNumber).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000)))
            };

            StageTestPublishedRecords();

            var command = new MarkPublishedRecordsAsInProcessCommand
            {
                Context = irmaContext,
                Instance = 99,
                MaxRecordsToProcess = 10000
            };

            // When.
            markPublishedRecordsCommandHandler.Execute(command);

            // Then.
            var markedIconPosPushPublishById = testPublishedRecords.Select(publish => publish.IConPOSPushPublishID).ToList();
            var markedIconPosPushPublishRecords = irmaContext.IConPOSPushPublish.Where(publish => markedIconPosPushPublishById.Contains(publish.IConPOSPushPublishID)).ToList();

            // Have to reload the entity since the update was done via stored procedure.
            foreach (var publishedRecord in markedIconPosPushPublishRecords)
            {
                irmaContext.Entry(publishedRecord).Reload();
            }

            bool eachRecordIsInProcess = markedIconPosPushPublishRecords.TrueForAll(publish => publish.InProcessBy == command.Instance);

            Assert.IsTrue(eachRecordIsInProcess);
        }

        [TestMethod]
        public void MarkPublishedRecordsAsInProcess_OneRecordIsReadyOneRecordHasProcessedDate_OneRecordShouldBeMarkedAsInProcess()
        {
            // Given.
            var random = new Random();

            testPublishedRecords = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder().WithStoreNumber(testStoreNumber).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithProcessedDate(DateTime.Now),
                new TestIconPosPushPublishBuilder().WithStoreNumber(testStoreNumber).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000)))
            };

            StageTestPublishedRecords();

            var command = new MarkPublishedRecordsAsInProcessCommand
            {
                Context = irmaContext,
                Instance = 99,
                MaxRecordsToProcess = 10000
            };

            // When.
            markPublishedRecordsCommandHandler.Execute(command);

            // Then.
            var markedIconPosPushPublishById = testPublishedRecords.Select(publish => publish.IConPOSPushPublishID).ToList();
            var markedIconPosPushPublishRecords = irmaContext.IConPOSPushPublish.Where(publish => markedIconPosPushPublishById.Contains(publish.IConPOSPushPublishID)).ToList();

            // Have to reload the entity since the update was done via stored procedure.
            foreach (var publishedRecord in markedIconPosPushPublishRecords)
            {
                irmaContext.Entry(publishedRecord).Reload();
            }

            bool eachRecordIsInProcess = markedIconPosPushPublishRecords.TrueForAll(publish => publish.InProcessBy == command.Instance);

            Assert.IsFalse(eachRecordIsInProcess);
            Assert.AreEqual(null, testPublishedRecords[0].InProcessBy);
            Assert.AreEqual(command.Instance, testPublishedRecords[1].InProcessBy);
        }

        [TestMethod]
        public void MarkPublishedRecordsAsInProcess_OneRecordIsReadyOneRecordHasProcessingFailedDate_OneRecordShouldBeMarkedAsInProcess()
        {
            // Given.
            var random = new Random();

            testPublishedRecords = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder().WithStoreNumber(testStoreNumber).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithProcessingFailedDate(DateTime.Now),
                new TestIconPosPushPublishBuilder().WithStoreNumber(testStoreNumber).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000)))
            };

            StageTestPublishedRecords();

            var command = new MarkPublishedRecordsAsInProcessCommand
            {
                Context = irmaContext,
                Instance = 99,
                MaxRecordsToProcess = 10000
            };

            // When.
            markPublishedRecordsCommandHandler.Execute(command);

            // Then.
            var markedIconPosPushPublishById = testPublishedRecords.Select(publish => publish.IConPOSPushPublishID).ToList();
            var markedIconPosPushPublishRecords = irmaContext.IConPOSPushPublish.Where(publish => markedIconPosPushPublishById.Contains(publish.IConPOSPushPublishID)).ToList();

            // Have to reload the entity since the update was done via stored procedure.
            foreach (var publishedRecord in markedIconPosPushPublishRecords)
            {
                irmaContext.Entry(publishedRecord).Reload();
            }

            bool eachRecordIsInProcess = markedIconPosPushPublishRecords.TrueForAll(publish => publish.InProcessBy == command.Instance);

            Assert.IsFalse(eachRecordIsInProcess);
            Assert.AreEqual(null, testPublishedRecords[0].InProcessBy);
            Assert.AreEqual(command.Instance, testPublishedRecords[1].InProcessBy);
        }

        [TestMethod]
        public void MarkPublishedRecordsAsInProcess_OneRecordHasProcessedDateOneRecordHasProcessingFailedDate_NoRecordsShouldBeMarkedAsInProcess()
        {
            // Given.
            var random = new Random();

            testPublishedRecords = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder().WithStoreNumber(testStoreNumber).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithProcessingFailedDate(DateTime.Now),
                new TestIconPosPushPublishBuilder().WithStoreNumber(testStoreNumber).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithProcessedDate(DateTime.Now)
            };

            StageTestPublishedRecords();

            var command = new MarkPublishedRecordsAsInProcessCommand
            {
                Context = irmaContext,
                Instance = 99,
                MaxRecordsToProcess = 10000
            };

            // When.
            markPublishedRecordsCommandHandler.Execute(command);

            // Then.
            var markedIconPosPushPublishById = testPublishedRecords.Select(publish => publish.IConPOSPushPublishID).ToList();
            var markedIconPosPushPublishRecords = irmaContext.IConPOSPushPublish.Where(publish => markedIconPosPushPublishById.Contains(publish.IConPOSPushPublishID)).ToList();

            // Have to reload the entity since the update was done via stored procedure.
            foreach (var publishedRecord in markedIconPosPushPublishRecords)
            {
                irmaContext.Entry(publishedRecord).Reload();
            }

            bool noRecordIsInProcess = !markedIconPosPushPublishRecords.TrueForAll(publish => publish.InProcessBy == command.Instance);

            Assert.IsTrue(noRecordIsInProcess);
        }

        [TestMethod]
        public void MarkPublishedRecordsAsInProcess_OneRecordIsReadyOneRecordIsInProcessByAnotherController_OneRecordShouldBeMarkedAsInProcess()
        {
            // Given.
            var random = new Random();

            testPublishedRecords = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder().WithStoreNumber(testStoreNumber).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))),
                new TestIconPosPushPublishBuilder().WithStoreNumber(testStoreNumber).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithInProcessBy(98)
            };

            StageTestPublishedRecords();

            var command = new MarkPublishedRecordsAsInProcessCommand
            {
                Context = irmaContext,
                Instance = 99,
                MaxRecordsToProcess = 10000
            };

            // When.
            markPublishedRecordsCommandHandler.Execute(command);

            // Then.
            var markedIconPosPushPublishById = testPublishedRecords.Select(publish => publish.IConPOSPushPublishID).ToList();
            var markedIconPosPushPublishRecords = irmaContext.IConPOSPushPublish.Where(publish => markedIconPosPushPublishById.Contains(publish.IConPOSPushPublishID)).ToList();

            // Have to reload the entity since the update was done via stored procedure.
            foreach (var publishedRecord in markedIconPosPushPublishRecords)
            {
                irmaContext.Entry(publishedRecord).Reload();
            }

            Assert.AreEqual(99, markedIconPosPushPublishRecords[0].InProcessBy);
            Assert.AreEqual(98, markedIconPosPushPublishRecords[1].InProcessBy);
        }

        [TestMethod]
        public void MarkPublishedRecordsAsInProcess_FiveRecordsAreAlreadyInProcess_NoAdditionalRecordsShouldBeMarked()
        {
            // Given.
            var random = new Random();

            testPublishedRecords = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder().WithStoreNumber(testStoreNumber).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithInProcessBy(99),
                new TestIconPosPushPublishBuilder().WithStoreNumber(testStoreNumber).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithInProcessBy(99),
                new TestIconPosPushPublishBuilder().WithStoreNumber(testStoreNumber).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithInProcessBy(99),
                new TestIconPosPushPublishBuilder().WithStoreNumber(testStoreNumber).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithInProcessBy(99),
                new TestIconPosPushPublishBuilder().WithStoreNumber(testStoreNumber).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithInProcessBy(99)
            };

            StageTestPublishedRecords();

            var command = new MarkPublishedRecordsAsInProcessCommand
            {
                Context = irmaContext,
                Instance = 99,
                MaxRecordsToProcess = 10000
            };

            // When.
            markPublishedRecordsCommandHandler.Execute(command);

            // Then.
            var markedRecords = irmaContext.IConPOSPushPublish.Where(pub => pub.InProcessBy == 99).ToList();

            Assert.AreEqual(testPublishedRecords.Count, markedRecords.Count);
        }

        private void StageTestPublishedRecords()
        {
            this.irmaContext.IConPOSPushPublish.AddRange(this.testPublishedRecords);
            this.irmaContext.SaveChanges();
        }
    }
}
