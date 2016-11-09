﻿using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.DataAccess.Commands;

using Icon.ApiController.DataAccess.Queries;
using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Framework;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Icon.ApiController.Tests.QueueReaders
{
    [TestClass]
    public class HierarchyQueueReaderTests
    {
        private HierarchyQueueReader queueReader;
        private Mock<ILogger<HierarchyQueueReader>> mockLogger;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IQueryHandler<GetMessageQueueParameters<MessageQueueHierarchy>, List<MessageQueueHierarchy>>> mockGetMessageQueueQuery;
        private Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueHierarchy>>> mockUpdateMessageQueueStatusCommandHandler;
        private Mock<ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueHierarchy>>> mockMarkEntriesAsInProcessCommandHandler;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger<HierarchyQueueReader>>();
            mockEmailClient = new Mock<IEmailClient>();
            mockGetMessageQueueQuery = new Mock<IQueryHandler<GetMessageQueueParameters<MessageQueueHierarchy>, List<MessageQueueHierarchy>>>();
            mockUpdateMessageQueueStatusCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueHierarchy>>>();
            mockMarkEntriesAsInProcessCommandHandler = new Mock<ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueHierarchy>>>();

            queueReader = new HierarchyQueueReader(
                mockLogger.Object,
                mockEmailClient.Object,
                mockGetMessageQueueQuery.Object,
                mockUpdateMessageQueueStatusCommandHandler.Object);
        }

        [TestMethod]
        public void GroupHierarchyMessages_InvalidArgument_ShouldThrowException()
        {
            // Given.
            int exceptionCount = 0;

            // When.
            var messages = new List<MessageQueueHierarchy>();

            try { queueReader.GroupMessagesForMiniBulk(messages); }
            catch (Exception) { exceptionCount++; }

            messages = null;

            try { queueReader.GroupMessagesForMiniBulk(messages); }
            catch (Exception) { exceptionCount++; }

            // Then.
            Assert.AreEqual(2, exceptionCount);
        }

        [TestMethod]
        public void GroupHierarchyMessages_OneMessage_ShouldReturnOneMessageForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueueHierarchies = new List<MessageQueueHierarchy>
            {
                TestHelpers.GetFakeMessageQueueHierarchy(1, "Brick", true)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueHierarchies);

            // Then.
            Assert.AreEqual(1, messages.Count);
            Assert.AreEqual(1, messages[0].HierarchyId);
        }

        [TestMethod]
        public void GroupHierarchyMessages_TwoMessagesWithDifferentHierarchyId_ShouldReturnOneMessageForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueueHierarchies = new List<MessageQueueHierarchy>
            {
                TestHelpers.GetFakeMessageQueueHierarchy(1, "Brick", true),
                TestHelpers.GetFakeMessageQueueHierarchy(2, "Brick", true)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueHierarchies);

            // Then.
            Assert.AreEqual(1, messages.Count);
            Assert.AreEqual(1, messages[0].HierarchyId);
        }

        [TestMethod]
        public void GroupHierarchyMessages_TwoMessageWithDifferentLevelName_ShouldReturnOneMessageForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueueHierarchies = new List<MessageQueueHierarchy>
            {
                TestHelpers.GetFakeMessageQueueHierarchy(1, "Brick", true),
                TestHelpers.GetFakeMessageQueueHierarchy(1, "Sub Brick", true)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueHierarchies);

            // Then.
            Assert.AreEqual(1, messages.Count);
            Assert.AreEqual(1, messages[0].HierarchyId);
        }

        [TestMethod]
        public void GroupHierarchyMessages_TwoMessagesWithDifferentItemsAttached_ShouldReturnOneMessageForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueueHierarchies = new List<MessageQueueHierarchy>
            {
                TestHelpers.GetFakeMessageQueueHierarchy(1, "Brick", true),
                TestHelpers.GetFakeMessageQueueHierarchy(1, "Brick", false)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueHierarchies);

            // Then.
            Assert.AreEqual(1, messages.Count);
            Assert.AreEqual(1, messages[0].HierarchyId);
        }

        [TestMethod]
        public void GroupHierarchyMessages_TwoMessagesThatMatchHierarchyAndPrototype_ShouldReturnTwoMessagesForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueueHierarchies = new List<MessageQueueHierarchy>
            {
                TestHelpers.GetFakeMessageQueueHierarchy(1, "Brick", true),
                TestHelpers.GetFakeMessageQueueHierarchy(1, "Brick", true)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueHierarchies);

            // Then.
            Assert.AreEqual(2, messages.Count);
            Assert.AreEqual(1, messages[0].HierarchyId);
            Assert.AreEqual(1, messages[1].HierarchyId);
        }

        [TestMethod]
        public void GetHierarchyMiniBulk_InvalidArguments_ExceptionShouldBeThrown()
        {
            // Given.
            var messages = new List<MessageQueueHierarchy>();
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

        [TestMethod]
        public void GetHierarchyMiniBulk_ThreeMessages_ShouldReturnMiniBulkWithThreeHierarchyClassElements()
        {
            // Given.
            var fakeMessageQueueHierarchies = new List<MessageQueueHierarchy>
            {
                TestHelpers.GetFakeMessageQueueHierarchy(1, "Brick", true),
                TestHelpers.GetFakeMessageQueueHierarchy(1, "Brick", true),
                TestHelpers.GetFakeMessageQueueHierarchy(1, "Brick", true)
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueHierarchies);

            // Then.
            Assert.AreEqual(3, miniBulk.@class.Length);
        }
    }
}
