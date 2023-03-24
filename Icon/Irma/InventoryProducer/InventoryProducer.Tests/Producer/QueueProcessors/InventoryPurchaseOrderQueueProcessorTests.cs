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

using PurchaseOrderCanonical = Icon.Esb.Schemas.Wfm.Contracts.PurchaseOrders;


namespace InventoryProducer.Tests.Producer.QueueProcessors
{
    [TestClass]
    public class InventoryPurchaseOrderQueueProcessorTests
    {
        private InventoryPurchaseOrderQueueProcessor queueProcessor;

        private Mock<ICanonicalMapper<PurchaseOrderCanonical, PurchaseOrdersModel>> purchaseOrderCanonicalMapper;
        private IMessagePublisher messagePublisher;
        private Mock<IErrorEventPublisher> errorEventPublisher;
        private Mock<IDAL<PurchaseOrdersModel>> purchaseOrderDAL;
        private Mock<IInstockDequeueService> instockDequeueService;
        private Mock<IArchiveInventoryEvents> archiveInventoryEvents;
        private Mock<IInventoryLogger<QueueProcessor<PurchaseOrderCanonical, PurchaseOrdersModel>>> inventoryLogger;
        private Mock<IActiveMQProducer> activeMQProducer;
        private InventoryProducerSettings settings;

        [TestInitialize]
        public void Initialize()
        {
            purchaseOrderCanonicalMapper = new Mock<ICanonicalMapper<PurchaseOrderCanonical, PurchaseOrdersModel>>();
            errorEventPublisher = new Mock<IErrorEventPublisher>();
            purchaseOrderDAL = new Mock<IDAL<PurchaseOrdersModel>>();
            instockDequeueService = new Mock<IInstockDequeueService>();
            archiveInventoryEvents = new Mock<IArchiveInventoryEvents>();
            inventoryLogger = new Mock<IInventoryLogger<QueueProcessor<PurchaseOrderCanonical, PurchaseOrdersModel>>>();
            settings = new InventoryProducerSettings();
            activeMQProducer = new Mock<IActiveMQProducer>();

            messagePublisher = new MessagePublisher(activeMQProducer.Object);

            queueProcessor = new InventoryPurchaseOrderQueueProcessor(
                settings,
                inventoryLogger.Object,
                instockDequeueService.Object,
                purchaseOrderDAL.Object,
                purchaseOrderCanonicalMapper.Object,
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
            purchaseOrderDAL.Verify(t => t.Get(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>()), Times.Never);
            purchaseOrderCanonicalMapper.Verify(t => t.TransformToXmlCanonical(It.IsAny<IList<PurchaseOrdersModel>>(), It.IsAny<InstockDequeueResult>()), Times.Never);
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
                    TestResources.GetInstockDequeueResult("PO_CRE")
                }
            );
            purchaseOrderDAL.Setup(t => t.Get(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>())).Returns(
                new List<PurchaseOrdersModel> { }
            );

            // When
            queueProcessor.ProcessMessageQueue();

            // Assert
            instockDequeueService.Verify(i => i.GetDequeuedMessages(), Times.Once);
            errorEventPublisher.Verify(e => e.PublishErrorEventToMammoth(It.IsAny<InstockDequeueResult>(), It.IsAny<Exception>()), Times.Never);
            purchaseOrderDAL.Verify(t => t.Get(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>()), Times.Once);
            purchaseOrderCanonicalMapper.Verify(t => t.TransformToXmlCanonical(It.IsAny<IList<PurchaseOrdersModel>>(), It.IsAny<InstockDequeueResult>()), Times.Never);
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
                    TestResources.GetInstockDequeueResult("PO_CRE")
                }
            );
            purchaseOrderDAL.Setup(t => t.Get(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>())).Throws(
                new Exception("Test")
            );

            // When
            queueProcessor.ProcessMessageQueue();

            // Assert
            instockDequeueService.Verify(i => i.GetDequeuedMessages(), Times.Once);
            errorEventPublisher.Verify(e => e.PublishErrorEventToMammoth(It.IsAny<InstockDequeueResult>(), It.IsAny<Exception>()), Times.Once);
            purchaseOrderDAL.Verify(t => t.Get(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>()), Times.Exactly(1));
            purchaseOrderCanonicalMapper.Verify(t => t.TransformToXmlCanonical(It.IsAny<IList<PurchaseOrdersModel>>(), It.IsAny<InstockDequeueResult>()), Times.Never);
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
                    TestResources.GetInstockDequeueResult("PO_CRE")
                }
            );
            purchaseOrderDAL.Setup(t => t.Get(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>())).Returns(
                TestResources.GetPurchaseOrderList(1)
            );
            purchaseOrderCanonicalMapper.Setup(m => m.TransformToXmlCanonical(It.IsAny<IList<PurchaseOrdersModel>>(), It.IsAny<InstockDequeueResult>())).Throws(new Exception("Test"));

            // When
            queueProcessor.ProcessMessageQueue();

            // Assert
            instockDequeueService.Verify(i => i.GetDequeuedMessages(), Times.Once);
            errorEventPublisher.Verify(e => e.PublishErrorEventToMammoth(It.IsAny<InstockDequeueResult>(), It.IsAny<Exception>()), Times.Once);
            purchaseOrderDAL.Verify(t => t.Get(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>()), Times.Exactly(1));
            purchaseOrderCanonicalMapper.Verify(t => t.TransformToXmlCanonical(It.IsAny<IList<PurchaseOrdersModel>>(), It.IsAny<InstockDequeueResult>()), Times.Once);
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
                    TestResources.GetInstockDequeueResult("PO_CRE")
                }
            );
            purchaseOrderDAL.Setup(t => t.Get(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>())).Returns(
                TestResources.GetPurchaseOrderList(1)
            );
            purchaseOrderCanonicalMapper.Setup(m => m.TransformToXmlCanonical(It.IsAny<IList<PurchaseOrdersModel>>(), It.IsAny<InstockDequeueResult>())).Returns(new PurchaseOrderCanonical());
            purchaseOrderCanonicalMapper.Setup(m => m.SerializeToXml(It.IsAny<PurchaseOrderCanonical>())).Throws(new Exception("Test"));

            // When
            queueProcessor.ProcessMessageQueue();

            // Assert
            instockDequeueService.Verify(i => i.GetDequeuedMessages(), Times.Once);
            purchaseOrderDAL.Verify(t => t.Get(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>()), Times.Once);
            purchaseOrderCanonicalMapper.Verify(t => t.TransformToXmlCanonical(It.IsAny<IList<PurchaseOrdersModel>>(), It.IsAny<InstockDequeueResult>()), Times.Once);
            purchaseOrderCanonicalMapper.Verify(t => t.SerializeToXml(It.IsAny<PurchaseOrderCanonical>()), Times.Once);
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
                    TestResources.GetInstockDequeueResult("PO_CRE")
                }
            );
            purchaseOrderDAL.Setup(t => t.Get(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>())).Returns(
                TestResources.GetPurchaseOrderList(1)
            );
            purchaseOrderCanonicalMapper.Setup(m => m.TransformToXmlCanonical(It.IsAny<IList<PurchaseOrdersModel>>(), It.IsAny<InstockDequeueResult>())).Returns(new PurchaseOrderCanonical());
            purchaseOrderCanonicalMapper.Setup(m => m.SerializeToXml(It.IsAny<PurchaseOrderCanonical>())).Returns("XML");
            activeMQProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
            archiveInventoryEvents.Setup(a => a.Archive(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            // When
            queueProcessor.ProcessMessageQueue();

            // Assert
            instockDequeueService.Verify(i => i.GetDequeuedMessages(), Times.Once);
            purchaseOrderDAL.Verify(t => t.Get(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>()), Times.Exactly(1));
            purchaseOrderCanonicalMapper.Verify(t => t.TransformToXmlCanonical(It.IsAny<IList<PurchaseOrdersModel>>(), It.IsAny<InstockDequeueResult>()), Times.Once);
            purchaseOrderCanonicalMapper.Verify(t => t.SerializeToXml(It.IsAny<PurchaseOrderCanonical>()), Times.Once);
            activeMQProducer.Verify(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
            archiveInventoryEvents.Verify(a => a.Archive(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
            ), Times.Once);
            errorEventPublisher.Verify(e => e.PublishErrorEventToMammoth(It.IsAny<InstockDequeueResult>(), It.IsAny<Exception>()), Times.Never);
        }
    }
}

