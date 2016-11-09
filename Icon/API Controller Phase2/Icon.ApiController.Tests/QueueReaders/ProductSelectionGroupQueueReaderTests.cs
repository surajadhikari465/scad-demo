using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.ApiController.Controller.QueueReaders;
using Icon.Logging;
using Moq;
using System.Collections.Generic;
using Icon.Framework;
using Icon.ApiController.DataAccess.Queries;
using Icon.ApiController.DataAccess.Commands;
using Icon.Common.DataAccess;

namespace Icon.ApiController.Tests.QueueReaders
{
    [TestClass]
    public class ProductSelectionGroupQueueReaderTests
    {
        private ProductSelectionGroupQueueReader queueReader;
        private Mock<ILogger<ProductSelectionGroupQueueReader>> mockLogger;
        private Mock<IQueryHandler<GetMessageQueueParameters<MessageQueueProductSelectionGroup>, List<MessageQueueProductSelectionGroup>>> mockGetMessageQueueQuery;
        private Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueProductSelectionGroup>>> mockUpdateMessageQueueStatusCommandHandler;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger<ProductSelectionGroupQueueReader>>();
            mockGetMessageQueueQuery = new Mock<IQueryHandler<GetMessageQueueParameters<MessageQueueProductSelectionGroup>, List<MessageQueueProductSelectionGroup>>>();
            mockUpdateMessageQueueStatusCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueProductSelectionGroup>>>();

            queueReader = new ProductSelectionGroupQueueReader(
                mockLogger.Object,
                mockGetMessageQueueQuery.Object,
                mockUpdateMessageQueueStatusCommandHandler.Object);
        }

        [TestMethod]
        public void GroupMessagesForMiniBulk_InvalidArgument_ShouldThrowException()
        {
            // Given.
            int exceptionCount = 0;

            // When.
            var messages = new List<MessageQueueProductSelectionGroup>();

            try { queueReader.GroupMessagesForMiniBulk(messages); }
            catch (Exception) { exceptionCount++; }

            messages = null;

            try { queueReader.GroupMessagesForMiniBulk(messages); }
            catch (Exception) { exceptionCount++; }

            // Then.
            Assert.AreEqual(2, exceptionCount);
        }

        [TestMethod]
        public void GroupMessagesForMiniBulk_OneMessage_ShouldReturnOneMessageForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueueProductSelectionGroups = new List<MessageQueueProductSelectionGroup>
            {
                TestHelpers.GetFakeMessageQueueProductSelectionGroup()
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueProductSelectionGroups);
            
            // Then.
            Assert.AreEqual(1, messages.Count);
            Assert.AreEqual(1, messages[0].ProductSelectionGroupId);
        }

        [TestMethod]
        public void GroupMessagesForMiniBulk_TwoMessagesWithSameItemId_ShouldReturnOneMessageForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueueProductSelectionGroups = new List<MessageQueueProductSelectionGroup>
            {
                TestHelpers.GetFakeMessageQueueProductSelectionGroup(),
                TestHelpers.GetFakeMessageQueueProductSelectionGroup()
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueProductSelectionGroups);
            
            // Then.
            Assert.AreEqual(1, messages.Count);
            Assert.AreEqual(1, messages[0].ProductSelectionGroupId);
        }

        [TestMethod]
        public void GroupMessagesForMiniBulk_TwoMessagesWithDifferentIds_ShouldReturnTwoMessagesForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueueProductSelectionGroups = new List<MessageQueueProductSelectionGroup>
            {
                TestHelpers.GetFakeMessageQueueProductSelectionGroup(1),
                TestHelpers.GetFakeMessageQueueProductSelectionGroup(2)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueProductSelectionGroups);

            // Then.
            Assert.AreEqual(2, messages.Count);
            Assert.AreEqual(1, messages[0].ProductSelectionGroupId);
            Assert.AreEqual(2, messages[1].ProductSelectionGroupId);
        }

        [TestMethod]
        public void BuildMiniBulk_InvalidArguments_ExceptionShouldBeThrown()
        {
            // Given.
            var messages = new List<MessageQueueProductSelectionGroup>();
            
            int caughtExceptions = 0;

            // When.
            try { var miniBulk = queueReader.BuildMiniBulk(messages); }
            catch (ArgumentException) { caughtExceptions++; }

            messages = null;
            try { var miniBulk = queueReader.BuildMiniBulk(messages); }
            catch (ArgumentException) { caughtExceptions++; }

            // Then.
            int expectedExceptions = 2;
            Assert.AreEqual(expectedExceptions, caughtExceptions);
        }
    }
}