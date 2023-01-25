using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using Wfm.Aws.ExtendedClient.Serializer;
using Wfm.Aws.ExtendedClient.SNS;
using Wfm.Aws.S3;
using Wfm.Aws.SNS;

namespace Wfm.Aws.Tests.ExtendedClient.SNS
{
    [TestClass]
    public class SNSExtendedClientTests
    {
        private Mock<IS3Facade> s3FacadeMock;
        private Mock<ISNSFacade> snsFacadeMock;
        private Mock<IExtendedClientMessageSerializer> extendedClientMessageSerializerMock;

        [TestInitialize]
        public void Initialize()
        {
            s3FacadeMock = new Mock<IS3Facade>();
            snsFacadeMock = new Mock<ISNSFacade>();
            extendedClientMessageSerializerMock = new Mock<IExtendedClientMessageSerializer>();
        }

        [TestMethod]
        public void Publish_ValidTest()
        {
            // Given
            SNSExtendedClient snsExtendedClient = new SNSExtendedClient(snsFacadeMock.Object, s3FacadeMock.Object, extendedClientMessageSerializerMock.Object);
            string snsARN = "sns::arn";
            string s3Bucket = "sampleS3Bucket";
            string s3Key = "sampleS3Key";
            string data = "sampleData";
            IDictionary<string, string> messageAttributes = new Dictionary<string, string>()
            {
                { "key1", "value1" },
                { "key2", "value2" }
            };

            // When
            snsExtendedClient.Publish(snsARN, s3Bucket, s3Key, data, messageAttributes);

            // Then
            extendedClientMessageSerializerMock.Verify(serializer => serializer.Serialize(s3Bucket, s3Key, messageAttributes), Times.Once);
            s3FacadeMock.Verify(s3  => s3.PutObject(s3Bucket, s3Key, data, It.IsAny<IDictionary<string, string>>()), Times.Once);
            snsFacadeMock.Verify(sns => sns.Publish(snsARN, It.IsAny<string>(), messageAttributes), Times.Once);
        }
    }
}
