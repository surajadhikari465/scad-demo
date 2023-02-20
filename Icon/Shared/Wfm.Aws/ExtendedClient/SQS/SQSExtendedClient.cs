using Amazon.S3.Model;
using Amazon.SQS.Model;
using System.Collections.Generic;
using System.IO;
using Wfm.Aws.ExtendedClient.Model;
using Wfm.Aws.ExtendedClient.Serializer;
using Wfm.Aws.ExtendedClient.SQS.Model;
using Wfm.Aws.S3;
using Wfm.Aws.SQS;

namespace Wfm.Aws.ExtendedClient.SQS
{
    public class SQSExtendedClient : ISQSExtendedClient
    {
        public ISQSFacade sqsFacade { get; private set; }
        public IS3Facade s3Facade { get; private set; }
        private readonly IExtendedClientMessageSerializer extendedClientMessageSerializer;

        public SQSExtendedClient(ISQSFacade sqsFacade, IS3Facade s3Facade, IExtendedClientMessageSerializer extendedClientMessageSerializer)
        {
            this.sqsFacade = sqsFacade;
            this.s3Facade = s3Facade;
            this.extendedClientMessageSerializer = extendedClientMessageSerializer;
        }

        public IList<SQSExtendedClientReceiveModel> ReceiveMessage(string queueURL, int maxNumberOfMessages = 1, int waitTimeInSeconds = 0)
        {
            IList<SQSExtendedClientReceiveModel> sqsExtendedClientReceives = new List<SQSExtendedClientReceiveModel>();
            ReceiveMessageResponse receiveMessageResponse = sqsFacade.ReceiveMessage(queueURL, maxNumberOfMessages, waitTimeInSeconds);
            foreach (Message message in receiveMessageResponse.Messages)
            {
                IList<SQSExtendedClientReceiveModelS3Detail> sqsExtendedClientReceiveModelS3Details = new List<SQSExtendedClientReceiveModelS3Detail>();
                ExtendedClientMessageModel extendedClientMessageModel = extendedClientMessageSerializer.Deserialize(message.Body);
                foreach (ExtendedClientMessageModelS3Detail extendedClientMessageModelS3Detail in extendedClientMessageModel.S3Details)
                {
                    string data;
                    GetObjectResponse s3GetObjectResponse = s3Facade.GetObject(extendedClientMessageModelS3Detail.S3BucketName, extendedClientMessageModelS3Detail.S3Key);
                    Stream s3Stream = s3GetObjectResponse.ResponseStream;
                    StreamReader s3StreamReader = new StreamReader(s3Stream);
                    try
                    {
                        data = s3StreamReader.ReadToEnd();
                    }
                    finally
                    {
                        s3Stream.Close();
                        s3StreamReader.Close();
                    }
                    ICollection<string> metadataKeys = s3GetObjectResponse.Metadata.Keys;
                    IDictionary<string, string> metadata = new Dictionary<string, string>();
                    foreach (string metadataKey in metadataKeys)
                    {
                        metadata[metadataKey.Replace("x-amz-meta-", "")] = s3GetObjectResponse.Metadata[metadataKey];
                    }
                    sqsExtendedClientReceiveModelS3Details.Add(new SQSExtendedClientReceiveModelS3Detail()
                    {
                        Data = data,
                        S3Key = extendedClientMessageModelS3Detail.S3Key,
                        S3BucketName = extendedClientMessageModelS3Detail.S3BucketName,
                        Metadata = metadata
                    });
                }
                sqsExtendedClientReceives.Add(new SQSExtendedClientReceiveModel()
                {
                    S3Details = sqsExtendedClientReceiveModelS3Details,
                    MessageAttributes = extendedClientMessageModel.MessageAttributes,
                    SQSAttributes = message.Attributes,
                    SQSMessage = message,
                    SQSMessageID = message.MessageId,
                    SQSReceiptHandle = message.ReceiptHandle,
                });
            }
            return sqsExtendedClientReceives;
        }

        public DeleteMessageResponse DeleteMessage(string queueURL, string receiptHandle)
        {
            return sqsFacade.DeleteMessage(queueURL, receiptHandle);
        }

        public SendMessageResponse SendMessage(string queueURL, string s3BucketName, string s3Key, string data, IDictionary<string, string> messageAttributes)
        {
            string sqsMessage = extendedClientMessageSerializer.Serialize(s3BucketName, s3Key, messageAttributes);
            s3Facade.PutObject(s3BucketName, s3Key, data, new Dictionary<string, string>());
            return sqsFacade.SendMessage(queueURL, sqsMessage, messageAttributes);
        }
    }
}
