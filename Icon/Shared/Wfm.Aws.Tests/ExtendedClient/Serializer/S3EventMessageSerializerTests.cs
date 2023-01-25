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
        [TestMethod]
        public void Deserialize_ValidTest()
        {
            // Given
            S3EventMessageSerializer s3EventMessageSerializer = new S3EventMessageSerializer();

            // When
            IList<ExtendedClientMessageModel> extendedClientMessageModels = s3EventMessageSerializer.Deserialize(s3EventWithSingleRecord);

            // Then
            Assert.IsNotNull(extendedClientMessageModels);
            Assert.AreEqual(1, extendedClientMessageModels.Count);
            Assert.AreEqual("mybucket", extendedClientMessageModels[0].S3BucketName);
            Assert.AreEqual("canonical/year=2022/month=11/day=15/HappyFace.jpg", extendedClientMessageModels[0].S3Key);
        }

        [TestMethod]
        public void Deserialize_MutipleRecordsInEvent_ValidTest()
        {
            // Given
            S3EventMessageSerializer s3EventMessageSerializer = new S3EventMessageSerializer();

            // When
            IList<ExtendedClientMessageModel> extendedClientMessageModels = s3EventMessageSerializer.Deserialize(s3EventWithMultipleRecords);

            // Then
            Assert.IsNotNull(extendedClientMessageModels);
            Assert.AreEqual(2, extendedClientMessageModels.Count);
            Assert.AreEqual("mybucket1", extendedClientMessageModels[0].S3BucketName);
            Assert.AreEqual("canonical/year=2022/month=11/day=15/HappyFace1.jpg", extendedClientMessageModels[0].S3Key);
            Assert.AreEqual("mybucket2", extendedClientMessageModels[1].S3BucketName);
            Assert.AreEqual("canonical/year=2022/month=11/day=16/HappyFace2.jpg", extendedClientMessageModels[1].S3Key);
            Assert.IsNull(extendedClientMessageModels[1].MessageAttributes);
        }
    }
}
