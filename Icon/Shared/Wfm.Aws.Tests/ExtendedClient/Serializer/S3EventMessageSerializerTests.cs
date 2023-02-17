using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Wfm.Aws.ExtendedClient.Model;
using Wfm.Aws.ExtendedClient.Serializer;

namespace Wfm.Aws.Tests.ExtendedClient.Serializer
{
    [TestClass]
    public class S3EventMessageSerializerTests
    {
        private readonly string s3EventWithSingleRecord = @"
{  
   ""Records"":[  
      {  
         ""eventVersion"":""2.1"",
         ""eventSource"":""aws:s3"",
         ""awsRegion"":""us-west-2"",
         ""eventTime"":""1970-01-01T00:00:00.000Z"",
         ""eventName"":""ObjectCreated:Put"",
         ""userIdentity"":{  
            ""principalId"":""AIDAJDPLRKLG7UEXAMPLE""
         },
         ""requestParameters"":{  
            ""sourceIPAddress"":""127.0.0.1""
         },
         ""responseElements"":{  
            ""x-amz-request-id"":""C3D13FE58DE4C810"",
            ""x-amz-id-2"":""FMyUVURIY8/IgAtTv8xRjskZQpcIZ9KG4V5Wp6S7S/JRWeUWerMUE5JgHvANOjpD""
         },
         ""s3"":{  
            ""s3SchemaVersion"":""1.0"",
            ""configurationId"":""testConfigRule"",
            ""bucket"":{  
               ""name"":""mybucket"",
               ""ownerIdentity"":{  
                  ""principalId"":""A3NL1KOZZKExample""
               },
               ""arn"":""arn:aws:s3:::mybucket""
            },
            ""object"":{  
               ""key"":""canonical/year%3D2022/month%3D11/day%3D15/HappyFace.jpg"",
               ""size"":1024,
               ""eTag"":""d41d8cd98f00b204e9800998ecf8427e"",
               ""versionId"":""096fKKXTRTtl3on89fVO.nfljtsv6qko"",
               ""sequencer"":""0055AED6DCD90281E5""
            }
         }
      }
   ]
}
";
        private readonly string s3EventWithMultipleRecords = @"
{  
   ""Records"":[  
      {  
         ""eventVersion"":""2.1"",
         ""eventSource"":""aws:s3"",
         ""awsRegion"":""us-west-2"",
         ""eventTime"":""1970-01-01T00:00:00.000Z"",
         ""eventName"":""ObjectCreated:Put"",
         ""userIdentity"":{  
            ""principalId"":""AIDAJDPLRKLG7UEXAMPLE""
         },
         ""requestParameters"":{  
            ""sourceIPAddress"":""127.0.0.1""
         },
         ""responseElements"":{  
            ""x-amz-request-id"":""C3D13FE58DE4C810"",
            ""x-amz-id-2"":""FMyUVURIY8/IgAtTv8xRjskZQpcIZ9KG4V5Wp6S7S/JRWeUWerMUE5JgHvANOjpD""
         },
         ""s3"":{  
            ""s3SchemaVersion"":""1.0"",
            ""configurationId"":""testConfigRule"",
            ""bucket"":{  
               ""name"":""mybucket1"",
               ""ownerIdentity"":{  
                  ""principalId"":""A3NL1KOZZKExample""
               },
               ""arn"":""arn:aws:s3:::mybucket1""
            },
            ""object"":{  
               ""key"":""canonical/year%3D2022/month%3D11/day%3D15/HappyFace1.jpg"",
               ""size"":1024,
               ""eTag"":""d41d8cd98f00b204e9800998ecf8427e"",
               ""versionId"":""096fKKXTRTtl3on89fVO.nfljtsv6qko"",
               ""sequencer"":""0055AED6DCD90281E5""
            }
         }
      },
      {  
         ""eventVersion"":""2.1"",
         ""eventSource"":""aws:s3"",
         ""awsRegion"":""us-west-2"",
         ""eventTime"":""1970-01-01T00:00:00.000Z"",
         ""eventName"":""ObjectCreated:Put"",
         ""userIdentity"":{  
            ""principalId"":""AIDAJDPLRKLG7UEXAMPLE""
         },
         ""requestParameters"":{  
            ""sourceIPAddress"":""127.0.0.1""
         },
         ""responseElements"":{  
            ""x-amz-request-id"":""C3D13FE58DE4C810"",
            ""x-amz-id-2"":""FMyUVURIY8/IgAtTv8xRjskZQpcIZ9KG4V5Wp6S7S/JRWeUWerMUE5JgHvANOjpD""
         },
         ""s3"":{  
            ""s3SchemaVersion"":""1.0"",
            ""configurationId"":""testConfigRule"",
            ""bucket"":{  
               ""name"":""mybucket2"",
               ""ownerIdentity"":{  
                  ""principalId"":""A3NL1KOZZKExample""
               },
               ""arn"":""arn:aws:s3:::mybucket2""
            },
            ""object"":{  
               ""key"":""canonical/year%3D2022/month%3D11/day%3D16/HappyFace2.jpg"",
               ""size"":1024,
               ""eTag"":""d41d8cd98f00b204e9800998ecf8427e"",
               ""versionId"":""096fKKXTRTtl3on89fVO.nfljtsv6qko"",
               ""sequencer"":""0055AED6DCD90281E5""
            }
         }
      }
   ]
}
";
        private readonly string snstoSQSS3Event = @"{
    ""Type"": ""Notification"",
    ""MessageId"": ""6ca747f7-9487-5f73-973f-1b9f06cf1d2c"",
    ""TopicArn"": ""arn:aws:sns:us-west-2:385627060817:MammothGPMIngressTopic"",
    ""Subject"": ""Amazon S3 Notification"",
    ""Message"": ""{\""Records\"":[{\""eventVersion\"":\""2.1\"",\""eventSource\"":\""aws:s3\"",\""awsRegion\"":\""us-west-2\"",\""eventTime\"":\""2023-02-16T20:20:23.405Z\"",\""eventName\"":\""ObjectCreated:Put\"",\""userIdentity\"":{\""principalId\"":\""AWS:AROAV7CI6BENKMACDNJ3R:021cbeb41fe24a6bb9d912bd8c21b9d1\""},\""requestParameters\"":{\""sourceIPAddress\"":\""10.0.173.28\""},\""responseElements\"":{\""x-amz-request-id\"":\""2V706R30GNPT349K\"",\""x-amz-id-2\"":\""voLXYF1uWrY3seA6KvnuGxd1bqrOggXp2ARk6vDqXUhSky9wZj/9+9u3q5RGJ1B06rcl1KD0bGrC7Ix08aNCFCwFvb/Q+0a3\""},\""s3\"":{\""s3SchemaVersion\"":\""1.0\"",\""configurationId\"":\""ZGI4YmQyOTMtZGRkOS00NDczLWE3NmQtNjRkOTExZTI4ZGM0\"",\""bucket\"":{\""name\"":\""gpm-price-us-west-2-gamma\"",\""ownerIdentity\"":{\""principalId\"":\""A3V4PCDWX6GHO\""},\""arn\"":\""arn:aws:s3:::gpm-price-us-west-2-gamma\""},\""object\"":{\""key\"":\""ID%3AAWD0002110-58712-1676415226274-7%3A4%3A26%3A1%3A1-20230216T202023371928968Z\"",\""size\"":5570,\""eTag\"":\""f839d1c79cf1ddc7106b97bea26da1b9\"",\""sequencer\"":\""0063EE90075FC6B9EE\""}}}]}"",
    ""Timestamp"": ""2023-02-16T20:20:24.264Z"",
    ""SignatureVersion"": ""1"",
    ""Signature"": ""CpMe5+GJil6tus5Q0JW8k/up2lwcZCeiMU4wuw8ybGWo6NlMD8gOnIFjlMPCLDakkJGrFUqWgFWgsaY9HZqFUh3yNiiB2Id592Gyi9y38P3k+6oBn+uerTxHmZvp/GK7pVCncinElafz9rZC0tRZL4eNYk3woeO6j+SGRtQltdJnZYJStCaH4zUWRsve4JuBFVULMtV/6yoOn08KIyHjQfpXLrcW4ozKZ0CiRnhz7LEb+Fn1OqVmOKWACjEm8NR+oRa4zMgh+ZJwyd37lk/o46UPt9lO7Q8fQbQr3YsuzlCcSKUY150NuZpKP4bJpORbEpVwL0GVU1++l5fCg/+exA=="",
    ""SigningCertURL"": ""https://sns.us-west-2.amazonaws.com/SimpleNotificationService-56e67fcb41f6fec09b0196692625d385.pem"",
    ""UnsubscribeURL"": ""https://sns.us-west-2.amazonaws.com/?Action=Unsubscribe&SubscriptionArn=arn:aws:sns:us-west-2:385627060817:MammothGPMIngressTopic:d969f7ac-86f4-4963-9fce-db6760682b4f""
}";

        [TestMethod]
        public void Deserialize_ValidTest()
        {
            // Given
            S3EventMessageSerializer s3EventMessageSerializer = new S3EventMessageSerializer();

            // When
            ExtendedClientMessageModel extendedClientMessageModel = s3EventMessageSerializer.Deserialize(s3EventWithSingleRecord);

            // Then
            Assert.IsNotNull(extendedClientMessageModel);
            Assert.AreEqual(1, extendedClientMessageModel.S3Details.Count);
            Assert.AreEqual("mybucket", extendedClientMessageModel.S3Details[0].S3BucketName);
            Assert.AreEqual("canonical/year=2022/month=11/day=15/HappyFace.jpg", extendedClientMessageModel.S3Details[0].S3Key);
            Assert.IsNotNull(extendedClientMessageModel.MessageAttributes);
            Assert.AreEqual(0, extendedClientMessageModel.MessageAttributes.Count);
        }

        [TestMethod]
        public void Deserialize_MutipleRecordsInEvent_ValidTest()
        {
            // Given
            S3EventMessageSerializer s3EventMessageSerializer = new S3EventMessageSerializer();

            // When
            ExtendedClientMessageModel extendedClientMessageModel = s3EventMessageSerializer.Deserialize(s3EventWithMultipleRecords);

            // Then
            Assert.IsNotNull(extendedClientMessageModel);
            Assert.AreEqual(2, extendedClientMessageModel.S3Details.Count);
            Assert.AreEqual("mybucket1", extendedClientMessageModel.S3Details[0].S3BucketName);
            Assert.AreEqual("canonical/year=2022/month=11/day=15/HappyFace1.jpg", extendedClientMessageModel.S3Details[0].S3Key);
            Assert.AreEqual("mybucket2", extendedClientMessageModel.S3Details[1].S3BucketName);
            Assert.AreEqual("canonical/year=2022/month=11/day=16/HappyFace2.jpg", extendedClientMessageModel.S3Details[1].S3Key);
            Assert.IsNotNull(extendedClientMessageModel.MessageAttributes);
            Assert.AreEqual(0, extendedClientMessageModel.MessageAttributes.Count);
        }

        [TestMethod]
        public void Deserialize_SNSToSQSS3Event_ValidTest()
        {
            // Given
            S3EventMessageSerializer s3EventMessageSerializer = new S3EventMessageSerializer();

            // When
            ExtendedClientMessageModel extendedClientMessageModel = s3EventMessageSerializer.Deserialize(snstoSQSS3Event);

            // Then
            Assert.IsNotNull(extendedClientMessageModel);
            Assert.AreEqual(1, extendedClientMessageModel.S3Details.Count);
            Assert.AreEqual("gpm-price-us-west-2-gamma", extendedClientMessageModel.S3Details[0].S3BucketName);
            Assert.AreEqual("ID:AWD0002110-58712-1676415226274-7:4:26:1:1-20230216T202023371928968Z", extendedClientMessageModel.S3Details[0].S3Key);
            Assert.IsNotNull(extendedClientMessageModel.MessageAttributes);
            Assert.AreEqual(0, extendedClientMessageModel.MessageAttributes.Count);
        }
    }
}
