using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PushController.DataAccess.Commands;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PushController.Tests.DataAccess.Commands
{
    [TestClass]
    public class MarkStagingRecordsAsInProcessForEsbCommandTests
    {
        private MarkStagedRecordsAsInProcessForEsbCommandHandler markStagingRecordsCommandHandler;
        private GlobalIconContext context;
        private DbContextTransaction transaction;
        private List<IRMAPush> testIrmaPushRecords;

        [TestInitialize]
        public void Initialize()
        {
            context = new GlobalIconContext(new IconContext());
            transaction = context.Context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void BuildCommandHandler()
        {
            markStagingRecordsCommandHandler = new MarkStagedRecordsAsInProcessForEsbCommandHandler(context);
        }

        private void StageTestIrmaPushRecords()
        {
            context.Context.IRMAPush.AddRange(testIrmaPushRecords);
            context.Context.SaveChanges();
        }

        [TestMethod]
        public void MarkStagingRecordsAsInProcessForEsb_OneRecordIsReady_OneRecordShouldBeMarkedAsInProcess()
        {
            // Given.
            BuildCommandHandler();

            testIrmaPushRecords = new List<IRMAPush>
            {
                new TestIrmaPushBuilder()
            };

            StageTestIrmaPushRecords();

            var command = new MarkStagedRecordsAsInProcessForEsbCommand
            {
                Instance = 99,
                MaxRecordsToProcess = 10000
            };

            // When.
            markStagingRecordsCommandHandler.Execute(command);

            // Then.
            int markedIrmaPushId = testIrmaPushRecords[0].IRMAPushID;
            var markedIrmaPushRecord = context.Context.IRMAPush.Single(push => push.IRMAPushID == markedIrmaPushId);

            // Have to reload the entity since the update was done via stored procedure.
            context.Context.Entry(markedIrmaPushRecord).Reload();

            Assert.AreEqual(command.Instance, markedIrmaPushRecord.InProcessBy);
        }

        [TestMethod]
        public void MarkStagingRecordsAsInProcessForEsb_FiveRecordsAreReady_FiveRecordsShouldBeMarkedAsInProcess()
        {
            // Given.
            BuildCommandHandler();

            var random = new Random();

            testIrmaPushRecords = new List<IRMAPush>
            {
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))),
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))),
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))),
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))),
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000)))
            };

            StageTestIrmaPushRecords();

            var command = new MarkStagedRecordsAsInProcessForEsbCommand
            {
                Instance = 99,
                MaxRecordsToProcess = 10000
            };

            // When.
            markStagingRecordsCommandHandler.Execute(command);

            // Then.
            var markedIrmaPushById = testIrmaPushRecords.Select(push => push.IRMAPushID).ToList();
            var markedIrmaPushRecords = context.Context.IRMAPush.Where(push => markedIrmaPushById.Contains(push.IRMAPushID)).ToList();

            // Have to reload the entity since the update was done via stored procedure.
            foreach (var irmaPush in markedIrmaPushRecords)
            {
                context.Context.Entry(irmaPush).Reload();
            }

            bool eachRecordIsInProcess = markedIrmaPushRecords.TrueForAll(push => push.InProcessBy == command.Instance);

            Assert.IsTrue(eachRecordIsInProcess);
        }

        [TestMethod]
        public void MarkStagingRecordsAsInProcessForEsb_OneRecordIsReadyOneRecordHasEsbReadyDate_OneRecordShouldBeMarkedAsInProcess()
        {
            // Given.
            BuildCommandHandler();

            var random = new Random();

            testIrmaPushRecords = new List<IRMAPush>
            {
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithEsbReadyDate(DateTime.Now),
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000)))
            };

            StageTestIrmaPushRecords();

            var command = new MarkStagedRecordsAsInProcessForEsbCommand
            {
                Instance = 99,
                MaxRecordsToProcess = 10000
            };

            // When.
            markStagingRecordsCommandHandler.Execute(command);

            // Then.
            var markedIrmaPushById = testIrmaPushRecords.Select(push => push.IRMAPushID).ToList();
            var markedIrmaPushRecords = context.Context.IRMAPush.Where(push => markedIrmaPushById.Contains(push.IRMAPushID)).ToList();

            // Have to reload the entity since the update was done via stored procedure.
            foreach (var irmaPush in markedIrmaPushRecords)
            {
                context.Context.Entry(irmaPush).Reload();
            }

            bool eachRecordIsInProcess = markedIrmaPushRecords.TrueForAll(push => push.InProcessBy == command.Instance);

            Assert.IsFalse(eachRecordIsInProcess);
            Assert.AreEqual(null, testIrmaPushRecords[0].InProcessBy);
            Assert.AreEqual(command.Instance, testIrmaPushRecords[1].InProcessBy);
        }

        [TestMethod]
        public void MarkStagingRecordsAsInProcessForEsb_OneRecordIsReadyOneRecordHasEsbReadyFailedDate_OneRecordShouldBeMarkedAsInProcess()
        {
            // Given.
            BuildCommandHandler();

            var random = new Random();

            testIrmaPushRecords = new List<IRMAPush>
            {
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithEsbReadyFailedDate(DateTime.Now),
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000)))
            };

            StageTestIrmaPushRecords();

            var command = new MarkStagedRecordsAsInProcessForEsbCommand
            {
                Instance = 99,
                MaxRecordsToProcess = 10000
            };

            // When.
            markStagingRecordsCommandHandler.Execute(command);

            // Then.
            var markedIrmaPushById = testIrmaPushRecords.Select(push => push.IRMAPushID).ToList();
            var markedIrmaPushRecords = context.Context.IRMAPush.Where(push => markedIrmaPushById.Contains(push.IRMAPushID)).ToList();

            // Have to reload the entity since the update was done via stored procedure.
            foreach (var irmaPush in markedIrmaPushRecords)
            {
                context.Context.Entry(irmaPush).Reload();
            }

            bool eachRecordIsInProcess = markedIrmaPushRecords.TrueForAll(push => push.InProcessBy == command.Instance);

            Assert.IsFalse(eachRecordIsInProcess);
            Assert.AreEqual(null, testIrmaPushRecords[0].InProcessBy);
            Assert.AreEqual(command.Instance, testIrmaPushRecords[1].InProcessBy);
        }

        [TestMethod]
        public void MarkStagingRecordsAsInProcessForEsb_OneRecordHasEsbReadyDateOneRecordHasEsbReadyFailedDate_NoRecordsShouldBeMarkedAsInProcess()
        {
            // Given.
            BuildCommandHandler();

            var random = new Random();

            testIrmaPushRecords = new List<IRMAPush>
            {
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithEsbReadyFailedDate(DateTime.Now),
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithEsbReadyDate(DateTime.Now)
            };

            StageTestIrmaPushRecords();

            var command = new MarkStagedRecordsAsInProcessForEsbCommand
            {
                Instance = 99,
                MaxRecordsToProcess = 10000
            };

            // When.
            markStagingRecordsCommandHandler.Execute(command);

            // Then.
            var markedIrmaPushById = testIrmaPushRecords.Select(push => push.IRMAPushID).ToList();
            var markedIrmaPushRecords = context.Context.IRMAPush.Where(push => markedIrmaPushById.Contains(push.IRMAPushID)).ToList();

            // Have to reload the entity since the update was done via stored procedure.
            foreach (var irmaPush in markedIrmaPushRecords)
            {
                context.Context.Entry(irmaPush).Reload();
            }

            bool noRecordIsInProcess = !markedIrmaPushRecords.TrueForAll(push => push.InProcessBy == command.Instance);

            Assert.IsTrue(noRecordIsInProcess);
        }

        [TestMethod]
        public void MarkStagingRecordsAsInProcessForEsb_OneRecordIsReadyOneRecordIsInProcessByAnotherController_OneRecordShouldBeMarkedAsInProcess()
        {
            // Given.
            BuildCommandHandler();

            var random = new Random();

            testIrmaPushRecords = new List<IRMAPush>
            {
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))),
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithInProcessBy(98)
            };

            StageTestIrmaPushRecords();

            var command = new MarkStagedRecordsAsInProcessForEsbCommand
            {
                Instance = 99,
                MaxRecordsToProcess = 10000
            };

            // When.
            markStagingRecordsCommandHandler.Execute(command);

            // Then.
            var markedIrmaPushById = testIrmaPushRecords.Select(push => push.IRMAPushID).ToList();
            var markedIrmaPushRecords = context.Context.IRMAPush.Where(push => markedIrmaPushById.Contains(push.IRMAPushID)).ToList();

            // Have to reload the entity since the update was done via stored procedure.
            foreach (var irmaPush in markedIrmaPushRecords)
            {
                context.Context.Entry(irmaPush).Reload();
            }

            Assert.AreEqual(99, markedIrmaPushRecords[0].InProcessBy);
            Assert.AreEqual(98, markedIrmaPushRecords[1].InProcessBy);
        }

        [TestMethod]
        public void MarkStagingRecordsAsInProcessForEsb_FiveRecordsAreAlreadyInProcess_NoAdditionalRecordsShouldBeMarked()
        {
            // Given.
            BuildCommandHandler();

            var random = new Random();

            testIrmaPushRecords = new List<IRMAPush>
            {
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithInProcessBy(99),
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithInProcessBy(99),
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithInProcessBy(99),
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithInProcessBy(99),
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))).WithInProcessBy(99)
            };

            StageTestIrmaPushRecords();

            var command = new MarkStagedRecordsAsInProcessForEsbCommand
            {
                Instance = 99,
                MaxRecordsToProcess = 10000
            };

            // When.
            markStagingRecordsCommandHandler.Execute(command);

            // Then.
            var markedRecords = context.Context.IRMAPush.Where(push => push.InProcessBy == 99).ToList();

            Assert.AreEqual(testIrmaPushRecords.Count, markedRecords.Count);
        }
    }
}
