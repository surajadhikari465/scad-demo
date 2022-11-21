using System;
using System.Collections.Generic;
using Icon.Dvs.Model;

namespace Icon.Dvs.Tests
{
    public class TestResources
    {
        public const string DvsSqsMessageBody = @"
            {
              ""Type"" : ""Notification"",
              ""MessageId"" : ""messageId"",
              ""TopicArn"" : ""arn:aws:sns:us-west-2:1234567890:TestTopic"",
              ""Message"" : ""[\""software.amazon.payloadoffloading.PayloadS3Pointer\"",{\""s3BucketName\"":\""s3Bucket\"",\""s3Key\"":\""s3Key\""}]"",
              ""Timestamp"" : ""2022-11-17T06:36:22.651Z"",
              ""SignatureVersion"" : ""1"",
             ""Signature"": ""RxRLeTDr7LLZIZb4HqTp6kdQtArKdTcDNcU5Uz/Z7IbEmHAMVIdz9Op7aAic8EVv1qF3c9Ny+dBTHLzQ27ihDW5oM1UZHM7oF6tmW4nElhsMXVTi046JIoQnwj41cngPBNYznVU6ZwUzaLhwYomrESd0zOcyTjwIgPS8RXteYSLEMToANQ5swST/u8uuoKTHILqWLj4ASQRpVt+Rl6WWPohYVU9DNWjKHcCOJMenUPuhhhqa8K6MqgwFL72vfwaiJGmLitcpPrF9DPbmSj0thvP/WDXaIZypRIuziclFd+KHJiIZjVbTT5i3Ut9jxdsy1Pjs2SXhBgH7vrZjNN+UiQ=="",
              ""SigningCertURL"" : ""https://sns.us-west-2.amazonaws.com/SimpleNotificationService-56e67fcb41f6fec09b0196692625d385.pem"",
              ""UnsubscribeURL"" : ""https://sns.us-west-2.amazonaws.com/?Action=Unsubscribe&SubscriptionArn=arn:aws:sns:us-west-2:228490534830:AttributeTopic:6a8a7e66-d46b-47a1-9f3d-ecf4223d9f02"",
              ""MessageAttributes"" : {
                ""TransactionType"" : {""Type"":""String"",""Value"":""TestTransaction""},
                ""toBeReceivedBy"" : {""Type"":""String.Array"",""Value"":""[\""ALL\""]""},
                ""SequenceNumber"" : {""Type"":""String"",""Value"":""1668666982554""},
                ""Source"" : {""Type"":""String"",""Value"":""ICON""},
                ""MessageID"" : {""Type"":""String"",""Value"":""12""},
                ""TransactionID"" : {""Type"":""String"",""Value"":""SomeTransactionID""}
              }
            }";

        public static DvsMessage GetDvsMessage() 
        {
            return new DvsMessage(new DvsSqsMessage() { MessageId = "1" }, "MessageContent");
        }
    }
}
