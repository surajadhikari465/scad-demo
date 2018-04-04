using Icon.Common.DataAccess;
using Icon.Ewic.Models;
using Icon.Ewic.Serialization.Serializers;
using Icon.Ewic.Transmission.Producers;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Icon.Web.Tests.Unit.Managers
{
    [TestClass] [Ignore]
    public class RemoveEwicExclusionManagerHandlerTests
    {
        private RemoveEwicExclusionManagerHandler managerHandler;
        private Mock<ISerializer<EwicExclusionMessageModel>> mockSerializer;
        private Mock<IQueryHandler<GetEwicAgenciesWithExclusionParameters, List<Agency>>> mockGetEwicAgenciesWithExclusionQuery;
        private Mock<ICommandHandler<RemoveEwicExclusionCommand>> mockRemoveEwicExclusionCommandHandler;
        private Mock<ICommandHandler<SaveToMessageHistoryCommand>> mockSaveToMessageHistoryCommandHandler;
        private Mock<ICommandHandler<UpdateMessageHistoryMessageCommand>> mockUpdateMessageHistoryMessageCommandHandler;
        private Mock<IMessageProducer> mockProducer;
        private string testScanCode;

        [TestInitialize]
        public void Initialize()
        {
            testScanCode = "22222222";

            mockSerializer = new Mock<ISerializer<EwicExclusionMessageModel>>();
            mockGetEwicAgenciesWithExclusionQuery = new Mock<IQueryHandler<GetEwicAgenciesWithExclusionParameters, List<Agency>>>();
            mockRemoveEwicExclusionCommandHandler = new Mock<ICommandHandler<RemoveEwicExclusionCommand>>();
            mockSaveToMessageHistoryCommandHandler = new Mock<ICommandHandler<SaveToMessageHistoryCommand>>();
            mockUpdateMessageHistoryMessageCommandHandler = new Mock<ICommandHandler<UpdateMessageHistoryMessageCommand>>();
            mockProducer = new Mock<IMessageProducer>();

            managerHandler = new RemoveEwicExclusionManagerHandler(
                mockSerializer.Object,
                mockGetEwicAgenciesWithExclusionQuery.Object,
                mockRemoveEwicExclusionCommandHandler.Object,
                mockSaveToMessageHistoryCommandHandler.Object,
                mockUpdateMessageHistoryMessageCommandHandler.Object,
                mockProducer.Object);

            mockGetEwicAgenciesWithExclusionQuery.Setup(q => q.Search(It.IsAny<GetEwicAgenciesWithExclusionParameters>())).Returns(new List<Agency> { new Agency() });

            var mockXml = new XDocument(new XElement("body"));
            mockSerializer.Setup(s => s.Serialize(It.IsAny<EwicExclusionMessageModel>())).Returns(mockXml.ToString());
        }

        [TestMethod]
        public void RemoveEwicExclusion_Execute_AgenciesWithTheExclusionShouldBeQueried()
        {
            // Given.
            var manager = new RemoveEwicExclusionManager
            {
                ScanCode = testScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockGetEwicAgenciesWithExclusionQuery.Verify(q => q.Search(It.IsAny<GetEwicAgenciesWithExclusionParameters>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(CommandException))]
        public void RemoveEwicExclusion_ExclusionQueryEncountersError_ExceptionShouldBeThrown()
        {
            // Given.
            mockGetEwicAgenciesWithExclusionQuery.Setup(q => q.Search(It.IsAny<GetEwicAgenciesWithExclusionParameters>())).Throws(new Exception());

            var manager = new RemoveEwicExclusionManager
            {
                ScanCode = testScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RemoveEwicExclusion_NoAgenciesHaveTheExclusion_ExceptionShouldBeThrown()
        {
            // Given.
            mockGetEwicAgenciesWithExclusionQuery.Setup(q => q.Search(It.IsAny<GetEwicAgenciesWithExclusionParameters>())).Returns(new List<Agency>());

            var manager = new RemoveEwicExclusionManager
            {
                ScanCode = testScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            // Expected exception.
        }

        [TestMethod]
        [ExpectedException(typeof(CommandException))]
        public void RemoveEwicExclusion_RemoveEwicExclusionCommandThrowsException_SerializerShouldNotBeCalled()
        {
            // Given.
            mockRemoveEwicExclusionCommandHandler.Setup(c => c.Execute(It.IsAny<RemoveEwicExclusionCommand>())).Throws(new Exception());

            var manager = new RemoveEwicExclusionManager
            {
                ScanCode = testScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockRemoveEwicExclusionCommandHandler.Verify(c => c.Execute(It.IsAny<RemoveEwicExclusionCommand>()), Times.Once);
            mockSerializer.Verify(s => s.Serialize(It.Is<EwicExclusionMessageModel>(m => m.ActionTypeId == MessageActionTypes.Delete)), Times.Never);
        }

        [TestMethod]
        public void RemoveEwicExclusion_RemoveEwicExclusionCommandIsSuccessful_MessagesShouldBeSerialized()
        {
            // Given.
            var manager = new RemoveEwicExclusionManager
            {
                ScanCode = testScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockRemoveEwicExclusionCommandHandler.Verify(c => c.Execute(It.IsAny<RemoveEwicExclusionCommand>()), Times.Once);
            mockSerializer.Verify(s => s.Serialize(It.Is<EwicExclusionMessageModel>(m => m.ActionTypeId == MessageActionTypes.Delete)), Times.Once);
        }

        [TestMethod]
        public void RemoveEwicExclusion_SerializationIsSuccessful_SendMessageCommandShouldBeCalled()
        {
            // Given.
            var manager = new RemoveEwicExclusionManager
            {
                ScanCode = testScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockRemoveEwicExclusionCommandHandler.Verify(c => c.Execute(It.IsAny<RemoveEwicExclusionCommand>()), Times.Once);
            mockSerializer.Verify(s => s.Serialize(It.Is<EwicExclusionMessageModel>(m => m.ActionTypeId == MessageActionTypes.Delete)), Times.Once);
            mockProducer.Verify(p => p.SendMessages(It.IsAny<List<MessageHistory>>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(CommandException))]
        public void RemoveEwicExclusion_SendMessageCommandEncountersError_ExceptionShouldBeThrown()
        {
            // Given.
            mockProducer.Setup(p => p.SendMessages(It.IsAny<List<MessageHistory>>())).Throws(new Exception());

            var manager = new RemoveEwicExclusionManager
            {
                ScanCode = testScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockRemoveEwicExclusionCommandHandler.Verify(c => c.Execute(It.IsAny<RemoveEwicExclusionCommand>()), Times.Once);
            mockSerializer.Verify(s => s.Serialize(It.Is<EwicExclusionMessageModel>(m => m.ActionTypeId == MessageActionTypes.Delete)), Times.Once);
            mockProducer.Verify(p => p.SendMessages(It.IsAny<List<MessageHistory>>()), Times.Once);
        }

        [TestMethod]
        public void RemoveEwicExclusion_SendIsSuccessful_MessagesShouldBeSaved()
        {
            // Given.
            var manager = new RemoveEwicExclusionManager
            {
                ScanCode = testScanCode
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockRemoveEwicExclusionCommandHandler.Verify(c => c.Execute(It.IsAny<RemoveEwicExclusionCommand>()), Times.Once);
            mockSerializer.Verify(s => s.Serialize(It.Is<EwicExclusionMessageModel>(m => m.ActionTypeId == MessageActionTypes.Delete)), Times.Once);
            mockProducer.Verify(p => p.SendMessages(It.IsAny<List<MessageHistory>>()), Times.Once);
            mockSaveToMessageHistoryCommandHandler.Verify(c => c.Execute(It.Is<SaveToMessageHistoryCommand>(h =>
                h.Messages.TrueForAll(m =>
                    m.MessageTypeId == MessageTypes.Ewic &&
                    m.MessageStatusId == MessageStatusTypes.Sent)
                )), Times.Once);
        }
    }
}
