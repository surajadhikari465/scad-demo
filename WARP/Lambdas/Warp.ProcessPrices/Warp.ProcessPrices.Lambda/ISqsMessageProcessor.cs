using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;

namespace Warp.ProcessPrices.Lambda
{
    public interface ISqsMessageProcessor
    {
        void Process(SQSEvent.SQSMessage sqsMessage, ILambdaContext context);
    }
}