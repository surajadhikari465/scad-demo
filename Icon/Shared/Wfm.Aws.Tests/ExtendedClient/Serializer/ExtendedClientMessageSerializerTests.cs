using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Wfm.Aws.ExtendedClient.Model;
using Wfm.Aws.ExtendedClient.Serializer;

namespace Wfm.Aws.Tests.ExtendedClient.Serializer
{
    [TestClass]
    public class ExtendedClientMessageSerializerTests
    {
        private static readonly string S3_BUCKET_NAME = "gpm-nearrealtime-bucket-486771727843";
        private static readonly string S3_KEY = "ID_169.254.10.29-46435-1671691029390-47_1_1_1_3927";
        private static readonly string TRANSACTION_TYPE_ATTRIBUTE = "TransactionType";
        private static readonly string RESET_FLAG_ATTRIBUTE = "ResetFlag";
        private static readonly string TO_BE_RECEIVED_BY_ATTRIBUTE = "toBeReceivedBy";
        private static readonly string SEQUENCE_NUMBER_ATTRIBUTE = "SequenceNumber";
        private static readonly string CORRELATION_ID_ATTRIBUTE = "CorrelationID";
        private static readonly string SOURCE_ATTRIBUTE = "Source";
        private static readonly string TRANSACTION_ID_ATTRIBUTE = "TransactionID";
        private static readonly string SEQUENCE_ID_ATTRIBUTE = "SequenceID";
        private static readonly string TRANSACTION_TYPE_ATTRIBUTE_VALUE = "Price";
        private static readonly string RESET_FLAG_ATTRIBUTE_VALUE = "false";
        private static readonly string TO_BE_RECEIVED_BY_ATTRIBUTE_VALUE = "[\"ALL\"]";
        private static readonly string TO_BE_RECEIVED_BY_ATTRIBUTE_JSON_VALUE = "[\\\"ALL\\\"]";
        private static readonly string SEQUENCE_NUMBER_ATTRIBUTE_VALUE = "1671691200030";
        private static readonly string CORRELATION_ID_ATTRIBUTE_VALUE = "4224935-10251";
        private static readonly string SOURCE_ATTRIBUTE_VALUE = "Infor";
        private static readonly string TRANSACTION_ID_ATTRIBUTE_VALUE = "1667952013359";
        private static readonly string SEQUENCE_ID_ATTRIBUTE_VALUE = "4";
        private static readonly string EXTENDED_CLIENT_MESSAGE = $@"[""software.amazon.payloadoffloading.PayloadS3Pointer"",{{""s3BucketName"":""{S3_BUCKET_NAME}"",""s3Key"":""{S3_KEY}""}}]";
        private static readonly string EXTENDED_CLIENT_PAYLOAD = $@"{{
    ""Type"": ""Notification"",
    ""MessageId"": ""da63f9ac-5d56-5e12-af9d-a5b1a356f178"",
    ""TopicArn"": ""arn:aws:sns:us-west-2:486771727843:GPM-NearRealTime-Topic"",
    ""Message"": ""[\""software.amazon.payloadoffloading.PayloadS3Pointer\"",{{\""s3BucketName\"":\""{S3_BUCKET_NAME}\"",\""s3Key\"":\""{S3_KEY}\""}}]"",
    ""Timestamp"": ""2022-12-22T06:40:00.121Z"",
    ""SignatureVersion"": ""1"",
    ""Signature"": ""1eq9mkLa5iFH6MM0FqH9sxAbTbQZP2KM6SjTz1wRm/PjZrZmcJdSBz+LeM2zw1wAn1We/LAp1ZGZMVdU5MTTvjkAMivDX9XYFXWQ3bZjkwgPb+VeeqwlWB0mANhc3/6AF/Hnv6/9wPXKyBDJyfcDJIVmRPTDK55rvZ7mKm3XcVY0LKEUm6CmEcPbOOJv1tYluZYgCgtrePMKY1NrfeK9BUhjnvIxsRtEHDJpIj9icMiY1TYfGyvav8khlzwIGwnSS+ZmKwAovBXBnu4AQzVeIcNkXhyZu6lpARS4or63rClj398g5jEgB3hdx04qNSG+BVW2uFMsfB+1IMICmE/hOg=="",
    ""SigningCertURL"": ""https://sns.us-west-2.amazonaws.com/SimpleNotificationService-56e67fcb41f6fec09b0196692625d385.pem"",
    ""UnsubscribeURL"": ""https://sns.us-west-2.amazonaws.com/?Action=Unsubscribe&SubscriptionArn=arn:aws:sns:us-west-2:486771727843:GPM-NearRealTime-Topic:224d3edd-b875-4ffe-9e86-305a7a3c5765"",
    ""MessageAttributes"": {{
        ""{TRANSACTION_TYPE_ATTRIBUTE}"": {{
            ""Type"": ""String"",
            ""Value"": ""{TRANSACTION_TYPE_ATTRIBUTE_VALUE}""
        }},
        ""{RESET_FLAG_ATTRIBUTE}"": {{
            ""Type"": ""String"",
            ""Value"": ""{RESET_FLAG_ATTRIBUTE_VALUE}""
        }},
        ""{TO_BE_RECEIVED_BY_ATTRIBUTE}"": {{
            ""Type"": ""String.Array"",
            ""Value"": ""{TO_BE_RECEIVED_BY_ATTRIBUTE_JSON_VALUE}""
        }},
        ""{SEQUENCE_NUMBER_ATTRIBUTE}"": {{
            ""Type"": ""String"",
            ""Value"": ""{SEQUENCE_NUMBER_ATTRIBUTE_VALUE}""
        }},
        ""{CORRELATION_ID_ATTRIBUTE}"": {{
            ""Type"": ""String"",
            ""Value"": ""{CORRELATION_ID_ATTRIBUTE_VALUE}""
        }},
        ""{SOURCE_ATTRIBUTE}"": {{
            ""Type"": ""String"",
            ""Value"": ""{SOURCE_ATTRIBUTE_VALUE}""
        }},
        ""{TRANSACTION_ID_ATTRIBUTE}"": {{
            ""Type"": ""String"",
            ""Value"": ""{TRANSACTION_ID_ATTRIBUTE_VALUE}""
        }},
        ""{SEQUENCE_ID_ATTRIBUTE}"": {{
            ""Type"": ""String"",
            ""Value"": ""{SEQUENCE_ID_ATTRIBUTE_VALUE}""
        }}
    }}
}}";

        [TestMethod]
        public void Serialize_ValidTest()
        {
            // Given
            ExtendedClientMessageSerializer extendedClientMessageSerializer = new ExtendedClientMessageSerializer();

            // When
            string generatedMessage = extendedClientMessageSerializer.Serialize(S3_BUCKET_NAME, S3_KEY, new Dictionary<string, string>());

            // Then
            Assert.AreEqual(EXTENDED_CLIENT_MESSAGE, generatedMessage);
        }

        [TestMethod]
        public void Deserialize_ValidTest()
        {
            // Given
            ExtendedClientMessageSerializer extendedClientMessageSerializer = new ExtendedClientMessageSerializer();

            // When
            ExtendedClientMessageModel extendedClientMessageModel = extendedClientMessageSerializer.Deserialize(EXTENDED_CLIENT_PAYLOAD);

            // Then
            Assert.IsNotNull(extendedClientMessageModel);
            Assert.IsNotNull(extendedClientMessageModel.S3Details);
            Assert.IsNotNull(extendedClientMessageModel.MessageAttributes);
            Assert.AreEqual(Constants.EventSources.SNS, extendedClientMessageModel.EventSource);
            Assert.AreEqual(1, extendedClientMessageModel.S3Details.Count);
            Assert.AreEqual(8, extendedClientMessageModel.MessageAttributes.Count);
            Assert.AreEqual(S3_BUCKET_NAME, extendedClientMessageModel.S3Details[0].S3BucketName);
            Assert.AreEqual(S3_KEY, extendedClientMessageModel.S3Details[0].S3Key);
            Assert.AreEqual(extendedClientMessageModel.MessageAttributes[TRANSACTION_TYPE_ATTRIBUTE], TRANSACTION_TYPE_ATTRIBUTE_VALUE);
            Assert.AreEqual(extendedClientMessageModel.MessageAttributes[RESET_FLAG_ATTRIBUTE], RESET_FLAG_ATTRIBUTE_VALUE);
            Assert.AreEqual(extendedClientMessageModel.MessageAttributes[TO_BE_RECEIVED_BY_ATTRIBUTE], TO_BE_RECEIVED_BY_ATTRIBUTE_VALUE);
            Assert.AreEqual(extendedClientMessageModel.MessageAttributes[SEQUENCE_NUMBER_ATTRIBUTE], SEQUENCE_NUMBER_ATTRIBUTE_VALUE);
            Assert.AreEqual(extendedClientMessageModel.MessageAttributes[CORRELATION_ID_ATTRIBUTE], CORRELATION_ID_ATTRIBUTE_VALUE);
            Assert.AreEqual(extendedClientMessageModel.MessageAttributes[SOURCE_ATTRIBUTE], SOURCE_ATTRIBUTE_VALUE);
            Assert.AreEqual(extendedClientMessageModel.MessageAttributes[TRANSACTION_ID_ATTRIBUTE], TRANSACTION_ID_ATTRIBUTE_VALUE);
            Assert.AreEqual(extendedClientMessageModel.MessageAttributes[SEQUENCE_ID_ATTRIBUTE], SEQUENCE_ID_ATTRIBUTE_VALUE);
        }

        [TestMethod]
        public void Deserialize_SQS_ValidTest()
        {
            // Given
            ExtendedClientMessageSerializer extendedClientMessageSerializer = new ExtendedClientMessageSerializer();

            // When
            ExtendedClientMessageModel extendedClientMessageModel = extendedClientMessageSerializer.Deserialize(EXTENDED_CLIENT_MESSAGE);

            // Then
            Assert.IsNotNull(extendedClientMessageModel);
            Assert.IsNotNull(extendedClientMessageModel.S3Details);
            Assert.IsNotNull(extendedClientMessageModel.MessageAttributes);
            Assert.AreEqual(Constants.EventSources.SQS, extendedClientMessageModel.EventSource);
            Assert.AreEqual(1, extendedClientMessageModel.S3Details.Count);
            Assert.AreEqual(0, extendedClientMessageModel.MessageAttributes.Count);
            Assert.AreEqual(S3_BUCKET_NAME, extendedClientMessageModel.S3Details[0].S3BucketName);
            Assert.AreEqual(S3_KEY, extendedClientMessageModel.S3Details[0].S3Key);
        }
    }
}
