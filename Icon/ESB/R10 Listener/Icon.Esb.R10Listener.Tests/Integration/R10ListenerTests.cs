using Icon.Common.Email;
using Icon.Esb.R10Listener.Commands;
using Icon.Esb.R10Listener.Constants;
using Icon.Esb.R10Listener.Context;
using Icon.Esb.R10Listener.Infrastructure.Cache;
using Icon.Esb.R10Listener.MessageParsers;
using Icon.Esb.R10Listener.Models;
using Icon.Esb.Subscriber;
using Icon.Framework;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TIBCO.EMS;

namespace Icon.Esb.R10Listener.Tests.Integration
{
    [TestClass]
    public class R10ListenerTests
    {
        private R10Listener r10Listener;
        private R10ListenerApplicationSettings applicationSettings;
        private EsbConnectionSettings connectionSettings;
        private Mock<IEsbSubscriber> mockEsbSubscriber;
        private Mock<ILogger<R10Listener>> mockLogger;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IEsbMessage> mockEsbMessage;
        private Mock<IMessageQueueResendStatusCache> mockCache;

        private IconContext context;
        private GlobalContext globalContext;
        private DbContextTransaction transaction;

        private MessageHistory messageHistory;

        [TestInitialize]
        public void Initialize()
        {
            mockEmailClient = new Mock<IEmailClient>();
            mockLogger = new Mock<ILogger<R10Listener>>();
            mockEsbSubscriber = new Mock<IEsbSubscriber>();
            mockCache = new Mock<IMessageQueueResendStatusCache>();
            context = new IconContext();
            globalContext = new GlobalContext(context);
            transaction = context.Database.BeginTransaction();

            messageHistory = new MessageHistory
                {
                    Message = "<test>test</test>",
                    MessageStatusId = MessageStatusTypes.Sent,
                    MessageTypeId = MessageTypes.Product,
                    InsertDate = DateTime.Now
                };

            applicationSettings = new R10ListenerApplicationSettings
                {
                    ResendMessageCount = 1
                };
            connectionSettings = new EsbConnectionSettings
                {
                    SessionMode = SessionMode.ClientAcknowledge
                };

            r10Listener = new R10Listener(applicationSettings,
                connectionSettings,
                mockEsbSubscriber.Object,
                new ProcessR10MessageResponseCommandHandler(globalContext,
                    new AddMessageResponseCommandHandler(globalContext),
                    new ProcessFailedR10MessageResponseCommandHandler(globalContext,
                        new ResendMessageQueueEntriesCommandHandler(globalContext, mockCache.Object, applicationSettings),
                        new ResendMessageCommandHandler(globalContext, applicationSettings))),
                new R10MessageResponseParser(),
                mockEmailClient.Object,
                mockLogger.Object);

            mockEsbMessage = new Mock<IEsbMessage>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Dispose();
        }

        [TestMethod]
        public void HandleMessage_ValidMessageResponse_ShouldAddMessageResponse()
        {
            //Given
            globalContext.Context.MessageHistory.Add(messageHistory);
            globalContext.SaveChanges();

            XElement message = XElement.Load(@"TestMessages/valid_message_response.xml");
            mockEsbMessage.SetupGet(m => m.MessageText).Returns(message.ToString());
            mockEsbMessage.Setup(m => m.GetProperty(It.Is<string>(s => s == "TransactionID")))
                .Returns(messageHistory.MessageHistoryId.ToString());

            //When
            r10Listener.HandleMessage(null, new EsbMessageEventArgs { Message = mockEsbMessage.Object });

            //Then
            Assert.AreEqual(MessageStatusTypes.Sent, messageHistory.MessageStatusId);

            var r10MessageResponse = messageHistory.R10MessageResponse.Single();
            Assert.AreEqual(messageHistory.MessageHistoryId, r10MessageResponse.MessageHistoryId);
            Assert.AreEqual(true, r10MessageResponse.RequestSuccess);
            Assert.AreEqual(false, r10MessageResponse.SystemError);
            Assert.AreEqual(message.ToString(), r10MessageResponse.ResponseText);
            Assert.AreEqual(null, r10MessageResponse.FailureReasonCode);

            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Never);
            mockEsbMessage.Verify(m => m.Acknowledge(), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_MessageHasRequestSuccessSetToFalse_ShouldAddMessageResponse()
        {
            //Given
            globalContext.Context.MessageHistory.Add(messageHistory);
            globalContext.SaveChanges();

            XElement message = XElement.Load(@"TestMessages/fail_bottle_deposit_maintenance_message.xml");
            mockEsbMessage.SetupGet(m => m.MessageText).Returns(message.ToString());
            mockEsbMessage.Setup(m => m.GetProperty(It.Is<string>(s => s == "TransactionID")))
                .Returns(messageHistory.MessageHistoryId.ToString());

            //When
            r10Listener.HandleMessage(null, new EsbMessageEventArgs { Message = mockEsbMessage.Object });

            //Then
            Assert.AreEqual(MessageStatusTypes.Failed, messageHistory.MessageStatusId);

            var r10MessageResponse = messageHistory.R10MessageResponse.Single();
            Assert.AreEqual(messageHistory.MessageHistoryId, r10MessageResponse.MessageHistoryId);
            Assert.AreEqual(false, r10MessageResponse.RequestSuccess);
            Assert.AreEqual(false, r10MessageResponse.SystemError);
            Assert.AreEqual(message.ToString(), r10MessageResponse.ResponseText);
            Assert.AreEqual("InvalidRequest", r10MessageResponse.FailureReasonCode);

            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
            mockEsbMessage.Verify(m => m.Acknowledge(), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_MessageIsSystemError_ShouldAddMessageResponse()
        {
            //Given
            globalContext.Context.MessageHistory.Add(messageHistory);
            globalContext.SaveChanges();

            XElement message = XElement.Load(@"TestMessages/fail_system_error.xml");
            mockEsbMessage.SetupGet(m => m.MessageText).Returns(message.ToString());
            mockEsbMessage.Setup(m => m.GetProperty(It.Is<string>(s => s == "TransactionID")))
                .Returns(messageHistory.MessageHistoryId.ToString());

            //When
            r10Listener.HandleMessage(null, new EsbMessageEventArgs { Message = mockEsbMessage.Object });

            //Then
            Assert.AreEqual(MessageStatusTypes.Failed, messageHistory.MessageStatusId);
            Assert.IsFalse(messageHistory.MessageResendStatus.Any());

            var r10MessageResponse = messageHistory.R10MessageResponse.Single();
            Assert.AreEqual(messageHistory.MessageHistoryId, r10MessageResponse.MessageHistoryId);
            Assert.AreEqual(false, r10MessageResponse.RequestSuccess);
            Assert.AreEqual(true, r10MessageResponse.SystemError);
            Assert.AreEqual(message.ToString(), r10MessageResponse.ResponseText);
            Assert.AreEqual("TEST_REASON_CODE", r10MessageResponse.FailureReasonCode);

            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
            mockEsbMessage.Verify(m => m.Acknowledge(), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_MessageIsSystemTimout_ShouldAddMessageResponseAndResendMessage()
        {
            //Given
            globalContext.Context.MessageHistory.Add(messageHistory);
            globalContext.SaveChanges();

            XElement message = XElement.Load(@"TestMessages/fail_system_timeout-price.xml");
            mockEsbMessage.SetupGet(m => m.MessageText).Returns(message.ToString());
            mockEsbMessage.Setup(m => m.GetProperty(It.Is<string>(s => s == "TransactionID")))
                .Returns(messageHistory.MessageHistoryId.ToString());

            //When
            r10Listener.HandleMessage(null, new EsbMessageEventArgs { Message = mockEsbMessage.Object });

            //Then
            Assert.AreEqual(MessageStatusTypes.Ready, messageHistory.MessageStatusId);
            Assert.IsTrue(messageHistory.MessageResendStatus.Any());

            var messageResendStatus = messageHistory.MessageResendStatus.Single();
            Assert.AreEqual(messageHistory.MessageHistoryId, messageResendStatus.MessageHistoryId);
            Assert.AreEqual(1, messageResendStatus.NumberOfResends);

            var r10MessageResponse = messageHistory.R10MessageResponse.Single();
            Assert.AreEqual(messageHistory.MessageHistoryId, r10MessageResponse.MessageHistoryId);
            Assert.AreEqual(false, r10MessageResponse.RequestSuccess);
            Assert.AreEqual(true, r10MessageResponse.SystemError);
            Assert.AreEqual(message.ToString(), r10MessageResponse.ResponseText);
            Assert.AreEqual("WFM_SYS_HTTP_TIMEOUT", r10MessageResponse.FailureReasonCode);

            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
            mockEsbMessage.Verify(m => m.Acknowledge(), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_MessageIsThresholdExceeded_ShouldAddMessageResponseAndReprocessMessageQueueEntries()
        {
            //Given
            XElement message = XElement.Load(@"TestMessages/fail_threshold_exceeded.xml");
            List<MessageQueueProduct> productMessages = CreateTestThresholdExceededMessageQueueProducts(message);

            foreach (var productMessage in productMessages)
            {
                messageHistory.MessageQueueProduct.Add(productMessage);
            }
            globalContext.Context.MessageHistory.Add(messageHistory);
            globalContext.SaveChanges();

            mockEsbMessage.SetupGet(m => m.MessageText).Returns(message.ToString());
            mockEsbMessage.Setup(m => m.GetProperty(It.Is<string>(s => s == "TransactionID")))
                .Returns(messageHistory.MessageHistoryId.ToString());
            mockCache.Setup(m => m.Get(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int messageTypeId, int messageQueueId) => new MessageQueueResendStatus { MessageQueueId = messageQueueId, NumberOfResends = 0 });

            //When
            r10Listener.HandleMessage(null, new EsbMessageEventArgs { Message = mockEsbMessage.Object });

            //Then
            Assert.AreEqual(MessageStatusTypes.Failed, messageHistory.MessageStatusId);
            Assert.IsFalse(messageHistory.MessageResendStatus.Any());

            var r10MessageResponse = messageHistory.R10MessageResponse.Single();
            Assert.AreEqual(messageHistory.MessageHistoryId, r10MessageResponse.MessageHistoryId);
            Assert.AreEqual(false, r10MessageResponse.RequestSuccess);
            Assert.AreEqual(false, r10MessageResponse.SystemError);
            Assert.AreEqual(message.ToString(), r10MessageResponse.ResponseText);
            Assert.AreEqual("ProductIdentifierNotUnique, ThresholdExceededError", r10MessageResponse.FailureReasonCode);

            Assert.AreEqual(21, messageHistory.MessageQueueProduct.Count);
            Assert.AreEqual(MessageStatusTypes.Associated, messageHistory.MessageQueueProduct.First().MessageStatusId);
            foreach (var productMessage in messageHistory.MessageQueueProduct.Skip(1))
            {
                Assert.AreEqual(MessageStatusTypes.Ready, productMessage.MessageStatusId);
            }

            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
            mockEsbMessage.Verify(m => m.Acknowledge(), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_MessageIsThresholdExceededAndNumberOfResendsIsEqualOrMoreThanMaxNumberOfResends_ShouldAddMessageResponseAndNotReprocessMessageQueueEntries()
        {
            //Given
            XElement message = XElement.Load(@"TestMessages/fail_threshold_exceeded.xml");
            List<MessageQueueProduct> productMessages = CreateTestThresholdExceededMessageQueueProducts(message);

            foreach (var productMessage in productMessages)
            {
                messageHistory.MessageQueueProduct.Add(productMessage);
            }
            globalContext.Context.MessageHistory.Add(messageHistory);
            globalContext.SaveChanges();

            mockEsbMessage.SetupGet(m => m.MessageText).Returns(message.ToString());
            mockEsbMessage.Setup(m => m.GetProperty(It.Is<string>(s => s == "TransactionID")))
                .Returns(messageHistory.MessageHistoryId.ToString());
            mockCache.Setup(m => m.Get(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int messageTypeId, int messageQueueId) => new MessageQueueResendStatus { MessageQueueId = messageQueueId, NumberOfResends = 1 });

            //When
            r10Listener.HandleMessage(null, new EsbMessageEventArgs { Message = mockEsbMessage.Object });

            //Then
            Assert.AreEqual(MessageStatusTypes.Failed, messageHistory.MessageStatusId);
            Assert.IsFalse(messageHistory.MessageResendStatus.Any());

            var r10MessageResponse = messageHistory.R10MessageResponse.Single();
            Assert.AreEqual(messageHistory.MessageHistoryId, r10MessageResponse.MessageHistoryId);
            Assert.AreEqual(false, r10MessageResponse.RequestSuccess);
            Assert.AreEqual(false, r10MessageResponse.SystemError);
            Assert.AreEqual(message.ToString(), r10MessageResponse.ResponseText);
            Assert.AreEqual("ProductIdentifierNotUnique, ThresholdExceededError", r10MessageResponse.FailureReasonCode);

            Assert.AreEqual(21, messageHistory.MessageQueueProduct.Count);
            Assert.AreEqual(MessageStatusTypes.Associated, messageHistory.MessageQueueProduct.First().MessageStatusId);
            foreach (var productMessage in messageHistory.MessageQueueProduct.Skip(1))
            {
                Assert.AreEqual(MessageStatusTypes.Associated, productMessage.MessageStatusId);
            }

            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
            mockEsbMessage.Verify(m => m.Acknowledge(), Times.Once);
        }

        private List<MessageQueueProduct> CreateTestThresholdExceededMessageQueueProducts(XElement message)
        {
            var xmlAscii = XElement.Parse(message.Descendants(XName.Get("xmlAscii", XmlNamespaceConstants.R10CommonReferenceTypesNamespace)).First().Value);

            var itemIds = xmlAscii.Descendants(XName.Get("BusinessError", XmlNamespaceConstants.R10ServicesNamespace))
                        .Select(be => Convert.ToInt32(be.Descendants(XName.Get("MainId", XmlNamespaceConstants.R10ServicesNamespace)).First().Value));

            return itemIds.Select(id => new TestProductMessageBuilder()
                                                .WithItemId(id)
                                                .WithStatusId(MessageStatusTypes.Associated)
                                                .Build())
                            .ToList();
        }
    }
}
