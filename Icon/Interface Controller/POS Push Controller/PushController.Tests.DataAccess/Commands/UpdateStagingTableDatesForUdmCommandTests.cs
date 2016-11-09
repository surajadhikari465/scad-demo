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
    public class UpdateStagingTableDatesForUdmCommandTests
    {
        private UpdateStagingTableDatesForUdmCommandHandler updateUdmDatesCommandHandler;
        private GlobalIconContext context;
        private DbContextTransaction transaction;
        private Mock<ILogger<UpdateStagingTableDatesForUdmCommandHandler>> mockLogger;
        private List<IRMAPush> testPosData;
        private Random random;

        [TestInitialize]
        public void Initialize()
        {
            context = new GlobalIconContext(new IconContext());
            mockLogger = new Mock<ILogger<UpdateStagingTableDatesForUdmCommandHandler>>();
            updateUdmDatesCommandHandler = new UpdateStagingTableDatesForUdmCommandHandler(mockLogger.Object, context);
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
        public void UpdateStagingTableDatesForUdm_NullList_WarningShouldBeLogged()
        {
            // Given.
            var command = new UpdateStagingTableDatesForUdmCommand
            {
                ProcessedSuccessfully = true,
                StagedPosData = null,
                Date = DateTime.Now
            };

            // When.
            updateUdmDatesCommandHandler.Execute(command);

            // Then.
            mockLogger.Verify(l => l.Warn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void UpdateStagingTableDatesForUdm_EmptyList_WarningShouldBeLogged()
        {
            // Given.
            testPosData = new List<IRMAPush>();

            var command = new UpdateStagingTableDatesForUdmCommand
            {
                ProcessedSuccessfully = true,
                StagedPosData = testPosData,
                Date = DateTime.Now
            };

            // When.
            updateUdmDatesCommandHandler.Execute(command);

            // Then.
            mockLogger.Verify(l => l.Warn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void UpdateStagingTableDatesForUdm_OneRecordToUpdateAsUdmReady_OneRecordShouldBeUpdated()
        {
            // Given.
            testPosData = new List<IRMAPush>
            {
                new TestIrmaPushBuilder().WithInProcessBy(1)
            };

            StageTestPosData();

            var command = new UpdateStagingTableDatesForUdmCommand
            {
                ProcessedSuccessfully = true,
                StagedPosData = testPosData,
                Date = DateTime.Now
            };

            // When.
            updateUdmDatesCommandHandler.Execute(command);

            // Then.
            int testPosDataId = testPosData[0].IRMAPushID;
            var updatedRecord = context.Context.IRMAPush.Single(staging => staging.IRMAPushID == testPosDataId);

            // Have to reload the entity since the update was done via stored procedure.
            context.Context.Entry(updatedRecord).Reload();

            Assert.AreEqual(DateTime.Now.Date, updatedRecord.InUdmDate.Value.Date);
            Assert.IsNull(updatedRecord.InProcessBy);
        }

        [TestMethod]
        public void UpdateStagingTableDatesForUdm_FiveRecordsToUpdateAsUdmReady_FiveRecordsShouldBeUpdated()
        {
            // Given.
            testPosData = new List<IRMAPush>
            {
                new TestIrmaPushBuilder().WithInProcessBy(1).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000))),
                new TestIrmaPushBuilder().WithInProcessBy(1).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000))),
                new TestIrmaPushBuilder().WithInProcessBy(1).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000))),
                new TestIrmaPushBuilder().WithInProcessBy(1).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000))),
                new TestIrmaPushBuilder().WithInProcessBy(1).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000)))
            };

            StageTestPosData();

            var command = new UpdateStagingTableDatesForUdmCommand
            {
                ProcessedSuccessfully = true,
                StagedPosData = testPosData,
                Date = DateTime.Now
            };

            // When.
            updateUdmDatesCommandHandler.Execute(command);

            // Then.
            var testPosDataById = testPosData.Select(p => p.IRMAPushID).ToList();

            var updatedRecords = context.Context.IRMAPush.Where(staging => testPosDataById.Contains(staging.IRMAPushID)).ToList();

            // Have to reload the entity since the update was done via stored procedure.
            foreach (var updatedRecord in updatedRecords)
            {
                context.Context.Entry(updatedRecord).Reload();    
            }

            bool allRecordsWereUpdated = updatedRecords.TrueForAll(r =>
                r.InUdmDate.Value.Date == DateTime.Now.Date && r.InProcessBy == null);

            Assert.IsTrue(allRecordsWereUpdated);
        }

        [TestMethod]
        public void UpdateStagingTableDatesForUdm_OneRecordToUpdateAsUdmFailed_OneRecordShouldBeUpdated()
        {
            // Given.
            testPosData = new List<IRMAPush>
            {
                new TestIrmaPushBuilder().WithInProcessBy(1)
            };

            StageTestPosData();

            var command = new UpdateStagingTableDatesForUdmCommand
            {
                ProcessedSuccessfully = false,
                StagedPosData = testPosData,
                Date = DateTime.Now
            };

            // When.
            updateUdmDatesCommandHandler.Execute(command);

            // Then.
            int testPosDataId = testPosData[0].IRMAPushID;

            var updatedRecord = context.Context.IRMAPush.Single(staging => staging.IRMAPushID == testPosDataId);

            // Have to reload the entity since the update was done via stored procedure.
            context.Context.Entry(updatedRecord).Reload();

            Assert.AreEqual(DateTime.Now.Date, updatedRecord.UdmFailedDate.Value.Date);
            Assert.IsNull(updatedRecord.InProcessBy);
        }

        [TestMethod]
        public void UpdateStagingTableDatesForUdm_FiveRecordsToUpdateAsUdmFailed_FiveRecordsShouldBeUpdated()
        {
            // Given.
            testPosData = new List<IRMAPush>
            {
                new TestIrmaPushBuilder().WithInProcessBy(1).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000))),
                new TestIrmaPushBuilder().WithInProcessBy(1).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000))),
                new TestIrmaPushBuilder().WithInProcessBy(1).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000))),
                new TestIrmaPushBuilder().WithInProcessBy(1).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000))),
                new TestIrmaPushBuilder().WithInProcessBy(1).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000)))
            };

            StageTestPosData();

            var command = new UpdateStagingTableDatesForUdmCommand
            {
                ProcessedSuccessfully = false,
                StagedPosData = testPosData,
                Date = DateTime.Now
            };

            // When.
            updateUdmDatesCommandHandler.Execute(command);

            // Then.
            var testPosDataById = testPosData.Select(p => p.IRMAPushID).ToList();

            var updatedRecords = context.Context.IRMAPush.Where(staging => testPosDataById.Contains(staging.IRMAPushID)).ToList();

            // Have to reload the entity since the update was done via stored procedure.
            foreach (var updatedRecord in updatedRecords)
            {
                context.Context.Entry(updatedRecord).Reload();
            }

            bool allRecordsWereUpdated = updatedRecords.TrueForAll(r =>
                r.UdmFailedDate.Value.Date == DateTime.Now.Date && r.InProcessBy == null);

            Assert.IsTrue(allRecordsWereUpdated);
        }
    }
}
