using Icon.Logging;
using Icon.Testing.Builders;
using InterfaceController.Common;
using Irma.Framework;
using Irma.Testing.Builders;
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
    public class UpdatePublishTableDatesCommandTests
    {
        private UpdatePublishTableDatesCommandHandler updatePublishTableDatesCommandHandler;
        private IrmaContext context;
        private IrmaContextProvider contextProvider;
        private DbContextTransaction transaction;
        private Mock<ILogger<UpdatePublishTableDatesCommandHandler>> mockLogger;
        private List<IConPOSPushPublish> testPosData;
        private Random random;

        [TestInitialize]
        public void Initialize()
        {
            contextProvider = new IrmaContextProvider();
            context = contextProvider.GetRegionalContext(ConnectionBuilder.GetConnection("SP"));

            mockLogger = new Mock<ILogger<UpdatePublishTableDatesCommandHandler>>();
            updatePublishTableDatesCommandHandler = new UpdatePublishTableDatesCommandHandler(mockLogger.Object);
            
            random = new Random();

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void StageTestPosData()
        {
            context.IConPOSPushPublish.AddRange(testPosData);
            context.SaveChanges();
        }

        [TestMethod]
        public void UpdatePublishTableDates_NullList_WarningShouldBeLogged()
        {
            // Given.
            var command = new UpdatePublishTableDatesCommand
            {
                Context = context,
                ProcessedSuccessfully = true,
                PublishedPosData = null,
                Date = DateTime.Now
            };

            // When.
            updatePublishTableDatesCommandHandler.Execute(command);

            // Then.
            mockLogger.Verify(l => l.Warn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void UpdatePublishTableDates_EmptyList_WarningShouldBeLogged()
        {
            // Given.
            testPosData = new List<IConPOSPushPublish>();

            var command = new UpdatePublishTableDatesCommand
            {
                Context = context,
                ProcessedSuccessfully = true,
                PublishedPosData = testPosData,
                Date = DateTime.Now
            };

            // When.
            updatePublishTableDatesCommandHandler.Execute(command);

            // Then.
            mockLogger.Verify(l => l.Warn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void UpdatePublishTableDates_OneRecordToUpdateAsProcessedSuccessfully_OneRecordShouldBeUpdated()
        {
            // Given.
            testPosData = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder().WithStoreNumber(113).WithInProcessBy(1)
            };

            StageTestPosData();

            var command = new UpdatePublishTableDatesCommand
            {
                Context = context,
                ProcessedSuccessfully = true,
                PublishedPosData = testPosData,
                Date = DateTime.Now
            };

            // When.
            updatePublishTableDatesCommandHandler.Execute(command);

            // Then.
            int testPosDataId = testPosData[0].IConPOSPushPublishID;

            var updatedRecord = context.IConPOSPushPublish.Single(publish => publish.IConPOSPushPublishID == testPosDataId);

            // Have to reload the entity since the update was done via stored procedure.
            context.Entry(updatedRecord).Reload();

            Assert.AreEqual(DateTime.Now.Date, updatedRecord.ProcessedDate.Value.Date);
            Assert.IsNull(updatedRecord.InProcessBy);
        }

        [TestMethod]
        public void UpdatePublishTableDates_FiveRecordsToUpdateAsProcessedSuccessfully_FiveRecordsShouldBeUpdated()
        {
            // Given.
            testPosData = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder().WithInProcessBy(1).WithStoreNumber(113).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000))),
                new TestIconPosPushPublishBuilder().WithInProcessBy(1).WithStoreNumber(113).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000))),
                new TestIconPosPushPublishBuilder().WithInProcessBy(1).WithStoreNumber(113).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000))),
                new TestIconPosPushPublishBuilder().WithInProcessBy(1).WithStoreNumber(113).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000))),
                new TestIconPosPushPublishBuilder().WithInProcessBy(1).WithStoreNumber(113).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000)))
            };

            StageTestPosData();

            var command = new UpdatePublishTableDatesCommand
            {
                Context = context,
                ProcessedSuccessfully = true,
                PublishedPosData = testPosData,
                Date = DateTime.Now
            };

            // When.
            updatePublishTableDatesCommandHandler.Execute(command);

            // Then.
            var testPosDataById = testPosData.Select(p => p.IConPOSPushPublishID).ToList();

            var updatedRecords = context.IConPOSPushPublish.Where(publish => testPosDataById.Contains(publish.IConPOSPushPublishID)).ToList();

            // Have to reload the entity since the update was done via stored procedure.
            foreach (var updatedRecord in updatedRecords)
            {
                context.Entry(updatedRecord).Reload();
            }

            bool allRecordsWereUpdated = updatedRecords.TrueForAll(r =>
                r.ProcessedDate.Value.Date == DateTime.Now.Date && r.InProcessBy == null);

            Assert.IsTrue(allRecordsWereUpdated);
        }

        [TestMethod]
        public void UpdatePublishTableDates_OneRecordToUpdateAsProcessingFailed_OneRecordShouldBeUpdated()
        {
            // Given.
            testPosData = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder().WithStoreNumber(113).WithInProcessBy(1)
            };

            StageTestPosData();

            var command = new UpdatePublishTableDatesCommand
            {
                Context = context,
                ProcessedSuccessfully = false,
                PublishedPosData = testPosData,
                Date = DateTime.Now
            };

            // When.
            updatePublishTableDatesCommandHandler.Execute(command);

            // Then.
            int testPosDataId = testPosData[0].IConPOSPushPublishID;

            var updatedRecord = context.IConPOSPushPublish.Single(publish => publish.IConPOSPushPublishID == testPosDataId);

            // Have to reload the entity since the update was done via stored procedure.
            context.Entry(updatedRecord).Reload();

            Assert.AreEqual(DateTime.Now.Date, updatedRecord.ProcessingFailedDate.Value.Date);
            Assert.IsNull(updatedRecord.InProcessBy);
        }

        [TestMethod]
        public void UpdatePublishTableDates_FiveRecordsToUpdateAsProcessingFailed_FiveRecordsShouldBeUpdated()
        {
            // Given.
            testPosData = new List<IConPOSPushPublish>
            {
                new TestIconPosPushPublishBuilder().WithStoreNumber(113).WithInProcessBy(1).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000))),
                new TestIconPosPushPublishBuilder().WithStoreNumber(113).WithInProcessBy(1).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000))),
                new TestIconPosPushPublishBuilder().WithStoreNumber(113).WithInProcessBy(1).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000))),
                new TestIconPosPushPublishBuilder().WithStoreNumber(113).WithInProcessBy(1).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000))),
                new TestIconPosPushPublishBuilder().WithStoreNumber(113).WithInProcessBy(1).WithInsertDate(DateTime.Now.AddMilliseconds(random.Next(1000)))
            };

            StageTestPosData();

            var command = new UpdatePublishTableDatesCommand
            {
                Context = context,
                ProcessedSuccessfully = false,
                PublishedPosData = testPosData,
                Date = DateTime.Now
            };

            // When.
            updatePublishTableDatesCommandHandler.Execute(command);

            // Then.
            var testPosDataById = testPosData.Select(p => p.IConPOSPushPublishID).ToList();

            var updatedRecords = context.IConPOSPushPublish.Where(publish => testPosDataById.Contains(publish.IConPOSPushPublishID)).ToList();

            // Have to reload the entity since the update was done via stored procedure.
            foreach (var updatedRecord in updatedRecords)
            {
                context.Entry(updatedRecord).Reload();
            }

            bool allRecordsWereUpdated = updatedRecords.TrueForAll(r =>
                r.ProcessingFailedDate.Value.Date == DateTime.Now.Date && r.InProcessBy == null);

            Assert.IsTrue(allRecordsWereUpdated);
        }
    }
}
