using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amazon.S3;
using Amazon.S3.Model;
using Icon.Dvs.Model;
using Icon.Dvs.Serializer;
using Icon.Dvs.Serializer.Exception;
using Newtonsoft.Json;

namespace Icon.Dvs.Subscriber
{
    public class DvsSqsSubscriber: IDvsSubscriber
    {
        private readonly IAmazonSQS sqsClient;
        private readonly IAmazonS3 s3Client;

        private readonly DvsListenerSettings settings;

        public DvsSqsSubscriber(IAmazonS3 s3Client, IAmazonSQS sqsClient, DvsListenerSettings settings)
        {
            this.sqsClient = sqsClient;
            this.s3Client = s3Client;
            this.settings = settings;
        }

        /// <summary>
        /// Receives SQS messages from DVS subscribed queue
        /// </summary>
        /// <param name="count">number of messages to receive</param>
        /// <returns>Task that returns List of DvsSqsMessages</returns>
        public async Task<IList<DvsSqsMessage>> ReceiveDvsSqsMessages(int count)
        {
            ReceiveMessageResponse receiveMessageResponse = await sqsClient.ReceiveMessageAsync(
                new ReceiveMessageRequest()
                {
                    QueueUrl = settings.SqsQueueUrl,
                    MaxNumberOfMessages = count,
                    WaitTimeSeconds = settings.SqsTimeout
                }
            );
            IList<DvsSqsMessage> dataVayuSqsMessages = new List<DvsSqsMessage>();
            foreach (Message message in receiveMessageResponse.Messages)
            {
                var dataVayuSqsMessage = ConvertToDataVayuSqsMessage(message);
                dataVayuSqsMessages.Add(dataVayuSqsMessage);
            }
            return dataVayuSqsMessages;
        }

        /// <summary>
        /// Receives one SQS message from DVS subscribed queue and gets the S3 object associated with the SQS message
        /// Returns an object containing both DvsSqsMessage and S3 message content
        /// </summary>
        /// <returns>DvsMessage</returns>
        public DvsMessage ReceiveDvsMessage()
        {
            DvsMessage message = null;
            IList<DvsSqsMessage> dataVayuSqsMessages = ReceiveDvsSqsMessages(1).Result;
            if (dataVayuSqsMessages.Count > 0)
            {
                var dataVayuSqsMessage = dataVayuSqsMessages[0];
                string messageContent = GetS3ObjectAsString(dataVayuSqsMessage.S3BucketName, dataVayuSqsMessage.S3Key).Result;
                message = new DvsMessage(dataVayuSqsMessage, messageContent);
            }

            return message;
        }

        /// <summary>
        /// Returns S3 object as string
        /// </summary>
        /// <param name="bucketName">S3 bucket name</param>
        /// <param name="s3Key">S3 key</param>
        /// <returns>Task that returns string of S3 object</returns>
        public async Task<string> GetS3ObjectAsString(string bucketName, string s3Key)
        {
            GetObjectResponse getObjectResponse = await s3Client.GetObjectAsync(new GetObjectRequest()
            {
                BucketName = bucketName,
                Key = s3Key
            });

            Stream s3Stream = getObjectResponse.ResponseStream;
            StreamReader reader = new StreamReader(s3Stream);
            try
            {
                return reader.ReadToEnd();
            }
            finally
            {
                s3Stream.Close();
                reader.Close();
            }
        }

        /// <summary>
        /// Deletes the Sqs message from the queue
        /// Should be called after successful processing of messages
        /// </summary>
        /// <param name="receiptHandle">receipt handle of SQS message</param>
        /// <returns></returns>
        public DeleteMessageResponse DeleteSqsMessage(string receiptHandle)
        {
            DeleteMessageResponse response = sqsClient.DeleteMessageAsync(
                new DeleteMessageRequest()
                {
                    QueueUrl = settings.SqsQueueUrl,
                    ReceiptHandle = receiptHandle
                }
            ).Result;
            return response;
        }


        private DvsSqsMessage ConvertToDataVayuSqsMessage(Message sqsMessage)
        {
            DvsSqsMessage message;
            try
            {
                message = JsonConvert.DeserializeObject<DvsSqsMessage>(
                    sqsMessage.Body, new DvsSqsMessageConverter()
                );
            }
            catch (Exception ex)
            {
                throw new ParsingException(ex.ToString());
            } 
            
            message.SqsReceiptHandle = sqsMessage.ReceiptHandle;

            return message;
        }
    }
}
