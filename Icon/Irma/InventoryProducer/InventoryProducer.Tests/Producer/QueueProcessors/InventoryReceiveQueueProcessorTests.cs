using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Icon.ActiveMQ.Producer;
using InventoryProducer.Common;
using InventoryProducer.Common.InstockDequeue;
using InventoryProducer.Common.InstockDequeue.Model;
using InventoryProducer.Producer.Model.DBModel;
using InventoryProducer.Producer.QueueProcessors;
using InventoryProducer.Producer.Helpers;
using InventoryProducer.Producer.Publish;
using InventoryProducer.Producer.Mapper;
using InventoryProducer.Producer.DataAccess;

using OrderReceipts = Icon.Esb.Schemas.Wfm.Contracts.orderReceipts;


namespace InventoryProducer.Tests.Producer.QueueProcessors
{
    [TestClass]
    public class InventoryReceiveQueueProcessorTests
    {
        private InventoryReceiveQueueProcessor queueProcessor;

        private Mock<ICanonicalMapper<OrderReceipts, ReceiveModel>> receiveXmlCanonicalMapper;
        private IMessagePublisher messagePublisher;
        private Mock<IErrorEventPublisher> errorEventPublisher;
        private Mock<IDAL<ReceiveModel>> receiveDAL;
        private Mock<IInstockDequeueService> instockDequeueService;
        private Mock<IArchiveInventoryEvents> archiveInventoryEvents;
        private Mock<IInventoryLogger<QueueProcessor<OrderReceipts, ReceiveModel>>> inventoryLogger;
        private Mock<IActiveMQProducer> activeMQProducer;
        private InventoryProducerSettings settings;

        [TestInitialize]
        public void Initialize()
        {
            receiveXmlCanonicalMapper = new Mock<ICanonicalMapper<OrderReceipts, ReceiveModel>>();
            errorEventPublisher = new Mock<IErrorEventPublisher>();
            receiveDAL = new Mock<IDAL<ReceiveModel>>();
            instockDequeueService = new Mock<IInstockDequeueService>();
            archiveInventoryEvents = new Mock<IArchiveInventoryEvents>();
            inventoryLogger = new Mock<IInventoryLogger<QueueProcessor<OrderReceipts, ReceiveModel>>>();
            settings = new InventoryProducerSettings();

            activeMQProducer = new Mock<IActiveMQProducer>();

            messagePublisher = new MessagePublisher(activeMQProducer.Object);

            queueProcessor = new InventoryReceiveQueueProcessor(
                settings,
                inventoryLogger.Object,
                instockDequeueService.Object,
                receiveDAL.Object,
                receiveXmlCanonicalMapper.Object,
                messagePublisher,
                archiveInventoryEvents.Object,
                errorEventPublisher.Object);
        }

        [TestMethod]
        public void QueueProcessor_ProcessMessageQueue_WhenInstockDequeue_ReturnsEmpty()
        {
            // Given
            instockDequeueService.Setup(i => i.GetDequeuedMessages()).Returns(new List<InstockDequeueResult> { });

            // When
            queueProcessor.ProcessMessageQueue();

            //Assert
            instockDequeueService.Verify(i => i.GetDequeuedMessages(), Times.Once);
            errorEventPublisher.Verify(e => e.PublishErrorEventToMammoth(It.IsAny<InstockDequeueResult>(), It.IsAny<Exception>()), Times.Never);
            receiveDAL.Verify(t => t.Get(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>()), Times.Never);
            receiveXmlCanonicalMapper.Verify(t => t.TransformToXmlCanonical(It.IsAny<IList<ReceiveModel>>(), It.IsAny<InstockDequeueResult>()), Times.Never);
            activeMQProducer.Verify(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
            archiveInventoryEvents.Verify(a => a.Archive(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
            ), Times.Never);
        }

        [TestMethod]
        public void QueueProcessor_ProcessMessageQueue_WhenReceiveDAL_ReturnsEmpty()
        {
            // Given
            instockDequeueService.Setup(i => i.GetDequeuedMessages()).Returns(
                new List<InstockDequeueResult> {
                    TestResources.GetInstockDequeueResult("RCPT_CRE")
                }
            );
            receiveDAL.Setup(t => t.Get(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>())).Returns(
                new List<ReceiveModel> { }
            );

            // When
            queueProcessor.ProcessMessageQueue();

            // Assert
            instockDequeueService.Verify(i => i.GetDequeuedMessages(), Times.Once);
            errorEventPublisher.Verify(e => e.PublishErrorEventToMammoth(It.IsAny<InstockDequeueResult>(), It.IsAny<Exception>()), Times.Never);
            receiveDAL.Verify(t => t.Get(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>()), Times.Once);
            receiveXmlCanonicalMapper.Verify(t => t.TransformToXmlCanonical(It.IsAny<IList<ReceiveModel>>(), It.IsAny<InstockDequeueResult>()), Times.Never);
            activeMQProducer.Verify(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
            archiveInventoryEvents.Verify(a => a.Archive(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
            ), Times.Never);
        }

        [TestMethod]
        public void QueueProcessor_ProcessMessageQueue_WhenReceiveDAL_ThrowsException()
        {
            // Given
            instockDequeueService.Setup(i => i.GetDequeuedMessages()).Returns(
                new List<InstockDequeueResult> {
                    TestResources.GetInstockDequeueResult("RCPT_CRE")
                }
            );
            receiveDAL.Setup(t => t.Get(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>())).Throws(
                new Exception("Test")
            );

            // When
            queueProcessor.ProcessMessageQueue();

            // Assert
            instockDequeueService.Verify(i => i.GetDequeuedMessages(), Times.Once);
            errorEventPublisher.Verify(e => e.PublishErrorEventToMammoth(It.IsAny<InstockDequeueResult>(), It.IsAny<Exception>()), Times.Once);
            receiveDAL.Verify(t => t.Get(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>()), Times.Exactly(1));
            receiveXmlCanonicalMapper.Verify(t => t.TransformToXmlCanonical(It.IsAny<IList<ReceiveModel>>(), It.IsAny<InstockDequeueResult>()), Times.Never);
            activeMQProducer.Verify(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
            archiveInventoryEvents.Verify(a => a.Archive(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
            ), Times.Never);
        }

        [TestMethod]
        public void QueueProcessor_ProcessMessageQueue_WhenMapper_ThrowsError()
        {
            // Given
            instockDequeueService.Setup(i => i.GetDequeuedMessages()).Returns(
                new List<InstockDequeueResult> {
                    TestResources.GetInstockDequeueResult("RCPT_CRE")
                }
            );
            receiveDAL.Setup(t => t.Get(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>())).Returns(
                TestResources.GetReceiveList(1)
            );
            receiveXmlCanonicalMapper.Setup(m => m.TransformToXmlCanonical(It.IsAny<IList<ReceiveModel>>(), It.IsAny<InstockDequeueResult>())).Throws(new Exception("Test"));

            // When
            queueProcessor.ProcessMessageQueue();

            // Assert
            instockDequeueService.Verify(i => i.GetDequeuedMessages(), Times.Once);
            errorEventPublisher.Verify(e => e.PublishErrorEventToMammoth(It.IsAny<InstockDequeueResult>(), It.IsAny<Exception>()), Times.Once);
            receiveDAL.Verify(t => t.Get(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>()), Times.Exactly(1));
            receiveXmlCanonicalMapper.Verify(t => t.TransformToXmlCanonical(It.IsAny<IList<ReceiveModel>>(), It.IsAny<InstockDequeueResult>()), Times.Once);
            activeMQProducer.Verify(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
            archiveInventoryEvents.Verify(a => a.Archive(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
            ), Times.Never);
        }

        [TestMethod]
        public void QueueProcessor_ProcessMessageQueue_WhenMapperSerialization_ThrowsError()
        {
            // Given
            instockDequeueService.Setup(i => i.GetDequeuedMessages()).Returns(
                new List<InstockDequeueResult> {
                    TestResources.GetInstockDequeueResult("RCPT_CRE")
                }
            );
            receiveDAL.Setup(t => t.Get(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>())).Returns(
                TestResources.GetReceiveList(1)
            );
            receiveXmlCanonicalMapper.Setup(m => m.TransformToXmlCanonical(It.IsAny<IList<ReceiveModel>>(), It.IsAny<InstockDequeueResult>())).Returns(new OrderReceipts());
            receiveXmlCanonicalMapper.Setup(m => m.SerializeToXml(It.IsAny<OrderReceipts>())).Throws(new Exception("Test"));

            // When
            queueProcessor.ProcessMessageQueue();

            // Assert
            instockDequeueService.Verify(i => i.GetDequeuedMessages(), Times.Once);
            receiveDAL.Verify(t => t.Get(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>()), Times.Once);
            receiveXmlCanonicalMapper.Verify(t => t.TransformToXmlCanonical(It.IsAny<IList<ReceiveModel>>(), It.IsAny<InstockDequeueResult>()), Times.Once);
            receiveXmlCanonicalMapper.Verify(t => t.SerializeToXml(It.IsAny<OrderReceipts>()), Times.Once);
            activeMQProducer.Verify(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
            archiveInventoryEvents.Verify(a => a.Archive(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
            ), Times.Never);
            errorEventPublisher.Verify(e => e.PublishErrorEventToMammoth(It.IsAny<InstockDequeueResult>(), It.IsAny<Exception>()), Times.Once);
        }

        [TestMethod]
        public void QueueProcessor_ProcessMessageQueue_WhenNoError()
        {
            // Given
            instockDequeueService.Setup(i => i.GetDequeuedMessages()).Returns(
                new List<InstockDequeueResult> {
                    TestResources.GetInstockDequeueResult("RCPT_CRE")
                }
            );
            receiveDAL.Setup(t => t.Get(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>())).Returns(
                TestResources.GetReceiveList(1)
            );
            receiveXmlCanonicalMapper.Setup(m => m.TransformToXmlCanonical(It.IsAny<IList<ReceiveModel>>(), It.IsAny<InstockDequeueResult>())).Returns(new OrderReceipts());
            receiveXmlCanonicalMapper.Setup(m => m.SerializeToXml(It.IsAny<OrderReceipts>())).Returns("XML");
            activeMQProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
            archiveInventoryEvents.Setup(a => a.Archive(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            // When
            queueProcessor.ProcessMessageQueue();

            // Assert
            instockDequeueService.Verify(i => i.GetDequeuedMessages(), Times.Once);
            receiveDAL.Verify(t => t.Get(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>()), Times.Exactly(1));
            receiveXmlCanonicalMapper.Verify(t => t.TransformToXmlCanonical(It.IsAny<IList<ReceiveModel>>(), It.IsAny<InstockDequeueResult>()), Times.Once);
            receiveXmlCanonicalMapper.Verify(t => t.SerializeToXml(It.IsAny<OrderReceipts>()), Times.Once);
            activeMQProducer.Verify(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
            archiveInventoryEvents.Verify(a => a.Archive(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
            ), Times.Once);
            errorEventPublisher.Verify(e => e.PublishErrorEventToMammoth(It.IsAny<InstockDequeueResult>(), It.IsAny<Exception>()), Times.Never);
        }
    }
}
