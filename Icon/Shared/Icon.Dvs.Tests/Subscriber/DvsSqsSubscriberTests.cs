using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using Icon.Dvs.Model;
using Icon.Dvs.Subscriber;
using Moq;

namespace Icon.Dvs.Tests.Subscriber
{
    [TestClass]
    public class DvsSqsSubscriberTests
    {
        private Mock<IAmazonS3> mockS3Client;
        private Mock<IAmazonSQS> mockSqsClient;
        private DvsSqsSubscriber subscriber;

        [TestInitialize]
        public void initialize()
        {
            mockS3Client = new Mock<IAmazonS3>();
            mockSqsClient = new Mock<IAmazonSQS>();
            subscriber = new DvsSqsSubscriber(
                mockS3Client.Object,
                mockSqsClient.Object,
                DvsListenerSettings.CreateSettingsFromConfig()
            );
        }

        [TestMethod]
        public void ReceiveDvsSqsMessages_ValidCaseTest()
        {
            // Given
            var sqsMessage = new Message()
            {
                Body = TestResources.DvsSqsMessageBody
            };
            var receiveMessageResponse = new ReceiveMessageResponse()
            {
                Messages = new List<Message>() { sqsMessage, sqsMessage }
            };

            mockSqsClient.Setup(s => s.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), CancellationToken.None))
                .Returns(Task.FromResult<ReceiveMessageResponse>(receiveMessageResponse));

            // When
            IList<DvsSqsMessage> dvsSqsMessages = subscriber.ReceiveDvsSqsMessages(2).Result;

            // Then
            mockSqsClient.Verify(s => s.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), CancellationToken.None), Times.Once);
            Assert.AreEqual(2, dvsSqsMessages.Count);
            DvsSqsMessage dvsSqsMessage = dvsSqsMessages[0];
            Assert.AreEqual("messageId", dvsSqsMessage.MessageId);
            Assert.AreEqual("s3Bucket", dvsSqsMessage.S3BucketName);
            Assert.AreEqual("s3Key", dvsSqsMessage.S3Key);
            Assert.AreEqual("TestTransaction", dvsSqsMessage.MessageAttributes["TransactionType"]);
            Assert.AreEqual("[\"ALL\"]", dvsSqsMessage.MessageAttributes["toBeReceivedBy"]);
            Assert.AreEqual("1668666982554", dvsSqsMessage.MessageAttributes["SequenceNumber"]);
            Assert.AreEqual("ICON", dvsSqsMessage.MessageAttributes["Source"]);
            Assert.AreEqual("12", dvsSqsMessage.MessageAttributes["MessageID"]);
            Assert.AreEqual("SomeTransactionID", dvsSqsMessage.MessageAttributes["TransactionID"]);
        }

        [TestMethod]
        public void ReceiveDvsSqsMessages_InValidCaseTest()
        {
            // Given
            var sqsMessage = new Message()
            {
                Body = "InvalidMessageContent"
            };
            var receiveMessageResponse = new ReceiveMessageResponse()
            {
                Messages = new List<Message>() { sqsMessage, sqsMessage }
            };

            mockSqsClient.Setup(s => s.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), CancellationToken.None))
                .Returns(Task.FromResult<ReceiveMessageResponse>(receiveMessageResponse));

            // When
            try
            {
                var dvsSqsMessages = subscriber.ReceiveDvsSqsMessages(2).Result;
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }

            // Then
            mockSqsClient.Verify(s => s.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), CancellationToken.None), Times.Once);
        }

        [TestMethod]
        public void ReceieveDvsMessage_ValidCaseTest()
        {
            // Given
            var sqsMessage = new Message()
            {
                Body = TestResources.DvsSqsMessageBody
            };
            var receiveMessageResponse = new ReceiveMessageResponse()
            {
                Messages = new List<Message>() { sqsMessage, sqsMessage }
            };

            var stream = new MemoryStream();
            var streamWriter = new StreamWriter(stream);
            streamWriter.Write("TestS3Content");
            streamWriter.Flush();
            stream.Position = 0;

            var getObjectResponse = new GetObjectResponse()
            {
                ResponseStream = stream
            };

            mockSqsClient.Setup(s => s.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), CancellationToken.None))
                .Returns(Task.FromResult<ReceiveMessageResponse>(receiveMessageResponse));
            mockS3Client.Setup(s => s.GetObjectAsync(It.IsAny<GetObjectRequest>(), CancellationToken.None))
                .Returns(Task.FromResult<GetObjectResponse>(getObjectResponse));

            // When
            var dvsMessage = subscriber.ReceiveDvsMessage().Result;

            // Then
            mockSqsClient.Verify(s => s.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), CancellationToken.None), Times.Once);
            mockS3Client.Verify(s => s.GetObjectAsync(It.IsAny<GetObjectRequest>(), CancellationToken.None), Times.Once);
            Assert.AreEqual("TestS3Content", dvsMessage.MessageContent);
            Assert.AreEqual("messageId", dvsMessage.SqsMessage.MessageId);
            Assert.AreEqual("s3Bucket", dvsMessage.SqsMessage.S3BucketName);
            Assert.AreEqual("s3Key", dvsMessage.SqsMessage.S3Key);
            Assert.AreEqual("TestTransaction", dvsMessage.SqsMessage.MessageAttributes["TransactionType"]);
            Assert.AreEqual("[\"ALL\"]", dvsMessage.SqsMessage.MessageAttributes["toBeReceivedBy"]);
            Assert.AreEqual("1668666982554", dvsMessage.SqsMessage.MessageAttributes["SequenceNumber"]);
            Assert.AreEqual("ICON", dvsMessage.SqsMessage.MessageAttributes["Source"]);
            Assert.AreEqual("12", dvsMessage.SqsMessage.MessageAttributes["MessageID"]);
            Assert.AreEqual("SomeTransactionID", dvsMessage.SqsMessage.MessageAttributes["TransactionID"]);
        }

        [TestMethod]
        public void ReceiveDvsMessage_InValidCaseTest()
        {
            // Given
            var receiveMessageResponse = new ReceiveMessageResponse()
            {
                Messages = new List<Message>() { }
            };

            mockSqsClient.Setup(s => s.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), CancellationToken.None))
                .Returns(Task.FromResult<ReceiveMessageResponse>(receiveMessageResponse));

            // When
            var dvsMessage = subscriber.ReceiveDvsMessage().Result;

            // Then
            Assert.IsNull(dvsMessage);
        }

        [TestMethod]
        public void DeleteSqsMessage_ValidCaseTest()
        {
            // Given
            mockSqsClient.Setup(s => s.DeleteMessageAsync(It.IsAny<DeleteMessageRequest>(), CancellationToken.None))
                .Returns(Task.FromResult<DeleteMessageResponse>(new DeleteMessageResponse()));

            // When
            var deleteMessageResponse = subscriber.DeleteSqsMessage("receiptHandle").Result;

            // Then
            Assert.IsNotNull(deleteMessageResponse);
        }

        [TestMethod]
        public void DeleteSqsMessage_InValidCaseTest()
        {
            // Given
            mockSqsClient.Setup(s => s.DeleteMessageAsync(It.IsAny<DeleteMessageRequest>(), CancellationToken.None))
                .Throws(new Exception("Test Error"));

            // When
            try
            {
                var deleteMessageResponse = subscriber.DeleteSqsMessage("receiptHandle").Result;
            }
            catch (Exception ex)
            {
                // Then
                Assert.IsNotNull(ex);
            }
        }
    }
}
