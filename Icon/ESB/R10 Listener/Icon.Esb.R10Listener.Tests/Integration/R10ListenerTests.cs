using Dapper;
using Icon.Common.Email;
using Icon.Esb.R10Listener.Commands;
using Icon.Esb.R10Listener.Infrastructure.DataAccess;
using Icon.Esb.R10Listener.MessageParsers;
using Icon.Esb.R10Listener.Models;
using Icon.Esb.Subscriber;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Configuration;
using System.Data.SqlClient;
using System.Transactions;
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
        private TransactionScope transaction;
        private string testMessageId = "TestMessageId";
        private SqlConnection sqlConnection;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();

            mockEmailClient = new Mock<IEmailClient>();
            mockLogger = new Mock<ILogger<R10Listener>>();
            mockEsbSubscriber = new Mock<IEsbSubscriber>();

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
                new SaveR10MessageResponseCommandHandler(new DbFactory()),
                new R10MessageResponseParser(),
                mockEmailClient.Object,
                mockLogger.Object);

            mockEsbMessage = new Mock<IEsbMessage>();

            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void HandleMessage_ValidMessageResponse_ShouldAddMessageResponse()
        {
            //Given
            XElement message = XElement.Load(@"TestMessages/valid_message_response.xml");
            mockEsbMessage.SetupGet(m => m.MessageText).Returns(message.ToString());
            mockEsbMessage.Setup(m => m.GetProperty(It.Is<string>(s => s == "TransactionID")))
                .Returns(testMessageId);

            //When
            r10Listener.HandleMessage(null, new EsbMessageEventArgs { Message = mockEsbMessage.Object });

            //Then
            var r10MessageResponse = sqlConnection.QuerySingle<R10MessageResponseModel>(
                "SELECT * FROM app.MessageResponseR10 WHERE MessageId = @MessageId", 
                new { MessageId = testMessageId });
            Assert.AreEqual(true, r10MessageResponse.RequestSuccess);
            Assert.AreEqual(false, r10MessageResponse.SystemError);
            Assert.AreEqual(message.ToString(SaveOptions.DisableFormatting), r10MessageResponse.ResponseText);
            Assert.AreEqual(null, r10MessageResponse.FailureReasonCode);

            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Never);
            mockEsbMessage.Verify(m => m.Acknowledge(), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_MessageHasRequestSuccessSetToFalse_ShouldAddMessageResponse()
        {
            //Given
            XDocument message = XDocument.Load(@"TestMessages/fail_bottle_deposit_maintenance_message.xml");
            mockEsbMessage.SetupGet(m => m.MessageText).Returns(message.ToString());
            mockEsbMessage.Setup(m => m.GetProperty(It.Is<string>(s => s == "TransactionID")))
                .Returns(testMessageId);

            //When
            r10Listener.HandleMessage(null, new EsbMessageEventArgs { Message = mockEsbMessage.Object });

            //Then
            var r10MessageResponse = sqlConnection.QuerySingle<R10MessageResponseModel>(
                "SELECT * FROM app.MessageResponseR10 WHERE MessageId = @MessageId", 
                new { MessageId = testMessageId });
            Assert.AreEqual(false, r10MessageResponse.RequestSuccess);
            Assert.AreEqual(false, r10MessageResponse.SystemError);
            Assert.AreEqual(message.ToString(SaveOptions.DisableFormatting), r10MessageResponse.ResponseText);
            Assert.AreEqual("InvalidRequest", r10MessageResponse.FailureReasonCode);

            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
            mockEsbMessage.Verify(m => m.Acknowledge(), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_MessageIsSystemError_ShouldAddMessageResponse()
        {
            //Given
            XDocument message = XDocument.Load(@"TestMessages/fail_system_error.xml");
            mockEsbMessage.SetupGet(m => m.MessageText).Returns(message.ToString());
            mockEsbMessage.Setup(m => m.GetProperty(It.Is<string>(s => s == "TransactionID")))
                .Returns(testMessageId);

            //When
            r10Listener.HandleMessage(null, new EsbMessageEventArgs { Message = mockEsbMessage.Object });

            //Then
            var r10MessageResponse = sqlConnection.QuerySingle<R10MessageResponseModel>(
                "SELECT * FROM app.MessageResponseR10 WHERE MessageId = @MessageId", 
                new { MessageId = testMessageId });
            Assert.AreEqual(false, r10MessageResponse.RequestSuccess);
            Assert.AreEqual(true, r10MessageResponse.SystemError);
            Assert.AreEqual(message.ToString(SaveOptions.DisableFormatting), r10MessageResponse.ResponseText);
            Assert.AreEqual("TEST_REASON_CODE", r10MessageResponse.FailureReasonCode);

            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
            mockEsbMessage.Verify(m => m.Acknowledge(), Times.Once);
        }
    }
}
