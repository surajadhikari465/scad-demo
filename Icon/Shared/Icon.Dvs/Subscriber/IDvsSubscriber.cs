using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using Icon.Dvs.Model;

namespace Icon.Dvs.Subscriber
{
    public interface IDvsSubscriber
    {
        Task<IList<DvsSqsMessage>> ReceiveDvsSqsMessages(int count);
        DvsMessage ReceiveDvsMessage();
        Task<string> GetS3ObjectAsString(string bucketName, string s3Key);
        DeleteMessageResponse DeleteSqsMessage(string receiptHandle);
    }
}
