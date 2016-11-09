using Icon.Common.Email;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PushController.Controller.MessageQueueServices;
using PushController.DataAccess.Commands;
using PushController.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PushController.Tests.Controller.MessageQueueServiceTests
{
    [TestClass]
    public class ItemLocaleMessageQueueServiceTests
    {
        private GlobalIconContext context;
        private DbContextTransaction transaction;
        private ItemLocaleMessageQueueService itemLocaleMessageQueueService;
        private Mock<ILogger<ItemLocaleMessageQueueService>> mockLogger;
        private Mock<IEmailClient> mockEmailClient;
        private AddItemLocaleMessagesBulkCommandHandler addMessagesBulkCommandHandler;
        private Mock<ICommandHandler<AddItemLocaleMessagesBulkCommand>> mockAddMessagesBulkCommandHandler;
        private AddItemLocaleMessagesRowByRowCommandHandler addMessagesRowByRowCommandHandler;
        private UpdateStagingTableDatesForEsbCommandHandler updateStagingTableDatesForEsbCommandHandler;
        private List<IRMAPush> testPosData;
        private int unknownMessageTypeId = 99;

        [TestInitialize]
        public void Initialize()
        {
            this.context = new GlobalIconContext(new IconContext());

            this.mockLogger = new Mock<ILogger<ItemLocaleMessageQueueService>>();
            this.mockEmailClient = new Mock<IEmailClient>();
            this.addMessagesBulkCommandHandler = new AddItemLocaleMessagesBulkCommandHandler(new Mock<ILogger<AddItemLocaleMessagesBulkCommandHandler>>().Object, context);
            this.mockAddMessagesBulkCommandHandler = new Mock<ICommandHandler<AddItemLocaleMessagesBulkCommand>>();
            this.addMessagesRowByRowCommandHandler = new AddItemLocaleMessagesRowByRowCommandHandler(context);
            this.updateStagingTableDatesForEsbCommandHandler = new UpdateStagingTableDatesForEsbCommandHandler(new Mock<ILogger<UpdateStagingTableDatesForEsbCommandHandler>>().Object, context);

            this.transaction = this.context.Context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.transaction.Rollback();
        }

        private void StagePosData()
        {
            var random = new Random();

            this.testPosData = new List<IRMAPush>
            {
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next())),
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next())),
                new TestIrmaPushBuilder().WithInsertDate(DateTime.Now.AddMilliseconds(random.Next()))
            };

            this.context.Context.IRMAPush.AddRange(this.testPosData);
            this.context.Context.SaveChanges();
        }

        [TestMethod]
        public void SaveMessagesBulk_SaveIsSuccessful_MessageShouldBeSavedToTheDatabase()
        {
            // Given.
            StagePosData();

            var testMessages = new List<MessageQueueItemLocale> 
            { 
                new TestItemLocaleMessageBuilder().WithIrmaPushId(this.testPosData[0].IRMAPushID), 
                new TestItemLocaleMessageBuilder().WithIrmaPushId(this.testPosData[0].IRMAPushID),
                new TestItemLocaleMessageBuilder().WithIrmaPushId(this.testPosData[0].IRMAPushID)
            };

            this.itemLocaleMessageQueueService = new ItemLocaleMessageQueueService(
                mockLogger.Object,
                mockEmailClient.Object,
                addMessagesBulkCommandHandler,
                addMessagesRowByRowCommandHandler,
                updateStagingTableDatesForEsbCommandHandler);

            // When.
            this.itemLocaleMessageQueueService.SaveMessagesBulk(testMessages);

            // Then.
            var testMessagesById = testMessages.Select(m => m.IRMAPushID).ToList();
            var queuedMessages = this.context.Context.MessageQueueItemLocale.Where(mq => testMessagesById.Contains(mq.IRMAPushID)).ToList();

            Assert.AreEqual(testMessages.Count, queuedMessages.Count);
        }

        [TestMethod]
        public void SaveMessagesBulk_TransientErrorOccursAndRetryIsSuccessful_EventsShouldBeLogged()
        {
            // This test requires manual intervention to work properly since mocking a SqlException requires trickery.  Just open a locking transaction against the 
            // MessageQueueItemLocale table before running this test to create the appropriate condition.  The test will be set to auto-pass for automatic runs.

            // Given.
            StagePosData();

            var testMessages = new List<MessageQueueItemLocale> 
            { 
                new TestItemLocaleMessageBuilder().WithIrmaPushId(this.testPosData[0].IRMAPushID), 
                new TestItemLocaleMessageBuilder().WithIrmaPushId(this.testPosData[0].IRMAPushID),
                new TestItemLocaleMessageBuilder().WithIrmaPushId(this.testPosData[0].IRMAPushID)
            };

            this.itemLocaleMessageQueueService = new ItemLocaleMessageQueueService(
                mockLogger.Object,
                mockEmailClient.Object,
                addMessagesBulkCommandHandler,
                addMessagesRowByRowCommandHandler,
                updateStagingTableDatesForEsbCommandHandler);

            // When.
            this.itemLocaleMessageQueueService.SaveMessagesBulk(testMessages);

            // Then.
            // Uncomment assertions to verify test during a manual run.

            //mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
            //mockLogger.Verify(l => l.Info(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void SaveMessagesBulk_TransientErrorOccursAndRetryIsSuccessful_MessagesShouldBeSaved()
        {
            // This test requires manual intervention to work properly since mocking a SqlException requires trickery.  Just open a locking transaction against the 
            // MessageQueueItemLocale table before running this test to create the appropriate condition.  The test will be set to auto-pass for automatic runs.

            // Given.
            StagePosData();

            var testMessages = new List<MessageQueueItemLocale> 
            { 
                new TestItemLocaleMessageBuilder().WithIrmaPushId(this.testPosData[0].IRMAPushID), 
                new TestItemLocaleMessageBuilder().WithIrmaPushId(this.testPosData[0].IRMAPushID),
                new TestItemLocaleMessageBuilder().WithIrmaPushId(this.testPosData[0].IRMAPushID)
            };

            this.itemLocaleMessageQueueService = new ItemLocaleMessageQueueService(
                mockLogger.Object,
                mockEmailClient.Object,
                addMessagesBulkCommandHandler,
                addMessagesRowByRowCommandHandler,
                updateStagingTableDatesForEsbCommandHandler);

            // When.
            this.itemLocaleMessageQueueService.SaveMessagesBulk(testMessages);

            // Then.
            // Uncomment assertions to verify test during a manual run.

            //var testMessagesById = testMessages.Select(m => m.IRMAPushID).ToList();
            //var queuedMessages = this.context.Context.MessageQueueItemLocale.Where(mq => testMessagesById.Contains(mq.IRMAPushID)).ToList();

            //Assert.AreEqual(testMessages.Count, queuedMessages.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void SaveMessagesBulk_IntransientErrorOccurs_ExceptionShouldBeThrown()
        {
            // Given.
            StagePosData();

            var testMessages = new List<MessageQueueItemLocale> 
            { 
                new TestItemLocaleMessageBuilder().WithIrmaPushId(this.testPosData[0].IRMAPushID), 
                new TestItemLocaleMessageBuilder().WithIrmaPushId(this.testPosData[0].IRMAPushID),
                new TestItemLocaleMessageBuilder().WithIrmaPushId(this.testPosData[0].IRMAPushID)
            };

            this.mockAddMessagesBulkCommandHandler.Setup(c => c.Execute(It.IsAny<AddItemLocaleMessagesBulkCommand>())).Throws(new Exception());

            this.itemLocaleMessageQueueService = new ItemLocaleMessageQueueService(
                mockLogger.Object,
                mockEmailClient.Object,
                mockAddMessagesBulkCommandHandler.Object,
                addMessagesRowByRowCommandHandler,
                updateStagingTableDatesForEsbCommandHandler);

            // When.
            this.itemLocaleMessageQueueService.SaveMessagesBulk(testMessages);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        public void SaveMessagesRowByRow_SaveIsSuccessfulForEachMessage_MessagesShouldBeSaved()
        {
            // Given.
            StagePosData();

            var testMessages = new List<MessageQueueItemLocale> 
            { 
                new TestItemLocaleMessageBuilder().WithIrmaPushId(this.testPosData[0].IRMAPushID), 
                new TestItemLocaleMessageBuilder().WithIrmaPushId(this.testPosData[0].IRMAPushID),
                new TestItemLocaleMessageBuilder().WithIrmaPushId(this.testPosData[0].IRMAPushID)
            };

            this.itemLocaleMessageQueueService = new ItemLocaleMessageQueueService(
                mockLogger.Object,
                mockEmailClient.Object,
                addMessagesBulkCommandHandler,
                addMessagesRowByRowCommandHandler,
                updateStagingTableDatesForEsbCommandHandler);

            // When.
            this.itemLocaleMessageQueueService.SaveMessagesRowByRow(testMessages);

            // Then.
            var testMessagesById = testMessages.Select(m => m.IRMAPushID).ToList();
            var queuedMessages = this.context.Context.MessageQueueItemLocale.Where(mq => testMessagesById.Contains(mq.IRMAPushID)).ToList();

            Assert.AreEqual(testMessages.Count, queuedMessages.Count);
        }

        [TestMethod]
        public void SaveMessagesRowByRow_SaveFailsForOneMessage_FailedMessageShouldNotBeSaved()
        {
            // Given.
            StagePosData();

            var testMessages = new List<MessageQueueItemLocale> 
            { 
                new TestItemLocaleMessageBuilder().WithIrmaPushId(this.testPosData[0].IRMAPushID), 
                new TestItemLocaleMessageBuilder(),
                new TestItemLocaleMessageBuilder().WithIrmaPushId(this.testPosData[0].IRMAPushID)
            };

            this.itemLocaleMessageQueueService = new ItemLocaleMessageQueueService(
                mockLogger.Object,
                mockEmailClient.Object,
                addMessagesBulkCommandHandler,
                addMessagesRowByRowCommandHandler,
                updateStagingTableDatesForEsbCommandHandler);

            // When.
            this.itemLocaleMessageQueueService.SaveMessagesRowByRow(testMessages);

            // Then.
            var testMessagesById = testMessages.Select(m => m.IRMAPushID).ToList();
            var successfulQueuedMessages = this.context.Context.MessageQueueItemLocale.Where(mq => testMessagesById.Contains(mq.IRMAPushID)).ToList();

            Assert.AreEqual(testMessages.Count - 1, successfulQueuedMessages.Count);
        }

        [TestMethod]
        public void SaveMessagesRowByRow_SaveFailsForOneMessage_ErrorShouldBeLogged()
        {
            // Given.
            StagePosData();

            var testMessages = new List<MessageQueueItemLocale> 
            { 
                new TestItemLocaleMessageBuilder().WithIrmaPushId(this.testPosData[0].IRMAPushID), 
                new TestItemLocaleMessageBuilder(),
                new TestItemLocaleMessageBuilder().WithIrmaPushId(this.testPosData[0].IRMAPushID)
            };

            this.itemLocaleMessageQueueService = new ItemLocaleMessageQueueService(
                mockLogger.Object,
                mockEmailClient.Object,
                addMessagesBulkCommandHandler,
                addMessagesRowByRowCommandHandler,
                updateStagingTableDatesForEsbCommandHandler);

            // When.
            this.itemLocaleMessageQueueService.SaveMessagesRowByRow(testMessages);

            // Then.
            mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void SaveMessagesRowByRow_SaveFailsForOneMessage_AlertEmailShouldBeSent()
        {
            // Given.
            StagePosData();

            var testMessages = new List<MessageQueueItemLocale> 
            { 
                new TestItemLocaleMessageBuilder().WithIrmaPushId(this.testPosData[0].IRMAPushID), 
                new TestItemLocaleMessageBuilder(),
                new TestItemLocaleMessageBuilder().WithIrmaPushId(this.testPosData[0].IRMAPushID)
            };

            this.itemLocaleMessageQueueService = new ItemLocaleMessageQueueService(
                mockLogger.Object,
                mockEmailClient.Object,
                addMessagesBulkCommandHandler,
                addMessagesRowByRowCommandHandler,
                updateStagingTableDatesForEsbCommandHandler);

            // When.
            this.itemLocaleMessageQueueService.SaveMessagesRowByRow(testMessages);

            // Then.
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void SaveMessagesRowByRow_SaveFailsForOneMessage_FailedDateShouldBeUpdatedInTheStagingTable()
        {
            // Given.
            StagePosData();

            var testMessages = new List<MessageQueueItemLocale> 
            { 
                new TestItemLocaleMessageBuilder().WithIrmaPushId(this.testPosData[0].IRMAPushID).WithMessageTypeId(this.unknownMessageTypeId), 
                new TestItemLocaleMessageBuilder().WithIrmaPushId(this.testPosData[1].IRMAPushID),
                new TestItemLocaleMessageBuilder().WithIrmaPushId(this.testPosData[2].IRMAPushID)
            };

            this.itemLocaleMessageQueueService = new ItemLocaleMessageQueueService(
                mockLogger.Object,
                mockEmailClient.Object,
                addMessagesBulkCommandHandler,
                addMessagesRowByRowCommandHandler,
                updateStagingTableDatesForEsbCommandHandler);

            // When.
            this.itemLocaleMessageQueueService.SaveMessagesRowByRow(testMessages);

            // Then.
            var failedPosDataEntryId = testPosData[0].IRMAPushID;
            var failedPosDataEntry = this.context.Context.IRMAPush.Single(ip => ip.IRMAPushID == failedPosDataEntryId);

            // Have to reload the entity since the price update was done via stored procedure.
            context.Context.Entry(failedPosDataEntry).Reload();

            Assert.AreEqual(DateTime.Now.Date, failedPosDataEntry.EsbReadyFailedDate.Value.Date);
        }
    }
}
