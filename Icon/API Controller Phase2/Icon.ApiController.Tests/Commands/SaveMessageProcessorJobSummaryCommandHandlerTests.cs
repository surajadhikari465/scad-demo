using Icon.ApiController.DataAccess.Commands;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data.Entity;
using System.Linq;
namespace Icon.ApiController.Tests.Commands
{
    /// <summary>
    /// Tests for the SaveMessageProcessorJobSummaryCommandHandler
    /// </summary>
    [TestClass]
    public class SaveMessageProcessorJobSummaryCommandHandlerTests
    {
        private SaveMessageProcessorJobSummaryCommandHandler commandHandler;
        private Mock<ILogger<SaveMessageProcessorJobSummaryCommandHandler>> mockLogger;
        private IconContext context;
        private GlobalIconContext globalContext;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            globalContext = new GlobalIconContext(context);

            mockLogger = new Mock<ILogger<SaveMessageProcessorJobSummaryCommandHandler>>();
            commandHandler = new SaveMessageProcessorJobSummaryCommandHandler(mockLogger.Object, globalContext);

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private APIMessageProcessorLogEntry GetCopy(APIMessageProcessorLogEntry original)
        {
            return new APIMessageProcessorLogEntry
            {
                MessageTypeID = original.MessageTypeID,
                StartTime = original.StartTime,
                EndTime = original.EndTime,
                CountProcessedMessages = original.CountProcessedMessages,
                CountFailedMessages = original.CountFailedMessages
            };
        }

        [TestMethod]
        public void Execute_WhenMessageTypeIsProduct_ShouldSaveLogEntryWithExpectedData()
        {
            // Given. 
            var expected = new APIMessageProcessorLogEntry
            {
                MessageTypeID = MessageTypes.Product,
                StartTime = DateTime.Now.AddSeconds(-11d),
                EndTime = DateTime.Now.AddSeconds(-10d),
                CountProcessedMessages = 27,
                CountFailedMessages = 3
            };
            var logEntryToSave = GetCopy(expected);

            var command = new SaveMessageProcessorJobSummaryCommand<APIMessageProcessorLogEntry>
            {
                JobSummary = logEntryToSave
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            Assert.IsTrue(logEntryToSave.APIMessageMonitorLogID > 0, "object should have been assigned ID when saved");
            var actual = context.APIMessageMonitorLog.Single(l => l.APIMessageMonitorLogID == logEntryToSave.APIMessageMonitorLogID);
            
            Assert.AreEqual(expected.MessageTypeID, actual.MessageTypeID);
            Assert.AreEqual(expected.StartTime, actual.StartTime);
            Assert.AreEqual(expected.EndTime, actual.EndTime);
            Assert.AreEqual(expected.CountProcessedMessages, actual.CountProcessedMessages);
            Assert.AreEqual(expected.CountFailedMessages, actual.CountFailedMessages);
        }

        [TestMethod]
        public void Execute_WhenMessageTypeIsItemLocale_ShouldSaveLogEntryWithExpectedData()
        {
            // Given. 
            var expected = new APIMessageProcessorLogEntry
            {
                MessageTypeID = MessageTypes.ItemLocale,
                StartTime = DateTime.Now.AddMilliseconds(-15d),
                EndTime = DateTime.Now.AddMilliseconds(-5d),
                CountProcessedMessages = 9,
                CountFailedMessages = 1
            };
            var logEntryToSave = GetCopy(expected);

            var command = new SaveMessageProcessorJobSummaryCommand<APIMessageProcessorLogEntry>
            {
                JobSummary = logEntryToSave
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            Assert.IsTrue(logEntryToSave.APIMessageMonitorLogID > 0, "object should have been assigned ID when saved");
            var actual = context.APIMessageMonitorLog.AsNoTracking().Single(l => l.APIMessageMonitorLogID == logEntryToSave.APIMessageMonitorLogID);

            Assert.AreEqual(expected.MessageTypeID, actual.MessageTypeID);
            Assert.AreEqual(expected.StartTime, actual.StartTime);
            Assert.AreEqual(expected.EndTime, actual.EndTime);
            Assert.AreEqual(expected.CountProcessedMessages, actual.CountProcessedMessages);
            Assert.AreEqual(expected.CountFailedMessages, actual.CountFailedMessages);
        }

        [TestMethod]
        public void Execute_WhenMessageTypeIsHierarchy_ShouldSaveLogEntryWithExpectedData()
        {
            // Given. 
            var expected = new APIMessageProcessorLogEntry
            {
                MessageTypeID = MessageTypes.Hierarchy,
                StartTime = DateTime.Now.AddMilliseconds(-10d),
                EndTime = DateTime.Now.AddMilliseconds(-5d),
                CountProcessedMessages = 2,
                CountFailedMessages = 8
            };
            var logEntryToSave = GetCopy(expected);

            var command = new SaveMessageProcessorJobSummaryCommand<APIMessageProcessorLogEntry>
            {
                JobSummary = logEntryToSave
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            Assert.IsTrue(logEntryToSave.APIMessageMonitorLogID > 0, "object should have been assigned ID when saved");
            var actual = context.APIMessageMonitorLog.Single(l => l.APIMessageMonitorLogID == logEntryToSave.APIMessageMonitorLogID);

            Assert.AreEqual(expected.MessageTypeID, actual.MessageTypeID);
            Assert.AreEqual(expected.StartTime, actual.StartTime);
            Assert.AreEqual(expected.EndTime, actual.EndTime);
            Assert.AreEqual(expected.CountProcessedMessages, actual.CountProcessedMessages);
            Assert.AreEqual(expected.CountFailedMessages, actual.CountFailedMessages);
        }

        [TestMethod]
        public void Execute_WhenMessageTypeIsHierarchy_ShouldCallLoggerInfoMethod()
        {
            // Given. 
            var logEntryToSave = new APIMessageProcessorLogEntry
            {
                MessageTypeID = MessageTypes.Hierarchy,
                StartTime = DateTime.Now.AddMilliseconds(-10d),
                EndTime = DateTime.Now.AddMilliseconds(-5d),
                CountProcessedMessages = 2,
                CountFailedMessages = 8
            };

            var command = new SaveMessageProcessorJobSummaryCommand<APIMessageProcessorLogEntry>
            {
                JobSummary = logEntryToSave
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            mockLogger.Verify(l => l.Info(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Execute_WhenMessageTypeIsHierarchy_ShouldCallLoggerInfoMethodWithExpectedData()
        {
            // Given. 
            DateTime start = new DateTime(2016, 9, 26, 14, 51, 22, 200);
            DateTime end = new DateTime(2016, 9, 26, 14, 51, 23, 350);
            var logEntryToSave = new APIMessageProcessorLogEntry
            {
                MessageTypeID = MessageTypes.Hierarchy,
                StartTime = start,
                EndTime = end,
                CountProcessedMessages = 2,
                CountFailedMessages = 8
            };

            var command = new SaveMessageProcessorJobSummaryCommand<APIMessageProcessorLogEntry>
            {
                JobSummary = logEntryToSave
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            string expectedToString = $"ID:{logEntryToSave.APIMessageMonitorLogID} MessageType:2 Processed:2 Failed:8 (Total:10) Start:2016-09-26T14:51:22.200 End:2016-09-26T14:51:23.350";
            string expectedLoggerMsg = $"Saved message processor job summary [{expectedToString}] to the APIMessageMonitorLog table.";
            mockLogger.Verify(l => l.Info(expectedLoggerMsg), Times.Once);
        }
    }
}
