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

using TransferOrdersCanonical = Icon.Esb.Schemas.Wfm.Contracts.transferOrders;


namespace InventoryProducer.Tests.Producer.QueueProcessors
{
    [TestClass]
    public class InventoryTransferQueueProcessorTests
    {
        private InventoryTransferQueueProcessor queueProcessor;

        private Mock<ITransferOrderCanonicalMapper> transferOrderXmlCanonicalMapper;
        private IMessagePublisher messagePublisher;
        private Mock<IErrorEventPublisher> errorEventPublisher;
        private Mock<ITransferOrdersDAL> transferOrdersDal;
        private Mock<IInstockDequeueService> instockDequeueService;
        private Mock<IArchiveInventoryEvents> archiveInventoryEvents;
        private Mock<IInventoryLogger<InventoryTransferQueueProcessor>> inventoryLogger;
        private Mock<IActiveMQProducer> activeMQProducer;
        private InventoryProducerSettings settings;

        [TestInitialize]
        public void Initialize()
        {
            transferOrderXmlCanonicalMapper = new Mock<ITransferOrderCanonicalMapper>();
            errorEventPublisher = new Mock<IErrorEventPublisher>();
            transferOrdersDal = new Mock<ITransferOrdersDAL>();
            instockDequeueService = new Mock<IInstockDequeueService>();
            archiveInventoryEvents = new Mock<IArchiveInventoryEvents>();
            inventoryLogger = new Mock<IInventoryLogger<InventoryTransferQueueProcessor>>();
            settings = new InventoryProducerSettings();

            activeMQProducer = new Mock<IActiveMQProducer>();

            messagePublisher = new MessagePublisher(activeMQProducer.Object);

            queueProcessor = new InventoryTransferQueueProcessor(
                settings, 
                inventoryLogger.Object,
                instockDequeueService.Object, 
                transferOrdersDal.Object, 
                transferOrderXmlCanonicalMapper.Object, 
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
            transferOrdersDal.Verify(t => t.GetTransferOrders(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>()), Times.Never);
            transferOrderXmlCanonicalMapper.Verify(t => t.TransformToXmlCanonical(It.IsAny<IList<TransferOrdersModel>>(), It.IsAny<InstockDequeueResult>()), Times.Never);
            activeMQProducer.Verify(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
            archiveInventoryEvents.Verify(a => a.Archive(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
            ), Times.Never);
        }

        [TestMethod]
        public void QueueProcessor_ProcessMessageQueue_WhenTransferOrdersDal_ReturnsEmpty()
        {
            // Given
            instockDequeueService.Setup(i => i.GetDequeuedMessages()).Returns(
                new List<InstockDequeueResult> {
                    TestResources.GetInstockDequeueResult("TSF_CRE")
                }
            );
            transferOrdersDal.Setup(t => t.GetTransferOrders(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>())).Returns(
                new List<TransferOrdersModel> { }
            );

            // When
            queueProcessor.ProcessMessageQueue();

            // Assert
            instockDequeueService.Verify(i => i.GetDequeuedMessages(), Times.Once);
            errorEventPublisher.Verify(e => e.PublishErrorEventToMammoth(It.IsAny<InstockDequeueResult>(), It.IsAny<Exception>()), Times.Never);
            transferOrdersDal.Verify(t => t.GetTransferOrders(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>()), Times.Once);
            transferOrderXmlCanonicalMapper.Verify(t => t.TransformToXmlCanonical(It.IsAny<IList<TransferOrdersModel>>(), It.IsAny<InstockDequeueResult>()), Times.Never);
            activeMQProducer.Verify(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
            archiveInventoryEvents.Verify(a => a.Archive(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
            ), Times.Never);
        }
        
        [TestMethod]
        public void QueueProcessor_ProcessMessageQueue_WhenTransferOrdersDal_ThrowsException()
        {
            // Given
            instockDequeueService.Setup(i => i.GetDequeuedMessages()).Returns(
                new List<InstockDequeueResult> {
                    TestResources.GetInstockDequeueResult("TSF_CRE")
                }
            );
            transferOrdersDal.Setup(t => t.GetTransferOrders(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>())).Throws(
                new Exception("Test")
            );

            // When
            queueProcessor.ProcessMessageQueue();

            // Assert
            instockDequeueService.Verify(i => i.GetDequeuedMessages(), Times.Once);
            errorEventPublisher.Verify(e => e.PublishErrorEventToMammoth(It.IsAny<InstockDequeueResult>(), It.IsAny<Exception>()), Times.Once);
            transferOrdersDal.Verify(t => t.GetTransferOrders(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>()), Times.Exactly(1));
            transferOrderXmlCanonicalMapper.Verify(t => t.TransformToXmlCanonical(It.IsAny<IList<TransferOrdersModel>>(), It.IsAny<InstockDequeueResult>()), Times.Never);
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
                    TestResources.GetInstockDequeueResult("TSF_CRE")
                }
            );
            transferOrdersDal.Setup(t => t.GetTransferOrders(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>())).Returns(
                TestResources.GetTransferOrdersList(1)
            );
            transferOrderXmlCanonicalMapper.Setup(m => m.TransformToXmlCanonical(It.IsAny<IList<TransferOrdersModel>>(), It.IsAny<InstockDequeueResult>())).Throws(new Exception("Test"));

            // When
            queueProcessor.ProcessMessageQueue();

            // Assert
            instockDequeueService.Verify(i => i.GetDequeuedMessages(), Times.Once);
            errorEventPublisher.Verify(e => e.PublishErrorEventToMammoth(It.IsAny<InstockDequeueResult>(), It.IsAny<Exception>()), Times.Once);
            transferOrdersDal.Verify(t => t.GetTransferOrders(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>()), Times.Exactly(1));
            transferOrderXmlCanonicalMapper.Verify(t => t.TransformToXmlCanonical(It.IsAny<IList<TransferOrdersModel>>(), It.IsAny<InstockDequeueResult>()), Times.Once);
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
                    TestResources.GetInstockDequeueResult("TSF_CRE")
                }
            );
            transferOrdersDal.Setup(t => t.GetTransferOrders(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>())).Returns(
                TestResources.GetTransferOrdersList(1)
            );
            transferOrderXmlCanonicalMapper.Setup(m => m.TransformToXmlCanonical(It.IsAny<IList<TransferOrdersModel>>(), It.IsAny<InstockDequeueResult>())).Returns(new TransferOrdersCanonical());
            transferOrderXmlCanonicalMapper.Setup(m => m.SerializeToXml(It.IsAny<TransferOrdersCanonical>())).Throws(new Exception("test"));

            // When
            queueProcessor.ProcessMessageQueue();

            // Assert
            instockDequeueService.Verify(i => i.GetDequeuedMessages(), Times.Once);
            transferOrdersDal.Verify(t => t.GetTransferOrders(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>()), Times.Once);
            transferOrderXmlCanonicalMapper.Verify(t => t.TransformToXmlCanonical(It.IsAny<IList<TransferOrdersModel>>(), It.IsAny<InstockDequeueResult>()), Times.Once);
            transferOrderXmlCanonicalMapper.Verify(t => t.SerializeToXml(It.IsAny<TransferOrdersCanonical>()), Times.Once);
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
                    TestResources.GetInstockDequeueResult("TSF_CRE")
                }
            );
            transferOrdersDal.Setup(t => t.GetTransferOrders(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>())).Returns(
                TestResources.GetTransferOrdersList(1)
            );
            transferOrderXmlCanonicalMapper.Setup(m => m.TransformToXmlCanonical(It.IsAny<IList<TransferOrdersModel>>(), It.IsAny<InstockDequeueResult>())).Returns(new TransferOrdersCanonical());
            transferOrderXmlCanonicalMapper.Setup(m => m.SerializeToXml(It.IsAny<TransferOrdersCanonical>())).Returns("XML");
            activeMQProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
            archiveInventoryEvents.Setup(a => a.Archive(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            
            // When
            queueProcessor.ProcessMessageQueue();

            // Assert
            instockDequeueService.Verify(i => i.GetDequeuedMessages(), Times.Once);
            transferOrdersDal.Verify(t => t.GetTransferOrders(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int?>()), Times.Exactly(1));
            transferOrderXmlCanonicalMapper.Verify(t => t.TransformToXmlCanonical(It.IsAny<IList<TransferOrdersModel>>(), It.IsAny<InstockDequeueResult>()), Times.Once);
            transferOrderXmlCanonicalMapper.Verify(t => t.SerializeToXml(It.IsAny<TransferOrdersCanonical>()), Times.Once);
            activeMQProducer.Verify(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
            archiveInventoryEvents.Verify(a => a.Archive(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<char>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
            ), Times.Once);
            errorEventPublisher.Verify(e => e.PublishErrorEventToMammoth(It.IsAny<InstockDequeueResult>(), It.IsAny<Exception>()), Times.Never);
        }
    }
}
