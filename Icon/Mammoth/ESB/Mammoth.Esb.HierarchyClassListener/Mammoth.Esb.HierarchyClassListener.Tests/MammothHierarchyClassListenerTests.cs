using System;
using System.Collections.Generic;
using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.MessageParsers;
using Icon.Esb.Subscriber;
using Icon.Logging;
using Mammoth.Esb.HierarchyClassListener.Models;
using Mammoth.Esb.HierarchyClassListener.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mammoth.Esb.HierarchyClassListener.Commands;
using TIBCO.EMS;
using Mammoth.Common.DataAccess;
using Icon.Esb.Schemas.Wfm.Contracts;

namespace Mammoth.Esb.HierarchyClassListener.Tests
{
    [TestClass]
    public class MammothHierarchyClassListenerTests
    {
        private MammothHierarchyClassListener listener;
        private EsbConnectionSettings esbConnectionSettings;
        private ListenerApplicationSettings listenerApplicationSettings;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IHierarchyClassService<AddOrUpdateHierarchyClassRequest>> mockHierarchyClassService;
        private Mock<IHierarchyClassService<DeleteBrandRequest>> mockDeleteBrandService;
        private Mock<IHierarchyClassService<DeleteMerchandiseClassRequest>> mockDeleteMerchService;
        private Mock<ILogger<MammothHierarchyClassListener>> mockLogger;
        private Mock<IMessageParser<List<HierarchyClassModel>>> mockMessageParser;
        private Mock<IEsbSubscriber> mockSubscriber;
        private EsbMessageEventArgs eventArgs;
        private Mock<IEsbMessage> mockEsbMessage;

        [TestInitialize]
        public void Initialize()
        {
            esbConnectionSettings = new EsbConnectionSettings { SessionMode = SessionMode.ClientAcknowledge };
            listenerApplicationSettings = new ListenerApplicationSettings();
            mockEmailClient = new Mock<IEmailClient>();
            mockHierarchyClassService = new Mock<IHierarchyClassService<AddOrUpdateHierarchyClassRequest>>();
            mockDeleteBrandService = new Mock<IHierarchyClassService<DeleteBrandRequest>>();
            mockDeleteMerchService = new Mock<IHierarchyClassService<DeleteMerchandiseClassRequest>>();
            mockLogger = new Mock<ILogger<MammothHierarchyClassListener>>();
            mockMessageParser = new Mock<IMessageParser<List<HierarchyClassModel>>>();
            mockSubscriber = new Mock<IEsbSubscriber>();

            this.listener = new MammothHierarchyClassListener(listenerApplicationSettings,
                esbConnectionSettings,
                mockSubscriber.Object,
                mockEmailClient.Object,
                mockLogger.Object,
                mockMessageParser.Object,
                mockHierarchyClassService.Object,
                mockDeleteBrandService.Object,
                mockDeleteMerchService.Object);

            mockEsbMessage = new Mock<IEsbMessage>();
            eventArgs = new EsbMessageEventArgs { Message = mockEsbMessage.Object };
        }

        [TestMethod]
        public void HandleMessage_SuccessfulAddOrUpdateMessage_ShouldAcknowledgeMessageAndNotLogError()
        {
            //Given
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>()))
                .Returns(new List<HierarchyClassModel>
                {
                    new HierarchyClassModel
                    {
                        Action = ActionEnum.AddOrUpdate,
                        HierarchyClassId = 1,
                        HierarchyClassName = "test",
                        HierarchyId = Hierarchies.Brands
                    }
                });

            //When
            listener.HandleMessage(null, eventArgs);

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Never);
            mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
            mockHierarchyClassService.Verify(m => m.ProcessHierarchyClasses(It.IsAny<AddOrUpdateHierarchyClassRequest>()), Times.Once);
            mockEsbMessage.Verify(m => m.Acknowledge(), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_ErrorParsingMessage_ShouldLogAndNotifyException()
        {
            //Given
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>()))
                .Throws(new Exception());

            //When
            listener.HandleMessage(null, eventArgs);

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
            mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
            mockHierarchyClassService.Verify(m => m.ProcessHierarchyClasses(It.IsAny<AddOrUpdateHierarchyClassRequest>()), Times.Never);
            mockEsbMessage.Verify(m => m.Acknowledge(), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_ErrorCallingHierarchyClasses_ShouldLogAndNotifyException()
        {
            //Given
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>()))
                .Returns(new List<HierarchyClassModel>
                {
                    new HierarchyClassModel
                    {
                        Action = ActionEnum.AddOrUpdate,
                        HierarchyClassId = 1,
                        HierarchyClassName = "test",
                        HierarchyId = Hierarchies.Brands
                    }
                });
            mockHierarchyClassService.Setup(m => m.ProcessHierarchyClasses(It.IsAny<AddOrUpdateHierarchyClassRequest>()))
                .Throws(new Exception());

            //When
            listener.HandleMessage(null, eventArgs);

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
            mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
            mockHierarchyClassService.Verify(m => m.ProcessHierarchyClasses(It.IsAny<AddOrUpdateHierarchyClassRequest>()), Times.Once);
            mockEsbMessage.Verify(m => m.Acknowledge(), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_MessageHasBrandDeletes_ShouldCallBrandDeleteService()
        {
            // Given
            List<HierarchyClassModel> brands = new List<HierarchyClassModel>();
            brands.Add(new HierarchyClassModel
            {
                Action = Icon.Esb.Schemas.Wfm.Contracts.ActionEnum.Delete,
                HierarchyClassId = 4747454,
                HierarchyClassName = "Unit Test Brand 1",
                HierarchyClassParentId = 0,
                HierarchyId = Hierarchies.Brands,
                Timestamp = DateTime.UtcNow
            });

            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>()))
                .Returns(brands);

            // When
            listener.HandleMessage(null, eventArgs);

            // Then
            mockDeleteBrandService.Verify(ds => ds.ProcessHierarchyClasses(It.Is<DeleteBrandRequest>(r =>
                r.HierarchyClasses[0].HierarchyClassId == brands[0].HierarchyClassId
                && r.HierarchyClasses[0].HierarchyClassName == brands[0].HierarchyClassName
                && r.HierarchyClasses[0].HierarchyId == brands[0].HierarchyId)), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_MessageHasOnlyAddOrUpdateActions_ShouldCallOnlyAddOrUpdateHierarchyClassService()
        {
            // Given
            List<HierarchyClassModel> brands = new List<HierarchyClassModel>();
            brands.Add(new HierarchyClassModel
            {
                Action = Icon.Esb.Schemas.Wfm.Contracts.ActionEnum.AddOrUpdate,
                HierarchyClassId = 4747454,
                HierarchyClassName = "Unit Test Brand 1",
                HierarchyClassParentId = 0,
                HierarchyId = Hierarchies.Brands,
                Timestamp = DateTime.UtcNow
            });

            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>()))
                .Returns(brands);

            // When
            listener.HandleMessage(null, eventArgs);

            // Then
            mockDeleteBrandService.Verify(ds => ds.ProcessHierarchyClasses(It.IsAny<DeleteBrandRequest>()), Times.Never);
            mockDeleteMerchService.Verify(ds => ds.ProcessHierarchyClasses(It.IsAny<DeleteMerchandiseClassRequest>()), Times.Never);
            mockHierarchyClassService.Verify(s => s.ProcessHierarchyClasses(It.Is<AddOrUpdateHierarchyClassRequest>(r =>
                r.HierarchyClasses[0].Action == brands[0].Action
                && r.HierarchyClasses[0].HierarchyClassId == brands[0].HierarchyClassId
                && r.HierarchyClasses[0].HierarchyClassName == brands[0].HierarchyClassName
                && r.HierarchyClasses[0].HierarchyId == brands[0].HierarchyId)), Times.Once);
        }
    }
}
