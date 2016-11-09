using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PushController.DataAccess.Commands;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PushController.Tests.DataAccess.Commands
{
    [TestClass]
    public class UpdateStagingTableDatesForEsbCommandTests
    {
        private UpdateStagingTableDatesForEsbCommandHandler updateEsbDatesCommandHandler;
        private GlobalIconContext context;
        private DbContextTransaction transaction;
        private Mock<ILogger<UpdateStagingTableDatesForEsbCommandHandler>> mockLogger;
        private List<IRMAPush> testPosData;
        private Random random;

        [TestInitialize]
        public void Initialize()
        {
            context = new GlobalIconContext(new IconContext());
            mockLogger = new Mock<ILogger<UpdateStagingTableDatesForEsbCommandHandler>>();
            updateEsbDatesCommandHandler = new UpdateStagingTableDatesForEsbCommandHandler(mockLogger.Object, context);
            random = new Random();

            transaction = context.Context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void StageTestPosData()
        {
            context.Context.IRMAPush.AddRange(testPosData);
            context.Context.SaveChanges();
        }

        [TestMethod]
        public void UpdateStagingTableDatesForEsb_NullList_WarningShouldBeLogged()
        {
            // Given.
            var command = new UpdateStagingTableDatesForEsbCommand
            {
                ProcessedSuccessfully = true,
                StagedPosData = null,
                Date = DateTime.Now
            };

            // When.
            updateEsbDatesCommandHandler.Execute(command);

            // Then.
            mockLogger.Verify(l => l.Warn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void UpdateStagingTableDatesForEsb_EmptyList_WarningShouldBeLogged()
        {
            // Given.
            testPosData = new List<IRMAPush>();

            var command = new UpdateStagingTableDatesForEsbCommand
            {
                ProcessedSuccessfully = true,
                StagedPosData = testPosData,
                Date = DateTime.Now
            };

            // When.
            updateEsbDatesCommandHandler.Execute(command);

            // Then.
            mockLogger.Verify(l => l.Warn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void UpdateStagingTableDatesForEsb_OneRecordToUpdateAsEsbReady_OneRecordShouldBeUpdated()
        {
            // Given.
            testPosData = new List<IRMAPush>
            {
                new TestIrmaPushBuilder().WithInProcessBy(1)
            };

            StageTestPosData();

            var command = new UpdateStagingTableDatesForEsbCommand
            {
                ProcessedSuccessfully = true,
                StagedPosData = testPosData,
                Date = DateTime.Now
            };

            // When.
            updateEsbDatesCommandHandler.Execute(command);

            // Then.
            int testPosDataId = testPosData[0].IRMAPushID;

            var updatedRecord = context.Context.IRMAPush.Single(staging => staging.IRMAPushID == testPosDataId);

            // Have to reload the entity since the update was done via stored procedure.
            context.Context.Entry(updatedRecord).Reload();

            Assert.AreEqual(DateTime.Now.Date, updatedRecord.EsbReadyDate.Value.Date);
            Assert.IsNull(updatedRecord.InProcessBy);
        }

        [TestMethod]
        public void UpdateStagingTableDatesForEsb_FiveRecordsToUpdateAsEsbReady_FiveRecordsShouldBeUpdated()
        {
            // Given.
            testPosData = new List<IRMAPush>
            {
                new TestIrmaPushBuilder().WithInProcessBy(1).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))),
                new TestIrmaPushBuilder().WithInProcessBy(1).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))),
                new TestIrmaPushBuilder().WithInProcessBy(1).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))),
                new TestIrmaPushBuilder().WithInProcessBy(1).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))),
                new TestIrmaPushBuilder().WithInProcessBy(1).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000)))
            };

            StageTestPosData();

            var command = new UpdateStagingTableDatesForEsbCommand
            {
                ProcessedSuccessfully = true,
                StagedPosData = testPosData,
                Date = DateTime.Now
            };

            // When.
            updateEsbDatesCommandHandler.Execute(command);

            // Then.
            var testPosDataById = testPosData.Select(p => p.IRMAPushID).ToList();

            var updatedRecords = context.Context.IRMAPush.Where(staging => testPosDataById.Contains(staging.IRMAPushID)).ToList();

            // Have to reload the entity since the update was done via stored procedure.
            foreach (var updatedRecord in updatedRecords)
            {
                context.Context.Entry(updatedRecord).Reload();    
            }

            bool allRecordsWereUpdated = updatedRecords.TrueForAll(r =>
                r.EsbReadyDate.Value.Date == DateTime.Now.Date && r.InProcessBy == null);

            Assert.IsTrue(allRecordsWereUpdated);
        }

        [TestMethod]
        public void UpdateStagingTableDatesForEsb_OneRecordToUpdateAsEsbReadyFailed_OneRecordShouldBeUpdated()
        {
            // Given.
            testPosData = new List<IRMAPush>
            {
                new TestIrmaPushBuilder().WithInProcessBy(1)
            };

            StageTestPosData();

            var command = new UpdateStagingTableDatesForEsbCommand
            {
                ProcessedSuccessfully = false,
                StagedPosData = testPosData,
                Date = DateTime.Now
            };

            // When.
            updateEsbDatesCommandHandler.Execute(command);

            // Then.
            int testPosDataId = testPosData[0].IRMAPushID;

            var updatedRecord = context.Context.IRMAPush.Single(staging => staging.IRMAPushID == testPosDataId);

            // Have to reload the entity since the update was done via stored procedure.
            context.Context.Entry(updatedRecord).Reload();

            Assert.AreEqual(DateTime.Now.Date, updatedRecord.EsbReadyFailedDate.Value.Date);
            Assert.IsNull(updatedRecord.InProcessBy);
        }

        [TestMethod]
        public void UpdateStagingTableDatesForEsb_FiveRecordsToUpdateAsEsbReadyFailed_FiveRecordsShouldBeUpdated()
        {
            // Given.
            testPosData = new List<IRMAPush>
            {
                new TestIrmaPushBuilder().WithInProcessBy(1).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))),
                new TestIrmaPushBuilder().WithInProcessBy(1).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))),
                new TestIrmaPushBuilder().WithInProcessBy(1).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))),
                new TestIrmaPushBuilder().WithInProcessBy(1).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000))),
                new TestIrmaPushBuilder().WithInProcessBy(1).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(10000)))
            };

            StageTestPosData();

            var command = new UpdateStagingTableDatesForEsbCommand
            {
                ProcessedSuccessfully = false,
                StagedPosData = testPosData,
                Date = DateTime.Now
            };

            // When.
            updateEsbDatesCommandHandler.Execute(command);

            // Then.
            var testPosDataById = testPosData.Select(p => p.IRMAPushID).ToList();

            var updatedRecords = context.Context.IRMAPush.Where(staging => testPosDataById.Contains(staging.IRMAPushID)).ToList();

            // Have to reload the entity since the update was done via stored procedure.
            foreach (var updatedRecord in updatedRecords)
            {
                context.Context.Entry(updatedRecord).Reload();
            }

            bool allRecordsWereUpdated = updatedRecords.TrueForAll(r =>
                r.EsbReadyFailedDate.Value.Date == DateTime.Now.Date && r.InProcessBy == null);

            Assert.IsTrue(allRecordsWereUpdated);
        }
    }
}
