using Amazon.S3.Model;
using Amazon.SQS.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.IO;
using Wfm.Aws.ExtendedClient.Model;
using Wfm.Aws.ExtendedClient.Serializer;
using Wfm.Aws.ExtendedClient.SQS;
using Wfm.Aws.ExtendedClient.SQS.Model;
using Wfm.Aws.S3;
using Wfm.Aws.SQS;

namespace Wfm.Aws.Tests.ExtendedClient.SQS
{
    [TestClass]
    public class SQSExtendedClientTests
    {
        private Mock<IS3Facade> s3FacadeMock;
        private Mock<ISQSFacade> sqsFacadeMock;
        private Mock<IExtendedClientMessageSerializer> extendedClientMessageSerializerMock;

        [TestInitialize]
        public void Initialize()
        {
            s3FacadeMock = new Mock<IS3Facade>();
            sqsFacadeMock = new Mock<ISQSFacade>();
            extendedClientMessageSerializerMock = new Mock<IExtendedClientMessageSerializer>();
        }

        [TestMethod]
        public void DeleteMessage_ValidTest()
        {
            // Given
            SQSExtendedClient sqsExtendedClient = new SQSExtendedClient(sqsFacadeMock.Object, s3FacadeMock.Object, extendedClientMessageSerializerMock.Object);
            string queueURL = "sqs.url";
            string receiptHandle = "12345";

            // When
            sqsExtendedClient.DeleteMessage(queueURL, receiptHandle);

            // Then
            sqsFacadeMock.Verify(sqs => sqs.DeleteMessage(queueURL, receiptHandle), Times.Once);
        }

        [TestMethod]
        public void ReceiveMessage_ValidTest()
        {
            // Given
            SQSExtendedClient sqsExtendedClient = new SQSExtendedClient(sqsFacadeMock.Object, s3FacadeMock.Object, extendedClientMessageSerializerMock.Object);
            string queueURL = "sqs.url";
            string sqsBody = "Dummy body";
            string s3Bucket = "sampleS3Bucket";
            string s3Key = "sampleS3Key";
            string s3Data = "sampleData";
            IDictionary<string, string> messageAttributes = new Dictionary<string, string>()
            {
                { "key1", "value1" },
                { "key2", "value2" }
            };
            Message sqsMessage = new Message()
            {
                Body = sqsBody
            };
            ReceiveMessageResponse receiveMessageResponse = new ReceiveMessageResponse()
            {
                Messages = new List<Message>() { sqsMessage, sqsMessage }
            };
            ExtendedClientMessageModel extendedClientMessageModel = new ExtendedClientMessageModel()
            {
                S3Details = new List<ExtendedClientMessageModelS3Detail>()
                {
                    new ExtendedClientMessageModelS3Detail()
                    {
                        S3BucketName = s3Bucket,
                        S3Key = s3Key
                    },
                    new ExtendedClientMessageModelS3Detail()
                    {
                        S3BucketName = s3Bucket,
                        S3Key = s3Key
                    }
                },
                MessageAttributes = messageAttributes
            };
            sqsFacadeMock
                .Setup(s => s.ReceiveMessage(queueURL, 2, 0))
                .Returns(receiveMessageResponse);
            extendedClientMessageSerializerMock
                .Setup(s => s.Deserialize(sqsBody))
                .Returns(extendedClientMessageModel);
            s3FacadeMock
                .SetupSequence(s => s.GetObject(s3Bucket, s3Key))
                .Returns(GenerateGetObjectResponse(s3Data))
                .Returns(GenerateGetObjectResponse(s3Data))
                .Returns(GenerateGetObjectResponse(s3Data))
                .Returns(GenerateGetObjectResponse(s3Data));

            // When
            IList<SQSExtendedClientReceiveModel> sqsExtendedClientReceives = sqsExtendedClient.ReceiveMessage(queueURL, 2, 0);

            // Then
            extendedClientMessageSerializerMock.Verify(serializer => serializer.Deserialize(It.IsAny<string>()), Times.Exactly(2));
            s3FacadeMock.Verify(s3 => s3.GetObject(s3Bucket, s3Key), Times.Exactly(4));
            sqsFacadeMock.Verify(sqs => sqs.ReceiveMessage(queueURL, 2, 0), Times.Once);
            Assert.AreEqual(2, sqsExtendedClientReceives.Count);
        }

        [TestMethod]
        public void SendMessage_ValidTest()
        {
            // Given
            SQSExtendedClient sqsExtendedClient = new SQSExtendedClient(sqsFacadeMock.Object, s3FacadeMock.Object, extendedClientMessageSerializerMock.Object);
            string queueURL = "sqs.url";
            string s3Bucket = "sampleS3Bucket";
            string s3Key = "sampleS3Key";
            string data = "sampleData";
            IDictionary<string, string> messageAttributes = new Dictionary<string, string>()
            {
                { "key1", "value1" },
                { "key2", "value2" }
            };

            // When
            sqsExtendedClient.SendMessage(queueURL, s3Bucket, s3Key, data, messageAttributes);

            // Then
            extendedClientMessageSerializerMock.Verify(serializer => serializer.Serialize(s3Bucket, s3Key, messageAttributes), Times.Once);
            s3FacadeMock.Verify(s3 => s3.PutObject(s3Bucket, s3Key, data, It.IsAny<IDictionary<string, string>>()), Times.Once);
            sqsFacadeMock.Verify(sqs => sqs.SendMessage(queueURL, It.IsAny<string>(), messageAttributes), Times.Once);
        }

        private static GetObjectResponse GenerateGetObjectResponse(string s3Data)
        {
            return new GetObjectResponse()
            {
                ResponseStream = GenerateStream(s3Data)
            };
        }

        private static Stream GenerateStream(string s3Data)
        {
            Stream stream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(stream);
            streamWriter.Write(s3Data);
            streamWriter.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
